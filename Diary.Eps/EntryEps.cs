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
            Ep<Get, SetRes<Entry>>.DbTx<DiaryDb>(EntryRpcs.Get, Get),
            Ep<Update, Entry>.DbTx<DiaryDb>(EntryRpcs.Update, Update),
            Ep<Delete, Nothing>.DbTx<DiaryDb>(EntryRpcs.Delete, Delete),
        };

    private static async Task<Entry> Create(IRpcCtx ctx, DiaryDb db, ISession ses, Create req)
    {
        var entry = new DbEntry
        {
            User = ses.Id,
            Id = Id.New(),
            CreatedOn = DateTimeExt.UtcNowMilli(),
            Title = ctx.Get<IHtmlSanitizer>().Sanitize(req.Title),
            Body = ctx.Get<IHtmlSanitizer>().Sanitize(req.Body),
        };
        await db.Entries.AddAsync(entry, ctx.Ctkn);
        return entry.ToApi();
    }

    private static async Task<SetRes<Entry>> Get(IRpcCtx ctx, DiaryDb db, ISession ses, Get req)
    {
        throw new NotImplementedException();
    }

    private static async Task<Entry> Update(IRpcCtx ctx, DiaryDb db, ISession ses, Update req)
    {
        throw new NotImplementedException();
    }

    private static async Task<Nothing> Delete(IRpcCtx ctx, DiaryDb db, ISession ses, Delete req)
    {
        throw new NotImplementedException();
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
