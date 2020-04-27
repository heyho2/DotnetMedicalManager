using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Mall.Product
{
    /// <summary>
    /// 修改商品库存信息：追加库存、修改警戒库存（智慧云医）请求Dto
    /// </summary>
    public class ModifyProductInventoryOfDoctorCloudRequestDto:BaseDto
    {
        /// <summary>
        /// 商品guid
        /// </summary>
        [Required(ErrorMessage = "商品guid必填")]
        public string ProductGuid { get; set; }

        /// <summary>
        /// 追加库存数量
        /// </summary>
        public int ReplenishInventory { get; set; } = 0;

        /// <summary>
        /// 警戒库存
        /// </summary>
        public int WarningInventory { get; set; } = 0;
    }
}
