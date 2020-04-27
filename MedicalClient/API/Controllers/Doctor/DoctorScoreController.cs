using GD.Common;
using GD.Doctor;
using GD.Dtos.Doctor.Score;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.API.Controllers.Doctor
{
    /// <summary>
    /// 医生积分控制器
    /// </summary>
    public class DoctorScoreController : DoctorBaseController
    {
        /// <summary>
        /// 获取医生行为
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetDoctorScoreResponseDto>>))]
        public async Task<IActionResult> GetDoctorActionList()
        {
            return Success(await new DoctorScoreBiz().GetScoreRulesModel(Common.EnumDefine.UserType.Doctor));
        }

        /// <summary>
        /// 获取医生积分所有信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(GetDoctorScoreAllInfoResponseDto))]
        public async Task<IActionResult> GetDoctorScoreAllInfo([FromBody]GetDoctorScoreAllInfoRequestDto requestDto)
        {
            return Success(await new DoctorScoreBiz().GetDoctorScoreAllInfo(this.UserID, requestDto));
        }

        /// <summary>
        /// 查询医生积分
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<ScorePageDto<GetDoctorScoreResponseDto>>))]
        public async Task<IActionResult> GetDoctorScore([FromBody]GetDoctorScoreRequestDto requestDto)
        {
            return Success(await new DoctorScoreBiz().GetDoctorScoreResponseDtoList(this.UserID, requestDto));
        }
    }
}