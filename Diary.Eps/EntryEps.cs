using Common.Server;
using Common.Shared;
using Common.Shared.Auth;
using Diary.Api.Entry;
using Diary.Db;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using DbEntry = Diary.Db.Entry;
using Entry = Diary.Api.Entry.Entry;

namespace Diary.Eps;

internal static class EntryEps
{
    public static IReadOnlyList<IEp> Eps { get; } =
        new List<IEp>()
        {
            Ep<Create, Entry>.DbTx<DiaryDb>(EntryRpcs.Create, Create),
            Ep<Exact, Entry>.DbTx<DiaryDb>(EntryRpcs.GetOne, GetOne),
            Ep<Get, SetRes<Entry>>.DbTx<DiaryDb>(EntryRpcs.Get, Get),
            Ep<Update, Entry>.DbTx<DiaryDb>(EntryRpcs.Update, Update),
            Ep<Exact, Nothing>.DbTx<DiaryDb>(EntryRpcs.Delete, Delete),
        };

    private static async Task<Entry> Create(IRpcCtx ctx, DiaryDb db, ISession ses, Create req)
    {
        req.Title = ctx.Get<IHtmlSanitizer>().Sanitize(req.Title);
        ctx.ErrorFromValidationResult(req.Title.Validate(nameof(req.Title), 1, 250));
        req.Body = ctx.Get<IHtmlSanitizer>().Sanitize(req.Body);
        var entry = new DbEntry
        {
            User = ses.Id,
            Id = Id.New(),
            CreatedOn = DateTimeExt.UtcNowMilli(),
            Title = req.Title,
            Body = req.Body,
        };
        await db.Entries.AddAsync(entry, ctx.Ctkn);
        return entry.ToApi();
    }

    private static async Task<Entry> GetOne(IRpcCtx ctx, DiaryDb db, ISession ses, Exact req)
    {
        var res = await db.Entries.SingleOrDefaultAsync(
            x => x.User == ses.Id && x.Id == req.Id,
            ctx.Ctkn
        );
        ctx.NotFoundIf(res == null);
        return res.NotNull().ToApi();
    }

    private static async Task<SetRes<Entry>> Get(IRpcCtx ctx, DiaryDb db, ISession ses, Get req)
    {
        var qry = db.Entries.Where(x => x.User == ses.Id);
        if (req.CreatedOn?.Min != null)
        {
            qry = qry.Where(x => x.CreatedOn >= req.CreatedOn.Min);
        }
        if (req.CreatedOn?.Max != null)
        {
            qry = qry.Where(x => x.CreatedOn <= req.CreatedOn.Max);
        }
        if (req.After != null)
        {
            qry = req.Asc switch
            {
                true => qry.Where(x => string.Compare(x.Id, req.After) == 1),
                false => qry.Where(x => string.Compare(x.Id, req.After) == -1),
            };
        }
        qry = req.Asc switch
        {
            true => qry.OrderBy(x => x.Id),
            false => qry.OrderByDescending(x => x.Id),
        };
        qry = qry.Take(101);
        // when selecting a list of entries don't select the body, we only get the
        // body when we're getting the specific entry for reading/updating
        var res = await qry.Select(x => new Entry(x.User, x.Id, x.CreatedOn, x.Title, ""))
            .ToListAsync(ctx.Ctkn);
        return SetRes<Entry>.FromLimit(res, 101);
    }

    private static async Task<Entry> Update(IRpcCtx ctx, DiaryDb db, ISession ses, Update req)
    {
        req.Title = ctx.Get<IHtmlSanitizer>().Sanitize(req.Title);
        ctx.ErrorFromValidationResult(req.Title.Validate(nameof(req.Title), 1, 250));
        req.Body = ctx.Get<IHtmlSanitizer>().Sanitize(req.Body);
        var entry = await db.Entries.SingleOrDefaultAsync(
            x => x.User == ses.Id && x.Id == req.Id,
            ctx.Ctkn
        );
        ctx.NotFoundIf(entry == null, model: new { Name = "Entry" });
        entry.NotNull();
        entry.Title = req.Title;
        entry.Body = req.Body;
        return entry.ToApi();
    }

    private static async Task<Nothing> Delete(IRpcCtx ctx, DiaryDb db, ISession ses, Exact req)
    {
        await db
            .Entries.Where(x => x.User == ses.Id && x.Id == req.Id)
            .ExecuteDeleteAsync(ctx.Ctkn);
        return Nothing.Inst;
    }

    public static Task OnAuthActivation(IRpcCtx ctx, DiaryDb db, string id, string email) =>
        Task.CompletedTask;

    public static Task OnAuthDelete(IRpcCtx ctx, DiaryDb db, ISession ses) =>
        db.Entries.Where(x => x.User == ses.Id).ExecuteDeleteAsync(ctx.Ctkn);

    public static Task AuthValidateFcmTopic(
        IRpcCtx ctx,
        DiaryDb db,
        ISession ses,
        IReadOnlyList<string> topic
    )
    {
        ctx.BadRequestIf(topic.Count != 1);
        return Task.CompletedTask;
    }
}
