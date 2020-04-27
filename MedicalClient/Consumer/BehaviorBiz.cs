using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GD.DataAccess;
using GD.Dtos.Mall.Mall;
using GD.Models.Consumer;

namespace GD.Consumer
{
    /// <summary>
    /// 用户行为Biz
    /// </summary>
    public class BehaviorBiz
    {
        /// <summary>
        /// 计算用户浏览数
        /// </summary>
        /// <param name="targetGuid">=</param>
        /// <param name="enable">=</param>
        /// <returns></returns>
        public int GetViewNumByTargetGuid(string targetGuid, bool enable = true)
        {
            var strWhere = " where target_guid=@targetGuid and enable=@enable";
            var behaviorNum = MySqlHelper.Count<BehaviorModel>(strWhere, new { targetGuid, enable });
            return behaviorNum;
        }

        /// <summary>
        /// 计算用户浏览数(异步)
        /// </summary>
        /// <param name="targetGuid">=</param>
        /// <param name="enable">=</param>
        /// <returns></returns>
        public async Task<int> GetViewNumByTargetGuidAsync(string targetGuid, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select count(*) from t_consumer_article_view where target_guid=@targetGuid and enable=@enable";
                var likerNum = await conn.QueryFirstOrDefaultAsync<int>(sql, new { targetGuid, enable });
                return likerNum;
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add(BehaviorModel model)
        {
            return model.Insert();

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? Delete(BehaviorModel model)
        {
            return model.Delete();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int? Update(BehaviorModel model)
        {
            model.LastUpdatedDate = DateTime.Now;
            return model.Update();
        }

        /// <summary>
        /// 分页查询-用户行为记录
        /// </summary>
        /// <param name="requestDto">=</param>
        /// <param name="userId">=</param>
        /// <param name="enable">=</param>
        /// <returns></returns>
        public List<BehaviorModel> GetBehaviorRecordByBehaviorTypeAndUserGuid(GetFirstPageRecommendProductListRequestDto requestDto, string userId, bool enable = true)
        {
            var sql = @"SELECT * FROM t_consumer_behavior 
                                WHERE
	                            user_guid = @userId 
	                            AND behavior_type = @behaviorType 
	                            AND `enable` = @enable
	                            LIMIT  @pageIndex, @pageSize";
            var modelList = MySqlHelper.Select<BehaviorModel>(sql, new { userId,
                requestDto.BehaviorType,
                enable,
                pageIndex = (requestDto.PageIndex - 1) * requestDto.PageSize,
                pageSize = requestDto.PageSize,
            }).ToList();

            return modelList;
        }
    }
}
