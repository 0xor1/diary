using Common.Server.Test;
using Diary.Db;
using S = Diary.I18n.S;

namespace Diary.Eps.Test;

public class AppTests : IDisposable
{
    private readonly RpcTestRig<DiaryDb, Api.Api> Rig;

    public AppTests()
    {
        Rig = new RpcTestRig<DiaryDb, Api.Api>(S.Inst, DiaryEps.Eps, c => new Api.Api(c));
    }

    [Fact]
    public async void GetConfig_Success()
    {
        var (ali, _, _) = await Rig.NewApi("ali");
        var c = await ali.App.GetConfig();
        Assert.True(c.DemoMode);
        Assert.Equal("https://github.com/0xor1/diary", c.RepoUrl);
    }

    public void Dispose()
    {
        Rig.Dispose();
    }
}
