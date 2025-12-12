using Common.Client;
using Diary.Api;
using Diary.Client;
using Diary.I18n;

await Client.Run<App, IApi>(args, S.Inst, (client) => new Api(client));
