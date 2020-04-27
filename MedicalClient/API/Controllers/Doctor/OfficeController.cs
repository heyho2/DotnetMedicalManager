using GD.API.Code;
using GD.Common;
using GD.Doctor;
using GD.Dtos.Doctor.Office;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Doctor
{
    /// <summary>
    /// 科室 控制器
    /// </summary>
    public class OfficeController : DoctorBaseController
    {
        /// <summary>
        /// 获取医院某一级的科室Dto列表（带有图片）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<OfficeDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetHospitalOfficeDtoListAsync([FromBody]GetHospitalOfficeDtoListRequestDto requestDto)
        {
            var response = await new OfficeBiz().GetHospitalOfficeDtoAsync(requestDto.HospitalGuid, requestDto.ParentOfficeGuid);
            return Success(response);
        }

        /// <summary>
        /// 获取医院科室列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetHospitalOfficeListResponseDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetHospitalOfficeListAsync([FromBody]GetHospitalOfficeListRequestDto request)
        {
            var result = await new DoctorOfficeBiz().GetOfficeListByHospitalId(request.HospitalGuid);
            var response = result.Select(a => a.ToDto<GetHospitalOfficeListResponseDto>());
            return Success(response);
        }

        /// <summary>
        /// 获取推荐科室
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetRecommendOfficeItemDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetRecommendOfficeAsync([FromBody]GetRecommendOfficeRequestDto request)
        {
            var result = await new DoctorOfficeBiz().GetOfficeListByHospitalId(request.HospitalGuid, true);
            var response = result.OrderBy(a => a.Sort).Select(a => a.ToDto<GetRecommendOfficeItemDto>()).ToList();
            return Success(response);
        }

        /// <summary>
        /// 获取科室帅选列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetOfficeListPagingResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetOfficeListPaging([FromBody]GetOfficeListPagingRequestDto request)
        {
            var response = await new DoctorOfficeBiz().GetOfficeListPaging(request);
            return Success(response);
        }

        /// <summary>
        /// 获取去重科室
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetDistinctOfficeResponseDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetDistinctOfficeAsync()
        {
            var response = await new DoctorOfficeBiz().GetDistinctOfficeAsync();
            return Success(response);
        }
    }
}
