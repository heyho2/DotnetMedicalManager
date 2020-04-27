using GD.Common.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GD.Models.Consumer
{
    /// <summary>
    ///
    /// </summary>
    [Table("t_consumer_wechat_subscription")]
    public class WechatSubscriptionModel : BaseModel
    {
        /// <summary>
        /// 关注记录GUID
        /// </summary>
        [Column("subscription_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "关注记录GUID")]
        public string SubscriptionGuid { get; set; }

        /// <summary>
        /// 推荐用户GUID
        /// </summary>
        [Column("recommend_user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "推荐用户GUID")]
        public string RecommendUserGuid { get; set; }

        /// <summary>
        /// 来源端口：医生端、用户端
        /// </summary>
        [Column("entrance"), Required(ErrorMessage = "{0}必填"), Display(Name = "来源端口：医生端、用户端")]
        public string Entrance { get; set; } = EntranceEnum.Consumer.ToString();

        /// <summary>
        /// 关注公众号用户的微信OPENID
        /// </summary>
        [Column("open_id"), Required(ErrorMessage = "{0}必填"), Display(Name = "关注公众号用户的微信OPENID")]
        public string OpenId { get; set; }

        /// <summary>
        /// 来源端口：医生端、用户端
        /// </summary>
        public enum EntranceEnum
        {
            /// <summary>
            /// 医生
            /// </summary>
            [Description("医院")]
            Doctor = 1,
            /// <summary>
            /// 用户
            /// </summary>
            [Description("用户")]
            Consumer = 1,
        }
    }
}