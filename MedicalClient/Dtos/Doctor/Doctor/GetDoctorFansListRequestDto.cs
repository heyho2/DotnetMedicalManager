using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取医生粉丝列表请求Dto
    /// </summary>
    public class GetDoctorFansListRequestDto //: BasePageRequestDto
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 医生guid
        /// </summary>
        public string DoctorGuid { get; set; }
    }

    ///// <summary>
    ///// 获取医生粉丝列表响应 Dto
    ///// </summary>
    //public class GetDoctorFansListResponseDto : BasePageResponseDto<GetDoctorFansListItemDto>
    //{

    //}

    /// <summary>
    /// 获取医生粉丝列表Item Dto
    /// </summary>
    public class GetDoctorFansListItemDto : BaseDto
    {
        /// <summary>
        /// 用户guid
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户头像url
        /// </summary>
        public string PortraitUrl { get; set; }
    }
}
