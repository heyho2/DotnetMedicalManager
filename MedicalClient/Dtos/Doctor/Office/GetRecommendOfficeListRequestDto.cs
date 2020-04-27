using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Office
{
    /// <summary>
    /// 获取推荐科室
    /// </summary>
    public class GetRecommendOfficeRequestDto
    {
        /// <summary>
        /// 医院id
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "医院GUID")]
        public string HospitalGuid { get; set; }
    }
    /// <summary>
    /// 获取推荐科室 响应
    /// </summary>
    public class GetRecommendOfficeItemDto : BaseDto
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
        ///科室图片
        ///</summary>
        public string PictureGuid { get; set; }
    }
}
