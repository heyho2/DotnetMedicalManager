using Dapper;
using GD.DataAccess;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Component
{
    /// <summary>
    /// 点赞收藏表
    /// </summary>
    public class HotBiz
    {
        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="ownerGuid">ownerGuid</param>
        /// <returns></returns>
        public HotModel GetModel(string ownerGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return conn.Get<HotModel>(ownerGuid);
            }
        }
        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="ownerGuid">ownerGuid</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<HotModel> GetModelAsync(string ownerGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return  await conn.GetAsync<HotModel>(ownerGuid);
            }
        }
    }
}