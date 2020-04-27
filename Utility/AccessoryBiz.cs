using Dapper;
using GD.DataAccess;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Utility
{
    /// <summary>
    /// 附件模块业务类
    /// </summary>
    public class AccessoryBiz
    {
        #region 查询
        /// <summary>
        /// 通过附件Guid获取唯一附件对象实例
        /// </summary>
        /// <param name="accessoryGuid">附件Guid</param>
        /// <returns>附件对象实例</returns>
        public AccessoryModel GetAccessoryModelByGuid(string accessoryGuid, bool enable = true)
        {
            var sql = "select * from t_utility_accessory where accessory_guid=@accessoryGuid and enable=@enable ";
            var accessoryModel = MySqlHelper.SelectFirst<AccessoryModel>(sql, new { accessoryGuid, enable });
            return accessoryModel;
        }
        /// <summary>
        /// id 查找
        /// </summary>
        /// <param name="accessoryGuid"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<AccessoryModel> GetAsync(string accessoryGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<AccessoryModel>(accessoryGuid);
            }
        }

        /// <summary>
        /// 根据附件OwnerGuid查询附件列表
        /// </summary>
        /// <param name="ownerGuid">附件拥有着Guid</param>
        /// <returns>附件对象实例</returns>
        public async Task<List<AccessoryModel>> GetAccessoryListByOwnerGuid(string ownerGuid, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                 var sql = "select * from t_utility_accessory where owner_guid=@ownerGuid and enable=@enable ";
                return (await conn.QueryAsync<AccessoryModel>(sql, new { ownerGuid, enable })).ToList();
            }
        }
        #endregion

        #region 修改
        /// <summary>
        /// 更新附件记录
        /// </summary>
        /// <param name="accessoryModel">附件对象实例</param>
        /// <returns>返回是否成功</returns>
        public bool UpdateAccessoryModel(AccessoryModel accessoryModel)
        {
            accessoryModel.LastUpdatedDate = DateTime.Now;
            return accessoryModel.Update() == 1;
        }
        #endregion
    }
}
