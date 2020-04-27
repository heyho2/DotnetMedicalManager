using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.DataBoard
{
    /// <summary>
    /// 当天营收数据Dto
    /// </summary>
    public class GetTodayTotalResponseDto
    {
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalMoney { get; set; }
    }
    /// <summary>
    /// 微信扫码收款情况
    /// </summary>
    public class GetWeChatDetailResponseDto
    {
        /// <summary>
        /// 昨日营收
        /// </summary>
        public decimal YesterdayMoney { get; set; }
        /// <summary>
        /// 本月
        /// </summary>
        public decimal ThisMonth { get; set; }
        /// <summary>
        /// 上月
        /// </summary>
        public decimal LastMonth { get; set; }
    }
    /// <summary>
    /// 收款与占比Dto
    /// </summary>
    public class GetDayWeChatDetailAndScaleResponseDto
    {
        /// <summary>
        /// 医院列表
        /// </summary>
        public List<KeyValuePair<string, string>> HospitalList { get; set; }
        /// <summary>
        /// 数据结果对象
        /// </summary>
        public List<GetDayWeChatDetailResponseDto> DayWeChatDetailList { get; set; }
    }
    /// <summary>
    /// 单日医院业绩占比
    /// </summary>
    public class GetDayWeChatDetailAndtAchievementResponseDto
    {
        /// <summary>
        /// 更新时间日期
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 医院列表
        /// </summary>
        public List<KeyValuePair<string, string>> HospitalList { get; set; }
        /// <summary>
        /// 数据结果对象
        /// </summary>
        public List<GetDayWeChatDetailResponseDto> DayWeChatDetailList { get; set; }
    }
    /// <summary>
    /// 今日微信扫码收款情况
    /// </summary>
    public class GetDayWeChatDetailResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 医院名
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalMoney { get; set; }
    }
    /// <summary>
    /// 业绩日报对象
    /// </summary>
    public class GetDayDayAchievementResponseDto
    {
        /// <summary>
        /// 更新时间日期
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 医院列表
        /// </summary>
        public List<KeyValuePair<string, string>> HospitalList { get; set; }
        /// <summary>
        /// 数据结果对象
        /// </summary>
        public List<GetDayAchievementDetailResponseDto> DayAchievementDetailList { get; set; }
    }
    /// <summary>
    /// 业绩日报详情数据
    /// </summary>
    public class GetDayAchievementDetailResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 医院名
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 今日目标
        /// </summary>
        public decimal? TodayGoal { get; set; }
        /// <summary>
        /// 今日完成
        /// </summary>
        public decimal? TodayCompleteGoal { get; set; }
        /// <summary>
        /// 日环比
        /// </summary>
        public decimal? ChainRatio { get; set; }
    }
    /// <summary>
    /// 昨日医院支付笔数占比
    /// </summary>
    public class GetLastDayAchievementResponseDto
    {
        /// <summary>
        /// 更新时间日期
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 医院列表
        /// </summary>
        public List<KeyValuePair<string, string>> HospitalList { get; set; }
        /// <summary>
        /// 数据结果对象
        /// </summary>
        public List<GetLastDayPayDetailResponseDto> LastDayPayDetailList { get; set; }
    }
    /// <summary>
    /// 昨日医院支付笔数占比详情
    /// </summary>
    public class GetLastDayPayDetailResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 医院名
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 支付笔数量
        /// </summary>
        public int PayCount { get; set; }
    }
    /// <summary>
    /// 医院本月累计已完成
    /// </summary>
    public class GetMonthDetailResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 医院名
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 本月目标值
        /// </summary>
        public decimal? MonthGoal { get; set; }
        /// <summary>
        /// 本月完成值
        /// </summary>
        public decimal? MonthCompleteGoal { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }
    }
    /// <summary>
    /// 医院业绩趋势统计
    /// </summary>
    public class GetTrendResponseDto
    {
        /// <summary>
        /// 查询区间的连续日期
        /// </summary>
        public List<string> CollectionDates { get; set; }
        /// <summary>
        /// 医院列表
        /// </summary>
        public List<KeyValuePair<string, string>> HospitalList { get; set; }

        /// <summary>
        /// 时间收款总计
        /// </summary>
        public List<Data> Datas { get; set; }
    }
    /// <summary>
    /// 数据明细项
    /// </summary>
    public class Data
    {
        /// <summary>
        /// ID
        /// </summary>
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public List<decimal> Value { get; set; }
    }
    /// <summary>
    /// 趋势查询Dto
    /// </summary>
    public class GetTrendDetailResponseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string HospitalGuid { get; set; }
        /// <summary>
        /// 医院名
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 完成值
        /// </summary>
        public decimal CompleteGoal { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime AchievementDate { get; set; }
    }
}
