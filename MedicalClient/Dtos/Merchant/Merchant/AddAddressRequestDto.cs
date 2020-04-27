using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 消费者新增地址请求Dto
    /// </summary>
    public class AddAddressRequestDto : BaseDto
    {
        ///<summary>
        ///接收人
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "接收人")]
        public string Receiver
        {
            get;
            set;
        }

        ///<summary>
        ///接收人手机号
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "接收人手机号")]
        public string Phone
        {
            get;
            set;
        }

        ///<summary>
        ///省
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "省")]
        public string Province
        {
            get;
            set;
        }

        ///<summary>
        ///市
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "市")]
        public string City
        {
            get;
            set;
        }

        ///<summary>
        ///区
        ///</summary>
        public string Area
        {
            get;
            set;
        }

        ///<summary>
        ///省ID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "省ID")]
        public int ProvinceId
        {
            get;
            set;
        }

        ///<summary>
        ///市ID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "市ID")]
        public int CityId
        {
            get;
            set;
        }

        ///<summary>
        ///地区ID
        ///</summary>
        public int AreaId
        {
            get;
            set;
        }

        ///<summary>
        ///详细地址
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "详细地址")]
        public string DetailAddress
        {
            get;
            set;
        }

        ///<summary>
        ///是否是默认地址
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "是否是默认地址")]
        public bool IsDefault
        {
            get;
            set;
        }
    }
}
