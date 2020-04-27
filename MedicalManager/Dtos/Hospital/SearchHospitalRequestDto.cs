using GD.Common.Base;
using System.ComponentModel;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 搜索医院 请求
    /// </summary>
    public class SearchHospitalRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }
    /// <summary>
    /// 搜索医院 响应
    /// </summary>
    public class SearchHospitalResponseDto : BasePageResponseDto<SearchHospitalItemDto>
    {

    }
    /// <summary>
    /// 搜索医院 项
    /// </summary>
    public class SearchHospitalItemDto : BaseDto
    {
        ///<summary>
        ///医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }

        ///<summary>
        ///医院logo GUID
        ///</summary>
        public string LogoUrl { get; set; }

        ///<summary>
        ///详情
        ///</summary>
        public string HosDetailGuid { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        public string HosName { get; set; }

        ///<summary>
        /// 医院标签
        ///</summary>
        public string HosTag { get; set; }

        ///<summary>
        ///简介
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
