using GD.API.Code;
using GD.Common;
using GD.DataAccess;
using GD.Doctor;
using GD.Dtos.Doctor.Hospital;
using GD.Models.Doctor;
using GD.Models.Manager;
using GD.Models.Utility;
using GD.Module.WeChat;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GD.API.Controllers.Doctor
{
    /// <summary>
    /// 医院控制器
    /// </summary>
    public class HospitalController : DoctorBaseController
    {
        /// <summary>
        /// 获取所有医院数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetAllHospitalResponseDto>>)), AllowAnonymous]
        public IActionResult GetAllHospital()
        {
            HospitalBiz hospitalBiz = new HospitalBiz();
            var hospitals = hospitalBiz.GetAllHospital();
            if (hospitals == null || !hospitals.Any())
            {
                return Failed(ErrorCode.Empty, "没有获取到医院数据");
            }
            var response = hospitals.Select(a => new GetAllHospitalResponseDto
            {
                HospitalGuid = a.HospitalGuid,
                HosName = a.HosName
            });

            return Success(response);
        }

        /// <summary>
        /// 分页获取医院列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetHospitalPageListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetHospitalPageListAsync([FromBody]GetHospitalPageListRequestDto requestDto)
        {
            var response = await new HospitalBiz().GetHospitalPageListAsync(requestDto);
            return Success(response);
        }

        #region 医院详情
        /// <summary>
        /// 医院详情
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetHospitalInfoResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetHospitalInfoAsync([FromBody]GetHospitalInfoRequestDto request)
        {
            var result = await new HospitalBiz().GetAsync(request.HospitalGuid);
            var logo = await new AccessoryBiz().GetAsync(result?.LogoGuid);
            var response = result.ToDto<GetHospitalInfoResponseDto>();
            response.LogoPicture = $"{logo?.BasePath}{logo?.RelativePath}";//医院logo图片
            return Success(response);
        }

        #endregion
        /// <summary>
        /// 获取职业病资质医院
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetCcupationalDiseaseHospitalResponseDto>>)), AllowAnonymous]
        public IActionResult GetCcupationalDiseaseHospital([FromBody]GetCcupationalDiseaseHospitalRequestDto requestDto)
        {
            HospitalBiz hospitalBiz = new HospitalBiz();
            var hospitalModels = hospitalBiz.GetQualificationHospital(requestDto.PageIndex, requestDto.PageSize, DictionaryType.OccupationalDiseaseQualification);
            if (hospitalModels == null)
            {
                return Failed(ErrorCode.Empty);
            }
            var dtos = hospitalModels.Select(hos => new GetCcupationalDiseaseHospitalResponseDto
            {
                HospitalGuid = hos.HospitalGuid,
                HosName = hos.HosName,
                HosTag = hos.HosTag,
                HosAbstract = hos.HosAbstract,
                Picture = hos.LogoGuid,
                GuidanceUrl = hos.GuidanceUrl
            });
            foreach (var item in dtos)
            {
                var picture = MySqlHelper.GetModelById<AccessoryModel>(item.Picture);
                if (picture != null)
                {
                    item.Picture = picture.BasePath + picture.RelativePath;
                }
                else
                {
                    item.Picture = null;
                }
            }
            return Success(dtos);
        }

        /// <summary>
        /// 搜索医院
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<SearchHospitalResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> SearchHospitalAsync([FromBody]SearchHospitalRequestDto request)
        {
            CommonBiz commonBiz = new CommonBiz();
            if (!string.IsNullOrEmpty(UserID))
            {
                commonBiz.SearchHistory(UserID, request.Keyword);
            }
            commonBiz.HotWordSearch(request.Keyword);
            var response = await new HospitalBiz().SearchHospitalAsync(request);
            foreach (var item in response.CurrentPage)
            {
                item.HosLevel = GetHosLevelName(item.HosLevel);
            }
            return Success(response);
        }

        /// <summary>
        /// 获取指定医院信息
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<HospitalModel>))]
        [AllowAnonymous]
        public async Task<IActionResult> GetHospitalAsync([FromQuery]string hospitalGuid)
        {
            if (string.IsNullOrEmpty(hospitalGuid))
            {
                return Failed(ErrorCode.Empty, "医院不存在，参数不正确");
            }

            var hospitalBiz = new HospitalBiz();

            var hospital = await hospitalBiz.GetAsync(hospitalGuid);

            return Success(hospital);
        }

        /// <summary>
        /// 获取指定医院下二级科室
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, Produces(typeof(ResponseDto<List<OfficeModel>>))]
        public async Task<IActionResult> GetHospitalOfficesAsync([FromQuery]string hospitalGuid)
        {
            var officeBiz = new OfficeBiz();

            var offices = await officeBiz.GetHospitalFirstLevelOfficesAsync(hospitalGuid);

            return Success(offices);
        }

        /// <summary>
        /// 提交医院评价
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> EvaluvationAsync([FromBody]CreateHospitalEvaluvationRequestDto request)
        {
            if (request.Tags.Count() <= 0)
            {
                return Failed(ErrorCode.Empty, "评价标签必填");
            }

            var tag = string.Join("", request.Tags);
            if (tag.Length > 500)
            {
                return Failed(ErrorCode.Empty, "评价标签超过最大长度限制");
            }

            if (request.Score <= 0 || request.Score > 5)
            {
                return Failed(ErrorCode.Empty, "满意度参数不正确");
            }

            var hospitalBiz = new HospitalBiz();
            var hospital = await hospitalBiz.GetAsync(request.HospitalGuid);
            if (hospital is null)
            {
                return Failed(ErrorCode.Empty, "医院不存在，请检查提交参数");
            }

            if (!hospital.Enable)
            {
                return Failed(ErrorCode.Empty, $"医院“{hospital.HosName}”已被禁用，无法提交");
            }

            var officeBiz = new OfficeBiz();
            var office = await officeBiz.GetAsync(request.OfficeGuid);
            if (office is null)
            {
                return Failed(ErrorCode.Empty, "科室不存在，请检查提交参数");
            }

            if (!office.Enable)
            {
                return Failed(ErrorCode.Empty, $"科室“{office.OfficeName}”已被禁用，无法提交");
            }

            if (!office.HospitalGuid.Equals(hospital.HospitalGuid))
            {
                return Failed(ErrorCode.Empty, $"科室“{office.OfficeName}”不属于医院“{hospital.HosName}”，无法提交");
            }

            var evaluationBiz = new HospitalEvaluationBiz();
            var evaluvation = await evaluationBiz.GetAsync(request.EvaluationGuid);

            if (evaluvation is null)
            {
                return Failed(ErrorCode.Empty, "预评论参数不正确，请检查");
            }

            if (!string.IsNullOrEmpty(evaluvation.UserGuid))
            {
                return Failed(ErrorCode.Empty, "已提交，请勿重复提交");
            }

            evaluvation.UserGuid = UserID;
            evaluvation.HospitalGuid = hospital.HospitalGuid;
            evaluvation.OfficeGuid = office.OfficeGuid;
            evaluvation.EvaluationTag = JsonConvert.SerializeObject(request.Tags);
            evaluvation.Score = request.Score;
            evaluvation.ConditionDetail = request.ConditionDetail;
            evaluvation.Anonymous = request.Anonymous;
            evaluvation.CreatedBy = UserID;
            evaluvation.LastUpdatedBy = UserID;

            var result = await evaluationBiz.UpdateAsync(evaluvation);

            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, "提交评论失败，请稍后重试");
            }
            return Success();
        }

        /// <summary>
        /// 获取医院评价
        /// </summary>
        /// <param name="evaluationGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetHospitalEvaluvationResponseDto>))]
        public async Task<IActionResult> GetEvaluvationAsync([FromQuery] string evaluationGuid)
        {
            if (string.IsNullOrEmpty(evaluationGuid))
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            var evaluationBiz = new HospitalEvaluationBiz();
            var evaluvation = await evaluationBiz.GetEvaluationAsync(evaluationGuid);
            if (evaluvation is null || string.IsNullOrEmpty(evaluvation.UserGuid)
                || !evaluvation.UserGuid.Equals(UserID))
            {
                evaluvation = null;
            }

            return Success(evaluvation);
        }

        private string GetHosLevelName(string strEnum)
        {
            foreach (HospitalModel.HosLevelEnum item in Enum.GetValues(typeof(HospitalModel.HosLevelEnum)))
            {
                if (item.ToString().ToLower() == strEnum.ToLower())
                {
                    return item.GetDescription();
                }
            }
            return null;
        }
    }
}
