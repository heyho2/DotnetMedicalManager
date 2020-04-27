using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Utility.Message
{
    /// <summary>
    /// 消息发送者/接受者用户Dto
    /// </summary>
    public class MessageUserDto
    {
        /// <summary>
        /// 用户guid
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户头像url
        /// </summary>
        public string PortraitUrl { get; set; }

        /// <summary>
        /// 最近一次消息时间
        /// </summary>
        public string CreationDate { get; set; }




    }
}
