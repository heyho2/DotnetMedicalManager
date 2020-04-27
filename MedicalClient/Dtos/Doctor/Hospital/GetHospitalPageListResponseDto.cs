using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 获取医院分页列表响应Dto
    /// </summary>
    public class GetHospitalPageListResponseDto:BasePageResponseDto<GetHospitalPageListItemDto>
    {
    }

    /// <summary>
    /// 获取医院分页列表ItemDto
    /// </summary>
    public class GetHospitalPageListItemDto : BaseDto
    {
        ///<summary>
        ///医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }

        ///<summary>
        ///医院logo url
        ///</summary>
        public string LogoUrl { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        public string HosName { get; set; }

        ///<summary>
        /// 医院标签
        ///</summary>
        public string HosTag { get; set; }

        /// <summary>
        /// 方迪外联链接
        /// </summary>
        public string ExternalLink { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        public string HosAbstract { get; set; }
    }
}
