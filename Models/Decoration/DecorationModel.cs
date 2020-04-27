using GD.Common.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Decoration
{
    /// <summary>
    /// 装修记录表
    /// </summary>
    [Table("t_decoration")]
    public class DecorationModel : BaseModel
    {
        ///<summary>
        ///装修记录GUID
        ///</summary>
        [Column("decoration_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "装修记录GUID")]
        public string DecorationGuid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column("decoration_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "名称")]
        public string DecorationName { get; set; }

        /// <summary>
        /// 装修分类guid
        /// </summary>
        [Column("classification_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "装修类别guid")]
        public string ClassificationGuid { get; set; }

        /// <summary>
        /// 页面内容
        /// </summary>
        [Column("content")]
        public string Content { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column("sort")]
        public int Sort { get; set; }
    }
}



