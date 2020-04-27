using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Office
{
    /// <summary>
    /// 获取医院科室列表
    /// </summary>
    public class GetHospitalOfficeListRequestDto
    {
        /// <summary>
        /// 医院id
        /// </summary>
        public string HospitalGuid { get; set; }

    }
    /// <summary>
    /// 获取医院科室列表 响应
    /// </summary>
    public class GetHospitalOfficeListResponseDto : BaseDto
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
        ///科室图片guid
        ///</summary>
        public string PictureGuid { get; set; }

        /// <summary>
        /// 科室图片Url
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
