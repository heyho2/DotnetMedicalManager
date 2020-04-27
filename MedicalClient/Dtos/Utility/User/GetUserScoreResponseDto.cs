using GD.Common.Base;
using System;

namespace GD.Dtos.Utility.User
{
    /// <summary>
    /// 获取分页积分列表 响应
    /// </summary>
    public class GetUserScoreResponseDto : BaseDto
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
        /// 创建日期
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
