using GD.Common.Base;
using System.Collections.Generic;

namespace GD.Dtos.Doctor.Doctor
{
    ///<summary>
    ///医院
    ///</summary>
    public class GetHospitalOfficeTreeItemDto : BaseDto
    {
        ///<summary>
        ///医院GUID
        ///</summary>
        public string HospitalGuid { get; set; }

        ///<summary>
        ///详情
        ///关联富文本表
        ///</summary>
        public string HosDetail { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        public string HosName { get; set; }

        ///<summary>
        ///医院标签
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
        /// 是否可查询
        /// </summary>
        public bool Visibility { get; set; } = true;

        /// <summary>
        /// 下属科室
        /// </summary>
        public List<GetHospitalOfficeTreeOfficeItemDto> Offeces { get; set; }
    }
}