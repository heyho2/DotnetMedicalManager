using Dapper;
using GD.DataAccess;
using GD.Dtos.Health;
using GD.Models.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Consumer
{
    /// <summary>
    /// 健康报表详情
    /// </summary>
    public class ConsumerHealthReportDetailBiz : BizBase.BaseBiz<ConsumerHealthReportDetailModel>
    {
        /// <summary>
        /// 获取健康报表明细数据
        /// </summary>
        /// <param name="reportGuid"></param>
        /// <returns></returns>
        public async Task<List<ConsumerHealthReportDetailResponse>> GetConsumerHealthReportDetailAsync(string reportGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                        a.accessory_name,
	                        CONCAT( acc.base_path, acc.relative_path ) AS PicturePath,
                            acc.Extension
                        FROM
	                        t_consumer_health_report_detail a
	                        LEFT JOIN t_utility_accessory AS acc ON a.accessory_guid = acc.accessory_guid 
                        WHERE
	                        a.`enable` = 1 
	                        AND a.report_guid=@reportGuid";
                var consumerHealthReportDetailList = await conn.QueryAsync<ConsumerHealthReportDetailResponse>(sql, new { reportGuid });
                return consumerHealthReportDetailList?.ToList();
            }
        }
    }
}
