using GD.DataAccess;
using GD.Dtos.Health;
using GD.Models.Consumer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Consumer
{
    public class ConsumerHealthReportBiz : BizBase.BaseBiz<ConsumerHealthReportModel>
    {
        /// <summary>
        /// 获取健康档案分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetHealthInformationArchivesPageListResponseDto> GetHealthInformationArchivesListAsync(GetHealthInformationArchivesPageListRequestDto requestDto, string userID)
        {
            var condition = string.Empty;
            //筛选当前登录用户审批
            condition = $" AND a.user_guid ='{userID}' ";
            string sql = $@"
                        SELECT
                            a.report_guid,
	                        a.report_name,
	                        a.suggestion,
	                        a.creation_date
                        FROM
                            t_consumer_health_report a
                        WHERE
                            a.`enable` = 1
                            {condition} order by a.creation_date desc";
            return await MySqlHelper.QueryByPageAsync<GetHealthInformationArchivesPageListRequestDto, GetHealthInformationArchivesPageListResponseDto, GetHealthInformationArchivesPageListItemDto>(sql, requestDto);
        }
    }
}
