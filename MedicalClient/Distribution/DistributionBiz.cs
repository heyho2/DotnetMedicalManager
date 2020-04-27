using Dapper;
using GD.DataAccess;
using GD.Models.Distribution;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distribution
{
    /// <summary>
    /// 分销业务数据库操作
    /// </summary>
    public class DistributionBiz
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(DistributionModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, DistributionModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(DistributionModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }
        /// <summary>
        /// 根据id获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DistributionModel> GetModelAsync(string id, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<DistributionModel>("select * from t_distribution where distribution_guid=@id and `enable`=@enable", new { id, enable });
            }
        }
        /// <summary>
        /// 根据id获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DistributionModel> GetModelAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<DistributionModel>(id);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.DeleteAsync<DistributionModel>(id);
                return result > 0;
            }
        }
        /// <summary>
        /// 获取分销信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMyDistributionInfo(string userID)
        {
            var sql = "";

            using (var conn = MySqlHelper.GetConnection())
            {
                // var result = await conn.Query<>(sql, new { userID }).ToList();
            }


            return "";
        }

        /// <summary>
        /// 根据id获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DistributionUserModel> GetModelAsyncByID(string id, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<DistributionUserModel>("select * from t_distribution_user where distribution_guid=@id and `enable`=@enable", new { id, enable });
            }
        }

        /// <summary>
        /// 获取一级粉丝-byUserID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<UserModel>> GetFirstFansListModelAsyncByID(string id, bool enable = true)
        {
            var sql = @"SELECT
	                                * 
                                FROM
	                                t_utility_user 
                                WHERE
	                                recommend_guid = @id
	                                AND `enable` = @enable ";
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<UserModel>(sql, new { id, enable })).ToList();
            }
        }

        /// <summary>
        /// 获取二级粉丝-byUserID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<UserModel>> GetSecondFansListModelAsyncByID(string id, bool enable = true)
        {
            var sql = @"SELECT
	                                * 
                                FROM
	                                t_utility_user 
                                WHERE
	                                recommend_guid IN ( SELECT user_guid FROM t_utility_user WHERE recommend_guid =@id AND `enable` = @enable  ) 
	                                AND `enable` = 1 ";
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<UserModel>(sql, new { id, enable })).ToList();
            }
        }

        /// <summary>
        /// 根据id获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DistributionUserModel> GetModelAsyncById(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<DistributionUserModel>(id);
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(DistributionUserModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, DistributionUserModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(DistributionUserModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }


        /// <summary>
        /// 获取二级粉丝-byUserID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<DistributionRecordModel>> GetDistributionRecordListModelAsync(string id, bool enable = true)
        {
            var sql = @"SELECT
	                                * 
                                FROM
	                                t_distribution_record 
                                WHERE
	                                ( first_guid = @id OR second_guid = @id ) 
                                	AND `enable` = @enable ";
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<DistributionRecordModel>(sql, new { id, enable })).ToList();
            }
        }
    }
}