using GD.AppSettings;
using GD.Common.EnumDefine;
using GD.Common.Helper;
using GD.Communication.XMPP;
using GD.Crontab;
using GD.DataAccess;
using GD.Doctor;
using GD.Models.Doctor;
using GD.Utility;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GD.Scheduler
{
    /// <summary>
    /// 医生出席状态定时任务
    /// </summary>
    public class DoctorPresent : BaseJob
    {
        /// <summary>
        /// 任务间隔分钟
        /// </summary>
        private const int INTERVAL_MINUES = (1000 * 60) * 60;

        private static Client client = null;

        public DoctorPresent()
        {
            Interval = INTERVAL_MINUES;
        }

        static DoctorPresent()
        {
            var settings = Factory.GetSettings("host.json");
            var account = settings["XMPP:operationAccount"];
            var pwd = settings["XMPP:operationPassword"];
            client = new Client(account, pwd, $"{nameof(DoctorPresent)}#@@@#{Guid.NewGuid().ToString("N")}");
            client.OnPresence += Client_OnPresence;  // 此回调方法会实时返回好友在线状态
            client.ConnectAsync();


            var doctorPresentSwitch = settings["DoctorPresentSwitch"];
            var doctorPresentSwitchRes = false;
            if (bool.TryParse(doctorPresentSwitch, out doctorPresentSwitchRes))
            {
                if (doctorPresentSwitchRes)
                {
                    //服务重启删除所有出席状态redis记录
                    RedisHelper.Database.ScriptEvaluate(LuaScript.Prepare(
                         //Redis的keys模糊查询：
                         " local ks = redis.call('KEYS', @keypattern) " + //local ks为定义一个局部变量，其中用于存储获取到的keys
                          " for i=1,#ks,5000 do " +    //#ks为ks集合的个数, 语句的意思： for(int i = 1; i <= ks.Count; i+=5000)
                          "     redis.call('del', unpack(ks, i, math.min(i+4999, #ks))) " + //Lua集合索引值从1为起始，unpack为解包，获取ks集合中的数据，每次5000，然后执行删除
                          " end " +
                          " return true "),
                        new { keypattern = "PresenceStatus*" });
                }
            }



            //var pattern = "PresenceStatus*";
            //var redisResult = RedisHelper.Database.ScriptEvaluateAsync(LuaScript.Prepare(
            //                //Redis的keys模糊查询：
            //                " local res = redis.call('KEYS', @keypattern) " +
            //                " return res "), new { @keypattern = pattern }).Result;
            //var redisResults = (RedisResult[])redisResult;
            //foreach (var item in redisResults)
            //{
            //    var itemKey = item.ToString();
            //    //var itemValue = RedisHelper.Get<string>(itemKey);
            //    //var itemObject = JsonConvert.DeserializeObject<UserPresenceObject>(itemValue);
            //    //itemObject.OnlineTimes = 0;
            //    RedisHelper.Delete(item.ToString());
            //    //if (itemObject.OnlineTimes>0)
            //    //{
            //    //    itemObject.OnlineTimes--;
            //    //    RedisHelper.Set(itemKey, JsonConvert.SerializeObject(itemObject));
            //    //    Logger.Debug($"用户出席通知启动时数据优化：{itemKey}上线次数减一");
            //    //}
            //}



            // var result = client.CreateUserAsync("chris", "chris", "chris").Result;
            //
            //var task = client.AddRosterAsync("chris").Result;
            //var task1 = client.DeleteRosterAsync("chris");//删除失效的医生账号关联

            ////ba044c6eb41e48f59aff44a99e7e803b

            //var task = client.AddRosterAsync("hzl"); // 需要添加的好友账号和昵称
            //task.Wait();

            //if (!task.Result)
            //{
            //    // 添加失败
            //}
        }

        /// <summary>
        /// 用户出席通知
        /// </summary>
        /// <param name="client"></param>
        /// <param name="presence"></param>
        private static void Client_OnPresence(Client client, XmppPresence presence)
        {

            Settings settings = Factory.GetSettings("host.json");
            var doctorPresentSwitch = settings["DoctorPresentSwitch"];
            if (bool.TryParse(doctorPresentSwitch, out bool doctorPresentSwitchRes))
            {
                if (!doctorPresentSwitchRes)
                {
                    return;
                }
            }
            var userGuid = presence.Sender;
            var isOnline = presence.Online;

            Logger.Debug($"用户出席通知{userGuid}({presence.Resource})-{isOnline.ToString()}");

            #region 检测出席用户是否是医生
            var checkDoctorList = RedisHelper.Get<string>("CloudDoctor:DoctorList");
            if (checkDoctorList != null)
            {
                var doctorGuids = JsonConvert.DeserializeObject<List<string>>(checkDoctorList);
                if (!doctorGuids.Contains(userGuid))
                {
                    return;
                }
            }
            #endregion

            new UserPresenceBiz().SetPresenceStatus(userGuid, isOnline);

            var result = new UserPresenceBiz().GetPresenceStatus(userGuid);
            if (!result.IsOnline && result.LatestOnlineTime != null)
            {
                var model = new OnLineModel()
                {
                    OnlineGuid = Guid.NewGuid().ToString("N"),
                    DoctorGuid = userGuid,
                    LoginTime = result.LatestOnlineTime.Value,
                    LogoutTime = DateTime.Now,
                    CreatedBy = userGuid,
                    LastUpdatedBy = userGuid,
                    OrgGuid = ""
                };

                model.Duration = (decimal)Math.Round((model.LogoutTime - model.LoginTime).TotalMinutes, 2);

                var onlineResult = new HospitalManagerBiz().CreateDoctorOnlineRecord(model);
                if (string.IsNullOrEmpty(onlineResult))
                {
                    Logger.Warn($"添加医生“{userGuid}”在线时长记录失败");
                }
            }
        }

        /// <summary>
        /// 自动的执行任务
        /// </summary>
        /// <remarks>
        /// Interval 单位为毫秒
        /// Begin 从 2019-1-1 0:0:0 开始计时执行
        /// </remarks>
        public override void Run()
        {
            if (!CheckOperationOnline())
            {
                Logger.Error($"运营IM账号登录失败，本轮医生出席定时任务取消执行 at {nameof(DoctorPresent)}.Run");
                return;
            }
            Thread.Sleep(10000);
            var list = client.GetRosterAsync().Result.Select(a => a.Key);
            var doctorGuids = new DoctorBiz().GetAllDoctorIdsAsync().Result;

            RedisHelper.Set("CloudDoctor:DoctorList", JsonConvert.SerializeObject(doctorGuids));//缓存医生Id集合数据

            var expiredIds = list.Except(doctorGuids);
            foreach (var item in expiredIds)
            {
                var task = client.DeleteRosterAsync(item);//删除失效的医生账号关联
                task.Wait();

                if (!task.Result)
                {
                    // 删除失败
                }
                new UserPresenceBiz().DeletePresenceStatus(item);
            }
            if (expiredIds.Count() > 0)
            {
                Logger.Debug($"删除redis中无效用户的出席状态key:{JsonConvert.SerializeObject(expiredIds)}");
            }

            var exceptIds = doctorGuids.Except(list);
            var tmpExceptIds = new List<string>();
            foreach (var item in exceptIds)
            {
                var status = Client.QueryStatusAsync(item).Result;

                // 如果不存在，则无需创建关系
                if (status == IMStatus.NotExist)
                {
                    continue;
                }
                tmpExceptIds.Add(item);
                var task = client.AddRosterAsync(item); // 需要添加的好友账号和昵称
                task.Wait();

                if (!task.Result)
                {
                    // 添加失败
                }
            }
            if (tmpExceptIds.Count() > 0)
            {
                Logger.Debug($"需要添加用户好友关系的数据:{JsonConvert.SerializeObject(tmpExceptIds)}");
            }
        }

        /// <summary>
        /// 检查运营IM账户是否登录成功
        /// </summary>
        /// <returns></returns>
        private static bool CheckOperationOnline()
        {
            var begin = DateTime.Now;

            while (!client.Connected && (DateTime.Now - begin).TotalSeconds <= 3)
            {
                Thread.Sleep(100);
            }

            return client.Connected;
        }
    }
}