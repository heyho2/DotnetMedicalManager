using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Hospital
{
    /// <summary>
    /// 获取所有医院数据响应
    /// </summary>
    public class GetAllHospitalResponseDto : BaseDto
    {
        ///<summary>
        ///医院GUID
        ///</summary>
        public string HospitalGuid
        {
            get;
            set;
        }

        ///<summary>
        ///名称
        ///</summary>
        public string HosName
        {
            get;
            set;
        }

    }
}
