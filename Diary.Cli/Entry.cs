using Common.Shared;
using Diary.Api;
using Diary.Api.Entry;

namespace Diary.Cli;

public class Entry
{
    private readonly IApi _api;

    public Entry(IApi api)
    {
        _api = api;
    }

    /// <summary>
    /// Create a new entry
    /// </summary>
    /// <param name="title">-t, title</param>
    /// <param name="body">-b, body</param>
    public async Task Create(string title, string body, CancellationToken ctkn = default) =>
        Io.WriteYml(await _api.Entry.Create(new Create(title, body), ctkn));

    /// <summary>
    /// Get a set of entries
    /// </summary>
    /// <param name="minCreatedOn">-mco, min created on</param>
    /// <param name="maxCreatedOn">-xco, max created on</param>
    /// <param name="containing">-c, containing text</param>
    /// <param name="after">-a, after entry id</param>
    /// <param name="asc">-asc, order ascending</param>
    /// <param name="ctkn"></param>
    public async Task Get(
        DateTime? minCreatedOn = null,
        DateTime? maxCreatedOn = null,
        string? containing = null,
        string? after = null,
        bool asc = false,
        CancellationToken ctkn = default
    ) =>
        Io.WriteYml(
            await _api.Entry.Get(
                new Get(
                    MinMax<DateTime>.Create(minCreatedOn, maxCreatedOn),
                    containing,
                    after,
                    asc
                ),
                ctkn
            )
        );

    /// <summary>
    /// Update an entry
    /// </summary>
    /// <param name="id">-i, id</param>
    /// <param name="title">-t, title</param>
    /// <param name="body">-b, body</param>
    public async Task Update(
        string id,
        string title,
        string body,
        CancellationToken ctkn = default
    ) => Io.WriteYml(await _api.Entry.Update(new Update(id, title, body), ctkn));

    /// <summary>
    /// Delete an entry
    /// </summary>
    /// <param name="id">-i, id</param>
    /// <param name="title">-t, title</param>
    /// <param name="body">-b, body</param>
    public async Task Delete(string id, string title, string body, CancellationToken ctkn = default)
    {
        await _api.Entry.Delete(new Delete(id), ctkn);
        Io.WriteSuccess();
    }
}
