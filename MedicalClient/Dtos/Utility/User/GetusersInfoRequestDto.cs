using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Utility.User
{
    /// <summary>
    /// 获取多个用户的信息
    /// </summary>
    public class GetUsersInfoRequestDto : BaseDto
    {
        /// <summary>
        /// 用户id集合
        /// </summary>
        [Required(ErrorMessage = "用户id集合必填")]
        public List<string> UserIds { get; set; }
    }

    /// <summary>
    /// 获取多个用户的信息
    /// </summary>
    public class GetUsersInfoResponseDto : BaseDto
    {
        /// <summary>
        /// 用户guid
        /// </summary>
        public string UserGuid { get; set; }

        ///<summary>
        ///用户昵称
        ///</summary>
        public string NickName { get; set; }

        ///<summary>
        ///真实姓名
        ///</summary>
        public string UserName { get; set; }

        ///<summary>
        ///性别（M/F），默认为M
        ///</summary>
        public string Gender { get; set; } = "M";

        ///<summary>
        ///生日
        ///</summary>
        public DateTime Birthday { get; set; }


        ///<summary>
        ///头像
        ///关联附件表
        ///</summary>
        public string Portrait { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
    }
}
