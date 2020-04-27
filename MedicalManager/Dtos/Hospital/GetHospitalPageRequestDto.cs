using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Hospital
{
    /// <summary>
    /// 获取医院 列表 请求
    /// </summary>
    public class GetHospitalPageRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RegisteredBeginDate { get; set; }
        /// <summary>
        /// 注册时间 至
        /// </summary>
        public DateTime? RegisteredEndDate { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAscending { get; set; }
    }
    /// <summary>
    /// 获取医院 列表 响应
    /// </summary>
    public class GetHospitalPageResponseDto : BasePageResponseDto<GetHospitalPageItemDto>
    {

    }
    /// <summary>
    /// 获取医院 列表 项
    /// </summary>
    public class GetHospitalPageItemDto : BaseDto
    {
        ///<summary>
        ///医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }

        ///<summary>
        ///医院logo GUID
        ///</summary>
        public string LogoUrl { get; set; }
        /// <summary>
        /// 是否是医院 true:是,false:诊所
        /// </summary>
        public bool IsHospital { get; set; }

        ///<summary>
        ///详情
        ///</summary>
        public string HosDetailGuid { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        public string HosName { get; set; }

        ///<summary>
        /// 医院标签
        ///</summary>
        public string HosTag { get; set; }

        ///<summary>
        ///简介
        ///</summary>
        public string HosAbstract { get; set; }

        ///<summary>
        ///等级 
        ///</summary>
        public string HosLevel { get; set; }

        ///<summary>
        ///位置
        ///</summary>
        public string Location { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 创建时间，默认为系统当前时间
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisteredDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 导诊链接
        /// </summary>
        public string GuidanceUrl { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Longitude { get; set; }
    }
}
