using Dapper;
using GD.Common.Base;
using GD.DataAccess;
using GD.Dtos.Merchant;
using GD.Dtos.Merchant.MerchantFlowing;
using GD.Models.Merchant;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Merchant
{
    /// <summary>
    /// 商户流水逻辑处理类
    /// </summary>
    public class MerchantFlowingBiz
    {
        /// <summary>
        /// 根据交易流水GUID查询商户交易流水数据
        /// </summary>
        /// <param name="transactionFlowingGuid">商户交易流水GUID</param>
        /// <returns></returns>
        public async Task<List<MerchantFlowingModel>> GetModelByTransactionFlowingGuid(string transactionFlowingGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<MerchantFlowingModel>("select * from t_merchant_flowing where transaction_flowing_guid=@transaction_flowing_guid", new { transaction_flowing_guid = transactionFlowingGuid })).AsList();
            }
        }

        /// <summary>
        /// 根据商户账号查询商户流水信息
        /// </summary>
        /// <param name="requestDto">商户流水查询DTO</param>
        /// <returns></returns>
        public async Task<MerchantFlowingPageResponseDto> GetModelByMerchantAccount(MerchantFlowingPageRequestDto requestDto)
        {
            requestDto.StartTime = GetStartDate(requestDto.StartTime);
            requestDto.EndTime = GetEndDate(requestDto.EndTime);

            StringBuilder sb = new StringBuilder(@"select * from t_merchant_flowing where creation_date>@StartTime and creation_date<@EndTime");

            if (!string.IsNullOrWhiteSpace(requestDto.MerchantAccount))
            {
                sb.Append(" and merchant_account=@MerchantAccount ");
            }
            if (!string.IsNullOrWhiteSpace(requestDto.FlowStatus))
            {
                sb.Append(" and flow_status=@FlowStatus ");
            }
            return await MySqlHelper.QueryByPageAsync<BasePageRequestDto, MerchantFlowingPageResponseDto, MerchantFlowingPageItemDto>(sb.ToString(), requestDto);
        }



        /// <summary>
        /// 创建Sql
        /// </summary>
        /// <param name="dateSql">时间sql</param>
        /// <param name="merchantGuid">商户GUID</param>
        /// <param name="merchantName">商户名称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        private async Task<List<MerchantFlowingReportResponseDto>> SelectFlowingReportAsync(string dateSql, string merchantGuid, string merchantName, DateTime startTime, DateTime endTime)
        {
            string sqlstring = $@"SELECT 
                                        m.merchant_guid AS MerchantGuid,
                                        m.merchant_name AS MerchantName,
                                        m.date AS Date,
                                        mfs.product_count AS ProductCount,
                                        mfs.amount AS Amount,
                                        mfs.order_count AS OrderCount,
                                        ccs.consumption_count AS ConsumptionCount
                                    FROM ({dateSql}) AS m
                                    LEFT JOIN(
	                    	SELECT
										o.merchant_guid,
										DATE_FORMAT( o.payment_date, '%Y-%m-%d' ) AS date,
										SUM( o.product_count ) AS product_count,
										COUNT( o.order_guid ) AS order_count,
										Sum( o.paid_amount ) AS amount 
									FROM
										t_mall_order AS o 
									WHERE
										( o.payment_date >=@startTime  AND o.payment_date <=@endTime  ) 
										AND o.order_status IN ( 'Received', 'Completed', 'Shipped' ) 
										AND o.merchant_guid = @merchantGuid
									GROUP BY
										o.merchant_guid,
									  DATE_FORMAT( o.payment_date, '%Y-%m-%d' )

                                    ) AS mfs ON mfs.merchant_guid=m.merchant_guid and mfs.date=m.date
                                    LEFT JOIN(
	                                    SELECT
                                            cc.merchant_guid,
                                            DATE_FORMAT(cc.consumption_end_date, '%Y-%m-%d') AS date,
                                            COUNT(cc.consumption_guid) AS consumption_count
	                                    FROM t_consumer_consumption cc 
	                                    WHERE 
		                                    cc.merchant_guid=@merchantGuid
                                            AND cc.consumption_status in ('Completed' )
		                                    AND cc.consumption_end_date >= @startTime 
	                                        AND cc.consumption_end_date <= @endTime 
	                                    GROUP BY cc.merchant_guid,date
                                    ) AS ccs ON ccs.merchant_guid=m.merchant_guid and ccs.date=m.date
                                ORDER BY m.date 
                            ";// 'Booked','Arrive',
            using (MySql.Data.MySqlClient.MySqlConnection conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<MerchantFlowingReportResponseDto>(sqlstring, new { merchantGuid, merchantName, startTime, endTime }))?.AsList();
            }
        }

        /// <summary>
        /// 创建时间sql
        /// </summary>
        /// <param name="merchantGuid">商户GUID</param>
        /// <param name="merchantName">商户名称</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        private string CreateDateSql(DateTime startTime, DateTime endTime)
        {
            List<string> dateSql = new List<string>();
            DateTime date = startTime;
            while (date < endTime)
            {
                dateSql.Add($"select @merchantGuid AS merchant_guid,@merchantName AS merchant_name,'{date.ToString("yyyy-MM-dd")}' AS date");
                date = date.AddDays(1);
            }
            return string.Join(" UNION ALL ", dateSql);
        }

        /// <summary>
        /// 获取一天中的开始时间
        /// </summary>
        /// <param name="startDate">时间</param>
        /// <returns>时、分、秒清零</returns>
        public DateTime GetStartDate(DateTime startDate)
        {
            return new DateTime(startDate.Year, startDate.Month, startDate.Day);
        }

        /// <summary> 
        /// 计算某日结束日期
        /// </summary> 
        /// <param name="someDate">该周中任意一天</param> 
        /// <returns>时、分、秒改成最后1毫秒</returns> 
        public DateTime GetEndDate(DateTime endDate)
        {
            return new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// 商户流水报表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<MerchantFlowingReportResponseDto>> FlowingReportAsync(MerchantFlowingReportResquestDto requestDto)
        {
            requestDto.StartTime = GetStartDate(requestDto.StartTime ?? DateTime.Now);
            requestDto.EndTime = GetEndDate(requestDto.EndTime ?? DateTime.Now);
            TimeSpan ts = (requestDto.EndTime - requestDto.StartTime).Value;
            if (ts.Days > 60)
            {
                return null;
            }
            MerchantModel merchant = await new MerchantBiz().GetModelAsync(requestDto.MerchantGuid);
            if (merchant == null)
            {
                return null;
            }
            string dateSql = CreateDateSql(requestDto.StartTime.Value, requestDto.EndTime.Value);
            return await SelectFlowingReportAsync(dateSql, merchant.MerchantGuid, merchant.MerchantName, requestDto.StartTime.Value, requestDto.EndTime.Value);
        }

        /// <summary>
        /// 创建时间sql
        /// </summary>
        /// <param name="merchantGuid">商户GUID</param>
        /// <param name="merchantName">商户名称</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public string MerchantFlowingReportCreateDateSql(DateTime startTime, DateTime endTime)
        {
            List<string> dateSql = new List<string>();
            DateTime date = startTime;
            while (date < endTime)
            {
                dateSql.Add($"select @merchantGuid AS merchant_guid,@merchantName AS merchant_name,'{date.ToString("yyyy-MM-dd")}' AS date");
                date = date.AddDays(1);
            }
            return string.Join(" UNION ALL ", dateSql);
        }

        /// <summary>
        /// 创建Sql
        /// </summary>
        /// <param name="dateSql">时间sql</param>
        /// <param name="merchantGuid">商户GUID</param>
        /// <param name="merchantName">商户名称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public async Task<List<ProjectSoldReportRankingAsyncResponseDto>> ProjectSoldReportRankingAsync(ProjectSoldReportRankingAsyncRequestDto requestDto)
        {
            string sqlstring = $@"SELECT
                                                    a.merchant_guid,
	                                                b.product_guid,
	                                                b.product_name,
	                                                sum( b.product_count ) AS SoldCount 
                                                FROM
	                                                t_mall_order a,
	                                                t_mall_order_detail b 
                                                WHERE
	                                                ( 
	                                                ( a.payment_date >= @StartTime AND a.payment_date <= @EndTime )
	                                                OR 
	                                                ( a.creation_date >= @StartTime AND a.creation_date <= @EndTime ) 
	                                                ) 
	                                                AND a.merchant_guid = @MerchantGuid 
                                                    AND a.order_status in ('Received', 'Completed', 'Shipped' )
                                                    AND a.order_guid = b.order_guid
                                                GROUP BY
                                                    b.product_guid,
	                                                b.product_name
                                                ORDER BY
                                                    SoldCount DESC
                                                    LIMIT 10 ";
            using (MySql.Data.MySqlClient.MySqlConnection conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<ProjectSoldReportRankingAsyncResponseDto>(sqlstring, new { requestDto.MerchantGuid, requestDto.StartTime, requestDto.EndTime }))?.AsList();
            }
        }

    }
}