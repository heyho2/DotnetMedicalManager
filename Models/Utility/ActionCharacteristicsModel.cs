using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;
using GD.Common.EnumDefine;

namespace GD.Models.Utility
{
    /// <summary>
    /// 行为特性model
    /// </summary>
    [Table("t_utility_action_characteristics")]
    public class ActionCharacteristicsModel : BaseModel
    {
        /// <summary>
        /// 行为特性GUID
        /// </summary>
        [Column("action_characteristics_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "行为特性GUID")]
        public string ActionCharacteristicsGuid { get; set; }

        /// <summary>
        /// 行为特性code
        /// </summary>
        [Column("action_characteristics_code"), Required(ErrorMessage = "{0}必填"), Display(Name = "行为特性code")]
        public string ActioCcharacteristicsCode { get; set; }

        /// <summary>
        /// 行为特性名称
        /// </summary>
        [Column("action_characteristics_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "行为特性名称")]
        public string ActionCharacteristicsName { get; set; }

        /// <summary>
        /// 行为特征类型
        /// </summary>
        [Column("action_characteristics_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "行为特征类型")]
        public ActionCharacteristicsEnum ActionCharacteristicsType { get; set; }
    }
}