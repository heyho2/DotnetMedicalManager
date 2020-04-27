using GD.Common.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取医生列表 请求
    /// </summary>
    public class GetDoctorListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 男医生女医生（1，0，null）
        /// </summary>
        [Range(0, 1)]
        public int? Gender { get; set; }

        /// <summary>
        /// 是否专家
        /// </summary>
        public bool? IsExpert { get; set; }
        /// <summary>
        /// 排序类型(1评分,2工龄,3默认)
        /// </summary>
        public SortMenu Sort { get; set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public enum SortMenu
        {
            /// <summary>
            /// 评分
            /// </summary>
            [Description("评分")]
            Score = 1,
            /// <summary>
            /// 工龄
            /// </summary>
            [Description("工龄")]
            Experience = 2,
            /// <summary>
            /// 默认
            /// </summary>
            [Description("工龄")]
            Default = 3,
        }
    }

    /// <summary>
    /// 获取医生列表 响应
    /// </summary>
    public class GetDoctorListResponseDto : BasePageResponseDto<GetDoctorListItemDto>
    {

    }
    /// <summary>
    /// 获取医生列表 项
    /// </summary>
    public class GetDoctorListItemDto : BaseDto
    {
        ///<summary>
        ///医生GUID
        ///</summary>
        public string DoctorGuid { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        ///<summary>
        ///所属医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }

        ///<summary>
        ///所属医院名称
        ///</summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 医院图片
        /// </summary>
        public string HospitalPicture { get; set; }

        ///<summary>
        ///所属科室GUID
        ///</summary>
        public string OfficeGuid { get; set; }

        ///<summary>
        ///所属科室名称
        ///</summary>
        public string OfficeName { get; set; }

        ///<summary>
        ///职称
        ///关联字典表获取
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        ///擅长的标签
        ///</summary>
        public string AdeptTags { get; set; }

        /// <summary>
        /// 所获所获荣誉
        /// </summary>
        public string Honor { get; set; }

        /// <summary>
        /// 一寸照
        /// </summary>
        public string Picture { get; set; }
    }
}
