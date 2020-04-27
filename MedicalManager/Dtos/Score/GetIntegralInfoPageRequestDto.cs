using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Score
{
    /// <summary>
    /// 积分列表 请求
    /// </summary>
    public class GetIntegralInfoPageRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// user id
        /// </summary>
        [Required]
        public string UserGuid { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAscending { get; set; }
    }
    /// <summary>
    /// 积分列表 响应
    /// </summary>
    public class GetIntegralInfoPageResponseDto : BasePageResponseDto<GetIntegralInfoPageItemDto>
    {
    }
    /// <summary>
    /// 积分列表 item
    /// </summary>
    public class GetIntegralInfoPageItemDto : BaseDto
    {
        ///<summary>
        ///积分项GUID
        ///</summary>
        public string ScoreGuid { get; set; }

        ///<summary>
        ///用户GUID
        ///</summary>
        public string UserGuid { get; set; }

        ///<summary>
        ///积分变化，+/-
        ///</summary>
        public int Variation { get; set; }

        ///<summary>
        ///积分变化原因
        ///</summary>
        public string Reason { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }

        
    }
}
