using Dapper;
using GD.AppSettings;
using GD.Common.Helper;
using GD.DataAccess;
using GD.Dtos.Report.CommonReport;
using GD.Manager;
using GD.Models.Report;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Report
{
    /// <summary>
    /// 通用报表
    /// </summary>
    public class CommonReportThemeBiz : BaseBiz<ReportThemeModel>
    {
        /// <summary>
        /// host.json配置
        /// </summary>
        private static readonly string mysqlReportConn = string.Empty;

        /// <summary>
        /// 静态构造
        /// </summary>
        static CommonReportThemeBiz()
        {
            var settings = Factory.GetSettings("host.json");
            mysqlReportConn = settings["ConnectionString:MySqlReport"];
        }

        /// <summary>
        /// 通过主键获取model
        /// </summary>
        /// <param name="answerGuid"></param>
        /// <returns></returns>
        public async Task<List<ReportConditionModel>> GetModelListAsyncByThemeGuid(string themeGuid, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<ReportConditionModel>(" where theme_guid=@themeGuid and enable=@enable ", new { themeGuid, enable })).ToList();
            }
        }
        /// <summary>
        /// 添加申请
        /// </summary>
        /// <param name="enumStr"></param>
        /// <returns></returns>
        public async Task<bool> ApplyCreateReport(ReportThemeModel tModel, List<ReportConditionModel> conditionModelList)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, ReportThemeModel>(tModel))) { return false; }

                foreach (var item in conditionModelList)
                {
                    if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, ReportConditionModel>(item))) { return false; }
                }
                return true;
            });
        }
        /// <summary>
        /// 编辑申请
        /// </summary>
        /// <param name="enumStr"></param>
        /// <returns></returns>
        public async Task<bool> UpdateOneReportAsync(ReportThemeModel tModel, List<ReportConditionModel> conditionModelList)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                await conn.UpdateAsync(tModel);
                await conn.DeleteListAsync<ReportConditionModel>($" where  theme_guid='{tModel.ThemeGuid}' ");
                foreach (var item in conditionModelList)
                {
                    await conn.InsertAsync<string, ReportConditionModel>(item);
                }
                return true;
            });
        }
        /// <summary>
        /// 预览报表列表
        /// </summary>
        /// <param name="enumStr"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<dynamic> CurrentPage, int? Total)> PreviewResultListAsync(PreviewResultInSqlStrRequest request, ReportThemeModel themeModel, List<ReportConditionModel> conditionList, List<ReportConditionModel> columnList)
        {
            var sqlStr = themeModel.SQLStr;
            string sqlTotalNum = $"SELECT count(1) FROM ({sqlStr}) __t  limit 1";
            sqlStr = $@"{sqlStr} limit {(request.PageIndex - 1) * request.PageSize},{request.PageSize}";
            var parameters = new DynamicParameters();
            foreach (var item in request.PreviewConditionInfoList)
            {
                parameters.Add(item.FieldCodeValueString.Replace("@", ""), item.FieldValue);
            }
            var stopwatch = new Stopwatch();
            using (var conn = MySqlHelper.CreateConnection(mysqlReportConn))
            {
                stopwatch.Start();
                var currentPage = await conn.QueryAsync<dynamic>(sqlStr, parameters);
                var total = await conn.QueryFirstOrDefaultAsync<int?>(sqlTotalNum, parameters);
                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds / 1000 > 50)
                {
                    Logger.Info($"报表SQL执行耗时：{stopwatch.ElapsedMilliseconds / 1000  } 秒");
                }
                return (currentPage, total);
            }
        }
        /// <summary>
        /// 获取报表列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetReportListAsyncResponseDto> GetMyAppyList(GetReportListAsyncRequestDto request)
        {
            var sqlWhere = $@" `enable` = 1 ";
            if (!string.IsNullOrWhiteSpace(request.ApplyUserName))
            {
                sqlWhere = $"{sqlWhere} AND (apply_user_name like @ApplyUserName) ";
            }
            if (!string.IsNullOrWhiteSpace(request.KeyWord))
            {
                sqlWhere = $"{sqlWhere} AND (LOCATE(@KeyWord, demand) OR LOCATE(@KeyWord, name) ) ";
            }
            if (request.StartTime != null)
            {
                request.StartTime = request.StartTime?.Date;
                sqlWhere = $"{sqlWhere} AND creation_date > @StartTime ";
            }
            if (request.EndTime != null)
            {
                request.EndTime = request.EndTime?.AddDays(1).Date;
                sqlWhere = $"{sqlWhere} AND creation_date < @EndTime ";
            }

            if (request.RecordStatus > -1 && request.RecordStatus < 3)
            {
                sqlWhere = $"{sqlWhere} AND record_status = @RecordStatus ";
            }
            var sqlStr = $@"SELECT
	                                    theme_guid,
	                                    apply_user_name,
	                                    NAME,
	                                    demand,
	                                    sqlstr,
	                                    record_status,
	                                    Sort,
	                                    created_by,
	                                    creation_date,
	                                    last_updated_by,
	                                    last_updated_date 
                                    FROM
	                                    t_report_theme 
                                    WHERE
	                                    {sqlWhere}
                                    ORDER BY
	                                    creation_date DESC ";
            return await MySqlHelper.QueryByPageAsync<GetReportListAsyncRequestDto, GetReportListAsyncResponseDto, GetReportListAsyncItemDto>(sqlStr, request);
        }

        /// <summary>
        /// IT-我的审核列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<GetConditionOrColumnListResponseDto>> GetConditionListAsync(GetConditionOrColumnListRequest request)
        {
            var sqlStr = @"SELECT
	                        condition_guid,
	                        theme_guid,
	                        NAME,
	                        field_code,
	                        value_type,
	                        value_range,
	                        record_type,
	                        required,
	                        sort 
                        FROM
	                        t_report_condition 
                        WHERE
	                        `enable` = 1 
	                        AND theme_guid = @ThemeGuid 
	                        AND record_type = @RecordType";
            using (var conn = MySqlHelper.GetConnection())
            {
                var response = (await conn.QueryAsync<GetConditionOrColumnListResponseDto>(sqlStr, new { request.ThemeGuid, request.RecordType })).ToList();
                return response;
            }
        }
    }
}
