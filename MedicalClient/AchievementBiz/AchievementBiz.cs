using Dapper;
using GD.DataAccess;
using GD.Models.Achievement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Achievement
{
    public class AchievementBiz : GD.BizBase.BaseBiz<AchievementModel>
    {
        /// <summary>
        /// 根据日期查询当天数据是否存在
        /// </summary>
        /// <param name="achievementDate"></param>
        /// <returns></returns>
        public async Task<AchievementModel> GetAchievementModel(DateTime achievementDate, string achievementMonthId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"	SELECT * from t_achievement a  WHERE a.achievement_date=@achievementDate and a.goal_guid=@GoalId and a.`enable`=1";
                var achievementModel = await conn.QueryFirstOrDefaultAsync<AchievementModel>(sql, new { achievementDate = achievementDate.ToString("yyyy-MM-dd"), GoalId = achievementMonthId });
                return achievementModel;
            }
        }
    }
}
