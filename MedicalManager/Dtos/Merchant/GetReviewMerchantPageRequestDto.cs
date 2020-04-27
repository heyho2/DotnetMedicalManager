using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Merchant
{
    /// <summary>
    /// 审核商家列表
    /// </summary>
    public class GetReviewMerchantPageRequestDto : BasePageRequestDto, IBaseOrderBy
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
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
    /// 审核商家列表
    /// </summary>
    public class GetReviewMerchantPageResponseDto : BasePageResponseDto<GetReviewMerchantPageItemDto>
    {


    }
    /// <summary>
    /// 审核商家列表
    /// </summary>
    public class GetReviewMerchantPageItemDto : BaseDto
    {
        ///<summary>
        ///商户GUID
        ///</summary>
        public string MerchantGuid { get; set; }

        ///<summary>
        ///商户名
        ///</summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 账号申请状态
        /// 'reject','approved','submit','draft'
        /// </summary>
        public string Status { get; set; }

        ///<summary>
        ///签名附件guid
        ///</summary>
        public string SignatureGuid { get; set; }
        /// <summary>
        /// 签名附件guid
        /// </summary>
        public string SignatureUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// 商家电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 最后审核时间
        /// </summary>
        public DateTime? LastUpdatedDate { get; set; }
        /// <summary>
        /// 注册手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// username
        /// </summary>
        public string UserName { get; set; }
    }
}
