// Generated Code File, Do Not Edit.
// This file is generated with Common.Cli.
// see https://github.com/0xor1/common/blob/main/Common.Cli/Api.cs
// executed with arguments: api <abs_file_path_to>/Diary.Api

#nullable enable

using Common.Shared;
using MessagePack;


namespace Diary.Api.Entry;

public interface IEntryApi
{
    public Task<Entry> Create(Create arg, CancellationToken ctkn = default);
    public Task<SetRes<Entry>> Get(Get arg, CancellationToken ctkn = default);
    public Task<Entry> Update(Update arg, CancellationToken ctkn = default);
    public Task Delete(Delete arg, CancellationToken ctkn = default);
    
}

public class EntryApi : IEntryApi
{
    private readonly IRpcClient _client;

    public EntryApi(IRpcClient client)
    {
        _client = client;
    }

    public Task<Entry> Create(Create arg, CancellationToken ctkn = default) =>
        _client.Do(EntryRpcs.Create, arg, ctkn);
    
    public Task<SetRes<Entry>> Get(Get arg, CancellationToken ctkn = default) =>
        _client.Do(EntryRpcs.Get, arg, ctkn);
    
    public Task<Entry> Update(Update arg, CancellationToken ctkn = default) =>
        _client.Do(EntryRpcs.Update, arg, ctkn);
    
    public Task Delete(Delete arg, CancellationToken ctkn = default) =>
        _client.Do(EntryRpcs.Delete, arg, ctkn);
    
    
}

public static class EntryRpcs
{
    public static readonly Rpc<Create, Entry> Create = new("/entry/create");
    public static readonly Rpc<Get, SetRes<Entry>> Get = new("/entry/get");
    public static readonly Rpc<Update, Entry> Update = new("/entry/update");
    public static readonly Rpc<Delete, Nothing> Delete = new("/entry/delete");
    
}



[MessagePackObject]
public record Entry
{
    public Entry(
        string user,
        string id,
        DateTime createdOn,
        string title,
        string body
        
    )
    {
        User = user;
        Id = id;
        CreatedOn = createdOn;
        Title = title;
        Body = body;
        
    }
    
    [Key(0)]
    public string User { get; set; }
    [Key(1)]
    public string Id { get; set; }
    [Key(2)]
    public DateTime CreatedOn { get; set; }
    [Key(3)]
    public string Title { get; set; }
    [Key(4)]
    public string Body { get; set; }
    
}



[MessagePackObject]
public record Create
{
    public Create(
        string title,
        string body
        
    )
    {
        Title = title;
        Body = body;
        
    }
    
    [Key(0)]
    public string Title { get; set; }
    [Key(1)]
    public string Body { get; set; }
    
}



[MessagePackObject]
public record Update
{
    public Update(
        string id,
        string title,
        string body
        
    )
    {
        Id = id;
        Title = title;
        Body = body;
        
    }
    
    [Key(0)]
    public string Id { get; set; }
    [Key(1)]
    public string Title { get; set; }
    [Key(2)]
    public string Body { get; set; }
    
}



[MessagePackObject]
public record Delete
{
    public Delete(
        string id
        
    )
    {
        Id = id;
        
    }
    
    [Key(0)]
    public string Id { get; set; }
    
}



[MessagePackObject]
public record Get
{
    public Get(
        MinMax<DateTime>? createdOn = null,
        string? containing = null,
        string? after = null,
        bool asc = false
        
    )
    {
        CreatedOn = createdOn;
        Containing = containing;
        After = after;
        Asc = asc;
        
    }
    
    [Key(0)]
    public MinMax<DateTime>? CreatedOn { get; set; } = null;
    [Key(1)]
    public string? Containing { get; set; } = null;
    [Key(2)]
    public string? After { get; set; } = null;
    [Key(3)]
    public bool Asc { get; set; } = false;
    
}



