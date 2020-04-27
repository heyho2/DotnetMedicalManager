using System;
using System.Threading.Tasks;
using GD.Common;
using GD.Common.EnumDefine;
using GD.Dtos.Utility.Score;
using GD.Module;
using GD.Utility;
using Microsoft.AspNetCore.Mvc;

namespace GD.API.Controllers.Utility
{
    /// <summary>
    /// 积分
    /// </summary>
    public class ScoreController : UtilityBaseController
    {
        /// <summary>
        /// 添加积分
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(string))]
        public async Task<IActionResult> AddScore([FromBody]AddScoreDto dto)
        {
            string rulesGuid = await new ScoreRulesBiz().AddScoreByRules(this.UserID, Enum.Parse<ActionEnum>(dto.RulesCode), this.UserType);
            if (!string.IsNullOrWhiteSpace(rulesGuid))
            {
                return Success(rulesGuid, null);
            }
            else
            {
                return Failed(ErrorCode.Unknown, "");
            }
        }

        /// <summary>
        /// 获取用户对应角色的总积分
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<int>))]
        public async Task<IActionResult> GetUserTotalScoreAsync(UserType userType = UserType.Doctor)
        {
            var result = await new ScoreExBiz().GetTotalScore(UserID, userType);
            return Success(result);
        }
    }
}