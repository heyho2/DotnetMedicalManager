using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GD.Common.Base;

namespace GD.Models.Manager
{
    ///<summary>
    ///系统字典表模型
    ///</summary>
    [Table("t_manager_dictionary")]
    public class DictionaryModel : BaseModel
    {
        ///<summary>
        ///系统字典GUID
        ///</summary>
        [Column("dic_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "系统字典GUID")]
        public string DicGuid { get; set; }

        ///<summary>
        ///字典类型CODE(如HABBIT表示生活习惯)
        ///</summary>
        [Column("type_code"), Required(ErrorMessage = "{0}必填"), Display(Name = "字典类型CODE(如HABBIT表示生活习惯)")]
        public string TypeCode { get; set; }

        ///<summary>
        ///字段类型值（如生活习惯）
        ///</summary>
        [Column("type_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "字段类型值（如生活习惯）")]
        public string TypeName { get; set; }

        ///<summary>
        ///配置项CODE，如SMOKE表示抽烟
        ///</summary>
        [Column("config_code"), Required(ErrorMessage = "{0}必填"), Display(Name = "配置项CODE，如SMOKE表示抽烟")]
        public string ConfigCode { get; set; }

        ///<summary>
        ///配置项名称
        ///</summary>
        [Column("config_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "配置项名称")]
        public string ConfigName { get; set; }

        ///<summary>
        ///取值类型，默认为字符类型
        ///</summary>
        [Column("value_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "取值类型，默认为字符类型")]
        public string ValueType { get; set; }

        ///<summary>
        ///取值范围
        ///</summary>
        [Column("value_range"), Required(ErrorMessage = "{0}必填"), Display(Name = "取值范围")]
        public string ValueRange { get; set; }


        ///<summary>
        ///父GUID
        ///</summary>
        [Column("parent_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "父GUID")]
        public string ParentGuid { get; set; }

        ///<summary>
        ///排序(理论上相同TYPE_CODE的排序不能相同）
        ///</summary>
        [Column("sort"), Required(ErrorMessage = "{0}必填"), Display(Name = "排序(理论上相同TYPE_CODE的排序不能相同）")]
        public int Sort { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        [Column("extension_field"), Display(Name = "扩展字段")]
        public string ExtensionField { get; set; }

    }
}