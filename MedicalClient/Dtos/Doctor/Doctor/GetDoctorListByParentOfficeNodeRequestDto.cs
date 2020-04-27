using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取当前科室和下属所有科室的医生列表请求Dto
    /// </summary>
    public class GetDoctorListByParentOfficeNodeRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 医院guid
        /// </summary>
        [Required(ErrorMessage ="医院guid必填")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 科室guid(不填则表示全部科室)
        /// </summary>
        public string OfficeGuid { get; set; }

        /// <summary>
        /// 医生性别筛选条件
        /// </summary>
        public GenderEnum? Gender { get; set; } = GenderEnum.M;

        /// <summary>
        /// 不需要传入参数
        /// </summary>
        public List<string> OfficeIds { get; set; }
    }

    /// <summary>
    /// 性别枚举
    /// </summary>
    public enum GenderEnum
    {
        /// <summary>
        /// 男性
        /// </summary>
        [Description("男性")]
        M =1,

        /// <summary>
        /// 女性
        /// </summary>
        [Description("女性")]
        F
    }
}
