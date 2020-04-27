using GD.Common;
using GD.Dtos.Report.CommonReport;
using GD.Models.CommonEnum;
using GD.Models.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GD.API.Controllers.Report
{
    /// <summary>
    /// 通用报表
    /// </summary>
    public class CommonReportController : ReportBaseController
    {
        #region 运营接口
        /// <summary>
        /// 运营-申请创建报表/被驳回后重新申请
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> ApplyCreateReport([FromBody]ApplyCreateReportRequest requestDto)
        {
            var result = false;
            var commonReportBiz = new CommonReportThemeBiz();
            if (string.IsNullOrWhiteSpace(requestDto.ApproveGuid))
            {
                //新申请
                var themeModel = new ReportThemeModel()
                {
                    ThemeGuid = Guid.NewGuid().ToString("N"),
                    Name = requestDto.Name,
                    Demand = requestDto.Demand,
                    //ConditionDemand = requestDto.ConditionDemand,
                    CreatedBy = UserID,
                    CreationDate = DateTime.Now,
                    LastUpdatedBy = UserID,
                    LastUpdatedDate = DateTime.Now,
                    //PlatformType = requestDto.PlatformType,
                    Enable = true
                };
                var approveModel = new ReportApproveModel()
                {
                    ApproveGuid = Guid.NewGuid().ToString("N"),
                    ThemeGuid = themeModel.ThemeGuid,
                    ApplyUserGuid = UserID,//申请提交人
                    //SQLWriterGuid = "",//需求审批人
                    //SQLApproverGuid = "",//sql审核人
                    //ListApproverGuid="",//列表审核人
                    //ApprovedReason= null,
                    //ApprovedDatetime = null,
                    ApproveScheduleEnum = ReportApproveScheduleEnums.Apply.ToString(),
                    ApproveStatus = ReportApproveStatusEnum.Pending.ToString(),
                    CreatedBy = UserID,
                    CreationDate = DateTime.Now,
                    LastUpdatedBy = UserID,
                    LastUpdatedDate = DateTime.Now,
                    OrgGuid = "test",
                    Enable = true
                };
                result = await commonReportBiz.ApplyCreateReport(themeModel, approveModel);
            }
            else
            {
                //重新申请
                var approveModel = await commonReportBiz.GetReportApproveModelByGuid(requestDto.ApproveGuid);
                if (approveModel == null)
                {
                    return Failed(ErrorCode.DataBaseError, "审批记录为空！");
                }
                //驳回状态才可重新申请
                if (!approveModel.ApproveStatus.Equals(ReportApproveStatusEnum.Reject.ToString()))
                {
                    return Failed(ErrorCode.DataBaseError, "该审批没有被驳回！");
                }

                var themeModel = await commonReportBiz.GetReportThemeModelByThemeGuid(approveModel.ThemeGuid);
                if (approveModel == null)
                {
                    return Failed(ErrorCode.DataBaseError, "查询不到该审批记录的申请信息！");
                }
                themeModel.Name = requestDto.Name;
                themeModel.Demand = requestDto.Demand;
                //themeModel.ConditionDemand = requestDto.ConditionDemand;
                themeModel.LastUpdatedBy = UserID;
                themeModel.LastUpdatedDate = DateTime.Now;

                approveModel.ApproveScheduleEnum = ReportApproveScheduleEnums.Apply.ToString();
                approveModel.ApproveStatus = ReportApproveStatusEnum.Pending.ToString();
                approveModel.LastUpdatedBy = UserID;
                approveModel.LastUpdatedDate = DateTime.Now;
                result = await commonReportBiz.UpdateThemeAndApproveModel(themeModel, approveModel);
            }
            return Success(result);
        }

        /// <summary>
        /// 运营-我的申请列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetMyAppyListResponseDto>))]
        public async Task<IActionResult> GetMyAppyListAsync([FromBody]GetMyAppyListRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.UserID))
            {
                requestDto.UserID = UserID;
            }
            var responseList = await new CommonReportThemeBiz().GetMyAppyList(requestDto);
            return Success(responseList);
        }

        /// <summary>
        /// 运营-列表审核List
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetListToApproveResponseDto>))]
        public async Task<IActionResult> GetListToApprove([FromBody]GetListToApproveRequest requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.UserID))
            {
                requestDto.UserID = UserID;
            }
            var responseList = await new CommonReportThemeBiz().GetListToApprove(requestDto);
            return Success(responseList);
        }

        /// <summary>
        /// 运营-取消我的申请
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<string>))]
        public async Task<IActionResult> CancelMyApprove([FromBody]CancelMyApproveRequest requestDto)
        {
            var commonReportBiz = new CommonReportThemeBiz();
            var approveModel = await commonReportBiz.GetReportApproveModelByGuid(requestDto.ApproveGuid);
            if (!approveModel.ApplyUserGuid.Equals(UserID))
            {
                return Failed(ErrorCode.UserData, "您不是该记录申请人，无法取消！");
            }
            if (approveModel.ApproveScheduleEnum.Equals(ReportApproveScheduleEnums.Complete.ToString()))
            {
                return Failed(ErrorCode.DataBaseError, "该记录状态为已完成，无法取消！");
            }
            approveModel.ApproveStatus = ReportApproveStatusEnum.Cancel.ToString();//取消
            approveModel.LastUpdatedDate = DateTime.Now;
            approveModel.LastUpdatedBy = UserID;

            var result = await commonReportBiz.UpdateApproveModel(approveModel);
            return Success(result);
        }
        /// <summary>
        /// 运营-获取单条申请以及列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetOneApproveWithListResponseDto>))]
        public async Task<IActionResult> GetOneApproveWithList([FromBody]GetOneApproveWithListRequestDto requestDto)
        {
            var response = await new CommonReportThemeBiz().GetOneApproveWithList(requestDto);
            if (response == null)
            {
                return Failed(ErrorCode.DataBaseError, "无数据，请检查!");
            }
            return Success(response);
        }
        /// <summary>
        /// 运营-预览列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetReportListResponse>))]
        public async Task<IActionResult> GetRecondReportList([FromBody]GetRecondReportListRequest requestDto)
        {
            var commonReportBiz = new CommonReportThemeBiz();
            var approveModel = await commonReportBiz.GetReportApproveModelByGuid(requestDto.ApproveGuid);
            if (approveModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "无数据，请检查!");
            }
            if (!IsRightStatusForOperation(approveModel))
            {
                return Failed(ErrorCode.DataBaseError, "该申请状态暂无权限查看列表，请检查!");
            }
            var themeModel = await commonReportBiz.GetReportThemeModelByThemeGuid(approveModel.ThemeGuid);
            if (themeModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "无主题数据，请检查!");
            }
            if (string.IsNullOrWhiteSpace(themeModel.SQLStr))
            {
                return Failed(ErrorCode.DataBaseError, "主题SQL语句为空，请检查!");
            }

            GetReportListRequest request = new GetReportListRequest
            {
                SqlStr = themeModel.SQLStr//暂时不分页
                //PageIndex = requestDto.PageIndex,
                //PageSize = requestDto.PageSize
            };
            try
            {
                if (!IsRightSQLStr(request.SqlStr))
                {
                    return Failed(ErrorCode.DataBaseError, "SQL语句有误，请检查!");
                }
                var responseDto = commonReportBiz.GetReportList(request);
                if (responseDto == null)
                {
                    return Failed(ErrorCode.DataBaseError, "获取报表数据失败，请检查!");
                }
                //foreach (var item in responseDto.CurrentPage)
                //{
                //    item.Phone = Regex.Replace(item.Phone, "(\\d{3})\\d{4}(\\d{4})", "$1****$2");
                //}

                return Success(responseDto);
            }
            catch (Exception ex)
            {
                return Failed(ErrorCode.DataBaseError, "SQL语句执行错误，请检查!");
            }


        }

        /// <summary>
        /// 检查SQL是否正确
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        private bool IsRightSQLStr(string sqlStr)
        {
            string[] sqlPara1 = { "select", "user_name", "phone", "from", "age", "sex" };
            string[] sqlPara2 = { "select", "username", "phone", "from" };
            string[] sqlPara3 = { "delete", "drop", "update", "insert", "set" };
            var sp1 = true;
            var sp2 = true;
            var sp3 = false;
            foreach (var item in sqlPara1)
            {
                var isTrue = sqlStr.ToLower().Contains(item);
                if (!isTrue)
                {
                    sp1 = false;
                    break;
                }
            }
            foreach (var item in sqlPara2)
            {
                var isTrue = sqlStr.ToLower().Contains(item);
                if (!isTrue)
                {
                    sp2 = false;
                    break;
                }
            }
            foreach (var item in sqlPara3)
            {
                var isTrue = sqlStr.ToLower().Contains(item);
                if (isTrue)
                {
                    sp3 = true;
                    break;
                }
            }
            if ((sp1 || sp2) && (!sp3))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查条件SQL是否正确
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        private bool IsRightConditionSQLStr(string sqlStr)
        {
            string[] sqlPara1 = { "select", "dkey", "dvalue", "from" };
            string[] sqlPara3 = { "delete", "drop", "update", "insert", "set" };
            var sp1 = true;
            var sp3 = false;
            foreach (var item in sqlPara1)
            {
                var isTrue = sqlStr.ToLower().Contains(item);
                if (!isTrue)
                {
                    sp1 = false;
                    break;
                }
            }

            foreach (var item in sqlPara3)
            {
                var isTrue = sqlStr.ToLower().Contains(item);
                if (isTrue)
                {
                    sp3 = true;
                    break;
                }
            }
            if (sp1 && (!sp3))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 可审批状态-运营
        /// </summary>
        /// <param name="approveModel"></param>
        /// <returns></returns>
        private bool IsRightStatusForOperation(ReportApproveModel approveModel)
        {
            var schedule = approveModel.ApproveScheduleEnum.Equals(ReportApproveScheduleEnums.Approve.ToString());
            var status = approveModel.ApproveStatus.Equals(ReportApproveStatusEnum.Pending.ToString());
            var statusFirst = schedule && status;
            if (statusFirst)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 运营-审批列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> ApproveReportList([FromBody]ApproveReportListRequest requestDto)
        {
            //申请人审批 也可以不是申请人
            var commonReportBiz = new CommonReportThemeBiz();
            var approveModel = await commonReportBiz.GetReportApproveModelByGuid(requestDto.ApproveGuid);
            if (approveModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "无审批数据，请检查!");
            }
            if (!approveModel.ApplyUserGuid.Equals(UserID))
            {
                return Failed(ErrorCode.DataBaseError, "不是申请人，无权限操作该记录!");
            }
            if (IsRightStatusForOperation(approveModel))
            {
                approveModel.ListApproverGuid = UserID;
                approveModel.ApprovedReason = requestDto.ApprovedReason;
                approveModel.ApprovedDatetime = DateTime.Now;
                if (requestDto.ApproveStatus.ToString().Equals(ReportApproveStatusEnum.Adopt.ToString()))
                {
                    //注意多一个状态
                    approveModel.ApproveScheduleEnum = ReportApproveScheduleEnums.Complete.ToString();
                    approveModel.ApproveStatus = ReportApproveStatusEnum.Adopt.ToString();
                }
                if (requestDto.ApproveStatus.ToString().Equals(ReportApproveStatusEnum.Reject.ToString()))
                {
                    //运营拒绝后，审批单到了SqlWrite 还是 SqlApprove，暂定 SqlApprove
                    // SqlApprove可自己重写sql，也可以拒绝后到SqlWrite
                    approveModel.ApproveScheduleEnum = ReportApproveScheduleEnums.Approve.ToString();
                    approveModel.ApproveStatus = ReportApproveStatusEnum.Reject.ToString();
                }
                var result = await commonReportBiz.UpdateApproveModel(approveModel);
                return Success(result);
            }
            else { return Failed(ErrorCode.DataBaseError, "该申请状态不是可审批状态，请检查!"); }
        }

        #endregion

        #region IT接口

        /// <summary>
        /// IT-我的审核列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetMyApproveListResponse>))]
        public async Task<IActionResult> GetMyApproveList([FromBody]GetMyApproveListRequest requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.UserID))
            {
                requestDto.UserID = UserID;
            }
            var responseList = await new CommonReportThemeBiz().GetMyApproveList(requestDto);
            return Success(responseList);
        }


        /// <summary>
        /// IT-获取单条申请
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<ConfirmDemandAndProvideSqlResponse>))]
        public async Task<IActionResult> GetConfirmDemandAndProvideSql([FromBody]ConfirmDemandAndProvideSqlRequest requestDto)
        {
            var commonReportThemeBiz = new CommonReportThemeBiz();
            var response = await commonReportThemeBiz.GetConfirmDemandAndProvideSql(requestDto);
            if (response == null)
            {
                return Failed(ErrorCode.DataBaseError, "无数据，请检查!");
            }
            var conditionList = await commonReportThemeBiz.GetConditionListByThemeGuid(response.ThemeGuid);
            response.ApproveConditionInfoList = conditionList;
            return Success(response);

        }
        /// <summary>
        /// IT-提交SQL以及条件配置 或驳回需求
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> ITSubmitSqlOrRefuseDemand([FromBody]ITSubmitSqlOrRefuseDemandRequest requestDto)
        {
            //var currentUserGuid = "942c0c1db7794095ac9e2abfe6bde264";//zhiliang 942c0c1db7794095ac9e2abfe6bde264  
            //if (!UserID.Equals(currentUserGuid))
            //{
            //    return Failed(ErrorCode.DataBaseError, "您不是SQL撰写人，无法操作!");
            //}
            var commonReportBiz = new CommonReportThemeBiz();
            var approveModel = await commonReportBiz.GetReportApproveModelByGuid(requestDto.ApproveGuid);
            if (approveModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "无审批数据，请检查!");
            }
            var themeModel = await commonReportBiz.GetReportThemeModelByThemeGuid(approveModel.ThemeGuid);
            if (themeModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "无主题数据，请检查!");
            }
            if (IsITRightStatusForOperation(approveModel))
            {
                approveModel.ApprovedReason = requestDto.ApprovedReason;
                approveModel.ApprovedDatetime = DateTime.Now;
                approveModel.SQLWriterGuid = UserID;
                approveModel.LastUpdatedBy = UserID;
                approveModel.LastUpdatedDate = DateTime.Now;
                var result = false;
                if (requestDto.ApproveStatus.ToString().Equals(ReportApproveStatusEnum.Adopt.ToString()))
                {
                    if (!IsRightSQLStr(requestDto.Sqlstr))
                    {
                        return Failed(ErrorCode.UserData, "SQL有误，请检查!");
                    }
                    themeModel.SQLStr = requestDto.Sqlstr;
                    themeModel.LastUpdatedBy = UserID;
                    themeModel.LastUpdatedDate = DateTime.Now;

                    approveModel.ApproveScheduleEnum = ReportApproveScheduleEnums.SqlWrite.ToString();
                    approveModel.ApproveStatus = ReportApproveStatusEnum.Pending.ToString();

                    var conditionModelList = new List<ReportConditionModel>();
                    foreach (var item in requestDto.ConditionInfoList)
                    {
                        if (!IsRightConditionSQLStr(item.FieldValueSql))
                        {
                            return Failed(ErrorCode.UserData, "条件SQL有误，请检查!");
                        }
                        var model = new ReportConditionModel
                        {
                            ConditionGuid = Guid.NewGuid().ToString("N"),
                            ThemeGuid = approveModel.ThemeGuid,
                            Name = item.Name,
                            FieldCode = item.FieldCode,
                            FieldValueSql = item.FieldValueSql,//需要加验证
                            IsRightSql = item.IsRightSql,
                            ValueType = item.ValueType,
                            ValueRange = item.ValueRange,
                            ExtensionField = "",
                            Sort = item.Sort,
                            CreatedBy = UserID,
                            CreationDate = DateTime.Now,
                            LastUpdatedBy = UserID,
                            LastUpdatedDate = DateTime.Now,
                            OrgGuid = "..",
                            Enable = true
                        };
                        conditionModelList.Add(model);
                    }
                    result = await commonReportBiz.UpdateThemeAndApproveModel(themeModel, approveModel, conditionModelList);
                }

                if (requestDto.ApproveStatus.ToString().Equals(ReportApproveStatusEnum.Reject.ToString()))
                {
                    if (string.IsNullOrWhiteSpace(requestDto.ApprovedReason))
                    {
                        return Failed(ErrorCode.UserData, "审核驳回，原因不能为空！");
                    }
                    approveModel.ApproveScheduleEnum = ReportApproveScheduleEnums.Apply.ToString();
                    approveModel.ApproveStatus = ReportApproveStatusEnum.Reject.ToString();
                    result = await commonReportBiz.UpdateApproveModel(approveModel);
                }
                return Success(result);
            }
            else { return Failed(ErrorCode.DataBaseError, "该申请状态不是可审批状态，请检查!"); }
        }


        /// <summary>
        /// IT-审核SQL语句 -我的审核列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetSQLApproveListResponse>))]
        public async Task<IActionResult> GetMySQLApproveList([FromBody]GetSQLApproveListRequest requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.UserID))
            {
                requestDto.UserID = UserID;
            }
            var responseList = await new CommonReportThemeBiz().GetSQLApproveList(requestDto);
            return Success(responseList);
        }

        /// <summary>
        /// IT-审核SQL语句
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> ITApproveSql([FromBody]ITApproveSqlRequest requestDto)
        {
            //var currentUserGuid = "f071f4959e9a4f118095f58de3ce548a";//zhikai
            //if (!UserID.Equals(currentUserGuid))
            //{
            //    return Failed(ErrorCode.DataBaseError, "您不是SQL审核人，无法操作!");
            //}
            var commonReportBiz = new CommonReportThemeBiz();
            var approveModel = await commonReportBiz.GetReportApproveModelByGuid(requestDto.ApproveGuid);
            if (approveModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "无审批数据，请检查!");
            }
            if (approveModel.SQLWriterGuid.Equals(UserID))
            {
                return Failed(ErrorCode.DataBaseError, "无法审批，SQL审批需要不同的人操作!");
            }
            var themeModel = await commonReportBiz.GetReportThemeModelByThemeGuid(approveModel.ThemeGuid);
            if (themeModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "无主题数据，请检查!");
            }

            if (IsITSQLRightStatusForOperation(approveModel))
            {
                approveModel.ApprovedReason = requestDto.ApprovedReason;
                approveModel.ApprovedDatetime = DateTime.Now;
                approveModel.SQLApproverGuid = UserID;
                approveModel.LastUpdatedBy = UserID;
                approveModel.LastUpdatedDate = DateTime.Now;
                var result = false;
                if (requestDto.ApproveStatus.ToString().Equals(ReportApproveStatusEnum.Adopt.ToString()))
                {
                    if (!IsRightSQLStr(requestDto.ThemeSqlStr))
                    {
                        return Failed(ErrorCode.UserData, "SQL有误，请检查!");
                    }
                    themeModel.SQLStr = requestDto.ThemeSqlStr;
                    themeModel.LastUpdatedBy = UserID;
                    themeModel.LastUpdatedDate = DateTime.Now;

                    approveModel.ApproveScheduleEnum = ReportApproveScheduleEnums.Approve.ToString();
                    approveModel.ApproveStatus = ReportApproveStatusEnum.Pending.ToString();

                    result = await commonReportBiz.UpdateThemeAndApproveModel(themeModel, approveModel);
                }
                if (requestDto.ApproveStatus.ToString().Equals(ReportApproveStatusEnum.Reject.ToString()))
                {
                    if (string.IsNullOrWhiteSpace(requestDto.ApprovedReason))
                    {
                        return Failed(ErrorCode.UserData, "审核驳回，原因不能为空！");
                    }
                    approveModel.ApproveScheduleEnum = ReportApproveScheduleEnums.SqlWrite.ToString();
                    approveModel.ApproveStatus = ReportApproveStatusEnum.Reject.ToString();
                    result = await commonReportBiz.UpdateApproveModel(approveModel);
                }
                return Success(result);
            }
            else { return Failed(ErrorCode.DataBaseError, "该申请状态不是可审批状态，请检查!"); }
        }

        /// <summary>
        /// IT-编写SQL可审批状态
        /// </summary>
        /// <param name="approveModel"></param>
        /// <returns></returns>
        private bool IsITRightStatusForOperation(ReportApproveModel approveModel)
        {
            //新的申请   提交SQL
            var schedule = approveModel.ApproveScheduleEnum.Equals(ReportApproveScheduleEnums.Apply.ToString());
            var status = approveModel.ApproveStatus.ToString().Equals(ReportApproveStatusEnum.Pending.ToString());
            //人员列表被驳回，重新写SQL
            var schedule2 = approveModel.ApproveScheduleEnum.Equals(ReportApproveScheduleEnums.SqlWrite.ToString());
            var status2 = approveModel.ApproveStatus.ToString().Equals(ReportApproveStatusEnum.Reject.ToString());

            var statusFirst = schedule && status;
            var statusSecond = schedule2 && status2;
            if (statusFirst || statusSecond)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// IT-SQL可审批状态
        /// </summary>
        /// <param name="approveModel"></param>
        /// <returns></returns>
        private bool IsITSQLRightStatusForOperation(ReportApproveModel approveModel)
        {
            var schedule = approveModel.ApproveScheduleEnum.Equals(ReportApproveScheduleEnums.SqlWrite.ToString());
            var status = approveModel.ApproveStatus.Equals(ReportApproveStatusEnum.Pending.ToString());

            var schedule2 = approveModel.ApproveScheduleEnum.Equals(ReportApproveScheduleEnums.Approve.ToString());
            var status2 = approveModel.ApproveStatus.Equals(ReportApproveStatusEnum.Reject.ToString());

            var statusFirst = schedule && status;
            var statusSecond = schedule2 && status2;
            if (statusFirst || statusSecond)
            {
                return true;
            }
            return false;
        }
        #endregion





    }
}
