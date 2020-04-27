using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Recommend
{
    /// <summary>
    /// 获取推荐医院列表 请求
    /// </summary>
    public class GetRecommendHospitalListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 推荐Guid
        /// </summary>
        public string RecommendGuid { get; set; }
    }

    /// <summary>
    /// 获取推荐医院列表 响应
    /// </summary>
    public class GetRecommendHospitalListResponseDto : BasePageResponseDto<GetHospitalRecommendListItemDto>
    {
    }
    /// <summary>
    /// 获取推荐医院列表 项
    /// </summary>
    public class GetHospitalRecommendListItemDto : BaseDto
    {
        /// <summary>
        /// 推荐id
        /// </summary>
        public string RecommendGuid { get; set; }

        ///<summary>
        ///所属医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 医院logo
        /// </summary>
        public string LogoGuid { get; set; }
        /// <summary>
        /// login
        /// </summary>
        public string LogoUrl { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        public string HosName { get; set; }

        ///<summary>
        ///医院简介
        ///</summary>
        public string HosAbstract { get; set; }

        ///<summary>
        ///等级
        ///</summary>
        public string HosLevel { get; set; }
        ///<summary>
        ///位置
        ///</summary>
        public string Location { get; set; }
    }
}
