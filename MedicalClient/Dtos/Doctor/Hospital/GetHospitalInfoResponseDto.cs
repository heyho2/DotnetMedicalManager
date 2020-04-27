using GD.Common.Base;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 响应
    /// </summary>
    public class GetHospitalInfoResponseDto : BaseDto
    {

        ///<summary>
        ///所属医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        public string HosName { get; set; }
        ///<summary>
        ///医院简介
        ///</summary>
        public string HosAbstract { get; set; }

        /// <summary>
        /// 医院详情富文本guid
        /// </summary>
        public string HosDetailGuid { get; set; }

        ///<summary>
        ///等级
        ///</summary>
        public string HosLevel { get; set; }

        ///<summary>
        ///位置
        ///</summary>
        public string Location { get; set; }

        ///<summary>
        ///是否可以查询
        ///</summary>
        public string Visibility { get; set; }

        /// <summary>
        /// 医院联系电话
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// 医院logo
        /// </summary>
        public string LogoPicture { get; set; }

        /// <summary>
        /// 导诊链接
        /// </summary>
        public string GuidanceUrl { get; set; }

        /// <summary>
        /// 是否是医院
        /// </summary>
        public bool IsHospital { get; set; }
    }
}
