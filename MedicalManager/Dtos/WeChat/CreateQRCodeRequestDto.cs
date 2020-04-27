using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.WeChat
{
    /// <summary>
    /// 创建二维码请求参数
    /// </summary>
    public class CreateQRCodeRequestDto
    {
        /// <summary>
        /// 二维码类型
        /// </summary>
        [JsonProperty("action_name")]
        public string ActionName { get; set; }

        /// <summary>
        /// 二维码详细信息
        /// </summary>
        [JsonProperty("action_info")]
        public QRCodeActionInfo ActionInfo { get; set; }

        /// <summary>
        /// 二维码类型枚举
        /// </summary>
        public enum ActionNameEnum
        {
            /// <summary>
            /// 临时的整型参数值
            /// </summary>
            QR_SCENE,

            /// <summary>
            /// 临时的字符串参数值
            /// </summary>
            QR_STR_SCENE,

            /// <summary>
            /// 永久的整型参数值
            /// </summary>
            QR_LIMIT_SCENE,

            /// <summary>
            /// 永久的字符串参数值
            /// </summary>
            QR_LIMIT_STR_SCENE
        }
    }

    /// <summary>
    /// 创建临时二维码请求参数
    /// </summary>
    public class CreateTemporaryQRCodeRequestDto : CreateQRCodeRequestDto
    {
        /// <summary>
        /// 该二维码有效时间，以秒为单位。 最大不超过2592000（即30天），此字段如果不填，则默认有效期为30秒。
        /// </summary>
        [JsonProperty("expire_seconds")]
        public int ExpireSeconds { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QRCodeActionInfo
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("scene")]
        public QRCodeActionInfoSceneBase Scene { get; set; }
    }
    /// <summary>
    /// 场景值ID
    /// </summary>
    public class QRCodeActionInfoSceneBase
    {

    }
    /// <summary>
    /// 数值场景ID
    /// </summary>
    public class QRCodeActionInfoNumberScene : QRCodeActionInfoSceneBase
    {
        /// <summary>
        /// 场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000（目前参数只支持1--100000）
        /// </summary>
        [JsonProperty("scene_id")]
        public string SceneId { get; set; }
    }

    /// <summary>
    ///字符串场景ID
    /// </summary>
    public class QRCodeActionInfoStringScene : QRCodeActionInfoSceneBase
    {
        /// <summary>
        /// 场景值ID（字符串形式的ID），字符串类型，长度限制为1到64
        /// </summary>
        [JsonProperty("scene_str")]
        public string SceneStr { get; set; }
    }
    /// <summary>
    /// 创建二维码响应结果
    /// </summary>
    public class CreateQRCodeResponseDto : WeChatResponseDto
    {
        /// <summary>
        /// 获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码。
        /// </summary>
        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        /// <summary>
        /// 该二维码有效时间，以秒为单位。 最大不超过2592000（即30天）。
        /// </summary>
        [JsonProperty("expire_seconds")]
        public int ExpireSeconds { get; set; }

        /// <summary>
        /// 二维码图片解析后的地址，开发者可根据该地址自行生成需要的二维码图片
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// 业务上二维码过期的时间
        /// </summary>
        public DateTime? Deadline { get; set; }
    }
}
