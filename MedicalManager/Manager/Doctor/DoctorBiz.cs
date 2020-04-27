using Dapper;
using GD.DataAccess;
using GD.Dtos.Doctor;
using GD.Dtos.Doctor.Doctor;
using GD.Models.Doctor;
using GD.Models.Manager;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.Manager.Doctor
{
    /// <summary>
    /// 医生模块业务类
    /// </summary>
    public class DoctorBiz : BaseBiz<DoctorModel>
    {
        #region 校验工号是否存在
        public async Task<bool> ExistJobNumber(string jobNumber, string doctorGuid = null)
        {
            var sql = @"select 1 from t_doctor 
                      where job_number = @jobNumber";

            if (!string.IsNullOrEmpty(doctorGuid))
            {
                sql = $"{sql} and doctor_guid <> @doctorGuid";
            }

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.ExecuteScalarAsync(sql, new { jobNumber, doctorGuid });

                return (result is null) ? false : true;
            }
        } 
        #endregion

        #region 查询
        /// <summary>
        /// 搜索医生
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SearchDoctorResponseDto> SearchDoctorAsync(SearchDoctorRequestDto request)
        {
            var sqlWhere = $@"AND status='{DoctorModel.StatusEnum.Approved.ToString()}'";

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                sqlWhere = $"{sqlWhere} AND (hospital_name like @Keyword  OR office_name like @Keyword  OR work_city like @Keyword OR user_name like @Keyword)";
            }
            var sql = $@"
SELECT * FROM(
	SELECT
		A.*,
        CONCAT( B.base_path, B.relative_path ) AS PortraitUrl,
		C.user_name 
	FROM
		t_doctor A
		LEFT JOIN t_utility_accessory B ON B.accessory_guid = A.portrait_guid
		LEFT JOIN t_utility_user C ON C.user_guid = A.doctor_guid 
        WHERE C.ENABLE = 1 AND A.ENABLE = 1
	) t 
WHERE
	1 = 1 {sqlWhere}
ORDER BY
	creation_date";
            request.Keyword = $"%{request.Keyword}%";
            return await MySqlHelper.QueryByPageAsync<SearchDoctorRequestDto, SearchDoctorResponseDto, SearchDoctorItemDto>(sql, request);
        }
        /// <summary>
        /// 审核医生列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetReviewDoctorPageResponseDto> GetReviewDoctorPageAsync(GetReviewDoctorPageRequestDto request)
        {
            var sqlWhere = $@"1=1 ";//and enable=1

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                sqlWhere = $"{sqlWhere} AND user_name like @Name";
            }
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                sqlWhere = $"{sqlWhere} AND status = @Status";
            }
            if (request.BeginDate != null)
            {
                request.BeginDate = request.BeginDate?.Date;
                sqlWhere = $"{sqlWhere} AND creation_date > @BeginDate";
            }
            if (request.EndDate != null)
            {
                request.EndDate = request.EndDate?.AddDays(1).Date;
                sqlWhere = $"{sqlWhere} AND creation_date < @EndDate";
            }
            var sql = $@"
SELECT * FROM(
	SELECT
	    A.*,
	    CONCAT( B.base_path, B.relative_path ) AS PortraitUrl,
	    CONCAT( D.base_path, D.relative_path ) AS signatureUrl,
	    C.user_name,
        C.Gender
    FROM
	    t_doctor A
	    LEFT JOIN t_utility_accessory B ON B.accessory_guid = A.portrait_guid
	    LEFT JOIN t_utility_accessory D ON D.accessory_guid = A.signature_guid
	    LEFT JOIN t_utility_user C ON C.user_guid = A.doctor_guid
) t 
WHERE
	{sqlWhere}
