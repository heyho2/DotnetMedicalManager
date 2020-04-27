using Dapper;
using GD.DataAccess;
using GD.Dtos.Report.CommonReport;
using GD.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GD.Dtos.Report.CommonReport.ITSubmitSqlOrRefuseDemandRequest;

namespace Report
{
    /// <summary>
    /// 通用报表
    /// </summary>
    public class CommonReportThemeBiz
    {
        /// <summary>
        /// 通过主键获取model
        /// </summary>
        /// <param name="answerGuid"></param>
        /// <returns></returns>
        public async Task<ReportThemeModel> GetModelAsync(string guid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<ReportThemeModel>(guid);
            }
        }


        /// <summary>
        /// 批量运行sql
        /// </summary>
        /// <param name="sqls"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        private async Task<bool> ExcuteBatchSqlAsync(List<string> sqls, MySql.Data.MySqlClient.MySqlConnection conn)
        {
            if (!sqls.Any())
            {
                return true;
            }
            foreach (var sql in sqls)
            {
                await conn.ExecuteAsync(sql);
            }
            return true;
        }

        /// <summary>
        /// 添加申请
        /// </summary>
        /// <param name="enumStr"></param>
        /// <returns></returns>
        public async Task<bool> ApplyCreateReport(ReportThemeModel tModel, ReportApproveModel raModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                if (await conn.InsertAsync<int, ReportThemeModel>(tModel) > 0)
                {
                    return false;
                }
                if (await conn.InsertAsync<int, ReportApproveModel>(raModel) > 0)
                {
                    return false;
                }
                return true;
            });
        }

        /// <summary>
        /// 更新model
        /// </summary>
        /// <param name="enumStr"></param>
        /// <returns></returns>
        public async Task<bool> UpdateThemeAndApproveModel(ReportThemeModel tModel, ReportApproveModel apModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
           {
               if (await conn.UpdateAsync(tModel) < 1)
               {
                   return false;
               }
               if (await conn.UpdateAsync(apModel) < 1)
               {
                   return false;
               }
               return true;
           });
        }

        /// <summary>
        /// IT写SQL等更新
        /// </summary>
        /// <param name="tModel"></param>
        /// <param name="apModel"></param>
        /// <param name="conditionModelList"></param>
        /// <returns></returns>
        public async Task<bool> UpdateThemeAndApproveModel(ReportThemeModel tModel, ReportApproveModel apModel, List<ReportConditionModel> conditionModelList)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                if (await conn.UpdateAsync(tModel) < 1)
                {
                    return false;
                }
                if (await conn.UpdateAsync(apModel) < 1)
                {
                    return false;
                }
                foreach (var model in conditionModelList)
                {
                    if (await conn.InsertAsync(model) < 1)
                    {
                        return false;
                    }
                }
                return true;
            });
        }
        /// <summary>
        /// GetReportApproveModelByGuid
        /// </summary>
        /// <param name="enumStr"></param>
        /// <returns></returns>
        public async Task<ReportApproveModel> GetReportApproveModelByGuid(string approveGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<ReportApproveModel>(approveGuid);
            }
        }

        /// <summary>
        /// GetSMSThemeModelByThemeGuid
        /// </summary>
        /// <param name="themeGuid"></param>
        /// <returns></returns>
        public async Task<ReportThemeModel> GetReportThemeModelByThemeGuid(string themeGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<ReportThemeModel>(themeGuid);
            }
        }

        /// <summary>
        /// 更新model
        /// </summary>
        /// <param name="enumStr"></param>
        /// <returns></returns>
        public async Task<bool> UpdateApproveModel(ReportApproveModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.UpdateAsync(model) > 0;
            }
        }

        /// <summary>
        /// 预览报表列表
        /// </summary>
        /// <param name="enumStr"></param>
        /// <returns></returns>
        public GetReportListResponse GetReportList(GetReportListRequest requestDto)
        {
            return MySqlHelper.QueryByPage<GetReportListRequest, GetReportListResponse, GetPhoneListItemDto>(requestDto.SqlStr, requestDto);
        }

        /// <summary>
        /// 运营-我的申请列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetMyAppyListResponseDto> GetMyAppyList(GetMyAppyListRequestDto requestDto)
        {
            var sqlStr = @"SELECT
	                                    th.theme_guid,
	                                    th.name,
	                                    th.demand,
                                        th.condition_demand,
	                                    ap.approve_guid,
	                                    ap.apply_user_guid,
	                                    u.user_name AS ApplyUserName,
	                                    ap.sql_writer_guid,
	                                    u2.user_name AS SQLWriterName,
	                                    ap.sql_approver_guid,
	                                    u3.user_name AS SQLApproverName,
	                                    ap.list_approver_guid,
	                                    u4.user_name AS ListApproverName,
	                                    ap.approved_reason,
	                                    ap.approved_datetime,
	                                    ap.approve_schedule_enum,
	                                    ap.approve_status,
	                                    ap.creation_date 
                                    FROM
	                                    t_report_approve AS ap
	                                    LEFT JOIN t_report_theme AS th ON ap.theme_guid = th.theme_guid 
	                                    AND ap.ENABLE =
	                                    TRUE LEFT JOIN t_utility_user AS u ON ap.apply_user_guid = u.user_guid
	                                    LEFT JOIN t_utility_user AS u2 ON ap.sql_writer_guid = u2.user_guid 
	                                    LEFT JOIN t_utility_user AS u3 ON ap.sql_approver_guid = u3.user_guid 
	                                    LEFT JOIN t_utility_user AS u4 ON ap.list_approver_guid = u4.user_guid 
                                    WHERE
	                                    th.`enable` = TRUE 
	                                    AND ap.apply_user_guid = @UserID 
                                    ORDER BY
	                                    ap.creation_date DESC ";
            return await MySqlHelper.QueryByPageAsync<GetMyAppyListRequestDto, GetMyAppyListResponseDto, GetMyAppyListItemDto>(sqlStr, requestDto);
        }

        /// <summary>
        /// 运营-审核列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetListToApproveResponseDto> GetListToApprove(GetListToApproveRequest requestDto)
        {
            var sqlStr = @"SELECT
	                                    th.theme_guid,
	                                    th.name,
	                                    th.demand,
                                        th.condition_demand,
	                                    ap.approve_guid,
	                                    ap.apply_user_guid,
	                                    u.user_name AS ApplyUserName,
	                                    ap.sql_writer_guid,
	                                    u2.user_name AS SQLWriterName,
	                                    ap.sql_approver_guid,
	                                    u3.user_name AS SQLApproverName,
	                                    ap.list_approver_guid,
	                                    u4.user_name AS ListApproverName,
	                                    ap.approved_reason,
	                                    ap.approved_datetime,
	                                    ap.approve_schedule_enum,
	                                    ap.approve_status,
	                                    ap.creation_date 
                                    FROM
	                                    t_report_approve AS ap
	                                    LEFT JOIN t_report_theme AS th ON ap.theme_guid = th.theme_guid 
	                                    AND ap.ENABLE =
	                                    TRUE LEFT JOIN t_utility_user AS u ON ap.apply_user_guid = u.user_guid
	                                    LEFT JOIN t_utility_user AS u2 ON ap.sql_writer_guid = u2.user_guid 
	                                    LEFT JOIN t_utility_user AS u3 ON ap.sql_approver_guid = u3.user_guid 
	                                    LEFT JOIN t_utility_user AS u4 ON ap.list_approver_guid = u4.user_guid 
                                    WHERE
	                                    th.`enable` = TRUE 
	                                    AND 
	                                    ( ap.approve_schedule_enum = 'Approve' AND ap.approve_status = 'Pending' ) 
                                    ORDER BY
	                                    ap.creation_date DESC ";
            return await MySqlHelper.QueryByPageAsync<GetListToApproveRequest, GetListToApproveResponseDto, GetListToApproveItemDto>(sqlStr, requestDto);
        }

        /// <summary>
        ///  运营-获取单条申请以及发送列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetOneApproveWithListResponseDto> GetOneApproveWithList(GetOneApproveWithListRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlStr = @"SELECT
	                                        th.theme_guid,
	                                        th.name,
	                                        th.demand,
                                            th.condition_demand,
	                                        th.sqlstr,
	                                        ap.approve_guid,
	                                        ap.apply_user_guid,
	                                        u.user_name AS ApplyUserName,
	                                        ap.sql_writer_guid,
	                                        u2.user_name AS SQLWriterName,
	                                        ap.creation_date 
                                        FROM
	                                        t_report_approve AS ap
	                                        LEFT JOIN t_report_theme AS th ON ap.theme_guid = th.theme_guid 
	                                        AND ap.ENABLE =
	                                        TRUE LEFT JOIN t_utility_user AS u ON ap.apply_user_guid = u.user_guid
	                                        LEFT JOIN t_utility_user AS u2 ON ap.sql_writer_guid = u2.user_guid 
                                        WHERE
	                                        th.`enable` = TRUE 
	                                        AND ap.approve_guid = @ApproveGuid  ";
                var response = await conn.QueryFirstAsync<GetOneApproveWithListResponseDto>(sqlStr, new { requestDto.ApproveGuid });
                return response;
            }
        }


        /// <summary>
        /// IT-SQL审核-获取我的审核列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetSQLApproveListResponse> GetSQLApproveList(GetSQLApproveListRequest requestDto)
        {
            var sqlStr = @"SELECT
	                                    th.theme_guid,
	                                    th.name,
	                                    th.demand,
                                        th.condition_demand,
	                                    ap.approve_guid,
	                                    ap.apply_user_guid,
	                                    u.user_name AS ApplyUserName,
	                                    ap.sql_writer_guid,
	                                    u2.user_name AS SQLWriterName,
	                                    ap.sql_approver_guid,
	                                    u3.user_name AS SQLApproverName,
	                                    ap.list_approver_guid,
	                                    u4.user_name AS ListApproverName,
	                                    ap.approved_reason,
	                                    ap.approved_datetime,
	                                    ap.approve_schedule_enum,
	                                    ap.approve_status,
	                                    ap.creation_date 
                                    FROM
	                                    t_report_approve AS ap
	                                    LEFT JOIN t_report_theme AS th ON ap.theme_guid = th.theme_guid 
	                                    AND ap.ENABLE =
	                                    TRUE LEFT JOIN t_utility_user AS u ON ap.apply_user_guid = u.user_guid
	                                    LEFT JOIN t_utility_user AS u2 ON ap.sql_writer_guid = u2.user_guid 
	                                    LEFT JOIN t_utility_user AS u3 ON ap.sql_approver_guid = u3.user_guid 
	                                    LEFT JOIN t_utility_user AS u4 ON ap.list_approver_guid = u4.user_guid 
                                    WHERE
	                                    th.`enable` = TRUE 
	                                    AND (
	                                    ( ap.approve_schedule_enum = 'SqlWrite' AND ap.approve_status = 'Pending' ) 
	                                    OR ( ap.approve_schedule_enum = 'Approve' AND ap.approve_status = 'Reject' ) 
	                                    ) 
                                    ORDER BY
	                                    ap.creation_date DESC ";
            return await MySqlHelper.QueryByPageAsync<GetSQLApproveListRequest, GetSQLApproveListResponse, GetSQLApproveListItemDto>(sqlStr, requestDto);
        }


        /// <summary>
        ///  IT-获取单条申请
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ConfirmDemandAndProvideSqlResponse> GetConfirmDemandAndProvideSql(ConfirmDemandAndProvideSqlRequest requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlStr = @"SELECT
	                                        th.theme_guid,
	                                        th.name,
	                                        th.demand,
                                            th.condition_demand,
	                                        th.sqlstr,
	                                        ap.approve_guid,
	                                        ap.apply_user_guid,
	                                        u.user_name AS ApplyUserName,
	                                        ap.creation_date 
                                        FROM
	                                        t_utility_sms_approve AS ap
	                                        LEFT JOIN t_utility_sms_theme AS th ON ap.theme_guid = th.theme_guid 
	                                        AND ap.ENABLE =
	                                        TRUE LEFT JOIN t_utility_user AS u ON ap.approved_user_guid = u.user_guid
	                                        LEFT JOIN t_utility_user AS u2 ON ap.approving_user_guid = u2.user_guid 
                                        WHERE
	                                        th.`enable` = TRUE 
	                                        AND ap.approve_guid = @ApproveGuid 
                                        ORDER BY
	                                        ap.creation_date ";
                var response = await conn.QueryFirstAsync<ConfirmDemandAndProvideSqlResponse>(sqlStr, new { requestDto.ApproveGuid });
                return response;
            }
        }

        /// <summary>
        /// IT-我的审核列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetMyApproveListResponse> GetMyApproveList(GetMyApproveListRequest requestDto)
        {
            var sqlStr = @" SELECT
                                        th.theme_guid,
	                                    th.name,
	                                    th.demand,
                                        th.condition_demand,
	                                    ap.approve_guid,
	                                    ap.apply_user_guid,
	                                    u.user_name AS ApplyUserName,
	                                    ap.sql_writer_guid,
	                                    u2.user_name AS SQLWriterName,
	                                    ap.sql_approver_guid,
	                                    u3.user_name AS SQLApproverName,
	                                    ap.list_approver_guid,
	                                    u4.user_name AS ListApproverName,
	                                    ap.approved_reason,
	                                    ap.approved_datetime,
	                                    ap.approve_schedule_enum,
	                                    ap.approve_status,
	                                    ap.creation_date 
                                    FROM
	                                    t_report_approve AS ap
	                                    LEFT JOIN t_report_theme AS th ON ap.theme_guid = th.theme_guid 
	                                    AND ap.ENABLE =
	                                    TRUE LEFT JOIN t_utility_user AS u ON ap.apply_user_guid = u.user_guid
	                                    LEFT JOIN t_utility_user AS u2 ON ap.sql_writer_guid = u2.user_guid 
	                                    LEFT JOIN t_utility_user AS u3 ON ap.sql_approver_guid = u3.user_guid 
	                                    LEFT JOIN t_utility_user AS u4 ON ap.list_approver_guid = u4.user_guid 
                                    WHERE
	                                    th.`enable` = TRUE 
	                                    AND (
	                                    ( ap.approve_schedule_enum = 'Apply' AND ap.approve_status = 'Pending' ) 
	                                    OR ( ap.approve_schedule_enum = 'SqlWrite' AND ap.approve_status = 'Reject' ) 
	                                    ) 
                                    ORDER BY
	                                    ap.creation_date DESC ";
            return await MySqlHelper.QueryByPageAsync<GetMyApproveListRequest, GetMyApproveListResponse, GetMyApproveListItem>(sqlStr, requestDto);
        }



        /// <summary>
        /// IT-我的审核列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<ApproveConditionInfo>> GetConditionListByThemeGuid(string themeGuid)
        {
            var sqlStr = @"SELECT
	                                    `NAME`,
	                                    field_code,
	                                    field_value_sql,
	                                    is_right_sql,
	                                    value_type,
	                                    value_range,
	                                    sort 
                                    FROM
	                                    t_report_condition 
                                    WHERE
	                                    theme_guid = @themeGuid 
	                                    AND `ENABLE` = 1 ";
            using (var conn = MySqlHelper.GetConnection())
            {
                var response = (await conn.QueryAsync<ApproveConditionInfo>(sqlStr, new { themeGuid })).ToList();
                return response;
            }
        }





    }
}
