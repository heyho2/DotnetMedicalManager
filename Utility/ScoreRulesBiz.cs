using System;
using System.Threading.Tasks;
using Dapper;
using GD.Common.EnumDefine;
using GD.Common.Helper;
using GD.Models.Utility;
using GD.DataAccess;
using GD.Models.Utility;

namespace GD.Utility
{
    /// <summary>
    /// 积分规则业务类
    /// </summary>
    public class ScoreRulesBiz
    {
        /// <summary>
        /// 获取积分规则model
        /// </summary>
        /// <param name="rulesGuid">规则guid</param>
        /// <returns></returns>
        public ScoreRulesModel GetModel(string guid)
        {
            return MySqlHelper.GetModelById<ScoreRulesModel>(guid);
        }

        /// <summary>
        /// 使用用户类型和操作类型查询积分规则model
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="userType">用户类型</param>
        /// <returns></returns>
        public async Task<ScoreRulesModel> GetModelByUserAction(string userActionGuid)
        {
            string sqlstring = "select * from t_utility_score_rules where user_action_guid=@user_action_guid";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("user_action_guid", userActionGuid);
            using (MySql.Data.MySqlClient.MySqlConnection conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstAsync<ScoreRulesModel>(sqlstring, parameters);
            }
        }

        /// <summary>
        /// 按照积分规则添加积分
        /// </summary>
        /// <returns></returns>
        public async Task<string> AddScoreByRules(string userGuid, ActionEnum operateType, UserType userType)
        {
            try
            {
                //获取用户行为model
                UserActionCharacteristics userAction = await new UserActionCharacteristicsBiz().GetOperateModel(userType, operateType);
                //用户无当前行为
                if (userAction == null)
                {
                    return string.Empty;
                }
                //获取积分规则数据
                ScoreRulesModel rulesModel = await GetModelByUserAction(userAction.UserActionGuid);
                DateTime? startdate = null;
                DateTime? enddate = null;
                //积分为0不进行操作
                if (rulesModel.Variation == 0)
                {
                    return string.Empty;
                }
                //使用次数为0,为无限制次数
                if (rulesModel.UsageCount != 0)
                {
                    //根据规则获取时间段
                    GetStartAndEndDate(rulesModel.RulesType, ref startdate, ref enddate);
                    //获取时间段内积分数量
                    int scoreCount = GetScoreCount(userGuid, rulesModel.RulesGuid, startdate, enddate);
                    if (!(scoreCount < rulesModel.UsageCount))
                    {
                        return string.Empty;
                    }
                }
                var model = new ScoreModel
                {
                    ScoreGuid = Guid.NewGuid().ToString("N"),
                    Variation = rulesModel.Variation,
                    RulesGuid = rulesModel.RulesGuid,
                    Reason = userAction.ActionCharacteristicsName,
                    UserGuid = userGuid,
                    UserTypeGuid = userAction.UserTypeGuid,
                    OrgGuid = "",
                    CreatedBy = userGuid,
                    LastUpdatedBy = userGuid
                };
                return new ScoreBiz().InsertScore(model) ? model.ScoreGuid : string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Error($"userGuid:{userGuid}   rulesType:{operateType.ToString()}   message:{ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据规则返回时间
        /// </summary>
        /// <param name="rulesType">规则类型</param>
        /// <param name="startdate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        private void GetStartAndEndDate(ScoreRulesCycle rulesType, ref DateTime? startdate, ref DateTime? enddate)
        {
            switch (rulesType)
            {
                case ScoreRulesCycle.Year:
                    startdate = new DateTime(DateTime.Now.Year, 1, 1);
                    enddate = startdate.Value.AddYears(1);
                    break;
                case ScoreRulesCycle.Month:
                    startdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    enddate = startdate.Value.AddMonths(1);
                    break;
                case ScoreRulesCycle.Day:
                    startdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    enddate = startdate.Value.AddDays(1);
                    break;
                case ScoreRulesCycle.Week:
                    startdate = GetMondayDate(DateTime.Now);
                    enddate = GetSundayDate(DateTime.Now);
                    break;
                case ScoreRulesCycle.Unlimited:
                default:
                    break;
            }
        }

        /// <summary>
        /// 根据时间查询用户在某个规则下的积分数据数量
        /// </summary>
        /// <param name="userGuid">用户guid</param>
        /// <param name="rulesGuid">规则guid</param>
        /// <param name="stardate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <returns></returns>
        private int GetScoreCount(string userGuid, string rulesGuid, DateTime? stardate, DateTime? enddate)
        {
            string sqlstring = "where user_guid=@user_guid and rules_guid=@rules_guid";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("user_guid", userGuid, System.Data.DbType.String);
            parameters.Add("rules_guid", rulesGuid, System.Data.DbType.String);
            if (stardate != null && enddate != null)
            {
                sqlstring = sqlstring + " and creation_date>=@start_date and creation_date<@end_date";
                parameters.Add("start_date", stardate, System.Data.DbType.DateTime);
                parameters.Add("end_date", enddate, System.Data.DbType.DateTime);
            }
            return MySqlHelper.Count<ScoreModel>(sqlstring, parameters);
        }

        /// <summary> 
        /// 计算某日起始日期（礼拜一的日期） 
        /// </summary> 
        /// <param name="someDate">该周中任意一天</param> 
        /// <returns>返回礼拜一日期，后面的具体时、分、秒清零</returns> 
        private DateTime GetMondayDate(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;// i值 > = 0 ，因为枚举原因，Sunday排在最前，此时Sunday-Monday=-1，必须+7=6。 
            TimeSpan ts = new TimeSpan(i, someDate.Hour, someDate.Minute, someDate.Second, someDate.Millisecond);
            return someDate.Subtract(ts);
        }

        /// <summary> 
        /// 计算某日结束日期（礼拜日的日期） 
        /// </summary> 
        /// <param name="someDate">该周中任意一天</param> 
        /// <returns>返回礼拜日日期，后面的具体时、分、秒改成最后1毫秒</returns> 
        private DateTime GetSundayDate(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Sunday;
            if (i != 0) i = 7 - i;// 因为枚举原因，Sunday排在最前，相减间隔要被7减。 
            TimeSpan ts = new TimeSpan(i, (23 - someDate.Hour), (59 - someDate.Minute), (59 - someDate.Second), (999 - someDate.Millisecond));
            return someDate.Add(ts);
        }
    }
}