using GD.Common.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Payment
{
    /// <summary>
    /// 
    /// </summary>
    [Table("t_wechat_scene")]
    public class WechatSceneModel : BaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("scene_id"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "")]
        public string SceneId { get; set; }

        /// <summary>
        /// 场景名称
        /// </summary>
        [Column("scene_name"), Required(ErrorMessage = "{0}必填"), Display(Name = "场景名称")]
        public string SceneName { get; set; }

        /// <summary>
        /// 动作枚举
        /// </summary>
        [Column("action")]
        public string Action { get; set; }

        /// <summary>
        /// 扩展数据
        /// </summary>
        [Column("extension"), Required(ErrorMessage = "{0}必填"), Display(Name = "扩展数据")]
        public string Extension { get; set; }
    }
}



