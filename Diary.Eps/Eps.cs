using Common.Server;
using Common.Server.Auth;
using Diary.Db;
using Ganss.Xss;
using Microsoft.Extensions.DependencyInjection;

namespace Diary.Eps;

public static class DiaryEps
{
    private static IReadOnlyList<IEp>? _eps;
    public static IReadOnlyList<IEp> Eps
    {
        get
        {
            if (_eps == null)
            {
                var eps =
                    (List<IEp>)
                        new CommonEps<DiaryDb>(
                            5,
                            true,
                            5,
                            EntryEps.OnAuthActivation,
                            EntryEps.OnAuthDelete,
                            EntryEps.AuthValidateFcmTopic
                        ).Eps;
                eps.AddRange(EntryEps.Eps);
                _eps = eps;
            }

            return _eps;
        }
    }

    public static void AddServices(IServiceCollection sc)
    {
        sc.AddSingleton<IHtmlSanitizer, HtmlSanitizer>();
    }
}
