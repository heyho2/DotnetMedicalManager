using Dapper;
using GD.DataAccess;
using GD.Dtos.Achievement;
using GD.Models.Achievement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Achievement
{
    public class AchievementMonthBiz : GD.BizBase.BaseBiz<AchievementMonthGoalModel>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<AchievementMonthGoalModel> GetAchievementMonthGoalModel(CreateMonthAchievementDto requestDto, string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT * FROM t_achievement_month_goal where `year`=@year and `month`=@month and hospital_guid=@userId ";
                var achievementMonthGoalModel = await conn.QueryFirstOrDefaultAsync<AchievementMonthGoalModel>(sql, new { year = requestDto.Date.Year, month = requestDto.Date.Month, userId = userId });
                return achievementMonthGoalModel;
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetMonthAchievementDto>> GetMonthAchievementDtoAsync(string userID)
        {
            var sql = @"SELECT
	                    a.goal_guid,
	                    a.`year`,
	                    a.`month` 
                    FROM
	                    t_achievement_month_goal a where a.hospital_guid=@userID and a.`enable`=1
                    ORDER BY
	                    a.`year` asc,
	                    a.`month` asc,
	                    a.goal_guid ";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetMonthAchievementDto>(sql, new { userID = userID });
                return result.ToList();
            }
        }
        /// <summary>
        /// 根据月目标获取对应数据
        /// </summary>
        /// <param name="goalGuid"></param>
        /// <returns></returns>
        public async Task<List<AchievementDayDto>> GetAchievementDtoAsync(string goalGuid)
        {
            var sql = @"	
                        SELECT
	                        a.achievement_guid,
	                        a.achievement_date,
	                        a.today_goal,
	                        a.today_complete 
                        FROM
	                        t_achievement a 
                        WHERE
	                        a.goal_guid = @goalGuid and a.`enable`=1
                        ORDER BY
	                        a.achievement_date ";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<AchievementDayDto>(sql, new { goalGuid });
                return result.ToList();
            }
        }
        //public async Task<bool> CreateAchievementMonthAsync(ConsumerIndicatorModel consumerIndicatorModel, List<ConsumerIndicatorDetailModel> consumerIndicatorDetailModelList)
        //{
        //    return await MySqlHelper.TransactionAsync(async (conn, trans) =>
        //    {
        //        //保存用户健康指标
        //        await conn.InsertAsync<string, ConsumerIndicatorModel>(consumerIndicatorModel);
        //        //保存用户健康明细数据
        //        consumerIndicatorDetailModelList.InsertBatch(conn);
        //        return true;
        //    });
        //}
    }
}
