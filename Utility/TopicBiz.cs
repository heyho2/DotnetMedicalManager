using System.Collections.Generic;
using System.Linq;
using GD.Common.Base;
using GD.DataAccess;
using GD.Models.Utility;

namespace GD.Utility
{
    /// <summary>
    ///  主题业务
    /// </summary>
    public class TopicBiz
    {
        /// <summary>
        /// 按ID查询
        /// </summary>
        /// <param name="topicGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public TopicModel GetModelById(string topicGuid, bool enable = true)
        {
            const string sql = "select * from t_utility_topic  where topic_guid=@topicGuid and enable=@enable";
            return MySqlHelper.SelectFirst<TopicModel>(sql, new { topicGuid, enable });
        }

        /// <summary>
        /// 按targetGuid查询
        /// </summary>
        /// <param name="aboutGuid">话题关于guid(如来自商品或直接问的医生）</param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public List<TopicModel> GetListByTarget(string aboutGuid, bool enable = true)
        {
            const string sql = "select * from t_utility_topic where about_guid=@aboutGuid and enable=@enable ";
            return MySqlHelper.Select<TopicModel>(sql, new { aboutGuid, enable }).ToList();
        }

        /// <summary>
        /// 按UserId分页查询
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public List<TopicModel> GetListByUserId(string userId, BasePageRequestDto page, bool enable = true)
        {
            const string sql = @"SELECT  * FROM  t_utility_topic
                                                  WHERE  (sponsor_guid =@userId OR receiver_guid =@userId)
                                                  AND ENABLE = @enable
                                                  ORDER BY  creation_date desc  LIMIT @pageIndex, @pageSize ";
            return MySqlHelper.Select<TopicModel>(sql, new { userId, enable, pageIndex = (page.PageIndex - 1) * page.PageSize, pageSize = page.PageSize }).ToList();
        }

        /// <summary>
        /// 批量新增Topic记录
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public bool AddModelList(List<TopicModel> modelList)
        {
            const string sql = @"
INSERT INTO t_utility_topic
VALUES
	(
		@TopicGuid,
		@SponsorGuid,
		@ReceiverGuid,
		@AboutGuid,
		@EnumTb,
		@BeginDate,
		@EndDate,
		@CreatedBy,
		@CreationDate,
		@LastUpdatedBy,
		@LastUpdatedDate,
		@OrgGuid,
	@ENABLE 
	)";
            var conn = Dapper.SqlMapper.Execute(MySqlHelper.GetConnection(), sql, modelList);
            return conn > 0;
        }

        /// <summary>
        /// 新增Topic记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns>新增ID</returns>
        public string Add(TopicModel model)
        {
            return model.Insert();
        }
    }
}
