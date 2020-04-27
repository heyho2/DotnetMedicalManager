using GD.Common.EnumDefine;
using GD.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using GD.Models.Utility;
using System.Linq;
using GD.Dtos.FAQs.FAQsIntergral;
using static GD.Dtos.FAQs.FAQsIntergral.GetUserSignInDataCurrentMonthResponse;

namespace GD.Module
{
    public class ScoreExBiz
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
        public bool CheckSignInScores(string userId, string userType = "Consumer")
        {
            var sql = "select * from t_utility_score where creation_date>timestamp(DATE_FORMAT(now(), '%Y-%m-%d')) and user_guid=@userId and `enable`=1 and user_type_guid=@userType  and reason='签到积分'";
            var model = MySqlHelper.Select<ScoreModel>(sql, new { userId, userType = userType.ToString() });
            return model != null && model.Any();
        }



        /// <summary>
        /// 获取用户积分变化记录集合
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public List<ScoreModel> GetScores(string userId, UserType userType = UserType.Consumer)
        {
            var sql = "select * from t_utility_score where user_guid=@userId  and enable=@enable and user_type_guid=@userType ";
            var characterModels = MySqlHelper.Select<ScoreModel>(sql, new { userId, userType = userType.ToString() });
            return characterModels?.ToList();
        }

        /// <summary>
        /// 获取用户对应角色的总积分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="platformType"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<int> GetTotalScore(string userId, UserType userType = UserType.Consumer)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"select IFNULL(sum(variation),0) from t_utility_score where user_guid=@userId  and `enable`=1 and user_type_guid=@userType";
                var sumScore = await conn.QueryFirstOrDefaultAsync<int?>(sql, new { userId, userType = userType.ToString() });
                return sumScore ?? 0;
            }
        }

        /// <summary>
        /// 获取用户指定平台今日总积分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="platformType"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<int> GetTotalDaySocre(string userId, UserType userType = UserType.Consumer)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"select IFNULL(sum(variation),0) from t_utility_score where user_guid=@userId and  creation_date>timestamp(DATE_FORMAT(now(), '%Y-%m-%d')) and enable=1 and user_type_guid=@userType";
                var sumScore = await conn.QueryFirstOrDefaultAsync<int?>(sql, new { userId, userType = userType.ToString() });
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


        #region  积分任务(部分) 


        /// <summary>
        /// 判断用户指定角色今日是否签到
        /// </summary>
        /// <returns></returns>
        public bool CheckInSendIntergral(string userId, string userType = "Consumer")
        {
            var sql = "select * from t_utility_score where creation_date>timestamp(DATE_FORMAT(now(), '%Y-%m-%d')) and user_guid=@userId and `enable`=1 and user_type_guid=@userType  and reason='连续签到送积分'";
            var model = MySqlHelper.Select<ScoreModel>(sql, new { userId, userType = userType.ToString() });
            return model != null && model.Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="scoreSourceType">积分来源类型</param>
        /// <returns></returns>
        public async Task<ScoreModel> GetCheckInSendIntergralRecord(string userID, string reasonEnum, UserType userType = UserType.Consumer)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = @"WHERE
	                                                user_guid = @userID
	                                                AND reason = @reasonEnum
                                                    AND user_type_guid=@userType
                                                    AND `enable` = 1 
                                                ORDER BY
	                                                creation_date DESC 
	                                                LIMIT 1";
                return (await conn.GetListAsync<ScoreModel>(sqlWhere, new { userID, reasonEnum, userType = userType.ToString() })).FirstOrDefault();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="scoreSourceType">积分来源类型</param>
        /// <returns></returns>
        public async Task<ScoreModel> GetIntergralRecordByCondition(string userID, string reasonEnum)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = @"WHERE
	                                                user_guid = @userID
	                                                AND reason = @reasonEnum
                                                    AND `enable` = 1 
                                                ORDER BY
	                                                creation_date DESC  ";
                return (await conn.GetListAsync<ScoreModel>(sqlWhere, new { userID, reasonEnum })).FirstOrDefault();
            }

        }

        /// <summary>
        /// 获取今日某类型的记录
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="scoreSourceType">积分来源类型</param>
        /// <returns></returns>
        public async Task<List<ScoreModel>> GetToDaysIntergralRecordByCondition(string userID, string reasonEnum)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = @"WHERE
	                                                user_guid = @userID
	                                                AND reason = @reasonEnum
                                                    AND to_days( creation_date ) = to_days( now( ) ) 
                                                    AND  `enable` = 1 
                                                ORDER BY
	                                                creation_date DESC ";
                return (await conn.GetListAsync<ScoreModel>(sqlWhere, new { userID, reasonEnum }))?.ToList();
            }
        }

        /// <summary>
        /// 当月数据连续签到数据
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="scoreSourceType">积分来源类型</param>
        /// <returns></returns>
        public async Task<List<ScoreModel>> GetUserSignInDataCurrentMonth(GetUserSignInDataCurrentMonthRequest request)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = @"WHERE
	                                                user_guid = @userID
	                                                AND reason = '连续签到送积分'
                                                    AND date_format( creation_date, '%Y-%m' ) = date_format( DATE_SUB( curdate( ), INTERVAL 0 MONTH ), '%Y-%m' ) 
                                                    AND  `enable` = 1 
                                                ORDER BY
	                                                creation_date ASC ";
                return (await conn.GetListAsync<ScoreModel>(sqlWhere, new { request.UserID, request.UserType }))?.ToList();
            }
        }

        /// <summary>
        /// 当月数据连续签到数据
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="scoreSourceType">积分来源类型</param>
        /// <returns></returns>
        public async Task<List<CheckInInfo>> GetUserSignInDataCurrentMonthAllDays(GetUserSignInDataCurrentMonthRequest request, string dateSql)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = $@"SELECT
	                                                IFNULL( mm.IsCheckIn, 0 ) AS IsCheckIn,
	                                                IFNULL( mm.Intergral, 0 ) AS Intergral,
	                                                m.CheckInDate 
                                                FROM
	                                                (
                                                     {dateSql}
	                                                ) AS m
	                                                LEFT JOIN (
                                                SELECT
	                                                1 AS IsCheckIn,
	                                                variation AS Intergral,
	                                                date_format( creation_date, '%Y-%m-%d' ) AS CheckInDate 
                                                FROM
	                                                t_utility_score 
                                                WHERE
	                                                user_guid = @UserID
	                                                AND reason = '连续签到送积分' 
                                                    AND user_type_guid=@UserType
	                                                AND `enable` = 1 
                                                GROUP BY
	                                                CheckInDate,
	                                                Intergral,
	                                                IsCheckIn 
	                                                ) AS mm ON m.CheckInDate = mm.CheckInDate 
                                                ORDER BY
	                                                m.CheckInDate DESC ";
                return (await conn.QueryAsync<CheckInInfo>(sqlWhere, new { request.UserID, request.UserType }))?.ToList();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="scoreSourceType">积分来源类型</param>
        /// <returns></returns>
        public async Task<List<ScoreModel>> GetModelListByCondition(string userID, string userType, int limitNumber)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = @" WHERE
	                                                user_guid = @userID
	                                                AND reason = '连续签到送积分' 
                                                    AND user_type_guid = @userType
                                                    AND  `enable` = 1 
                                                ORDER BY
	                                                creation_date DESC  LIMIT @limitNumber ";
                return (await conn.GetListAsync<ScoreModel>(sqlWhere, new { userID, userType, limitNumber }))?.ToList();
            }
        }


        #endregion


    }
}
