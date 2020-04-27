using Dapper;
using GD.Common.EnumDefine;
using GD.DataAccess;
using GD.Dtos.Member;
using GD.Models.Utility;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager.Utility
{
    /// <summary>
    /// 用户模块业务类
    /// </summary>
    public class UserBiz : BaseBiz<UserModel>
    {
        /// <summary>
        /// 会员列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetMemberPageResponseDto> GetMemberPageAsync(GetMemberPageRequestDto request)
        {
            var sqlWhere = $@"1 = 1 and enable=1";
            if (request.BeginDate != null)
            {
                request.BeginDate = request.BeginDate?.Date;
                sqlWhere = $"{sqlWhere} AND creation_date > @BeginDate";
            }
            if (request.EndDate != null)
            {
                request.EndDate = request.EndDate?.AddDays(1).Date;
                sqlWhere = $"{sqlWhere} AND creation_date < @EndDate";
            }
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                sqlWhere = $"{sqlWhere} AND (nick_name like @Name or user_name like @Name)";
            }
            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                sqlWhere = $"{sqlWhere} AND phone like @phone";
            }
            var sqlOrderBy = "last_buy_date desc";
            var sql = $@"
SELECT * FROM(
    SELECT
	    a.*,
	    b.order_qty,
	    b.order_total_amount,
	    b.order_average,
	    b.last_buy_date,
        c.variation
    FROM
	    t_utility_user a
	    LEFT JOIN (
	        SELECT
		        user_guid,
		        count( 1 ) AS order_qty,
		        SUM( IFNULL( paid_amount, 0 ) ) AS order_total_amount,
		        AVG( paid_amount ) AS order_average,
		        MAX( creation_date ) AS last_buy_date 
	        FROM
		        t_mall_order 
	        WHERE
		        order_status IN ( 'Shipped', 'Received', 'Completed' ) and enable=1
	        GROUP BY
		        user_guid 
	    ) b ON a.user_guid = b.user_guid
        LEFT JOIN (SELECT user_guid,SUM(variation) variation FROM t_utility_score WHERE `enable`=1 AND user_type_guid='{UserType.Consumer}' GROUP BY user_guid ) c ON a.user_guid = c.user_guid
)___T
 WHERE
	{sqlWhere}
ORDER BY
	{sqlOrderBy}";
            request.Name = $"%{request.Name}%";
            request.Phone = $"%{request.Phone}%";
            return await MySqlHelper.QueryByPageAsync<GetMemberPageRequestDto, GetMemberPageResponseDto, GetMemberPageItemDto>(sql, request);
        }
        public async Task<dynamic> GetConsumerAsync(string userGuid)
        {
            var sql = $@"
SELECT
	user_guid,
	count( 1 ) AS OrderQty,
	SUM( IFNULL( paid_amount, 0 ) ) AS OrderTotalAmount,
	AVG( paid_amount ) AS OrderAverage,
	MAX( creation_date ) AS LastBuyDate 
FROM
	t_mall_order 
WHERE
	order_status IN ( 'Shipped', 'Received', 'Completed' ) and user_Guid=@userGuid and enable=1
GROUP BY
	user_guid 
";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync(sql, new { userGuid });
                return result;
            }
        }


        public async Task<GetMemberOrderPageResponseDto> GetMemberOrderPageAsync(GetMemberOrderPageRequestDto request)
        {
            var sqlWhere = $@"1 = 1 and enable=1 and user_Guid=@UserGuid";
            var sqlOrderBy = "creation_date desc";
            var sql = $@"
SELECT * FROM(
   SELECT
	    a.*,
	    b.merchant_name 
    FROM
	    t_mall_order a
	    LEFT JOIN t_merchant b ON a.merchant_guid = b.merchant_guid
)___T
 WHERE
	{sqlWhere}
ORDER BY
	{sqlOrderBy}";
            return await MySqlHelper.QueryByPageAsync<GetMemberOrderPageRequestDto, GetMemberOrderPageResponseDto, GetMemberOrderPageItemDto>(sql, request);
        }
        public async Task<GetOrderDetailPageResponseDto> GetOrderDetailPageAsync(GetOrderDetailPageRequestDto request)
        {
            var sqlWhere = $@"1 = 1 and enable=1 and order_Guid=@OrderGuid";
            var sqlOrderBy = "creation_date desc";
            var sql = $@"
SELECT * FROM(
   SELECT
	    a.* ,
	    b.category_name
    FROM
	    t_mall_order_detail a
	    LEFT JOIN t_mall_product b ON a.product_guid = b.product_guid
)___T
 WHERE
	{sqlWhere}
ORDER BY
	{sqlOrderBy}";
            return await MySqlHelper.QueryByPageAsync<GetOrderDetailPageRequestDto, GetOrderDetailPageResponseDto, GetOrderDetailPageItemDto>(sql, request);
        }

        public async Task<UserModel> GetByPnoneAsync(string phone)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<UserModel>("where phone=@phone", new { phone });
                return result.FirstOrDefault();
            }
        }
    }
}
