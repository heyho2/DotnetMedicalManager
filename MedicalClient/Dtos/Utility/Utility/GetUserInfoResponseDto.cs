using System;
using System.Collections.Generic;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Utility.Utility
{
    /// <summary>
    /// 获取用户信息
    /// </summary>
    public class GetUserInfoResponseDto : BaseDto
    {
        ///<summary>
        ///用户昵称
        ///</summary>
        public string NickName
        {
            get;
            set;
        }

        ///<summary>
        ///真实姓名
        ///</summary>
        public string UserName
        {
            get;
            set;
        }

        ///<summary>
        ///性别（M/F），默认为M
        ///</summary>
        public string Gender
        {
            get;
            set;
        } = "M";

        ///<summary>
        ///生日
        ///</summary>
        public DateTime? Birthday
        {
            get;
            set;
        }

        ///<summary>
        ///身份证号
        ///</summary>
        public string IdentityNumber
        {
            get;
            set;
        }

        ///<summary>
        ///头像
        ///关联附件表
        ///</summary>
        public string Portrait
        {
            get;
            set;
        }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone
        {
            get;
            set;
        }
    }
}
