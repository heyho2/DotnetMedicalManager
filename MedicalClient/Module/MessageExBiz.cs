using GD.DataAccess;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace GD.Module
{
    public class MessageExBiz
    {
        public void PushMessageToRedis()
        {
            
            string msgsKey = "CloudDoctor:IMMessageList";
            RedisHelper.Database.KeyDelete(msgsKey);
            MessageModel model = new MessageModel {

            };
            var jsonModel= JsonConvert.SerializeObject(model);
            for (int i = 0; i < 10; i++)
            {
                RedisHelper.Database.ListRightPush(msgsKey, (i+1).ToString());
            }
        }

        public void SaveMessageFromRedis()
        {
            string msgsKey = "CloudDoctor:IMMessageList";
            var lst= RedisHelper.Database.ListRange(msgsKey,0,99);
            RedisHelper.Database.ListTrim(msgsKey, lst.Count(), -1);
            var result= lst.Select(a => a.ToString()).ToList();

            


        }
    }
}
