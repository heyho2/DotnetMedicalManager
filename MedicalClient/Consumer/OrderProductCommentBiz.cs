using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Consumer.Consumer;
using GD.Models.Consumer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Consumer
{
    /// <summary>
    /// 订单商品评价业务类
    /// </summary>
    public class OrderProductCommentBiz : BaseBiz<OrderProductCommentModel>
    {
        /// <summary>
        /// 通过订单guid获取评价记录集合
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<List<OrderProductCommentModel>> GetModelsByOrderGuidAsync(string orderId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<OrderProductCommentModel>("where order_guid=@orderId and enable=1", new { orderId });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 订单详情Id
        /// </summary>
        /// <param name="orderDetailGuids"></param>
        /// <returns></returns>
        public async Task<List<OrderProductCommentModel>> GetModelsByOrderDetailGuidsAsync(string[] orderDetailGuids, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<OrderProductCommentModel>("where order_detail_guid in @orderDetailGuids and enable=@enable", new { orderDetailGuids, enable });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 获取用户订单商品评价列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetUserOrderProductCommentPageListResponseDto> GetUserOrderProductCommentPageListAsync(GetUserOrderProductCommentPageListRequestDto requestDto)
        {
            var sql = @"SELECT
	                        a.product_comment_guid,
	                        a.product_guid,
	                        a.product_name,
	                        b.creation_date AS order_date,
	                        c.merchant_name,
	                        a.comment_status,
	                        a.comment_guid,
	                        d.content,
	                        d.score,
	                        d.anonymous,
	                        d.creation_date AS comment_date ,
	                        CONCAT(acc.base_path,acc.relative_path) as product_picture
                        FROM
	                        t_consumer_order_product_comment a
	                        INNER JOIN t_mall_order b ON a.order_guid = b.order_guid
	                        INNER JOIN t_merchant c ON c.merchant_guid = b.merchant_guid
	                        LEFT JOIN t_consumer_comment d ON d.comment_guid = a.comment_guid 
	                        left join t_mall_product pro on pro.product_guid=a.product_guid
	                        left join t_utility_accessory acc on acc.accessory_guid=pro.picture_guid
                        WHERE
	                        a.user_guid = @UserGuid 
	                        AND a.`enable` = 1 
                        ORDER BY
	                        a.comment_status,
	                        a.creation_date DESC";
            return await MySqlHelper.QueryByPageAsync<GetUserOrderProductCommentPageListRequestDto, GetUserOrderProductCommentPageListResponseDto, GetUserOrderProductCommentPageListItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 评价订单商品
        /// </summary>
        /// <param name="model"></param>
        /// <param name="commentModel"></param>
        /// <returns></returns>
        public async Task<bool> CommentOrderProductAsync(OrderProductCommentModel model, CommentModel commentModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                await conn.UpdateAsync(model);
                await conn.InsertAsync<string, CommentModel>(commentModel);
                return true;

            });
        }
    }
}