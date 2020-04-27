using GD.Common.Base;
using GD.Dtos.Doctor.Doctor;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Health.HealthManager
{
    /// <summary>
    /// 获取健康管理师基础信息响应Dto
    /// </summary>
    public class GetHealthManagerBasicInfoResponseDto : BaseDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Portrait { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 性别： 男-M；女-F
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 职业等级：一级健康管理师（FirstLevel），二级健康管理师(SecondLevel)，三级健康管理师(ThirdLevel)
        /// </summary>
        public string OccupationGrade { get; set; }
    }
}
