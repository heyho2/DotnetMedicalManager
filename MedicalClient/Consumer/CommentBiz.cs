using Dapper;
using GD.DataAccess;
using GD.Models.Consumer;
using GD.Models.Mall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GD.Dtos.Consumer.Consumer;
using GD.Dtos.Consumer.Comment;

namespace GD.Consumer
{
    public class CommentBiz
    {
        /// <summary>
        /// 获取评论Model
        /// </summary>
        /// <param name="guid">主键guid</param>
        /// <returns></returns>
        public CommentModel GetModel(string guid)
        {
            return MySqlHelper.GetModelById<CommentModel>(guid);
        }

        /// <summary>
        /// 异步获取唯一实例
        /// </summary>
        /// <param name="guid">评论guid</param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<CommentModel> GetAsync(string guid, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select * from t_consumer_comment where comment_guid=@guid and enable=@enable ";
                return await conn.QueryFirstOrDefaultAsync<CommentModel>(sql, new { guid, enable });
            }
        }

        /// <summary>
        /// 由评论目标获取Model集合
        /// </summary>
        /// <param name="targetGuid">目标Guid</param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public List<CommentModel> GetModelsByTargetGuid(string targetGuid, bool enable = true)
        {
            var sql = "select * from t_consumer_comment where target_guid=@targetGuid and enable=@enable order by last_updated_date asc";
            return MySqlHelper.Select<CommentModel>(sql, new { targetGuid, enable }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">单页条数</param>
        /// <param name="strWhere">条件 where condition=@condition</param>
        /// <param name="orderBy">排序 col desc</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<CommentModel> GetModels(int pageIndex, int pageSize, string strWhere, string orderBy, object parameters = null)
        {
            return MySqlHelper.Select<CommentModel>(pageIndex, pageSize, strWhere, orderBy, parameters).ToList();
        }

        /// <summary>
        /// 获取商铺下商品的所有评论(分页)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetProductCommentsOfMerchantResponseDto> GetProductCommentsOfMerchantAsync(GetProductCommentsOfMerchantRequestDto requestDto)
        {
            var productNameWhere = "";
            if (!string.IsNullOrWhiteSpace(requestDto.ProductName))
            {
                productNameWhere = $"AND a.product_name LIKE '%{requestDto.ProductName}%'";
            }
            var receiveStatusWhere = "";
            if (requestDto.ReceiveStatus != ReceiveStatusEnum.All)
            {
                receiveStatusWhere = $"HAVING MAX( receive.comment_guid ) is {(requestDto.ReceiveStatus == ReceiveStatusEnum.Replied ? "not" : "")} null";
            }
            var sql = $@"SELECT
	                        b.comment_guid AS CommentGuid,
	                        a.product_guid AS ProductGuid,
	                        a.product_name AS ProductName,
	                        c.user_guid AS UserGuid,
	                        c.nick_name AS UserNickName,
	                        b.score AS Score,
	                        b.creation_date AS CommentDate,
	                        b.content AS CommentContent
                        FROM
	                        t_mall_product a
	                        LEFT JOIN t_consumer_comment b ON a.product_guid = b.target_guid 
	                        AND b.`enable` = 1
	                        LEFT JOIN t_utility_user c ON b.created_by = c.user_guid 
	                        AND c.`enable` = 1
	                        LEFT JOIN t_consumer_comment receive ON receive.target_guid = b.comment_guid 
	                        AND receive.`enable` = 1 and receive.created_by='{requestDto.MerchantGuid}'
                        WHERE
	                        a.merchant_guid = '{requestDto.MerchantGuid}' 
	                        AND a.`enable` = 1 
	                        AND b.creation_date BETWEEN '{requestDto.StartDate.ToString("yyyy-MM-dd HH:mm:ss")}' 
	                        AND '{requestDto.EndDate.ToString("yyyy-MM-dd HH:mm:ss")}' { productNameWhere } 
                        GROUP BY
	                        b.comment_guid,
	                        a.product_guid,
	                        a.product_name,
	                        c.user_guid,
	                        c.nick_name,
	                        b.score,
	                        b.creation_date,
	                        b.content 
                        {receiveStatusWhere}
                        ORDER BY
	                        b.creation_date DESC ";
            return await MySqlHelper.QueryByPageAsync<GetProductCommentsOfMerchantRequestDto, GetProductCommentsOfMerchantResponseDto, GetProductCommentsOfMerchantItemDto>(sql, requestDto);

        }


        /// <summary>
        /// 新增评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(CommentModel model)
        {
            model.LastUpdatedDate = DateTime.Now;
            return !string.IsNullOrWhiteSpace(model.Insert());
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<bool> AddRangeAsync(List<CommentModel> models)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                foreach (var item in models)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, CommentModel>(item)))
                    {
                        return false;
                    }
                }
                return true;

            }
        }

        /// <summary>
        /// 从订单明细评价商品
        /// </summary>
        /// <param name="model"></param>
        /// <param name="detailModel"></param>
        /// <returns></returns>
        public async Task<bool> CommentProductFromOrderDetailAsync(CommentModel model, OrderDetailModel detailModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                if (string.IsNullOrEmpty(await conn.InsertAsync<string, CommentModel>(model))) return false;
                if (await conn.UpdateAsync(detailModel) != 1) return false;
                return true;
            });
        }
        /// <summary>
        ///  分页--获取双美全部评价
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<GetAllCLEvaluateResponseDto>> GetAllCLEvaluateAsync(GetAllCLEvaluateRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                                com.created_by AS userGuid,
	                                u.nick_name,
	                                CONCAT( acc.base_path, acc.relative_path ) AS PortraitUrl,
	                                com.creation_date AS CommentDate,
	                                com.Content,
	                                CONCAT( accp.base_path, accp.relative_path ) AS PictureUrl,
	                                pro.product_name AS TargetName,
	                                com.Score 
                                FROM
	                                t_consumer_comment AS com
	                                LEFT JOIN t_utility_user AS u ON com.created_by = u.user_guid
	                                LEFT JOIN t_utility_accessory AS acc ON u.portrait_guid = acc.accessory_guid
	                                LEFT JOIN t_mall_product AS pro ON com.target_guid = pro.product_guid
	                                LEFT JOIN t_utility_accessory AS accp ON pro.picture_guid = acc.accessory_guid 
                                WHERE
	                                com.ENABLE = @ENABLE 
	                                AND com.target_guid = @targetGuid 
                                    Order by com.sort  ";
                var result = await conn.QueryAsync<GetAllCLEvaluateResponseDto>(sql, new { requestDto.Enable, requestDto.TargetGuid });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 统计用户某一天对目标进行评价的次数
        /// </summary>
        /// <param name="userGuid">用户guid</param>
        /// <param name="targetGuid">目标guid</param>
        /// <param name="theDate">日期</param>
        /// <returns></returns>
        public async Task<int> CountUserHasCommentTargetOneDayAsync(string userGuid, string targetGuid, DateTime theDate)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $"where created_by=@userGuid and target_guid=@targetGuid and creation_date BETWEEN '{theDate.ToString("yyyy-MM-dd") + " 00:00:00"}' and '{theDate.ToString("yyyy-MM-dd") + " 23:59:59"}'";
                var count = await conn.RecordCountAsync<CommentModel>(sql, new { userGuid, targetGuid });
                return count;
            }
        }

        /// <summary>
        /// 获取目标的平均评论分数
        /// </summary>
        /// <param name="targetGuid">目标guid</param>
        /// <returns>目标的平均评论分数</returns>
        public async Task<decimal> GetTargetAvgScoreAsync(string targetGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<decimal>("select IFNULL(AVG(score),5) from t_consumer_comment where target_guid=@targetGuid and `enable`=1", new { targetGuid });
            }
        }

        /// <summary>
        /// 获取目标评论分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetTargetCommentPageListResponseDto> GetTargetCommentPageListAsync(GetTargetCommentPageListRequestDto requestDto)
        {
            var sql = @"SELECT
                          a.comment_guid,
	                        a.content,
	                        a.score,
	                        a.creation_date,
	                        b.nick_name ,
                            ifnull(hot.like_count,0) AS total_like,
	                        CONCAT(c.base_path,c.relative_path) as portrait,
                            CASE
		                        WHEN l.like_guid IS NULL THEN 0 
		                        ELSE 1 
	                        END AS is_like ,
	                        count(distinct son.comment_guid) as total_reply
                        FROM
	                        t_consumer_comment a
	                        LEFT JOIN t_utility_user b ON a.created_by = b.user_guid
	                        left join t_utility_accessory c on c.accessory_guid=b.portrait_guid
	                        left join t_consumer_comment son on son.target_guid=a.comment_guid
	                        left join t_utility_hot hot ON hot.owner_guid = a.comment_guid
                                AND hot.`enable` = 1
                            LEFT JOIN t_consumer_like l ON l.target_guid = a.comment_guid 
	                        AND l.created_by = @UserId 
	                        AND l.`enable` = 1 
	                    where a.`enable`=1 and a.target_guid=@TargetGuid
                        group by 
	                        a.comment_guid,
	                        a.content,
	                        a.score,
	                        a.creation_date,
	                        b.nick_name ,
                            total_like,
	                        portrait,
                            is_like
                        ORDER BY a.creation_date";
            return await MySqlHelper.QueryByPageAsync<GetTargetCommentPageListRequestDto, GetTargetCommentPageListResponseDto, GetTargetCommentPageListItemDto>(sql, requestDto);
        }


    }
}
