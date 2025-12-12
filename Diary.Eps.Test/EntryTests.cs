using Common.Server.Test;
using Diary.Db;
using S = Diary.I18n.S;

namespace Diary.Eps.Test;

public class EntryTests : IDisposable
{
    private readonly RpcTestRig<DiaryDb, Api.Api> _rpcTestRig;

    public EntryTests()
    {
        _rpcTestRig = new RpcTestRig<DiaryDb, Api.Api>(
            S.Inst,
            DiaryEps.Eps,
            c => new Api.Api(c),
            DiaryEps.AddServices
        );
    }

    [Fact]
    public async Task Create_Success()
    {
        // TODO
    }

    public void Dispose()
    {
        _rpcTestRig.Dispose();
    }
}
