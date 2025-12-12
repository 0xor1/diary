// Generated Code File, Do Not Edit.
// This file is generated with Common.Cli.
// see https://github.com/0xor1/common/blob/main/Common.Cli/Api.cs
// executed with arguments: api <abs_file_path_to>/Diary.Api

using Common.Shared;
using Common.Shared.Auth;
using Diary.Api.Entry;


namespace Diary.Api;

public interface IApi : Common.Shared.Auth.IApi
{
    public IEntryApi Entry { get; }
    
}

public class Api : IApi
{
    public Api(IRpcClient client)
    {
        App = new AppApi(client);
        Auth = new AuthApi(client);
        Entry = new EntryApi(client);
        
    }

    public IAppApi App { get; }
    public IAuthApi Auth { get; }
    public IEntryApi Entry { get; }
    
}