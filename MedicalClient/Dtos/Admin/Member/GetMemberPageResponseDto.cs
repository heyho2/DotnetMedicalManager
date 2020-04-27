using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.Member
{
    /// <summary>
    /// 会员列表
    /// </summary>
    public class GetMemberPageRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 注册时间 至
        /// </summary>
        public DateTime? EndDate { get; set; }
        
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
    /// 会员列表
    /// </summary>
    public class GetMemberPageResponseDto : BasePageResponseDto<GetMemberPageItemDto>
    {

    }
    /// <summary>
    /// 会员列表
    /// </summary>
    public class GetMemberPageItemDto : BaseDto
    {

        ///<summary>
        ///GUID
        ///</summary>
        public string UserGuid { get; set; }

        ///<summary>
        ///用户昵称
        ///</summary>
        public string NickName { get; set; }

        ///<summary>
        ///真实姓名
        ///</summary>
        public string UserName { get; set; }
        

        ///<summary>
        ///手机号码
        ///</summary>
        public string Phone { get; set; }

        ///<summary>
        ///性别（M/F），默认为M
        ///</summary>
        public string Gender { get; set; }

        ///<summary>
        ///生日
        ///</summary>
        public DateTime Birthday { get; set; }
        
        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderQty { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal OrderTotalAmount { get; set; }
        /// <summary>
        /// 平均订单金额
        /// </summary>
        public decimal OrderAverage { get; set; }

        /// <summary>
        /// 创建时间，默认为系统当前时间   
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 使能标志，默认为 true
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 最后一次消费时间
        /// </summary>
        public DateTime? LastBuyDate { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public int Variation { get; set; }
    }
}
