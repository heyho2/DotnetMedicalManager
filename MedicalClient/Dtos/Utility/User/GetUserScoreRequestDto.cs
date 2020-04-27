using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Utility.User
{
    /// <summary>
    /// 用户积分传入Dto(含分页信息)
    /// </summary>
    public class GetUserScoreRequestDto : BaseDto
    {
        /// <summary>
        /// 页码
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "页码")]
        public int PageNumber { get; set; }

        /// <summary>
        /// 单页记录条数
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "单页记录条数")]
        public int PageSize { get; set; }

        /// <summary>
        /// 积分类型（收入("+")、支出("-")、所有("")）
        /// </summary>
        public string ScoreType { get; set; } = "";

        /// <summary>
        /// 用户类型Guid
        /// </summary>
        public string UserTypeGuid { get; set; }



    }
}
