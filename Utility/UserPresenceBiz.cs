using GD.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Utility
{
    public class UserPresenceBiz
    {
        string PresenceStatusKey = "PresenceStatus:";
        /// <summary>
        /// 设置用户在线状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isOnline"></param>
        public void SetPresenceStatus(string userId, bool isOnline)
        {
            var key = $"{PresenceStatusKey}{userId}";
            var value = RedisHelper.Get<string>($"{PresenceStatusKey}{userId}");
            var redisPresenceObject = new UserPresenceObject();
            try
            {
                redisPresenceObject = string.IsNullOrWhiteSpace(value) ? new UserPresenceObject() : JsonConvert.DeserializeObject<UserPresenceObject>(value);
            }
            catch (Exception)
            {
                var theBool = bool.TryParse(value, out bool resultBool);
                var theInt = int.TryParse(value, out int resultInt);
                if (theBool || theInt)
                {
                    redisPresenceObject = new UserPresenceObject();
                }
            }

            var onlineNum = redisPresenceObject.OnlineTimes;
            if (isOnline)
            {
                onlineNum++;
            }
            else
            {
                onlineNum--;
            }
            onlineNum = onlineNum < 0 ? 0 : onlineNum;
            if (redisPresenceObject.OnlineTimes ==0 && onlineNum > 0)
            {
                redisPresenceObject.LatestOnlineTime = DateTime.Now;
            }

            redisPresenceObject.OnlineTimes = onlineNum;

            RedisHelper.Set($"{PresenceStatusKey}{userId}", JsonConvert.SerializeObject(redisPresenceObject));
        }

        /// <summary>
        /// 获取用户在线状态
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserPresenceStatus GetPresenceStatus(string userId)
        {
            var key = $"{PresenceStatusKey}{userId}";
            if (!RedisHelper.Database.KeyExists(key))
            {
                return new UserPresenceStatus();
            }
            var value = RedisHelper.Get<string>(key);
            var presenceObject = new UserPresenceObject();
            try
            {
                presenceObject = string.IsNullOrWhiteSpace(value) ? new UserPresenceObject() : JsonConvert.DeserializeObject<UserPresenceObject>(value);
            }
            catch (Exception)
            {
                var theBool = bool.TryParse(value, out bool resultBool);
                var theInt = int.TryParse(value, out int resultInt);
                if (theBool || theInt)
                {
                    presenceObject = new UserPresenceObject();
                }
            }
            return new UserPresenceStatus
            {
                IsOnline = presenceObject.OnlineTimes > 0,
                LatestOnlineTime = presenceObject.LatestOnlineTime
            };
        }

        /// <summary>
        /// 删除redis中无效用户的出席状态key
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeletePresenceStatus(string userId)
        {
            if (!RedisHelper.Database.KeyExists($"{PresenceStatusKey}{userId}"))
            {
                return true;
            }
            return RedisHelper.Delete($"{PresenceStatusKey}{userId}");
        }
    }

    public class UserPresenceObject
    {
        /// <summary>
        /// 上线次数
        /// </summary>
        public int OnlineTimes { get; set; } = 0;

        /// <summary>
        /// 最早登录时间
        /// </summary>
        public DateTime? LatestOnlineTime { get; set; }
    }

    public class UserPresenceStatus
    {
        /// <summary>
        /// 上线次数
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// 最早登录时间
        /// </summary>
        public DateTime? LatestOnlineTime { get; set; }
    }
}