ORDER BY
	creation_date desc";
            request.Name = $"%{request.Name}%";
            return await MySqlHelper.QueryByPageAsync<GetReviewDoctorPageRequestDto, GetReviewDoctorPageResponseDto, GetReviewDoctorPageItemDto>(sql, request);
        }

        #endregion

        public async Task<bool> ReviewDoctorAsync(DoctorModel model, string rejectReason)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.UpdateAsync(model);
                var reviewRecordModel = new ReviewRecordModel
                {
                    OwnerGuid = model.DoctorGuid,
                    CreatedBy = model.LastUpdatedBy,
                    Enable = true,
                    LastUpdatedBy = model.LastUpdatedBy,
                    OrgGuid = string.Empty,
                    RejectReason = rejectReason,
                    Status = model.Status,
                    ReviewGuid = Guid.NewGuid().ToString("N"),
                    Type = ReviewRecordModel.TypeEnum.Doctors.ToString()
                };
                await conn.InsertAsync<string, ReviewRecordModel>(reviewRecordModel);
                return true;
            });
            return result;
        }
        /// <summary>
        /// 医生列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetDoctorPageResponseDto> GetDoctorPageAsync(GetDoctorPageRequestDto request)
        {
            var whereSql = $@"1=1 and status='{DoctorModel.StatusEnum.Approved.ToString()}'";

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                request.Name = $"{request.Name}%";
                whereSql = $"{whereSql} AND user_name like @Name";
            }
            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                request.Phone = $"{request.Phone}%";
                whereSql = $"{whereSql} AND phone like @Phone";
            }
            if (!string.IsNullOrWhiteSpace(request.HospitalGuid))
            {
                whereSql = $"{whereSql} AND Hospital_Guid like @HospitalGuid";
            }
            if (!string.IsNullOrWhiteSpace(request.OfficeGuid))
            {
                whereSql = $"{whereSql} AND Office_Guid like @OfficeGuid";
            }
            if (request.BeginDate != null)
            {
                request.BeginDate = request.BeginDate?.Date;
                whereSql = $"{whereSql} AND creation_date > @BeginDate";
            }
            if (request.EndDate != null)
            {
                request.EndDate = request.EndDate?.AddDays(1).Date;
                whereSql = $"{whereSql} AND creation_date < @EndDate";
            }
            var sql = $@"
SELECT * FROM(
	SELECT
		A.*,
        CONCAT( B.base_path, B.relative_path ) AS PortraitUrl,
		C.user_name ,
        C.phone,
        D.count as article_qty,
        E.count as advisory_qty,
        F.count as fans_qty
	FROM
		t_doctor A
		LEFT JOIN t_utility_accessory B ON B.accessory_guid = A.portrait_guid
		LEFT JOIN t_utility_user C ON C.user_guid = A.doctor_guid 
        LEFT JOIN (SELECT author_guid,count(1) as count FROM t_utility_article WHERE `enable`=1 GROUP BY  author_guid) D ON D.author_guid = A.doctor_guid 
        LEFT JOIN (select doctor_guid, sum(times) as count from t_doctor_consult_statistic WHERE `enable`=1 GROUP BY doctor_guid) E ON E.doctor_guid = A.doctor_guid 
        left join (select target_guid,count(1) as count from t_consumer_collection where `enable`=1 and target_type='doctor' GROUP BY target_guid) F on F.target_guid=A.doctor_guid
	) ___t 
WHERE
	{whereSql}
