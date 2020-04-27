using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 分页获取从下级分销消费得到的积分提成列表请求Dto
    /// </summary>
    public class GetScoresOfDistributionRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 下级分销用户guid
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "下级分销用户guid")]
        public string UserGuid { get; set; }
    }
}
