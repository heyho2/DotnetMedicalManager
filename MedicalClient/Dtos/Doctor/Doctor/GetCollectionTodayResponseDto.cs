using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 今日未(收款)数据集
    /// </summary>
    public class GetMoneyTodayResponseDto
    {
        /// <summary>
        /// 收款数据
        /// </summary>
        public GetCollectionTodayResponseDto Collected { get; set; }
        /// <summary>
        /// 未收款数据
        /// </summary>
        public GetUncollectedTodayResponseDto Uncollected { get; set; }
    }
    /// <summary>
    /// 获取诊所当日收款数据
    /// </summary>
    public class GetCollectionTodayResponseDto
    {
        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal AmountCollected { get; set; }

        /// <summary>
        /// 较昨日上涨百分比,例如上涨为25.5%，则此处的值为25.5
        /// </summary>
        public decimal Increase { get; set; }
        /// <summary>
        /// 所占百分比
        /// </summary>
        public decimal Percentage { get; set; }
    }
    /// <summary>
    /// 未收款
    /// </summary>
    public class GetUncollectedTodayResponseDto
    {
        /// <summary>
        /// 未收款金额
        /// </summary>
        public decimal AmountUncollected { get; set; }

        /// <summary>
        /// 较昨日上涨百分比,例如上涨为25.5%，则此处的值为25.5
        /// </summary>
        public decimal Increase { get; set; }
        /// <summary>
        /// 所占百分比
        /// </summary>
        public decimal Percentage { get; set; }
    }
    /// <summary>
    /// 本月(年)收款
    /// </summary>
    public class GetMoneyMonthsAndYearResponseDto
    {
        /// <summary>
        /// 本月
        /// </summary>
        public GetCollectionMonthsResponseDto MonthsCollected { get; set; }
        /// <summary>
        /// 本年
        /// </summary>
        public GetCollectionYearResponseDto YearCollected { get; set; }
    }
    /// <summary>
    /// 本月收款
    /// </summary>
    public class GetCollectionMonthsResponseDto
    {
        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal AmountCollected { get; set; }

        /// <summary>
        /// 较上个月上涨百分比,例如上涨为25.5%，则此处的值为25.5
        /// </summary>
        public decimal Increase { get; set; }
    }
    /// <summary>
    /// 本年收款
    /// </summary>
    public class GetCollectionYearResponseDto
    {
        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal AmountCollected { get; set; }

        /// <summary>
        /// 较上个年上涨百分比,例如上涨为25.5%，则此处的值为25.5
        /// </summary>
        public decimal Increase { get; set; }
    }
    public class GetCollectionRatioRequest
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [Required(ErrorMessage = "开始时间不能为空")]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Required(ErrorMessage = "结束时间不能为空")]
        public DateTime EndDate { get; set; }
    }
    /// <summary>
    /// 收款比例
    /// </summary>
    public class GetCollectionRatioResponse
    {
        /// <summary>
        /// 已收款
        /// </summary>
        public decimal Receivable { get; set; }
        /// <summary>
        /// 未收款
        /// </summary>
        public decimal Uncollected { get; set; }
    }
    /// <summary>
    /// 收款统计请求Dto
    /// </summary>
    public class GetCollectionRequest
    {
        /// <summary>
        /// 是否按月份查询 false:按天查询 true:按月份查询统计数据
        /// </summary>
        public bool IsMonths { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Required(ErrorMessage = "开始时间不能为空")]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Required(ErrorMessage = "结束时间不能为空")]
        public DateTime EndDate { get; set; }
    }
    /// <summary>
    /// 统计收款图
    /// </summary>
    public class GetCollectionResponseDto
    {

        /// <summary>
        /// 查询区间的连续日期
        /// </summary>
        public List<string> CollectionDates { get; set; }
        /// <summary>
        /// 科室名称集合
        /// </summary>
        public List<string> OfficeNames { get; set; }

        /// <summary>
        /// 时间收款总计
        /// </summary>
        public List<Data> Datas { get; set; }
        // public List<GetCollectionTimeto> StatisticsDatas { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetCollectionTimeto
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string CollectionDate { get; set; }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// 总计
        /// </summary>
        public decimal Quantity { get; set; }
    }
    public class Data
    {
        /// <summary>
        /// 科室名称
        /// </summary>
        public string OfficeName { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public List<decimal> Value { get; set; }
    }
}