ORDER BY
	creation_date desc";
            return await MySqlHelper.QueryByPageAsync<GetDoctorPageRequestDto, GetDoctorPageResponseDto, GetDoctorPageItemDto>(sql, request);
        }
        /// <summary>
        /// 获取消息对话列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetDoctorTopicResponseDto> GetDoctorTopicAsync(GetDoctorTopicRequestDto request)
        {
            //TopicModel
            var whereSql = "1=1 and enable=1";
            if (!string.IsNullOrWhiteSpace(request.DoctorGuid))
            {
                whereSql = $"{whereSql} and( receiver_guid=@DoctorGuid or sponsor_guid=@DoctorGuid )";
            }
            if (request.BeginDate != null)
            {
                request.BeginDate = request.BeginDate?.Date;
                whereSql = $"{whereSql} AND creation_date > @BeginDate";
            }
            if (request.EndDate != null)
            {
                request.EndDate = request.EndDate?.AddDays(1).Date;
                whereSql = $"{whereSql} AND creation_date < @EndDate";
            }
            var orderbySql = "creation_date desc";
            string sql = $@"
SELECT * FROM(
    SELECT
	    a.*,
	    b.user_name AS receiver_name,
	    c.user_name AS sponsor_name
    FROM
	    t_utility_topic a
	    LEFT JOIN t_utility_user b ON a.receiver_guid = b.user_guid
	    LEFT JOIN t_utility_user c ON a.sponsor_guid = c.user_guid
)__T
WHERE
    {whereSql}
ORDER BY
    {orderbySql}
";
            return await MySqlHelper.QueryByPageAsync<GetDoctorTopicRequestDto, GetDoctorTopicResponseDto, GetDoctorTopicItemDto>(sql, request);
        }

        /// <summary>
        /// 获取 会话最后一条消息
        /// </summary>
        /// <param name="topicGuid"></param>
        /// <returns></returns>
        public async Task<string> GetTopicLastMessageAsync(string topicGuid)
        {
            var whereSql = "1=1 and enable=1 and topic_guid=@topicGuid";
            var sql = $@"
SELECT context FROM t_utility_message 
WHERE
    {whereSql}
ORDER BY 
    creation_date DESC 
limit 1";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync<string>(sql, new { topicGuid });
                return result;
            }
        }
        public async Task<TopicMessageResponseDto> TopicMessageAsync(TopicMessageRequestDto request)
        {
            var whereSql = "1=1 and enable=1";
            if (!string.IsNullOrWhiteSpace(request.TopicGuid))
            {
                whereSql = $"{whereSql} and Topic_Guid=@TopicGuid ";
            }
            var orderbySql = "creation_date desc";
            string sql = $@"
SELECT * FROM(
    SELECT
	    a.* ,
	    b.user_name AS from_name,
	    c.user_name AS to_name 
    FROM
	    t_utility_message a 
	    LEFT JOIN t_utility_user b ON a.from_guid = b.user_guid
	    LEFT JOIN t_utility_user c ON a.to_guid = c.user_guid
)__T
WHERE
    {whereSql}
ORDER BY
    {orderbySql}
";
            return await MySqlHelper.QueryByPageAsync<TopicMessageRequestDto, TopicMessageResponseDto, TopicMessageItemDto>(sql, request);
        }

        /// <summary>
        /// 修改医生
        /// </summary>
        /// <param name="doctorModel"></param>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<bool> UpdateDoctorAsync(DoctorModel doctorModel, UserModel userModel, IEnumerable<CertificateModel> addCertificates, IEnumerable<CertificateModel> deleteCertificates, IEnumerable<CertificateModel> updateCertificates)
        {
            if (doctorModel == null)
            {
                return false;
            }
            bool result = await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                await conn.UpdateAsync(doctorModel);
                await conn.UpdateAsync(userModel);
                //证书
                foreach (var certificate in addCertificates)
                {
                    if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, CertificateModel>(certificate)))
                    {
                        return false;
                    }
                }
                foreach (var item in deleteCertificates)
                {
                    await conn.DeleteAsync(item);
                }
                foreach (var certificate in updateCertificates)
                {
                    await conn.UpdateAsync(certificate);
                }
                return true;
            });
            return result;
        }
        public async Task<bool> AddDoctorAsync(DoctorModel doctorModel, UserModel userModel, bool userIsInsert, IEnumerable<CertificateModel> certificates)
        {
            if (doctorModel == null)
            {
                return false;
            }
            bool result = await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                await conn.InsertAsync<string, DoctorModel>(doctorModel);
                if (userIsInsert)
                {
                    await conn.InsertAsync<string, UserModel>(userModel);
                }
                else
                {
                    await conn.UpdateAsync(userModel);
                }
                //证书
                foreach (var certificate in certificates)
                {
                    if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, CertificateModel>(certificate)))
                    {
                        return false;
                    }
                }
                return true;
            });
            return result;
        }
        public async Task<int> RecordCountAsync()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.RecordCountAsync<DoctorModel>("where enable=@enable and status=@status", new { enable = true, status = DoctorModel.StatusEnum.Approved.ToString() });
            }
        }

        public async Task<GetDoctorIntegralPageResponseDto> GetDoctorIntegralAsync(GetDoctorIntegralPageRequestDto request)
        {
            var whereSql = $"1=1 and enable=1 and status='{DoctorModel.StatusEnum.Approved.ToString()}'";
            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                request.Phone = $"{request.Phone}%";
                whereSql = $"{whereSql} and phone LIKE @Phone ";
            }
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                request.Name = $"{request.Name}%";
                whereSql = $"{whereSql} and user_name LIKE @Name ";
            }
            if (request.BeginDate != null)
            {
                request.BeginDate = request.BeginDate?.Date;
                whereSql = $"{whereSql} AND creation_date > @BeginDate";
            }
            if (request.EndDate != null)
            {
                request.EndDate = request.EndDate?.AddDays(1).Date;
                whereSql = $"{whereSql} AND creation_date < @EndDate";
            }
            var orderbySql = "creation_date desc";
            string sql = $@"
