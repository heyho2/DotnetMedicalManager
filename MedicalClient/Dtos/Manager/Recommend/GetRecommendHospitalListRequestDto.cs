using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Dtos.Manager.Recommend
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
        [Column("recommend_guid")]
        public string RecommendGuid { get; set; }

        ///<summary>
        ///所属医院GUID
        ///</summary>
        [Column("hospital_guid")]
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 医院logo
        /// </summary>
        [Column("logo_guid")]
        public string LogoGuid { get; set; }
        /// <summary>
        /// login
        /// </summary>
        public string LogoUrl { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        [Column("hos_name")]
        public string HosName { get; set; }

        ///<summary>
        ///医院简介
        ///</summary>
        [Column("hos_abstract")]
        public string HosAbstract { get; set; }

        ///<summary>
        ///等级
        ///</summary>
        [Column("hos_level")]
        public string HosLevel { get; set; }
        ///<summary>
        ///位置
        ///</summary>
        [Column("location")]
        public string Location { get; set; }
    }
}
