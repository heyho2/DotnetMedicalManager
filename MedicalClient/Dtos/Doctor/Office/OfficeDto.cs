using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Office
{
    /// <summary>
    /// 医院科室dto
    /// </summary>
    public class OfficeDto:BaseDto
    {
        ///<summary>
        ///科室GUID
        ///</summary>
        public string OfficeGuid { get; set; }

        ///<summary>
        ///科室名称
        ///</summary>
        public string OfficeName { get; set; }

        ///<summary>
        ///所属医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }
        ///<summary>
        ///所属医院名称
        ///</summary>
        public string HospitalName { get; set; }
        ///<summary>
        ///上级科室
        ///</summary>
        public string ParentOfficeGuid { get; set; }

        ///<summary>
        ///科室图片
        ///</summary>
        public string PictureGuid { get; set; }

        ///<summary>
        ///科室图片url
        ///</summary>
        public string PictureUrl { get; set; }


        ///<summary>
        ///是否推荐
        ///</summary>
        public bool Recommend { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }


        /// <summary>
		/// 组织ID
		/// </summary>
		/// <remarks>
		/// 多组织平台，每一笔数据都归属于具体的组织
		/// </remarks>
        public string OrgGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 使能标志，默认为 true
        /// </summary>
        /// <remarks>
        /// 可用此字段进行软删除，禁用等相关状态
        /// </remarks>
        public bool Enable
        {
            get;
            set;
        } = true;


        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间，默认为系统当前时间
        /// </summary>
        public DateTime CreationDate
        {
            get;
            set;
        } = DateTime.Now;


        /// <summary>
        /// 最后修改人
        /// </summary>
        public string LastUpdatedBy
        {
            get;
            set;
        }

        /// <summary>
        /// 最后修改日期，默认为系统当前时间
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get;
            set;
        } = DateTime.Now;


    }
}
