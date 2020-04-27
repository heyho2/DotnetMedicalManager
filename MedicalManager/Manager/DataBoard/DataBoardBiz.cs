using Dapper;
using GD.DataAccess;
using GD.Dtos.DataBoard;
using GD.Models.Achievement;
using GD.Models.Doctor;
using GD.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Manager.DataBoard
{
    /// <summary>
    /// 数据Board
    /// </summary>
    public class DataBoardBiz : BaseBiz<HospitalPaymentModel>
    {
        /// <summary>
        /// 获取当天总营收数据
        /// </summary>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        /// <returns></returns>
        public async Task<decimal> GetTodayTotalAsync(DateTime sDate, DateTime eDate)
        {
            var sql = $@"SELECT
	                        SUM( a.amount )/100 
                        FROM
	                        t_doctor_hospital_payment a 
                        WHERE
	                        a.`enable` = 1 
	                        AND a.`status` = 'Success' 
	                        AND a.creation_date BETWEEN @StartDate 
	                        AND @EndDate order by a.amount desc";
            using (var conn = MySqlHelper.GetConnection())
            {
                var count = await conn.QueryFirstOrDefaultAsync<decimal?>(sql, new
                {
                    StartDate = sDate,
                    EndDate = eDate
                });
                return count ?? 0;
            }
        }
        /// <summary>
        /// 微信扫码收款情况
        /// </summary>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        /// <returns></returns>
        public async Task<GetWeChatDetailResponseDto> GetWeChatDetailAsync()
        {
            GetWeChatDetailResponseDto result = null;
            var sql = $@"SELECT
	                        SUM( a.amount ) /100
                        FROM
	                        t_doctor_hospital_payment a 
                        WHERE
	                        a.`enable` = 1 
	                        AND a.`status` = 'Success' 
	                        AND a.creation_date BETWEEN @StartDate 
	                        AND @EndDate order by a.amount desc";
            using (var conn = MySqlHelper.GetConnection())
            {
                result = new GetWeChatDetailResponseDto();
                result.YesterdayMoney = await conn.ExecuteScalarAsync<decimal?>(sql, new
                {
                    StartDate = DateTime.Now.Date.AddDays(-1),
                    EndDate = DateTime.Now.Date
                }) ?? 0;
                DateTime sDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                result.ThisMonth = await conn.QueryFirstOrDefaultAsync<decimal?>(sql, new
                {
                    StartDate = sDate,
                    EndDate = DateTime.Now
                }) ?? 0;
                result.LastMonth = await conn.QueryFirstOrDefaultAsync<decimal?>(sql, new
                {
                    StartDate = sDate.AddMonths(-1),
                    EndDate = sDate.AddSeconds(-1)
                }) ?? 0;
                return result;
            }
        }
        /// <summary>
        /// 今日微信扫码收款情况
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetDayWeChatDetailResponseDto>> GetDayWeChatDetailAsync()
        {
            var sql = $@"SELECT
	                    b.hos_name as HospitalName,
	                    b.hospital_guid as HospitalGuid,
	                    SUM( a.amount )/ 100 as TotalMoney
                    FROM
	                    t_doctor_hospital_payment a
	                    RIGHT JOIN t_doctor_hospital b ON a.hospital_guid = b.hospital_guid  AND a.`enable` = 1 
	                    AND a.`status` = 'Success' 
	                    AND a.creation_date BETWEEN @StartDate 
	                    AND @EndDate 
                        where  b.is_hospital=1 and  b.`enable`=1
                    GROUP BY
	                    b.hospital_guid,
	                    b.hos_name order by TotalMoney desc ";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetDayWeChatDetailResponseDto>(sql, new
                {
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1)
                });
                return result?.ToList();
            }
        }
        /// <summary>
        /// 昨日医院支付笔数占比
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetLastDayPayDetailResponseDto>> GetLastDayWeChatPayDetailAsync()
        {
            var sql = $@"SELECT
	                    b.hos_name as HospitalName,
	                    b.hospital_guid as HospitalGuid,
	                    COUNT( a.amount ) as PayCount
                    FROM
	                    t_doctor_hospital_payment a
	                    RIGHT JOIN t_doctor_hospital b ON a.hospital_guid = b.hospital_guid  AND a.`enable` = 1 
	                    AND a.`status` = 'Success' 
	                    AND a.creation_date BETWEEN @StartDate 
	                    AND @EndDate 
                        where b.is_hospital=1 and  b.`enable`=1
                    GROUP BY
	                    b.hospital_guid,
	                    b.hos_name";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetLastDayPayDetailResponseDto>(sql, new
                {
                    StartDate = DateTime.Now.Date.AddDays(-1),
                    EndDate = DateTime.Now.Date
                });
                return result?.ToList();
            }
        }
        /// <summary>
        /// 单日医院业绩占比
        /// </summary>
        /// <returns></returns>
        public async Task<(List<GetDayWeChatDetailResponseDto>, string date)> GetDayWeChatAchievementAsync()
        {
            var achievementDate = string.Empty;
            List<GetDayWeChatDetailResponseDto> result = null;
            var sql = $@"SELECT
	                    c.hos_name AS HospitalName,
	                    c.hospital_guid AS HospitalGuid,
	                    a.today_complete AS TotalMoney 
                    FROM
	                    t_achievement_month_goal b
	                    inner JOIN t_achievement a ON b.goal_guid = a.goal_guid AND a.`enable` = 1 and a.achievement_date = @AchievementDate
	                    Right JOIN t_doctor_hospital c ON c.hospital_guid = b.hospital_guid 
                    WHERE
	                  c.is_hospital=1 and  c.`enable`=1
                    GROUP BY
	                    c.hospital_guid,
	                    c.hos_name,
	                    a.today_complete";
            using (var conn = MySqlHelper.GetConnection())
            {
                //查找最大的时间
                var maxResult = await conn.QueryFirstOrDefaultAsync<AchievementModel>("SELECT * from  t_achievement WHERE `enable`=1 ORDER BY achievement_date desc LIMIT 0,1");
                //if (maxResult == null)
                //{
                maxResult = new AchievementModel { AchievementDate = DateTime.Now.AddDays(-1) };
                //}
                result = new List<GetDayWeChatDetailResponseDto>();
                result = (await conn.QueryAsync<GetDayWeChatDetailResponseDto>(sql, new
                {
                    AchievementDate = maxResult.AchievementDate.ToString("yyyy-MM-dd")
                }))?.ToList();
                achievementDate = maxResult.AchievementDate.ToString("yyyy-MM-dd");
                return (result, achievementDate);
            }
        }
        /// <summary>
        /// 医院业绩日报
        /// </summary>
        /// <returns></returns>
        public async Task<(List<GetDayAchievementDetailResponseDto>, string)> GetDayAchievementAsync()
        {
            var achievementDate = string.Empty;
            List<GetDayAchievementDetailResponseDto> result = null;
            var sql = $@"SELECT
	                    c.hos_name AS HospitalName,
	                    c.hospital_guid AS HospitalGuid,
	                    a.today_complete AS TodayCompleteGoal,
                        a.today_goal AS TodayGoal
                    FROM
	                     t_doctor_hospital c LEFT JOIN
	                    t_achievement_month_goal b ON c.hospital_guid = b.hospital_guid and c.`enable`=1 and b.`year`=@Year and b.`month`=@Month
	                    LEFT JOIN t_achievement a ON b.goal_guid = a.goal_guid and a.`enable` = 1 and  a.achievement_date = @AchievementDate where  c.is_hospital=1 and  c.`enable`=1
                    GROUP BY
	                    c.hospital_guid,
	                    c.hos_name,
	                    a.today_complete,
                        a.today_goal order by a.today_goal desc ";
            using (var conn = MySqlHelper.GetConnection())
            {
                //查找最大的时间
                var maxResult = await conn.QueryFirstOrDefaultAsync<AchievementModel>("SELECT * from  t_achievement WHERE `enable`=1 ORDER BY achievement_date desc LIMIT 0,1");
                //if (maxResult == null)
                //{
                maxResult = new AchievementModel { AchievementDate = DateTime.Now.AddDays(-1) };
                //}
                result = new List<GetDayAchievementDetailResponseDto>();
                result = (await conn.QueryAsync<GetDayAchievementDetailResponseDto>(sql, new
                {
                    achievementDate = maxResult.AchievementDate.ToString("yyyy-MM-dd"),
                    Year = maxResult.AchievementDate.Year,
                    Month = maxResult.AchievementDate.Month
                }))?.ToList();
                achievementDate = maxResult.AchievementDate.ToString("yyyy-MM-dd");
                if (result != null)
                {
                    var lastResult = (await conn.QueryAsync<GetDayAchievementDetailResponseDto>(sql, new
                    {
                        achievementDate = maxResult.AchievementDate.AddDays(-1).ToString("yyyy-MM-dd"),
                        Year = maxResult.AchievementDate.Year,
                        Month = maxResult.AchievementDate.Month
                    }))?.ToList();
                    if (lastResult != null)
                    {
                        foreach (var item in result)
                        {
                            var lastValue = lastResult.Where(s => s.HospitalGuid == item.HospitalGuid).Select(s => s.TodayCompleteGoal).FirstOrDefault();
                            if (item.TodayCompleteGoal.HasValue)
                            {
                                if (lastValue.HasValue && lastValue.Value != 0)
                                {
                                    item.ChainRatio = Math.Round((item.TodayCompleteGoal.Value - lastValue.Value) / lastValue.Value, 2) * 100;
                                }
                                else
                                {
                                    if (item.TodayCompleteGoal.Value > 0)
                                    {
                                        item.ChainRatio = 100;
                                    }
                                    else
                                    {
                                        item.ChainRatio = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                return (result, achievementDate);
            }
        }
        /// <summary>
        /// 医院本月累计已完成
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetMonthDetailResponseDto>> GetMonthDetailAsync()
        {
            var sql = $@"SELECT
	                    c.hos_name AS HospitalName,
	                    c.hospital_guid AS HospitalGuid,
                        b.month_goal as MonthGoal,
	                    SUM(a.today_complete) AS MonthCompleteGoal 
                    FROM
	                    t_achievement_month_goal b
	                    LEFT JOIN t_achievement a ON b.goal_guid = a.goal_guid and a.`enable` = 1 
	                    RIGHT JOIN t_doctor_hospital c ON c.hospital_guid = b.hospital_guid and  b.year=@Year and b.month=@Month where c.is_hospital=1 and  c.`enable`=1
                    GROUP BY
	                    c.hospital_guid,
	                    c.hos_name,b.month_goal";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = (await conn.QueryAsync<GetMonthDetailResponseDto>(sql, new
                {
                    Year = DateTime.Now.Year,
                    Month = DateTime.Now.Month
                }))?.ToList();
                return result;
            }
        }
        /// <summary>
        /// 医院业绩趋势统计
        /// </summary>
        /// <returns></returns>
        public async Task<GetTrendResponseDto> GetMonthTrendAsync()
        {
            GetTrendResponseDto resultRespon = new GetTrendResponseDto();
            var sql = $@"SELECT
	                    c.hos_name AS HospitalName,
	                    c.hospital_guid AS HospitalGuid,
	                    a.today_complete AS CompleteGoal,
                        achievement_date
                    FROM
	                    t_achievement_month_goal b
	                    LEFT JOIN t_achievement a ON b.goal_guid = a.goal_guid and a.`enable` = 1 and  b.year=@Year and b.month=@Month 
	                    RIGHT JOIN t_doctor_hospital c ON c.hospital_guid = b.hospital_guid where c.is_hospital=1 and  c.`enable`=1
                    GROUP BY
	                    c.hospital_guid,
	                    c.hos_name,
                        achievement_date,a.today_complete ";
            using (var conn = MySqlHelper.GetConnection())
            {
                List<GetTrendDetailResponseDto> result = (await conn.QueryAsync<GetTrendDetailResponseDto>(sql, new
                {
                    Year = DateTime.Now.Year,
                    Month = DateTime.Now.Month
                }))?.ToList();
                if (result != null)
                {
                    var dates = new List<string>();
                    var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime endDate = startDate.AddMonths(1);
                    while (startDate < endDate)
                    {
                        dates.Add(startDate.ToString("yyyy-MM-dd"));
                        startDate = startDate.AddDays(1).Date;
                    }
                    resultRespon.CollectionDates = dates;
                    var hosIdList = result.Select(d => new Hos { HId = d.HospitalGuid, HName = d.HospitalName }).ToList();
                    hosIdList = hosIdList.GroupBy(d => d.HId).Select(d => d.FirstOrDefault()).ToList();
                    List<HosNameAndDate> hosNameAndDates = dates.Join(hosIdList.Distinct(), d => 1, gs => 1, (d, gs) => new HosNameAndDate
                    {
                        Datestr = d,
                        HosId = gs.HId,
                        HName = gs.HName
                    }).ToList();
                    resultRespon.HospitalList = result.Distinct().Select(s => new KeyValuePair<string, string>(s.HospitalGuid, s.HospitalName)).Distinct().ToList();
                    var datas = hosNameAndDates.GroupJoin(result,
                        d => new { Date = d.Datestr, HosId = d.HosId },
                        s => new { Date = s.AchievementDate.ToString("yyyy-MM-dd"), HosId = s.HospitalGuid },
                        (d, gs) => new GetTrendDetailResponseDto
                        {
                            HospitalGuid = d.HosId,
                            HospitalName = d.HName,
                            AchievementDate = Convert.ToDateTime(d.Datestr),
                            CompleteGoal = (gs.FirstOrDefault()?.CompleteGoal) ?? 0
                        }).ToList();
                    if (datas != null)
                    {
                        resultRespon.Datas = datas.GroupBy(s => s.HospitalGuid).Select(a => new Data
                        {
                            HospitalGuid = a.Key,
                            Value = a.ToList().Select(r => r.CompleteGoal).ToList()
                        }).ToList();
                    }
                }
                return resultRespon;
            }
        }
        public class HosNameAndDate
        {
            /// <summary>
            /// 时间
            /// </summary>
            public string Datestr { get; set; }
            /// <summary>
            /// 医院名称1
            /// </summary>
            public string HosId { get; set; }
            public string HName { get; set; }
        }
        public class Hos
        {
            public string HId { get; set; }
            public string HName { get; set; }
        }
    }
}
