using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Health.HealthManager
{
    public class GetBindRequestDto
    {
        /// <summary>
        /// 手机号码姓名
        /// </summary>
        public string KeyWord { get; set; }
    }
    public class DataUserInfo
    {
        /// <summary>
        /// 首字母
        /// </summary>
        public string Letter { get; set; }
        /// <summary>
        /// 所包含的数
        /// </summary>
        public List<GetBindResponseDto> data { get; set; }
    }
    public class GetBindResponseDto
    {
        /// <summary>
        /// 用户guid
        /// </summary>
        public string ConsumerGuid { get; set; }
        /// <summary>
        /// 当前用户名首字母
        /// </summary>
        public string Letter { get; set; }
        /// <summary>
        /// 绑定状态值
        /// </summary>
        public bool Checked { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 性别（M/F），默认为M
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string PortraitImg { get; set; }
    }
}
