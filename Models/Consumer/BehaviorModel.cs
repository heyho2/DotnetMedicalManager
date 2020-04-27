using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;
using GD.Models.CommonEnum;

namespace GD.Models.Consumer
{
    /// <inheritdoc />
    /// <summary>
    /// 用户行为表模型
    /// </summary>
    [Table("t_consumer_behavior")]
    public class BehaviorModel : BaseModel
    {
        ///<summary>
        ///行为GUID
        ///</summary>
        [Column("behavior_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "行为GUID")]
        public string BehaviorGuid
        {
            get;
            set;
        }

        ///<summary>
        ///用户GUID
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户GUID")]
        public string UserGuid
        {
            get;
            set;
        }

        ///<summary>
        ///行为类型（枚举）
        ///</summary>
        [Column("behavior_type"), Required(ErrorMessage = "{0}必填"), Display(Name = "行为类型")]
        public string BehaviorType { get; set; } = BehaviorTypeEnum.Page.ToString();

        ///<summary>
        ///目标
        ///</summary>
        [Column("target_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "目标")]
        public string TargetGuid
        {
            get;
            set;
        }

        ///<summary>
        ///进入时间
        ///</summary>
        [Column("entry_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "进入时间")]
        public DateTime EntryDate
        {
            get;
            set;
        }

        ///<summary>
        ///退出时间(默认为下次进入另外一个目标的时间）
        ///</summary>
        [Column("quit_date"), Required(ErrorMessage = "{0}必填"), Display(Name = "退出时间(默认为下次进入另外一个目标的时间）")]
        public DateTime QuitDate
        {
            get;
            set;
        }

        ///<summary>
        ///持续时间（分钟）=退出时间-进入时间
        ///</summary>
        [Column("duration"), Required(ErrorMessage = "{0}必填"), Display(Name = "持续时间（分钟）=退出时间-进入时间")]
        public int Duration
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