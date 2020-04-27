using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Manager.Banner
{
    /// <summary>
    /// 修改banner数据
    /// </summary>
    public class ModifyBannerInfoRequestDto
    {
        /// <summary>
        /// Banner所有者Guid
        /// </summary>
        [Required(ErrorMessage = "Banner所有者Guid必填")]
        public string OwnerGuid { get; set; }

        /// <summary>
        /// Banners
        /// </summary>
        [Required(ErrorMessage = "Banners列表必填")]
        public List<BannerBaseDto> Banners { get; set; }
    }
}
