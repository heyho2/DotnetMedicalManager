using GD.Common;
using GD.Common.Helper;
using GD.Dtos.Report.CommonReport;
using GD.Models.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GD.Models.Report.ReportConditionModel;

namespace GD.API.Controllers.Report
{
    /// <summary>
    /// 通用报表
    /// </summary>
    public class CommonReportController : ReportBaseController
    {
        #region 新需求报表

        /// <summary>
        /// 新增报表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> CreateReport([FromBody]CreateReportRequest requestDto)
        {
            //简单过滤SqlStr  格式必须是  select @ColumnStr from table
            if (!IsRightPreviewSql(requestDto.Sqlstr))
            {
                return Failed(ErrorCode.DataBaseError, "SQL格式有误，请检查!");
            }
            var result = false;
            var commonReportBiz = new CommonReportThemeBiz();
            //新申请
            var themeModel = new ReportThemeModel()
            {
                ThemeGuid = Guid.NewGuid().ToString("N"),
                ApplyUserName = requestDto.ApplyUserName,
                Name = requestDto.ReportName,
                Demand = requestDto.Demand,
                SQLStr = requestDto.Sqlstr,
                RecordStatus = requestDto.RecordStatus,
                Sort = requestDto.Sort,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                PlatformType = string.IsNullOrWhiteSpace(requestDto.PlatformType) ? "CloudDoctor" : requestDto.PlatformType,
                OrgGuid = string.Empty,
                Enable = true
            };
            var conditionAndColumnList = new List<ReportConditionModel>();
            foreach (var item in requestDto.CreateConditionInfoList)
            {
                var conditionModel = new ReportConditionModel()
                {
                    ConditionGuid = Guid.NewGuid().ToString("N"),
                    ThemeGuid = themeModel.ThemeGuid,
                    Name = item.ConditionName,
                    FieldCode = item.FieldCode,
                    FieldValueSql = string.Empty,
                    IsRightSql = true,
                    ValueType = item.ValueType,
                    ValueRange = string.IsNullOrWhiteSpace(item.ValueRange) ? "{}" : item.ValueRange,
                    ExtensionField = string.Empty,
                    CreatedBy = UserID,
                    CreationDate = DateTime.Now,
                    LastUpdatedBy = UserID,
                    LastUpdatedDate = DateTime.Now,
                    RecordType = RecordTypeEnum.Condition.ToString(),
                    Required = item.Required,
                    Sort = item.Sort,
                    OrgGuid = string.Empty,
                    Enable = true
                };
                conditionAndColumnList.Add(conditionModel);
            }
            foreach (var item in requestDto.CreateColumnInfoList)
            {
                var conditionModel = new ReportConditionModel()
                {
                    ConditionGuid = Guid.NewGuid().ToString("N"),
                    ThemeGuid = themeModel.ThemeGuid,
                    Name = item.ColumnName,
                    FieldCode = item.FieldCode,
                    FieldValueSql = string.Empty,
                    IsRightSql = true,
                    ValueType = item.ValueType,
                    ValueRange = string.IsNullOrWhiteSpace(item.ValueRange) ? "{}" : item.ValueRange,
                    ExtensionField = string.Empty,
                    CreatedBy = UserID,
                    CreationDate = DateTime.Now,
                    LastUpdatedBy = UserID,
                    LastUpdatedDate = DateTime.Now,
                    RecordType = RecordTypeEnum.Column.ToString(),
                    Required = item.Required,
                    Sort = item.Sort,
                    OrgGuid = string.Empty,
                    Enable = true
                };
                conditionAndColumnList.Add(conditionModel);
            }
            result = await commonReportBiz.ApplyCreateReport(themeModel, conditionAndColumnList);
            return Success(result);
        }

