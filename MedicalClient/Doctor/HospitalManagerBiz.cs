using Dapper;
using GD.AppSettings;
using GD.DataAccess;
using GD.Dtos.Doctor.Hospital;
using GD.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Doctor
{
    /// <summary>
    /// 医院管理业务类
    /// </summary>
    public class HospitalManagerBiz
    {
        /// <summary>
        /// 获取医院在线医生实时统计
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <returns></returns>
        public async Task<List<int>> GetHospitalDoctorOnlineNumber(string hospitalGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT 
				                 COUNT(DISTINCT v.user_guid) AS Number
		                    FROM t_utility_visit as v
			                    INNER JOIN t_doctor as d ON v.user_guid = d.doctor_guid
		                    WHERE d.hospital_guid = @hospitalGuid AND v.user_type  = 'Doctor' AND  (v.creation_date >= @beginDate AND v.creation_date < @endDate)  AND d.`status` = 'approved' AND d.`enable` = 1
                    UNION ALL              
                    SELECT 
                            COUNT(d.doctor_guid) AS Number 
                    FROM t_doctor as d
                    WHERE d.hospital_guid = @hospitalGuid AND d.`enable` = 1 AND d.`status` = 'approved'
                    UNION ALL
                    SELECT 
		                    COUNT(DISTINCT m.from_guid)  AS Number
                    FROM t_utility_message AS m
	                    INNER JOIN t_doctor AS d ON m.to_guid = d.doctor_guid
                    WHERE d.hospital_guid = @hospitalGuid AND d.`enable` = 1 AND d.`status` = 'approved' AND (m.creation_date >= @beginDate AND m.creation_date < @endDate)";

                return (await conn.QueryAsync<int>(sql, new
                {
                    hospitalGuid,
                    beginDate = DateTime.Today,
                    endDate = DateTime.Today.AddDays(1)
                })).ToList();
            }
        }

        /// <summary>
        /// 获取医院上线医生历史统计
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetHospitaHistoricalOnlineDoctorResponseDto> GetHospitaHistoricalOnlineDoctorNumber(GetHospitaHistoricalDoctorRequestDto request)
        {
            var response = new GetHospitaHistoricalOnlineDoctorResponseDto();

            using (var conn = MySqlHelper.GetConnection())
            {
                #region 查询医生在线
                var sql = @"SELECT 
	                    IFNULL(SUM(o.online_number),0) AS online_number,
                        IFNULL(SUM(o.total_number),0) AS total_number
                    FROM t_doctor_historical_online_statistic AS o
                    WHERE o.hospital_guid = @hospitalGuid ";

                sql = SqlWhere(request.Type, sql);

                var online = (await conn.QueryFirstOrDefaultAsync<HospitalOnline>(sql, new
                {
                    request.HospitalGuid,
                    beginDate = request.BeginDate?.ToString("yyyy-MM-dd"),
                    endDate = request.EndDate?.ToString("yyyy-MM-dd")
                }));
                #endregion

                #region 查询用户咨询
                sql = @"SELECT 
	                    IFNULL(SUM( o.number),0) AS number
                    FROM t_doctor_historical_consult_statistic AS o
                    WHERE o.hospital_guid = @hospitalGuid ";

                sql = SqlWhere(request.Type, sql);

                var consultTotal = (await conn.QueryFirstOrDefaultAsync<int>(sql, new
                {
                    request.HospitalGuid,
                    beginDate = request.BeginDate?.ToString("yyyy-MM-dd"),
                    endDate = request.EndDate?.ToString("yyyy-MM-dd")
                }));
                #endregion

                //统计医生上线
                response.OnlineTotal = online.OnlineNumber;
                response.AvgOnline = GetAvgByType(request, online.OnlineNumber);

                //统计用户咨询
                response.ConsultTotal = consultTotal;
                response.AvgConsult = GetAvgByType(request, response.ConsultTotal);

                //统计医生上线比例
                if (response.OnlineTotal > 0)
                {
                    response.OnlineRatio = Math.Round((decimal)online.OnlineNumber / online.TotalNumber, 4) * 100;
                }
            }
            return response;
        }

        /// <summary>
        /// 根据请求参数类型不同拼接SQL条件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        string SqlWhere(int type, string sql)
        {
            if (type == 0)
            {
                sql = $"{sql} AND o.statistic_date = '{DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd")}'";
            }
            else if (type == 1)
            {
                sql = $"{sql} AND o.statistic_date >= DATE_ADD(CURRENT_DATE,INTERVAL -6 DAY) AND o.statistic_date <= CURRENT_DATE";
            }
            else
            {
                sql = $"{sql} AND o.statistic_date >= @beginDate AND o.statistic_date <= @endDate";
            }

            return sql;
        }

        /// <summary>
        /// 根据请求参数类型计算均值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        int GetAvgByType(GetHospitaHistoricalDoctorRequestDto request, int total)
        {
            if (request.Type == 0)
            {
                return total;
            }
            else if (request.Type == 1)
            {
                return Convert.ToInt32(Math.Ceiling((decimal)total / 7));
            }
            else
            {
                var day = ((request.EndDate.Value - request.BeginDate.Value).Days);
                return Convert.ToInt32(Math.Ceiling((decimal)total / (day + 1)));
            }
        }

        /// <summary>
        /// 获取医院医生用户咨询排行榜
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<GetHospitalDoctorConsultRankResponseDto>> GetHospitalDoctorConsultRank(GetHospitaHistoricalDoctorRequestDto request)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT 
				                u.user_name as `name`,
							   (
                                    SELECT 
                                        IFNULL(SUM(o.times),0) 
                                    FROM      t_doctor_consult_statistic as o 
                                    WHERE o.doctor_guid = u.user_guid {SqlWhere(request.Type, string.Empty)}
                               ) AS times
						   FROM t_utility_user as u
								INNER JOIN t_doctor as d ON d.doctor_guid = u.user_guid
						   WHERE d.`status` = 'approved' AND d.`enable` = 1 
                           AND d.hospital_guid = @hospitalGuid ";

                var rankings = (await conn.QueryAsync<GetHospitalDoctorConsultRankResponseDto>(sql, new
                {
                    request.HospitalGuid,
                    beginDate = request.BeginDate?.ToString("yyyy-MM-dd"),
                    endDate = request.EndDate?.ToString("yyyy-MM-dd")
                }))
                .OrderByDescending(d => d.Times)
                .GroupBy(x => x.Times)
                .SelectMany((g, i) => g.Select(e => new GetHospitalDoctorConsultRankResponseDto()
                {
                    Name = e.Name,
                    Times = e.Times,
                    Rank = i + 1
                })).ToList();

                return rankings;
            }
        }

        /// <summary>
        /// 获取医院医生在线时长排行榜
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<GetHospitalDoctorOnlineRankResponseDto>> GetHospitalDoctorOnlineRank(GetHospitaHistoricalDoctorRequestDto request)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT 
				                u.user_name as `name`,
							   (
                                    SELECT 
                                        IFNULL(SUM(o.duration),0) 
                                    FROM      t_doctor_online_statistic as o 
                                    WHERE o.doctor_guid = u.user_guid {SqlWhere(request.Type, string.Empty)}
                               ) AS Duration
						   FROM t_utility_user as u
								INNER JOIN t_doctor as d ON d.doctor_guid = u.user_guid
						   WHERE d.`status` = 'approved' AND d.`enable` = 1 
                           AND d.hospital_guid = @hospitalGuid ";

                var rankings = (await conn.QueryAsync<GetHospitalDoctorOnlineRankResponseDto>(sql, new
                {
                    request.HospitalGuid,
                    beginDate = request.BeginDate?.ToString("yyyy-MM-dd"),
                    endDate = request.EndDate?.ToString("yyyy-MM-dd")
                }))
                .OrderByDescending(d => d.Duration)
                .GroupBy(x => x.Duration)
                .SelectMany((g, i) => g.Select(e => new GetHospitalDoctorOnlineRankResponseDto()
                {
                    Name = e.Name,
                    Duration = e.Duration,
                    Rank = i + 1
                })).ToList();

                return rankings;
            }
        }


        /// <summary>
        /// 获取医院医生解答问题排行榜
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<GetHospitalDoctorAnswerQuestionRankResponseDto>> GetHospitalDoctorAnswerQuestionRank(GetHospitaHistoricalDoctorRequestDto request)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT 
				                u.user_name as `name`,
							   (
                                    SELECT 
                                        IFNULL(SUM(o.times),0) 
                                    FROM      t_doctor_answer_question_statistic as o 
                                    WHERE o.doctor_guid = u.user_guid {SqlWhere(request.Type, string.Empty)}
                               ) AS times
						   FROM t_utility_user as u
								INNER JOIN t_doctor as d ON d.doctor_guid = u.user_guid
						   WHERE d.`status` = 'approved' AND d.`enable` = 1 
                           AND d.hospital_guid = @hospitalGuid ";

                var rankings = (await conn.QueryAsync<GetHospitalDoctorAnswerQuestionRankResponseDto>
                    (sql, new
                    {
                        request.HospitalGuid,
                        beginDate = request.BeginDate?.ToString("yyyy-MM-dd"),
                        endDate = request.EndDate?.ToString("yyyy-MM-dd")
                    }))
                   .OrderByDescending(d => d.Times)
                   .GroupBy(x => x.Times)
                   .SelectMany((g, i) => g.Select(e => new GetHospitalDoctorAnswerQuestionRankResponseDto()
                   {
                       Name = e.Name,
                       Times = e.Times,
                       Rank = i + 1
                   })).ToList();

                return rankings;
            }
        }

        /// <summary>
        /// 创建医生在线时长记录
        /// </summary>
        /// <param name="model"></param>
        public string CreateDoctorOnlineRecord(OnLineModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return conn.Insert<string, OnLineModel>(model);
            }
        }

        /// <summary>
        /// 医院医生（解答问题数、 用户咨询数、被采纳率等）列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetHospitalDoctorAnswerAndConsultPageListResponseDto> GetDoctorAnswerAndConsultListAsync(GetHospitalDoctorAnswerAndConsultPageListRequestDto requestDto)
        {
            var rows = (List<DoctorAnswerConsultItemDto>)null;

            var sql = $@"SELECT 
	                       d.doctor_guid as DoctorGuid, 
                           u.user_name as Name, 
                           u.phone as Phone,
                           (
                                SELECT 
                                    IFNULL(SUM(o.times),0)
                                FROM t_doctor_consult_statistic as o
                                WHERE o.doctor_guid = d.doctor_guid {SqlWhere(2, string.Empty)}
                            ) as ConsultNumber /*咨询总次数*/,
                            (
                                SELECT 
                                    IFNULL(SUM(o.duration),0) 
                                FROM t_doctor_online_statistic as o
                                WHERE o.doctor_guid = d.doctor_guid {SqlWhere(2, string.Empty)}
                            ) as Duration /*在线总时长*/,
                            (
                                SELECT 
                                    IFNULL(SUM(f.variation),0)
                                FROM `t_utility_score` as f
                                WHERE f.user_guid = d.doctor_guid AND f.reason LIKE '%咨询%'                     AND f.user_type_guid = 'Doctor' AND CAST(f.creation_date AS DATE)  >= @beginDate AND CAST(f.creation_date AS DATE) <= @endDate        
                            ) as Score /*总扣分*/,
                           (
                                SELECT
	                                IFNULL(SUM(o.times), 0)
                                FROM
	                                t_doctor_answer_question_statistic AS o 
                                WHERE o.doctor_guid = d.doctor_guid {SqlWhere(2, string.Empty)}
                           ) as AnswerQuestionNumber /*回答问题总数量*/,
                           (
                                SELECT COUNT(DISTINCT question_guid) 
	                            FROM t_faqs_answer as f 
                                WHERE f.user_guid = d.doctor_guid AND           
                                    f.main_answer = 1 AND CAST(f.creation_date AS DATE)  >= @beginDate AND CAST(f.creation_date AS DATE) <= @endDate
                            ) as RightTimes /*问题被采纳总个数*/,
                           (
                               SELECT 
                                    v.creation_date 
                               FROM t_utility_visit as v 
                               WHERE v.user_guid = u.user_guid AND v.user_type = 'Doctor' 
                               ORDER BY v.creation_date DESC LIMIT 1
                          ) as LastLoginTime    /*最后一次登录时间*/           
                        FROM t_doctor as d
	                        LEFT JOIN t_utility_user as u ON d.doctor_guid = u.user_guid
                        WHERE d.hospital_guid = @HospitalGuid AND d.`status` = 'approved' AND d.`enable` = 1";

            if (!string.IsNullOrEmpty(requestDto.Name?.Trim()))
            {
                sql = $"{sql} AND u.user_name like '%{requestDto.Name}%'";
            }

            if (!string.IsNullOrEmpty(requestDto.Phone?.Trim()))
            {
                sql = $"{sql} AND u.phone like '%{requestDto.Phone}%'";
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                var users = await conn.QueryAsync<HospitalDocorDbBasicData>
                       (sql, new
                       {
                           requestDto.HospitalGuid,
                           beginDate = requestDto.BeginDate.ToString("yyyy-MM-dd"),
                           endDate = requestDto.EndDate.ToString("yyyy-MM-dd")
                       });

                if (users.Count() <= 0)
                {
                    return null;
                }

                #region 获取医生在线配置数据

                var settings = Factory.GetSettings("host.json");
                var presence = settings["XMPP:presence"];
                var domain = settings["XMPP:domain"];
                #endregion

                rows = new List<DoctorAnswerConsultItemDto>();
                foreach (var u in users)
                {
                    var item = new DoctorAnswerConsultItemDto()
                    {
                        PresenceIcon = $"{presence}?jid={u.DoctorGuid}@{domain}",
                        Name = u.Name,
                        Phone = u.Phone,
                        LastLoginTime = u.LastLoginTime,
                        AnswerQuestionNumber = u.AnswerQuestionNumber,
                        ConsultNumber = u.ConsultNumber,
                        Duration = u.Duration,
                        Score = u.Score
                    };

                    if (u.AnswerQuestionNumber > 0)
                    {
                        item.AdopedRate = Math.Round((decimal)u.RightTimes / item.AnswerQuestionNumber, 4) * 100;
                    }

                    rows.Add(item);
                }
            }

            //根据医生在线和在线时长排序
            rows = rows.OrderByDescending(d => d.Duration).ToList();

            var total = rows.Count();

            var offset = (requestDto.PageIndex - 1) * requestDto.PageSize;

            rows = rows.Skip(offset).Take(requestDto.PageSize).ToList();

            return new GetHospitalDoctorAnswerAndConsultPageListResponseDto()
            {
                CurrentPage = rows,
                Total = total
            };
        }
    }
}