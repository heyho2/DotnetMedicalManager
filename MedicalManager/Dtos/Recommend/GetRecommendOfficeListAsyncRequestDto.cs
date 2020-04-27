using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Recommend
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
        public string OfficeGuid { get; set; }
        ///<summary>
        ///科室名称
        ///</summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// 所属医院guid
        /// </summary>
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 所属医院名称
        /// </summary>
        public string HospitalName { get; set; }
        ///<summary>
        ///科室图片
        ///</summary>
        public string PictureGuid { get; set; }
        /// <summary>
        /// 图片URL
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
