using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Consumer
{
    ///<summary>
    ///地址表模型
    ///</summary>
    [Table("t_consumer_address")]
    public class AddressModel : BaseModel
    {
        ///<summary>
        ///地址GUID
        ///</summary>
        [Column("address_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "地址GUID")]
        public string AddressGuid
        {
            get;
            set;
        }

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid
        {
            get;
            set;
        }

        ///<summary>
        ///接收人
        ///</summary>
        [Column("receiver"), Required(ErrorMessage = "{0}必填"), Display(Name = "接收人")]
        public string Receiver
        {
            get;
            set;
        }

        ///<summary>
        ///接收人手机号
        ///</summary>
        [Column("phone"), Required(ErrorMessage = "{0}必填"), Display(Name = "接收人手机号")]
        public string Phone
        {
            get;
            set;
        }

        ///<summary>
        ///省
        ///</summary>
        [Column("province"), Required(ErrorMessage = "{0}必填"), Display(Name = "省")]
        public string Province
        {
            get;
            set;
        }

        ///<summary>
        ///市
        ///</summary>
        [Column("city"), Required(ErrorMessage = "{0}必填"), Display(Name = "市")]
        public string City
        {
            get;
            set;
        }

        ///<summary>
        ///区
        ///</summary>
        [Column("area"), Required(ErrorMessage = "{0}必填"), Display(Name = "区")]
        public string Area
        {
            get;
            set;
        }

        ///<summary>
        ///省ID
        ///</summary>
        [Column("province_id"), Required(ErrorMessage = "{0}必填"), Display(Name = "省ID")]
        public int ProvinceId
        {
            get;
            set;
        }

        ///<summary>
        ///市ID
        ///</summary>
        [Column("city_id"), Required(ErrorMessage = "{0}必填"), Display(Name = "市ID")]
        public int CityId
        {
            get;
            set;
        }

        ///<summary>
        ///地区ID
        ///</summary>
        [Column("area_id"), Required(ErrorMessage = "{0}必填"), Display(Name = "地区ID")]
        public int AreaId
        {
            get;
            set;
        }

        ///<summary>
        ///详细地址
        ///</summary>
        [Column("detail_address"), Required(ErrorMessage = "{0}必填"), Display(Name = "详细地址")]
        public string DetailAddress
        {
            get;
            set;
        }

        ///<summary>
        ///是否是默认地址
        ///</summary>
        [Column("is_default"), Required(ErrorMessage = "{0}必填"), Display(Name = "是否是默认地址")]
        public bool IsDefault
        {
            get;
            set;
        }
    }
}