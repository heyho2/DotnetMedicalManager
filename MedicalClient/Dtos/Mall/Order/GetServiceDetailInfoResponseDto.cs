using GD.Common.Base;
using GD.Dtos.CommonEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Mall.Order
{
    /// <summary>
    /// 获取售后服务单详情响应dto
    /// </summary>
    public class GetServiceDetailInfoResponseDto : BaseDto
    {
        /// <summary>
        /// 服务单号
        /// </summary>
        public string ServiceNo { get; set; }

        /// <summary>
        /// 服务单状态
        /// </summary>
        public AfterSaleServiceStatusEnum ServiceStatus { get; set; }

        /// <summary>
        /// 服务单类型
        /// </summary>
        public AfterSaleServiceTypeEnum ServiceType { get; set; }

        /// <summary>
        /// 服务单状态显示名
        /// </summary>
        public string ServiceStatusDisplay { get; set; }

        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// 售后商品数量
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductPicture { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundFee { get; set; }

        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime? RefundDate { get; set; }

        /// <summary>
        /// 协商历史
        /// </summary>
        public List<GetServiceDetailConsultationDto> Consultations { get; set; }
    }

    /// <summary>
    /// 协商记录
    /// </summary>
    public class GetServiceDetailConsultationDto : BaseDto
    {
        /// <summary>
        /// 协商记录标题
        /// </summary>
        public string ConsultationTitle { get; set; }

        /// <summary>
        /// 协商记录内容
        /// </summary>
        public string ConsultationContent { get; set; }

        /// <summary>
        /// 协商记录生成时间
        /// </summary>
        public DateTime? ConsultationDate { get; set; }

        /// <summary>
        /// 协商发起角色类型
        /// </summary>
        public AfterSaleConsultationRoleEnum RoleType { get; set; }
    }



    /// <summary>
    /// 获取售后服务单详情中间数据dto
    /// </summary>
    public class GetServiceDetailInfoTmpDto : BaseDto
    {
        /// <summary>
        /// 服务单号
        /// </summary>
        public string ServiceNo { get; set; }

        /// <summary>
        /// 服务单状态
        /// </summary>
        public AfterSaleServiceStatusEnum ServiceStatus { get; set; }

        /// <summary>
        /// 服务单类型
        /// </summary>
        public AfterSaleServiceTypeEnum ServiceType { get; set; }

        /// <summary>
        /// 商品guid
        /// </summary>
        public string ProductGuid { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// 售后商品数量
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductPicture { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public int RefundFee { get; set; }

        /// <summary>
        /// 协商记录标题
        /// </summary>
        public string ConsultationTitle { get; set; }

        /// <summary>
        /// 协商记录内容
        /// </summary>
        public string ConsultationContent { get; set; }

        /// <summary>
        /// 协商记录生成时间
        /// </summary>
        public DateTime? ConsultationDate { get; set; }

        /// <summary>
        /// 协商发起角色类型
        /// </summary>
        public AfterSaleConsultationRoleEnum RoleType { get; set; }

        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime? RefundDate { get; set; }
    }
}
