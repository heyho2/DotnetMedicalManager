using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Payment
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_wechat_message_template")]
    public class WechatMessageemplateModel : BaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("template_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string TemplateGuid { get; set; }

        /// <summary>
        /// 微信模板消息ID
        /// </summary>
        [Column("wechat_template_id"), Required(ErrorMessage = "{0}必填"), Display(Name = "微信模板消息ID")]
        public string WechatTemplateId { get; set; }

        /// <summary>
        /// 微信模板消息标题
        /// </summary>
        [Column("template_title"), Required(ErrorMessage = "{0}必填"), Display(Name = "微信模板消息标题")]
        public string TemplateTitle { get; set; }

        /// <summary>
        /// 模板消息结构
        /// </summary>
        [Column("structure")]
        public string Structure { get; set; }

        /// <summary>
        /// JSON字符串，用于描述键值、键标题、键内容、内容颜色
        /// </summary>
        [Column("link"), Required(ErrorMessage = "{0}必填"), Display(Name = "JSON字符串，用于描述键值、键标题、键内容、内容颜色")]
        public string Link { get; set; }

        /// <summary>
        /// 跳转类型（LINK:'链接',PRODUCT:'商品',PAGE:'页面'）
        /// </summary>
        [Column("link_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "跳转类型（LINK:'链接',PRODUCT:'商品',PAGE:'页面'）")]
        public string LinkType { get; set; }

        /// <summary>
        /// 跳转链接ID,例如：ID1/ID2,用于前端回显绑定
        /// </summary>
        [Column("link_id"), Required(ErrorMessage = "{0}必填"), Display(Name = "跳转链接ID,例如：ID1/ID2,用于前端回显绑定")]
        public int LinkId { get; set; }
    }
}



