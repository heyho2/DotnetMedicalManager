using Dapper;
using GD.Common.EnumDefine;
using GD.Common.Helper;
using GD.DataAccess;
using GD.Dtos.Utility.Utility;
using GD.Models.Utility;
using GD.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Doctor
{
    /// <summary>
    /// 医生行为Biz
    /// </summary>
    public class DoctorActionBiz
    {
        /// <summary>
        /// 医生注册
        /// </summary>
        /// <param name="userID">userID</param>
        public async void RegisterDoctor(string userID)
        {
            await new ScoreRulesBiz().AddScoreByRules(userID, ActionEnum.Registered, UserType.Doctor);
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="doctorGuid">医生GUID</param>
        public async void AddArticleAsync(string doctorGuid)
        {
            await new ScoreRulesBiz().AddScoreByRules(doctorGuid, ActionEnum.PublishArticle, UserType.Doctor);
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="doctorGuid">医生GUID</param>
        public async void DeleteArticleAsync(string doctorGuid)
        {
            await new ScoreRulesBiz().AddScoreByRules(doctorGuid, ActionEnum.DeleteArticle, UserType.Doctor);
        }

        /// <summary>
        /// 获取TopicModel
        /// </summary>
        /// <param name="topicGuid">topicGuid</param>
        /// <returns></returns>
        private TopicModel GetTopicModel(string topicGuid)
        {
            string sqlstring = "select * from t_utility_topic where topic_guid=@topic_guid";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("topic_guid", topicGuid, System.Data.DbType.String);
            using (MySql.Data.MySqlClient.MySqlConnection conn = MySqlHelper.GetConnection())
            {
                return conn.QueryFirst<TopicModel>(sqlstring, parameters);
            }
        }

        /// <summary>
        /// 查询日期内是否有发送过消息
        /// </summary>
        /// <param name="toGuid">接收者GUID</param>
        /// <param name="fromGuid">发送者GUID</param>
        /// <param name="date">时间</param>
        /// <returns>true 发送过消息,false 没有发送过消息</returns>
        private bool WhetherToChatByDay(string toGuid, string fromGuid, DateTime date)
        {
            string sqlstring = "where from_guid=@from_guid and to_guid=@to_guid and creation_date>@start_date and creation_date<@end_date";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("from_guid", fromGuid, System.Data.DbType.String);
            parameters.Add("to_guid", toGuid, System.Data.DbType.String);
            parameters.Add("start_date", new DateTime(date.Year, date.Month, date.Day), System.Data.DbType.DateTime);
            parameters.Add("end_date", new DateTime(date.Year, date.Month, date.Day, 23, 59, 59), System.Data.DbType.DateTime);
            using (MySql.Data.MySqlClient.MySqlConnection conn = MySqlHelper.GetConnection())
            {
                return conn.RecordCount<MessageModel>(sqlstring, parameters) > 0;
            }
        }
    }
}