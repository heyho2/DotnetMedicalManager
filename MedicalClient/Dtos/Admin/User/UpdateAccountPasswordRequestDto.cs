using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.User
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class UpdateAccountPasswordRequestDto : BastDto
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
