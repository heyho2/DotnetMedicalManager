using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;
using GD.Models.CommonEnum;

namespace GD.Models.Manager
{
    ///<summary>
    ///头条表模型
    ///</summary>
    [Table("t_manager_headline")]
    public class HeadlineModel : BaseModel
    {
        ///<summary>
        ///头条GUID
        ///</summary>
        [Column("headline_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "头条GUID")]
        public string HeadlineGuid
        {
            get;
            set;
        }

        ///<summary>
        ///头条名称
        ///</summary>
        [Column("headline_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "头条名称")]
        public string HeadlineName
        {
            get;
            set;
        }

        ///<summary>
        ///头条简介
        ///</summary>
        [Column("headline_abstract"), Required(ErrorMessage = "{0}必填"), Display(Name = "头条简介")]
        public string HeadlineAbstract
        {
            get;
            set;
        }

        ///<summary>
        ///点击响应目标
        ///</summary>
        [Column("target"), Required(ErrorMessage = "{0}必填"), Display(Name = "点击响应目标")]
        public string Target
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

        ///<summary>
        ///平台类型
        ///</summary>
        [Column("platform_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "平台类型")]
        public string PlatformType
        {
            get;
            set;
        } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();
    }
}