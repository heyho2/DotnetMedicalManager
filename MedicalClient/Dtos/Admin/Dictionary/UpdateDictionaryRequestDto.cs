using GD.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Admin.Dictionary
{
    /// <summary>
    /// 修改Dictionary
    /// </summary>
    public class UpdateDictionaryRequestDto : BaseDto
    {
        ///<summary>
        ///系统字典GUID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "系统字典GUID")]
        public string DicGuid { get; set; }
        ///<summary>
        ///字典类型CODE(如HABBIT表示生活习惯)
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "字典类型CODE(如HABBIT表示生活习惯)")]
        public string TypeCode { get; set; }

        ///<summary>
        ///配置项CODE，如SMOKE表示抽烟
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "配置项CODE，如SMOKE表示抽烟")]
        public string ConfigCode { get; set; }

        ///<summary>
        ///配置项名称
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "配置项名称")]
        public string ConfigName { get; set; }

        ///<summary>
        ///取值类型，默认为字符类型
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "取值类型，默认为字符类型")]
        public string ValueType { get; set; }

        ///<summary>
        ///取值范围
        ///</summary>
        public string ValueRange { get; set; }

        ///<summary>
        ///排序(理论上相同TYPE_CODE的排序不能相同）
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "排序(理论上相同TYPE_CODE的排序不能相同）")]
        public int Sort { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        [Display(Name = "扩展字段")]
        public string ExtensionField { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}
