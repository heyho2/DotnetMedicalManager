using Newtonsoft.Json;
using System.Collections.Generic;

namespace GD.Dtos.Health
{
    /// <summary>
    /// 
    /// </summary>
    public class GetHealthManagerResponseDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string ManagerGuid { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string JobNumber { get; set; }

        /// <summary>
        /// 性别（M/F），默认为M
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityNumber { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 工作城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 1:一级健康管理师（FirstLevel），2：二级健康管理师(SecondLevel)，3：三级健康管理师(ThirdLevel)
        /// </summary>
        public string OccupationGrade { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string PortraitGuid { get; set; }
        /// <summary>
        /// 头像图片地址
        /// </summary>
        public string PortraitImg { get; set; }
        /// <summary>
        /// 职业资格证书
        /// </summary>
        [JsonIgnore]
        public string QualificationCertificateGuid { get; set; }
        /// <summary>
        /// 职业资格证书列表
        /// </summary>
        public List<QualificateCertificate> Certificates { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QualificateCertificate
    {
        /// <summary>
        /// 资格证书Id
        /// </summary>
        public string CertificateGuid { get; set; }
        /// <summary>
        /// 资格证书图片地址
        /// </summary>
        public string CertificateImg { get; set; }
    }
}