        /// <summary>
        /// 分页-报表列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetReportListAsyncResponseDto>))]
        public async Task<IActionResult> GetReportListAsync([FromBody]GetReportListAsyncRequestDto requestDto)
        {
            var responseList = await new CommonReportThemeBiz().GetMyAppyList(requestDto);
            return Success(responseList);
        }
        /// <summary>
        ///获取类型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<Dictionary<string, string>>)), AllowAnonymous]
        public IActionResult GetEnumType()
        {
            var typeDic = new Dictionary<string, string>
            {
                { "字符串", "string" },
                { "布尔值", "bool" },
                { "小数", "decimal" },
                { "枚举", "enum" },
                { "整数", "int" },
                { "日期", "datetime" }
            };
            return Success(typeDic);
        }
        /// <summary>
        /// 条件/列名 List-ByThemeGuid
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetConditionOrColumnListResponseDto>>))]
        public async Task<IActionResult> GetConditionListAsync([FromBody]GetConditionOrColumnListRequest request)
        {
            string[] strArr = new string[] { "condition", "column" };
            if (!strArr.Contains(request.RecordType.ToLower().Trim()))
            {
                return Failed(ErrorCode.DataBaseError, "查询类型输入有误，请检查!");
            }
            var responseList = await new CommonReportThemeBiz().GetConditionListAsync(request);
            return Success(responseList.OrderBy(a => a.Sort));
        }
        /// <summary>
        /// 获取单条报表记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<ReportThemeModel>))]
        public async Task<IActionResult> GetOneReport([FromBody]GetOneReportRequest request)
        {
            var response = await new CommonReportThemeBiz().GetAsync(request.ThemeGuid);
            return Success(response);
        }

        /// <summary>
        /// 编辑报表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> UpdateOneReport([FromBody]UpdateOneReportRequest requestDto)
        {
            //简单过滤SqlStr  格式必须是  select @ColumnStr from table
            if (!IsRightPreviewSql(requestDto.Sqlstr))
            {
                return Failed(ErrorCode.DataBaseError, "SQL格式有误，请检查!");
            }
            var commonReportBiz = new CommonReportThemeBiz();
            var themeModel = await commonReportBiz.GetAsync(requestDto.ThemeGuid);
            themeModel.ApplyUserName = requestDto.ApplyUserName;
            themeModel.Name = requestDto.ReportName;
            themeModel.Demand = requestDto.Demand;
            themeModel.SQLStr = requestDto.Sqlstr;
            themeModel.RecordStatus = requestDto.RecordStatus;
            themeModel.Sort = requestDto.Sort;
            themeModel.CreatedBy = UserID;
            themeModel.LastUpdatedBy = UserID;
            themeModel.LastUpdatedDate = DateTime.Now;

            var conditionAndColumnList = new List<ReportConditionModel>();
            foreach (var item in requestDto.CreateConditionInfoList)
            {
                var conditionModel = new ReportConditionModel()
                {
                    ConditionGuid = Guid.NewGuid().ToString("N"),
                    ThemeGuid = themeModel.ThemeGuid,
                    Name = item.ConditionName,
                    FieldCode = item.FieldCode,
                    FieldValueSql = string.Empty,
                    IsRightSql = true,
                    ValueType = item.ValueType,
                    ValueRange = string.IsNullOrWhiteSpace(item.ValueRange) ? "{}" : item.ValueRange,
                    ExtensionField = string.Empty,
                    CreatedBy = UserID,
                    CreationDate = DateTime.Now,
                    LastUpdatedBy = UserID,
                    LastUpdatedDate = DateTime.Now,
                    RecordType = RecordTypeEnum.Condition.ToString(),
                    Required = item.Required,
                    Sort = item.Sort,
                    OrgGuid = string.Empty,
                    Enable = true
                };
                conditionAndColumnList.Add(conditionModel);
            }
            foreach (var item in requestDto.CreateColumnInfoList)
            {
                var conditionModel = new ReportConditionModel()
                {
                    ConditionGuid = Guid.NewGuid().ToString("N"),
                    ThemeGuid = themeModel.ThemeGuid,
                    Name = item.ColumnName,
                    FieldCode = item.FieldCode,
                    FieldValueSql = string.Empty,
                    IsRightSql = true,
                    ValueType = item.ValueType,
                    ValueRange = string.IsNullOrWhiteSpace(item.ValueRange) ? "{}" : item.ValueRange,
                    ExtensionField = string.Empty,
                    CreatedBy = UserID,
                    CreationDate = DateTime.Now,
                    LastUpdatedBy = UserID,
                    LastUpdatedDate = DateTime.Now,
                    RecordType = RecordTypeEnum.Column.ToString(),
                    Required = item.Required,
                    Sort = item.Sort,
                    OrgGuid = string.Empty,
                    Enable = true
                };
                conditionAndColumnList.Add(conditionModel);
            }
            var result = await commonReportBiz.UpdateOneReportAsync(themeModel, conditionAndColumnList);
            return Success();
        }
        /// <summary>
        /// 发布记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<ReportThemeModel>))]
        public async Task<IActionResult> PublishOneReport([FromBody]PublishOneReportRequest request)
        {
            var commonReportBiz = new CommonReportThemeBiz();
            var model = await commonReportBiz.GetAsync(request.ThemeGuid);
            if (model == null) { return Failed(ErrorCode.DataBaseError, "ID有误，请检查!"); }
            model.RecordStatus = 2;
            var result = await commonReportBiz.UpdateAsync(model);
            return Success(result);
        }
        /// <summary>
        /// 执行sql执行结果-预览
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<PreviewResultInSqlResponseDto>))]
        public async Task<IActionResult> PreviewResultInSqlAsync([FromBody]PreviewResultInSqlStrRequest request)
        {
            var commonReportBiz = new CommonReportThemeBiz();
            var themeModel = await commonReportBiz.GetAsync(request.ThemeGuid);
            if (themeModel == null || string.IsNullOrWhiteSpace(themeModel.SQLStr))
            {
                return Failed(ErrorCode.DataBaseError, "主题SQL语句为空，请检查!");
            }
            foreach (var item in request.PreviewConditionInfoList)
            {
                if (item.Required && string.IsNullOrWhiteSpace(item.FieldValue))
                {
                    return Failed(ErrorCode.DataBaseError, $" {item.FieldCode} 的值不能为空！请检查!");
                }
            }

            if (!IsRightPreviewSql(themeModel.SQLStr))
            {
                return Failed(ErrorCode.DataBaseError, "SQL语句有误，请检查!");
            }
            var conditionOrColumnList = await commonReportBiz.GetModelListAsyncByThemeGuid(request.ThemeGuid);
            if (conditionOrColumnList == null || conditionOrColumnList.Count < 1)
            {
                return Failed(ErrorCode.DataBaseError, "未读取到条件与列信息，请检查!");
            }

            var condictionModelList = conditionOrColumnList.Where(a => a.RecordType.Equals(RecordTypeEnum.Condition.ToString())).ToList();
            var columnModelList = conditionOrColumnList.Where(a => a.RecordType.Equals(RecordTypeEnum.Column.ToString())).ToList();
            try
            {
                var (CurrentPage, Total) = await commonReportBiz.PreviewResultListAsync(request, themeModel, condictionModelList, columnModelList);
                var response = new PreviewResultInSqlResponseDto
                {
                    CurrentPage = CurrentPage,
                    Total = Total
                };
                return Success(response);
            }
            catch (Exception ex)
            {
                Logger.Info($"报表预览SQL执行报错=>Error: {ex.Message}");
                return Failed(ErrorCode.DataBaseError, "SQL语句执行错误，请检查!");
            }
        }
        /// <summary>
        /// 检查SQL是否正确
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        private bool IsRightPreviewSql(string sqlStr)
        {
            string[] sqlPara3 = { "delete", "drop", "update", "insert", "set", "truncate" };
            foreach (var item in sqlPara3)
            {
                if (sqlStr.ToLower().Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion


    }
}
