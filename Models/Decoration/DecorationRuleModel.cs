using GD.Common.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Decoration
{
    /// <summary>
    /// 装修规则
    /// </summary>
    [Table("t_decoration_rule")]
    public class DecorationRuleModel : BaseModel
    {
        /// <summary>
        /// 装修规则
        /// </summary>
        [Column("rule_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "装修规则")]
        public string RuleGuid { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        [Column("row_num"), Required(ErrorMessage = "{0}必填"), Display(Name = "行数")]
        public int RowNum { get; set; }

        /// <summary>
        /// 行数规则：Equal-等于,LessThan-小于或等于,GreaterThan-大于或等于
        /// </summary>
        [Column("row_rule"), Required(ErrorMessage = "{0}必填"), Display(Name = "行数规则：Equal-等于,LessThan-小于或等于,GreaterThan-大于或等于")]
        public string RowRule { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        [Column("column_num"), Required(ErrorMessage = "{0}必填"), Display(Name = "列数")]
        public int ColumnNum { get; set; }

        /// <summary>
        /// 列数规则：Equal-等于,LessThan-小于或等于,GreaterThan-大于或等于
        /// </summary>
        [Column("column_rule"), Required(ErrorMessage = "{0}必填"), Display(Name = "列数规则：Equal-等于,LessThan-小于或等于,GreaterThan-大于或等于")]
        public string ColumnRule { get; set; }

        /// <summary>
        /// 行图片样式：Slideshow-轮播图，Tile-平铺图
        /// </summary>
        [Column("style"), Required(ErrorMessage = "{0}必填"), Display(Name = "行图片样式：Slideshow-轮播图，Tile-平铺图")]
        public string Style { get; set; }

       
    }
}



