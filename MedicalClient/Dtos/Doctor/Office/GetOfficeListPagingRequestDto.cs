using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Office
{
    /// <summary>
    /// 科室筛选列表 请求
    /// </summary>
    public class GetOfficeListPagingRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 科室名称 
        /// </summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// 医生名称
        /// </summary>
        public string HospitalName { get; set; }
    }
    /// <summary>
    /// 科室筛选列表 响应
    /// </summary>
    public class GetOfficeListPagingResponseDto : BasePageResponseDto<GetOfficeListPagingItemDto>
    {

    }
    /// <summary>
    /// 科室筛选列表 项
    /// </summary>
    public class GetOfficeListPagingItemDto : BaseDto
    {
        ///<summary>
        ///科室GUID
        ///</summary>
        public string OfficeGuid { get; set; }

        ///<summary>
        ///科室名称
        ///</summary>
        public string OfficeName { get; set; }

        ///<summary>
        ///所属医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }
        ///<summary>
        ///所属医院名称
        ///</summary>
        public string HospitalName { get; set; }
        ///<summary>
        ///上级科室
        ///</summary>
        public string ParentOfficeGuid { get; set; }

        ///<summary>
        ///是否推荐
        ///</summary>
        public bool Recommend { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
        /// <summary>
        /// 科室图片
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
