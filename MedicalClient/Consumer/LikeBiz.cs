using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GD.DataAccess;
using GD.Models.Consumer;

namespace GD.Consumer
{
    public class LikeBiz
    {
        #region 修改
        /// <summary>
        /// 插入点赞记录
        /// </summary>
        /// <param name="model">点赞Model</param>
        /// <returns>是否成功</returns>
        public bool InsertModel(LikeModel model)
        {
            return !string.IsNullOrEmpty(model.Insert());
        }

        /// <summary>
        /// 更新点赞记录
        /// </summary>
        /// <param name="model">点赞Model</param>
        /// <returns></returns>
        public bool UpdateModel(LikeModel model)
        {
            model.LastUpdatedDate = DateTime.Now;
            return model.Update() == 1;
        }
        #endregion

        #region 查询
        /// <summary>
        /// 计算用户点赞数
        /// </summary>
        /// <param name="targetGuid">=</param>
        /// <param name="enable">=</param>
        /// <returns></returns>
        public int GetLikeNumByTargetGuid(string targetGuid, bool enable = true)
        {
            var strWhere = " where target_guid=@targetGuid and enable=@enable ";
            var likerNum = MySqlHelper.Count<LikeModel>(strWhere, new { targetGuid, enable });
            return likerNum;
        }

        /// <summary>
        /// 计算用户点赞数异步
        /// </summary>
        /// <param name="targetGuid">=</param>
        /// <param name="enable">=</param>
        /// <returns></returns>
        public async Task<int> GetLikeNumByTargetGuidAsync(string targetGuid, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var likerNum = await conn.QueryFirstOrDefaultAsync<int>("select count(*) from t_consumer_like where target_guid=@targetGuid and enable=@enable", new { targetGuid, enable });
                return likerNum;
            }
            
        }


        /// <summary>
        /// 判断用户是否点赞该目标
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="targetGuid">目标Guid</param>
        /// <returns>是否点赞</returns>
        public bool GetLikeState(string userGuid, string targetGuid, bool enable = true)
        {
            return MySqlHelper.Count<LikeModel>("where target_guid=@targetGuid and created_by=@userGuid and enable=@enable", new { userGuid, targetGuid, enable }) > 0;
        }
        /// <summary>
        /// 获取用户点赞目标记录
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="targetGuid">目标Guid</param>
        /// <returns></returns>
        public LikeModel GetTheLikeModelByUserId(string userGuid, string targetGuid, bool enable = true)
        {
            return MySqlHelper.SelectFirst<LikeModel>("select * from t_consumer_like where target_guid=@targetGuid and created_by=@userGuid and enable=@enable", new { userGuid, targetGuid, enable });
        }

        /// <summary>
        /// 获取用户点赞目标记录(忽略enable状态)
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <param name="targetGuid">目标Guid</param>
        /// <returns></returns>
        public LikeModel GetOneLikeModelByUserId(string userGuid, string targetGuid)
        {
            return MySqlHelper.SelectFirst<LikeModel>("select * from t_consumer_like where target_guid=@targetGuid and created_by=@userGuid", new { userGuid, targetGuid });
        }
        #endregion
    }
}
