using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Merchant
{
    ///<summary>
    ///类别扩展
    ///</summary>
    [Table("t_merchant_category_extension")]
    public class CategoryExtensionModel : BaseModel
    {

        ///<summary>
        ///主键
        ///</summary>
        [Column("category_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "主键")]
        public string CategoryGuid { get; set; }

        ///<summary>
        ///服务性类别GUID
        ///</summary>
        [Column("classify_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "服务性类别GUID")]
        public string ClassifyGuid { get; set; }

        ///<summary>
        ///服务性类别名称
        ///</summary>
        [Column("classify_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "服务性类别名称")]
        public string ClassifyName { get; set; }

        ///<summary>
        ///商户GUID
        ///</summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商户GUID")]
        public string MerchantGuid { get; set; }

        ///<summary>
        ///类别名称
        ///</summary>
        [Column("category_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "类别名称")]
        public string CategoryName { get; set; }

        ///<summary>
        ///地址
        ///</summary>
        [Column("address"), Required(ErrorMessage = "{0}必填"), Display(Name = "地址")]
        public string Address { get; set; }

        ///<summary>
        ///详细地址
        ///</summary>
        [Column("detail_address")]
        public string DetailAddress { get; set; }

        ///<summary>
        ///封面图片GUID
        ///</summary>
        [Column("cover_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "封面图片GUID")]
        public string CoverGuid { get; set; }

        ///<summary>
        ///联系电话
        ///</summary>
        [Column("telephone"), Required(ErrorMessage = "{0}必填"), Display(Name = "联系电话")]
        public string Telephone { get; set; }

        ///<summary>
        ///团队介绍
        ///</summary>
        [Column("introduction"), Required(ErrorMessage = "{0}必填"), Display(Name = "团队介绍")]
        public string Introduction { get; set; }

        ///<summary>
        ///纬度
        ///</summary>
        [Column("latitude")]
        public decimal Latitude { get; set; }

        ///<summary>
        ///经度
        ///</summary>
        [Column("longitude")]
        public decimal Longitude { get; set; }

        /// <summary>
        /// 类型下项目取消预约需要提前的时间
        /// </summary>
        [Column("limit_time")]
        public int LimitTime { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public int Sort { get; set; }
    }
}



