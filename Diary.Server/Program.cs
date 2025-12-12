using Common.Server;
using Diary.Db;
using Diary.Eps;
using Diary.I18n;

Server.Run<DiaryDb>(args, S.Inst, DiaryEps.Eps, DiaryEps.AddServices);
