using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.User
{
    /// <summary>
    /// 添加账号
    /// </summary>
    public class AddAccountRequestDto : BaseDto
    {
        ///<summary>
        ///账号
        ///</summary>
        [Required]
        public string Account { get; set; }
        ///<summary>
        ///密码
        ///</summary>
        [Required, MaxLength(32, ErrorMessage = "请输入正确的Md5值"), MinLength(32, ErrorMessage = "请输入正确的Md5值")]
        public string Password { get; set; }

        ///<summary>
        ///部门ID
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "部门ID")]
        public string OrganizationGuid { get; set; }

        /// <summary>
        /// 超级管理员
        /// </summary>
        public bool IsSuper { get; set; }

        ///<summary>
        ///EMAIL
        ///</summary>
        [EmailAddress(ErrorMessage ="email格式不正确")]
        public string Email { get; set; }

        ///<summary>
        ///微信号
        ///</summary>
        public string WechatOpenid { get; set; }

        ///<summary>
        ///真实姓名
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "真实姓名")]
        public string UserName { get; set; }

        ///<summary>
        ///手机号码
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "手机号码")]
        public string Phone { get; set; }

        ///<summary>
        ///性别（M/F），默认为M
        ///</summary>
        [Required(ErrorMessage = "{0}必填"), Display(Name = "性别（M/F），默认为M")]
        public string Gender { get; set; }

        ///<summary>
        ///生日
        ///</summary>
        public DateTime? Birthday { get; set; }

        ///<summary>
        ///头像
        ///</summary>
        public string PortraitGuid { get; set; }
        /// <summary>
        /// 是否能登陆
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 角色列表
        /// </summary>
        public string[] Roles { get; set; }
        
    }
}
