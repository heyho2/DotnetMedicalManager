using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GD.Common.EnumDefine;
using GD.Models.Utility;
using GD.DataAccess;

namespace GD.Utility
{
    /// <summary>
    /// 积分模块业务类
    /// </summary>
    public class ScoreBiz
    {
        #region 查询

        /// <summary>
        /// 获取单个积分变化记录
        /// </summary>
        /// <param name="scoreGuid">积分记录Guid</param>
        /// <param name="enable"></param>
        /// <returns>单个积分记录实例</returns>
        public ScoreModel GetScore(string scoreGuid, bool enable = true)
        {
            var sql = "select * from t_utility_score where score_guid=@scoreGuid  and enable=@enable ";
            var scoreModel = MySqlHelper.SelectFirst<ScoreModel>(sql, new { scoreGuid, enable });
            return scoreModel;
        }

        /// <summary>
        /// 判断用户指定角色今日是否签到
        /// </summary>
        /// <returns></returns>
        public bool CheckSignInScores(string userId, UserType userType = UserType.Consumer)
        {
            var sql = "select * from t_utility_score where creation_date>timestamp(DATE_FORMAT(now(), '%Y-%m-%d')) and user_guid=@userId and `enable`=1 and user_type_guid=@userType  and reason='签到积分'";
            var model = MySqlHelper.Select<ScoreModel>(sql, new { userId, userType });
            return model != null && model.Any();
        }

        /// <summary>
        /// 获取用户积分变化记录集合
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public List<ScoreModel> GetScores(string userId, string platformType = "CloudDoctor", bool enable = true)
        {
            var sql = "select * from t_utility_score where user_guid=@userId  and enable=@enable and platform_type=@platformType ";
            var characterModels = MySqlHelper.Select<ScoreModel>(sql, new { userId, enable });
            return characterModels?.ToList();
        }

        /// <summary>
        /// 获取用户指定平台今日总积分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="platformType"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<int> GetTotalDaySocre(string userId, string platformType = "CloudDoctor", bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"select IFNULL(sum(variation),0) from t_utility_score where user_guid=@userId and  creation_date>timestamp(DATE_FORMAT(now(), '%Y-%m-%d')) and enable=@enable and platform_type=@platformType";
                var sumScore = await conn.QueryFirstOrDefaultAsync<int?>(sql, new { userId, enable, platformType });
                return sumScore ?? 0;
            }
        }

        /// <summary>
        /// 分页获取用户积分变化记录
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="rowsPerPage">单页条数</param>
        /// <param name="conditions">条件,例如 where condition1=@condition1</param>
        /// <param name="orderby">排序标准,例如 condition2 desc</param>
        /// <param name="parameters">条件参数值,例如 new{condition1="123"}</param>
        /// <returns></returns>
        public List<ScoreModel> GetScores(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null)
        {
            var scores = MySqlHelper.Select<ScoreModel>(pageNumber, rowsPerPage, conditions, orderby, parameters);
            return scores?.ToList();
        }

        /// <summary>
        /// 通过订单编号获取积分列表
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="scoreSourceType">积分来源类型</param>
        /// <returns></returns>
        public async Task<List<ScoreModel>> GetScoresByOrderGuid(string orderId, string scoreSourceType)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            b.*
                            FROM
	                            t_consumer_score_record a
	                            INNER JOIN t_utility_score b ON a.score_record_guid = b.score_record_guid 
                            WHERE
	                            a.order_guid = @orderId 
	                            AND a.score_source_type = @scoreSourceType 
	                            AND b.score_lock = 1 
	                            AND a.`enable` = 1 
	                            AND b.`enable` =1";
                return (await conn.QueryAsync<ScoreModel>(sql, new { orderId, scoreSourceType }))?.ToList();
            }
        }

        #endregion

        #region 修改

        /// <summary>
        /// 新增积分记录
        /// </summary>
        /// <param name="scoreModel"></param>
        /// <returns>是否成功</returns>
        public bool InsertScore(ScoreModel scoreModel)
        {
            return !string.IsNullOrEmpty(scoreModel.Insert());
        }

        /// <summary>
        /// 批量更新model
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<bool> Update(List<ScoreModel> models)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                foreach (var item in models)
                {
                    if ((await conn.UpdateAsync(item)) != 1) return false;
                }

                return true;
            });
        }

        #endregion
    }
}
