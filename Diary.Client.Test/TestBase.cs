using Common.Client;
using Common.Server.Test;
using Diary.Api;
using Diary.Db;
using Diary.Eps;
using Radzen;
using S = Diary.I18n.S;

namespace Diary.Client.Test;

public class TestBase : IDisposable
{
    protected readonly RpcTestRig<DiaryDb, Api.Api> RpcTestRig;
    protected readonly List<TestPack> TestPacks = new();

    public TestBase()
    {
        RpcTestRig = new RpcTestRig<DiaryDb, Api.Api>(S.Inst, DiaryEps.Eps, c => new Api.Api(c));
    }

    protected async Task<TestPack> NewTestPack(string name)
    {
        var (api, email, pwd) = await RpcTestRig.NewApi(name);
        var ctx = new BunitContext();
        var l = new Localizer(S.Inst);
        var ns = new NotificationService();
        Common.Client.Client.Setup(ctx.Services, l, S.Inst, ns, (IApi)api);
        var tp = new TestPack(api, ctx, email, pwd);
        TestPacks.Add(tp);
        return tp;
    }

    public void Dispose()
    {
        RpcTestRig.Dispose();
        TestPacks.ForEach(x => x.Ctx.Dispose());
    }
}

public record TestPack(IApi Api, BunitContext Ctx, string Email, string Pwd);
