using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Consumer.Consumer
{
    /// <inheritdoc />
    ///添加用户浏览记录RequestDto
    public class AddConsumerBrowseInfoRequestDto : BaseDto
    {
        ///<summary>
        ///行为类型（枚举）
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "行为类型")]
        public string BehaviorType { get; set; } = BehaviorTypeEnum.Page.ToString();
        ///<summary>
        ///目标Guid
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "目标Guid")]
        public string TargetGuid
        {
            get;
            set;
        }

        ///<summary>
        ///进入时间
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "进入时间")]
        public DateTime EntryDate
        {
            get;
            set;
        }

        ///<summary>
        ///退出时间(默认为下次进入另外一个目标的时间）
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "退出时间(默认为下次进入另外一个目标的时间）")]
        public DateTime QuitDate
        {
            get;
            set;
        }

        ///<summary>
        ///持续时间（分钟）=退出时间-进入时间
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "持续时间（分钟）=退出时间-进入时间")]
        public int Duration { get; set; }

        ///<summary>
        ///平台类型
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "平台类型")]
        public string PlatformType
        {
            get;
            set;
        } = GD.Common.EnumDefine.PlatformType.CloudDoctor.ToString();

        /// <summary>
        /// 行为类型枚举
        /// </summary>
        public enum BehaviorTypeEnum
        {
            /// <summary>
            /// 页面
            /// </summary>
            [Description("页面")]
            Page,
            /// <summary>
            /// 产品
            /// </summary>
            [Description("产品")]
            Product,
            /// <summary>
            /// 文章
            /// </summary>
            [Description("文章")]
            Article,
            /// <summary>
            /// 动作、活动
            /// </summary>
            [Description("动作")]
            Movement
        }


    }
}
