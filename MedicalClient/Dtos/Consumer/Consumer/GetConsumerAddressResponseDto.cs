using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Consumer.Consumer
{
    /// <summary>
    /// 获取用户地址列表响应Dto
    /// </summary>
    public class GetConsumerAddressResponseDto : BaseDto
    {
        ///<summary>
        ///地址GUID
        ///</summary>
        public string AddressGuid
        {
            get;
            set;
        }

        ///<summary>
        ///用户GUID
        ///</summary>
        public string UserGuid
        {
            get;
            set;
        }

        ///<summary>
        ///接收人
        ///</summary>
        public string Receiver
        {
            get;
            set;
        }

        ///<summary>
        ///接收人手机号
        ///</summary>
        public string Phone
        {
            get;
            set;
        }

        ///<summary>
        ///省
        ///</summary>
        public string Province
        {
            get;
            set;
        }

        ///<summary>
        ///市
        ///</summary>
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
        public int ProvinceId
        {
            get;
            set;
        }

        ///<summary>
        ///市ID
        ///</summary>
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
        public string DetailAddress
        {
            get;
            set;
        }

        ///<summary>
        ///是否是默认地址
        ///</summary>
        public bool IsDefault
        {
            get;
            set;
        }
    }
}
