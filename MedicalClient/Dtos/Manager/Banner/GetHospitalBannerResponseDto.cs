using GD.Common.Base;

namespace GD.Dtos.Manager.Banner
{
    /// <summary>
    /// 获取医院Banner 请求
    /// </summary>
    public class GetHospitalBannerRequestDto : BaseDto
    {
        /// <summary>
        /// 医院guid
        /// </summary>
        public string HospitalGuid { get; set; }
    }
}
