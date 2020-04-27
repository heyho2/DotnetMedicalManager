using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取医生详情响应Dto
    /// </summary>
    public class GetDoctorDetailsResponseDto : BaseDto
    {
        ///<summary>
        ///医生GUID
        ///</summary>
        public string DoctorGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        ///<summary>
        ///所属医院GUID
        ///</summary>
        public string HospitalGuid
        {
            get;
            set;
        }
        ///<summary>
        ///所属医院名称
        ///</summary>
        public string HospitalName
        {
            get;
            set;
        }
        ///<summary>
        ///所属科室GUID
        ///</summary>
        public string OfficeGuid
        {
            get;
            set;
        }
        ///<summary>
        ///所属科室GUID
        ///</summary>
        public string OfficeName
        {
            get;
            set;
        }


        ///<summary>
        ///一寸照
        ///</summary>
        public string Portrait { get; set; }

        ///<summary>
        ///一寸照附件guid
        ///</summary>
        public string PortraitGuid { get; set; }

        /// <summary>
        /// 咨询量
        /// </summary>
        public int ConsultationVolume { get; set; } = 0;

        /// <summary>
        /// 粉丝量
        /// </summary>
        public int FansVolume { get; set; } = 0;

        /// <summary>
        /// 评论平均分
        /// </summary>
        public decimal CommentScore { get; set; } = 0M;



        ///<summary>
        ///工作城市
        ///</summary>
        public string WorkCity
        {
            get;
            set;
        }

        ///<summary>
        ///背景
        ///</summary>
        public string Background
        {
            get;
            set;
        }

        ///<summary>
        ///职称名称
        ///</summary>
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 职称guid
        /// </summary>
        public string TitleGuid { get; set; }

        ///<summary>
        ///擅长的标签
        ///</summary>
        public string AdeptTags
        {
            get;
            set;
        }

        /// <summary>
        /// 所获所获荣誉
        /// </summary>
        public string Honor
        {
            get;
            set;
        }
        /// <summary>
        /// 签名附件guid
        /// </summary>
        public string SignatureGuid { get; set; }

        /// <summary>
        /// 签名附件guid
        /// </summary>
        public string SignatureUrl { get; set; }


    }
}
