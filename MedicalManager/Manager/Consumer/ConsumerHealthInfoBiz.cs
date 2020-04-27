using Dapper;
using GD.DataAccess;
using GD.Models.Consumer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.Manager.Consumer
{
    /// <summary>
    /// 用户健康信息业务类
    /// </summary>
    public class ConsumerHealthInfoBiz : BaseBiz<ConsumerHealthInfoModel>
    {
        /// <summary>
        /// 查找用户信息问题
        /// </summary>
        /// <param name="informationGuid"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ConsumerHealthInfoModel> GetConsumerHealthInfoAsync(string informationGuid, string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<ConsumerHealthInfoModel>(@"
                   select * from t_consumer_health_info where information_guid = @informationGuid and user_guid = @userId and `enable`=1", new { informationGuid, userId });
            }
        }

        /// <summary>
        /// 创建或更新会员基础信息
        /// </summary>
        /// <param name="insertModels"></param>
        /// <param name="updateModels"></param>
        /// <returns></returns>
        public async Task<bool> CreateOrUpdateConsumerHealthInfo(List<ConsumerHealthInfoModel> insertModels, List<ConsumerHealthInfoModel> updateModels)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                if (insertModels != null && insertModels.Count > 0)
                {
                    insertModels.InsertBatch(conn);
                }

                if (updateModels != null && updateModels.Count > 0)
                {
                    await UpdateConsumerInfos(conn, updateModels);
                }

                return true;
            });
        }

        /// <summary>
        /// 批量更新会员基础信息
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="updateModels"></param>
        /// <returns></returns>
        async Task<bool> UpdateConsumerInfos(MySql.Data.MySqlClient.
      MySqlConnection conn, List<ConsumerHealthInfoModel> updateModels)
        {
            var sql = @"update t_consumer_health_info 
                        set 
                            result_value = @ResultValue,
                            option_guids = @OptionGuids,
                            last_updated_by = @LastUpdatedBy,
                            last_updated_date = @LastUpdatedDate
                        where info_record_guid = @InfoRecordGuid";

            return await conn.ExecuteAsync(sql, updateModels) > 0;
        }

    }
}
