using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Merchant
{
    ///<summary>
    ///经营范围表模型
    ///</summary>
    [Table("t_merchant_scope")]
    public class ScopeModel : BaseModel
    {
        ///<summary>
        ///经营范围GUID
        ///</summary>
        [Column("scope_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "经营范围GUID")]
        public string ScopeGuid
        {
            get;
            set;
        }

        ///<summary>
        ///经营范围字典Guid
        ///</summary>
        [Column("scope_dic_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "经营范围字典Guid")]
        public string ScopeDicGuid
        {
            get;
            set;
        }

        ///<summary>
        ///排序
        ///</summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序")]
        public int Sort
        {
            get;
            set;
        }

        /// <summary>
        /// 图片GUID
        /// </summary>
        [Column("picture_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "图片GUID")]
        public string PictureGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 商户GUID
        /// </summary>
        [Column("merchant_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "商户GUID")]
        public string MerchantGuid
        {
            get;
            set;
        }
    }
}