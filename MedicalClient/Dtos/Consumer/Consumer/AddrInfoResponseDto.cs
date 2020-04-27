using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 地址信息响应
    /// </summary>
    public abstract class AddrInfoResponseDto : BaseDto
    {
        /// <summary>
        /// 地址Guid
        /// </summary>
        public string AddressGuid { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 地址 省名称
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 地址 市名称
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 地址 区名称
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 地址 省Id
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 地址 市Id
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 地址 区Id
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress { get; set; }

        /// <summary>
        /// 是否默认地址
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
