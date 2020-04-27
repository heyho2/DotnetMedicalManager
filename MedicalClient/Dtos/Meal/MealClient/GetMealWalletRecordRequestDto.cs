using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;

namespace GD.Dtos.Meal.MealClient
{
    /// <summary>
    /// 获取钱包流水分页记录请求dto
    /// </summary>
    public class GetMealWalletRecordRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 账户所属医院guid
        /// </summary>
        [Required(ErrorMessage ="医院guid必填")]
        public string HospitalGuid { get; set; }

        /// <summary>
        /// 用户guid，选填,默认为当前登录用户guid
        /// </summary>
        public string UserGuid { get; set; }
    }

    /// <summary>
    /// 获取钱包流水分页记录响应dto
    /// </summary>
    public class GetMealWalletRecordResponseDto : BasePageResponseDto<GetMealWalletRecordItemDto>
    {
       
    }

    /// <summary>
    /// 获取钱包流水分页记录item dto
    /// </summary>
    public class GetMealWalletRecordItemDto : BaseDto
    {
        /// <summary>
        /// 账户类型
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        /// 流水内容描述
        /// </summary>
        public string AccountDetailDescription { get; set; }

        /// <summary>
        /// 流水金额
        /// </summary>
        public decimal AccountDetailFee { get; set; }

        /// <summary>
        /// 收支类型，0：收入，1：支出
        /// </summary>
        public sbyte AccountDetailIncomeType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationDate { get; set; }


    }

}
