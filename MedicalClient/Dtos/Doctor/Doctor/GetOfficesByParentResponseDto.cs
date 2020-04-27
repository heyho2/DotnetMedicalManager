using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取下属科室响应Dto
    /// </summary>
    public class GetOfficesByParentResponseDto : BaseDto
    {
        ///<summary>
        ///科室GUID
        ///</summary>
        public string OfficeGuid
        {
            get;
            set;
        }

        ///<summary>
        ///科室名称
        ///</summary>
        public string OfficeName
        {
            get;
            set;
        }
    }
}
