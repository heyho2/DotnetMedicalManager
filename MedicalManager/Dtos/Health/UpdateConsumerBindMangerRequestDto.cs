using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateConsumerBindMangerRequestDto : BaseDto
    {
        /// <summary>
        /// 健康管理师
        /// </summary>
        [Required(ErrorMessage = "健康管理师参数未提供")]
        public string ManagerGuid { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        [Required(ErrorMessage = "会员参数未提供")]
        public string ConsumerGuid { get; set; }
    }
}
