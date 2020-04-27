using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Manager
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_manager_page_configuration")]
    public class PageConfigurationModel : BaseModel
    {

        /// <summary>
        /// 
        /// </summary>
        [Column("page_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string PageGuid { get; set; }

        /// <summary>
        /// 上级页面GUID
        /// </summary>
        [Column("parent_guid")]
        public string ParentGuid { get; set; }

        /// <summary>
        /// 页面名称
        /// </summary>
        [Column("page_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "页面名称")]
        public string PageName { get; set; }

        /// <summary>
        /// 页面地址
        /// </summary>
        [Column("page_url"), Required(ErrorMessage = "{0}必填"), Display(Name = "页面地址")]
        public string PageUrl { get; set; }

        /// <summary>
        /// 上级页面ID,2/3/6,用于前端回显展示数据
        /// </summary>
        [Column("url_id")]
        public string UrlId { get; set; }

    }
}
