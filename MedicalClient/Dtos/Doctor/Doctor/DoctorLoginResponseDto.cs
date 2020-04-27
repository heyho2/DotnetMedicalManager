using GD.Common.EnumDefine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    public class DoctorLoginRequestDto
    {
        /// <summary>
        /// code
        /// </summary>
        [Required(ErrorMessage = "Code必填")]
        public string Code { get; set; }
        /// <summary>
        /// 0:医生移动端 1:医生PC端
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "用户类型")]
        public UserType UserType
        {
            get;
            set;
        }

        /// <summary>
        /// 登录有效天数，默认为1。非正数则表示永不过期
        /// </summary>
        public double Days { get; set; } = 1;
    }
    public class DoctorLoginResponseDto
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// XMPP服务器
        /// </summary>
        public string Xmpp { get; set; }

        /// <summary>
        /// XMPP服务器域名
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// RabbitMQ JS 连接 URL
        /// </summary>
        public string RabbitMQ { get; set; }
        /// <summary>
        /// 是否注册
        /// </summary>
        public bool WhetherRegister { get; set; }
        /// <summary>
        /// 注册状态注册状态： 'reject' 驳回,'approved' 通过审核,'submit' 审核中,'draft' 草稿
        /// </summary>
        public string RegisterState { get; set; } 
        /// <summary>
        /// 原因
        /// </summary>
         public string ApprovalMessage { get; set; }
    }
    public class DoctorLoginModel
    {
        /// <summary>
        /// 医生Id
        /// </summary>
        public string DoctorGuid { get; set; }
        /// <summary>
        /// 医生名
        /// </summary>
        public string DoctorName { get; set; }
    }
}
