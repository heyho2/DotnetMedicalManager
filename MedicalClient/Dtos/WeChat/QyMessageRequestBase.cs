using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 企业微信发送应用消息基类
    /// </summary>
    public class QyMessageRequestBase
    {
        /// <summary>
        /// 指定接收消息的成员，成员ID列表（多个接收者用‘|’分隔，最多支持1000个）。
        /// 特殊情况：指定为”@all”，则向该企业应用的全部成员发送
        /// </summary>
        [JsonProperty("touser")]
        public string ToUser { get; set; }

        /// <summary>
        /// 部门ID列表，多个接收者用‘|’分隔，最多支持100个。
        /// 当touser为@all时忽略本参数
        /// </summary>
        [JsonProperty("toparty")]
        public string ToParty { get; set; }

        /// <summary>
        /// 标签ID列表，多个接收者用‘|’分隔，最多支持100个。当touser为@all时忽略本参数
        /// </summary>
        [JsonProperty("totag")]
        public string ToTag { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [JsonProperty("msgtype")]
        public string MsgType { get; set; }

        /// <summary>
        /// 企业应用的id，整型。企业内部开发，可在应用的设置页面查看；
        /// </summary>
        [JsonProperty("agentid")]
        public string AgentId { get; set; }

        /// <summary>
        /// 表示是否开启重复消息检查，0表示否，1表示是，默认0
        /// </summary>
        [JsonProperty("enable_duplicate_check")]
        public int EnableDuplicateCheck { get; set; } = 0;

        /// <summary>
        /// 表示是否重复消息检查的时间间隔，默认1800s，最大不超过4小时
        /// </summary>
        [JsonProperty("duplicate_check_interval")]
        public int DuplicateCheckInterval { get; set; } = 1800;




    }
    /// <summary>
    /// 发送预警信息
    /// </summary>
    public class QyMessageWarningRequest : QyMessageRequestBase
    {
        /// <summary>
        /// 模板数据信息
        /// </summary>
        public WarningTextcard Textcard { get; set; }
    }
    /// <summary>
    /// 内容信息数据
    /// </summary>
    public class WarningTextcard
    {
        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
        /// <summary>
        /// 是否开启转移
        /// </summary>
        [JsonProperty("enable_id_trans")]
        public int EnableIdTrans { get; set; }
        /// <summary>
        /// 按钮
        /// </summary>
        [JsonProperty("btntxt")]
        public string Btntxt { get; set; } = "详情";
    }
}
