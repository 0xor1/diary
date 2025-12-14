using Common.Server.Auth;
using Microsoft.EntityFrameworkCore;
using ApiEntry = Diary.Api.Entry.Entry;

namespace Diary.Db;

public class DiaryDb : DbContext, IAuthDb
{
    public DiaryDb(DbContextOptions<DiaryDb> opts)
        : base(opts) { }

    public DbSet<Auth> Auths { get; set; } = null!;

    public DbSet<FcmReg> FcmRegs { get; set; } = null!;
    public DbSet<Entry> Entries { get; set; } = null!;
}

[PrimaryKey(nameof(User), nameof(Id))]
public class Entry
{
    public required string User { get; set; }
    public required string Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }

    public ApiEntry ToApi() => new(User, Id, CreatedOn, Title, Body);
}
