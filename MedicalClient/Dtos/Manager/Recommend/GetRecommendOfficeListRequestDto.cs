using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GD.Dtos.Manager.Recommend
{
    /// <summary>
    /// 获取科室推荐 请求
    /// </summary>
    public class GetRecommendOfficeListAsyncRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 推荐Guid
        /// </summary>
        public string RecommendGuid { get; set; }
        /// <summary>
        /// 医院Guid
        /// </summary>
        public string HospitalGuid { get; set; }
    }
    /// <summary>
    /// 获取医生推荐列表 响应
    /// </summary>
    public class GetRecommendOfficeListResponseDto : BasePageResponseDto<GetRecommendOfficeListItemDto>
    {
    }
    /// <summary>
    /// 获取医生推荐列表 项
    /// </summary>
    public class GetRecommendOfficeListItemDto : BaseDto
    {
        ///<summary>
        ///科室GUID
        ///</summary>
        [Column("office_guid")]
        public string OfficeGuid { get; set; }
        ///<summary>
        ///科室名称
        ///</summary>
        [Column("office_name")]
        public string OfficeName { get; set; }
        /// <summary>
        /// 所属医院guid
        /// </summary>
        [Column("hospital_guid")]
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 所属医院名称
        /// </summary>
        [Column("hospital_name")]
        public string HospitalName { get; set; }
        ///<summary>
        ///科室图片
        ///</summary>
        [Column("picture_guid")]
        public string PictureGuid { get; set; }
        /// <summary>
        /// 图片URL
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