SELECT * FROM(
    SELECT
	a.* ,
    c.user_name,
    c.phone
FROM
	t_doctor a
	LEFT JOIN t_utility_user c ON a.doctor_guid = c.user_guid
)__T
WHERE
    {whereSql}
ORDER BY
    {orderbySql}
";
            return await MySqlHelper.QueryByPageAsync<GetDoctorIntegralPageRequestDto, GetDoctorIntegralPageResponseDto, GetDoctorIntegralPageItemDto>(sql, request);
        }
        public async Task<IEnumerable<GetDoctorIntegralPageItemDto>> ExportDoctorIntegralAsync(ExportDoctorIntegralRequestDto request)
        {
            var whereSql = $"1=1";
            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                request.Phone = $"{request.Phone}%";
                whereSql = $"{whereSql} and phone LIKE @Phone ";
            }
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                request.Name = $"{request.Name}%";
                whereSql = $"{whereSql} and user_name LIKE @Name ";
            }
            if (request.BeginDate != null)
            {
                request.BeginDate = request.BeginDate?.Date;
                whereSql = $"{whereSql} AND creation_date > @BeginDate";
            }
            if (request.EndDate != null)
            {
                request.EndDate = request.EndDate?.AddDays(1).Date;
                whereSql = $"{whereSql} AND creation_date < @EndDate";
            }
            var orderbySql = "creation_date desc";
            string sql = $@"
SELECT * FROM(
    SELECT
	    a.creation_date,
	    a.hospital_name,
	    a.office_name,
	    c.user_name,
	    c.phone,
	    ( SELECT sum( variation ) FROM t_utility_score WHERE user_guid = c.user_guid AND `enable` = 1 ) AS TotalPoints,
	    ( SELECT sum( variation ) FROM t_utility_score WHERE user_guid = c.user_guid AND `enable` = 1 AND variation > 0 ) AS EarnPoints,
	    ( SELECT sum( variation ) FROM t_utility_score WHERE user_guid = c.user_guid AND `enable` = 1 AND variation < 0 ) AS usePoints 
    FROM
	    t_doctor a
	    LEFT JOIN t_utility_user c ON a.doctor_guid = c.user_guid
    WHERE a.enable=1 and a.status='{DoctorModel.StatusEnum.Approved.ToString()}'
)__T
WHERE
    {whereSql}
ORDER BY
    {orderbySql}
";
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryAsync<GetDoctorIntegralPageItemDto>(sql, request);
            }
        }

    }
}
