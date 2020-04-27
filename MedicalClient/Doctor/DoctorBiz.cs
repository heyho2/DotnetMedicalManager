using Dapper;
using GD.DataAccess;
using GD.Dtos.Admin.Doctor;
using GD.Dtos.Common;
using GD.Dtos.Doctor.Doctor;
using GD.Models.Doctor;
using GD.Models.Manager;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Doctor
{
    /// <summary>
    /// 医生模块业务类
    /// </summary>
    public class DoctorBiz
    {
        #region 查询

        /// <summary>
        /// 通过医生Guid获取唯一医生实例
        /// </summary>
        /// <param name="doctorGuid"></param>
        /// <param name="enable"></param>
        /// <returns>唯一消医生实例</returns>
        public DoctorModel GetDoctor(string doctorGuid, bool enable = true)
        {
            var sql = "select * from t_doctor where doctor_guid=@doctorGuid and enable=@enable";
            var doctorModel = MySqlHelper.SelectFirst<DoctorModel>(sql, new { doctorGuid, enable });
            return doctorModel;
        }
        /// <summary>
        /// 根据工号获取医生
        /// </summary>
        /// <param name="jobNumber"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<DoctorModel> GetByJobNumberDoctor(string jobNumber, bool enable = true)
        {
            var sql = "select * from t_doctor doc  where job_number=@jobNumber and doc.enable=@enable";
            using (var conn = MySqlHelper.GetConnection())
            {
                var doctorModel = await conn.QueryFirstOrDefaultAsync<DoctorModel>(sql, new { jobNumber, enable });
                return doctorModel;
            }
        }
        /// <summary>
        /// 获取所有有效医生id集合
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetAllDoctorIdsAsync(bool enalbe = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<string>("select  a.doctor_guid from t_doctor a where a.`enable`=@enalbe and a.`status`='approved'", new { enalbe });
                return result?.AsList();
            }
        }

        public async Task<int> RecordCountAsync()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.RecordCountAsync<DoctorModel>("where enable=@enable and status=@status", new { enable = true, status = DoctorModel.StatusEnum.Approved.ToString() });
            }
        }

        /// <summary>
        /// 按ID查询
        /// </summary>
        /// <param name="dicGuid"></param>
        /// <returns></returns>
        public async Task<DoctorModel> GetAsync(string guid, bool enable = true, bool ignorEnable = false)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var enablewhere = "and enable=@enable";
                if (ignorEnable)
                {
                    enablewhere = "";
                }
                return await conn.QueryFirstOrDefaultAsync<DoctorModel>($"select * from t_doctor where doctor_guid=@guid {enablewhere}", new { guid, enable });
            }
        }






        /// <summary>
        /// 获取问医医生列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetAskedDoctorsResponseDto> GetAskedDoctors(GetAskedDoctorsRequestDto requestDto)
        {
            string keywordWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(requestDto.HospitalGuid))
            {
                keywordWhere = $"AND doc.hospital_guid = @HospitalGuid";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.OfficeName))
            {
                keywordWhere = $"AND doc.office_name = @OfficeName";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.AdeptName))
            {
                keywordWhere = $"AND doc.adept_tags like @AdeptName";
                requestDto.AdeptName = $"%{requestDto.AdeptName}%";
            }
            var sql = $@"SELECT
	                        doc.doctor_guid AS DoctorGuid,
	                        tbUser.user_name AS DoctorName,
	                        doc.hospital_guid AS HospitalGuid,
	                        doc.hospital_name AS HospitalName,
	                        doc.office_guid AS OfficeGuid,
	                        doc.office_name AS OfficeName,
	                        jobTitle.config_name AS Title,
	                        doc.adept_tags AS AdeptTags,
	                        doc.honor AS Honor,
	                        CONCAT( picture.base_path, picture.relative_path ) AS Picture ,
	                        CONCAT( hospicture.base_path, hospicture.relative_path ) AS HospitalPicture 
                        FROM
	                        t_doctor AS doc
	                        LEFT JOIN t_manager_dictionary AS jobTitle ON doc.title_guid = jobTitle.dic_guid and jobTitle.`enable`=TRUE
	                        LEFT JOIN t_utility_accessory AS picture ON accessory_guid = doc.portrait_guid and picture.`enable`=TRUE
	                        LEFT JOIN t_utility_user AS tbUser ON tbUser.user_guid = doc.doctor_guid and tbUser.`enable`=TRUE
                            LEFT JOIN t_doctor_hospital AS hos ON hos.hospital_guid = doc.hospital_guid AND hos.`enable` =TRUE 
                            LEFT JOIN t_utility_accessory AS hospicture ON hospicture.accessory_guid = hos.logo_guid 
                        where doc.`enable`=true and doc.`status`='approved' {keywordWhere} ORDER BY doc.creation_date";
            var response = await MySqlHelper.QueryByPageAsync<GetAskedDoctorsRequestDto, GetAskedDoctorsResponseDto, GetAskedDoctorsItemDto>(sql, requestDto);
            return response;
        }

        /// <summary>
        /// 搜索医生
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SearchDoctorResponseDto> SearchDoctorAsync(SearchDoctorRequestDto request)
        {
            var sqlWhere = $@"AND ENABLE = 1 and  status='{DoctorModel.StatusEnum.Approved.ToString()}'";

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
	    C.user_name 
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

        /// <summary>
        /// 获取医生列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetDoctorListResponseDto> GetDoctorListAsync(GetDoctorListRequestDto request)
        {
            var sqlWhere = $@"AND ENABLE = 1 and `status`='approved'  and enable=1";

            if (request.Gender != null)
            {
                sqlWhere = $"{sqlWhere} AND gender = '{(request.Gender.Value == 0 ? "F" : "M")}'";
            }
            if (request.IsExpert != null)
            {
                sqlWhere = $"{sqlWhere} AND IsExpert = {(request.IsExpert.Value ? "'1'" : "'0'")}";
            }
            var sqlOrderBy = "creation_date desc";
            switch (request.Sort)
            {
                case GetDoctorListRequestDto.SortMenu.Score:
                    sqlOrderBy = "score desc";
                    break;
                case GetDoctorListRequestDto.SortMenu.Experience:
                    sqlOrderBy = "work_age desc";
                    break;
                default:
                    sqlOrderBy = "creation_date desc";
                    break;
            }
            var sql = $@"
SELECT * FROM(
    SELECT
	    A.*,
	    E.config_name,
	    ifnull(E.extension_field,'0') as IsExpert,
	    E.extension_field,
	    F.gender,
	    F.user_name AS DoctorName,
	    (SELECT AVG( D.score ) FROM t_consumer_comment D WHERE D.target_guid = A.doctor_guid GROUP BY D.target_guid ) AS score,
	    CONCAT( G.base_path, G.relative_path ) AS Picture,
	    CONCAT( H.base_path, H.relative_path ) AS HospitalPicture 
    FROM
	    t_doctor A
	    LEFT JOIN t_doctor_hospital B ON A.hospital_guid = B.hospital_guid
	    LEFT JOIN t_doctor_office C ON C.office_guid = A.office_guid
	    LEFT JOIN t_manager_dictionary E ON E.dic_guid = A.title_guid
	    LEFT JOIN t_utility_user F ON F.user_guid = A.doctor_guid
	    LEFT JOIN t_utility_accessory G ON G.accessory_guid = A.portrait_guid
	    LEFT JOIN t_utility_accessory H ON H.accessory_guid = A.portrait_guid
)T WHERE
	1 = 1 {sqlWhere}
ORDER BY
	{sqlOrderBy}";

            return await MySqlHelper.QueryByPageAsync<GetDoctorListRequestDto, GetDoctorListResponseDto, GetDoctorListItemDto>(sql, request);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 更新医生model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateModel(DoctorModel model)
        {
            return model.Update() == 1;

        }
        #endregion


        #region 生美医生
        /// <summary>
        ///生美- 查询推荐产品
        /// </summary>
        /// <param name="productGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<DoctorModel> GetDoctorListByGuidAsync(string doctorGuid, bool enable = true)
        {
            //该方法可用于公共方法
            const string sql = @"SELECT
	                                pro.`product_guid`,
	                                pro.`merchant_guid`,
	                                pro.`category_guid`,
	                                pro.`category_name`,
	                                CONCAT( acc.base_path, acc.relative_path ) AS picture_guid,
	                                pro.`product_name`,
	                                pro.`product_label`,
	                                pro.`introduce_guid`,
	                                pro.`brand`,
	                                pro.`standerd`,
	                                pro.`retention_period`,
	                                pro.`manufacture`,
	                                pro.`approval_number`,
	                                pro.`price`,
	                                pro.`cost_price`,
	                                pro.`market_price`,
	                                pro.`freight`,
	                                pro.`pro_detail_guid`,
	                                pro.`inventory`,
	                                pro.`location`,
	                                pro.`recommend`,
	                                pro.`sort`,
	                                pro.`created_by`,
	                                pro.`creation_date`,
	                                pro.`last_updated_by`,
	                                pro.`last_updated_date`,
	                                pro.`platform_type`,
	                                pro.`org_guid`,
	                                pro.`enable` 
                                FROM
	                                t_mall_product AS pro
	                                LEFT JOIN t_utility_accessory AS acc ON pro.picture_guid = acc.accessory_guid 
                                WHERE
	                                pro.product_guid = @productGuid
	                                and pro.`enable`=@enable   ";
            var parameters = new
            {
                doctorGuid,
                enable
            };
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<DoctorModel>(sql, parameters);
                return result.First();
            }
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
        /// 更新医生model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(DoctorModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.UpdateAsync(model)) > 0;
            }
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
                whereSql = $"{whereSql} AND user_name like @Name";
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
        D.count as article_qty,
        E.count as advisory_qty
	FROM
		t_doctor A
		LEFT JOIN t_utility_accessory B ON B.accessory_guid = A.portrait_guid
		LEFT JOIN t_utility_user C ON C.user_guid = A.doctor_guid 
        LEFT JOIN (SELECT author_guid,count(1) as count FROM t_utility_article WHERE `enable`=1 GROUP BY  author_guid) D ON D.author_guid = A.doctor_guid 
        LEFT JOIN (SELECT receiver_guid,count(1) as count FROM t_utility_topic WHERE `enable`=1 GROUP BY  receiver_guid) E ON E.receiver_guid = A.doctor_guid 
	) ___t 
WHERE
	{whereSql}
ORDER BY
	creation_date desc";
            request.Name = $"%{request.Name}%";
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
        /// 注册医生
        /// </summary>
        /// <param name="doctorModel">医生Model实例</param>
        /// <param name="certificates">证书项实例集合</param>
        /// <param name="accessories">附件实例集合</param>
        /// <param name="portraitAccessoryModel">一寸照附件</param>
        /// <returns></returns>
        public async Task<bool> UpdateDoctorAsync(DoctorModel doctorModel, UserModel userModel)
        {
            if (doctorModel == null)
            {
                return false;
            }
            bool result = await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                await conn.UpdateAsync(doctorModel);
                await conn.UpdateAsync(userModel);
                return true;
            });
            return result;
        }

        /// <summary>
        /// 获取医生头像和姓名
        /// </summary>
        /// <param name="doctorGuid"></param>
        /// <returns></returns>
        public async Task<GetDoctorNameAndNameResponseDto> GetDoctorNameAndNameAsync(string doctorGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.doctor_guid AS DoctorGuid,
	                            b.user_name AS DoctorName,
	                            CONCAT( c.base_path, c.relative_path ) AS DoctorPortraitUrl 
                            FROM
	                            t_doctor a
	                            INNER JOIN t_utility_user b ON a.doctor_guid = b.user_guid 
	                            AND a.`enable` = b.`enable`
	                            LEFT JOIN t_utility_accessory c ON a.portrait_guid = c.accessory_guid 
	                            AND a.`enable` = c.`enable` 
                            WHERE
	                            a.doctor_guid = @doctorGuid 
	                            AND a.`enable` =1";
                return await conn.QueryFirstOrDefaultAsync<GetDoctorNameAndNameResponseDto>(sql, new { doctorGuid });
            }
        }

        /// <summary>
        /// 获取当前科室和下属所有科室的医生列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetDoctorListByParentOfficeNodeResponseDto> GetDoctorListByParentOfficeNodeAsync(GetDoctorListByParentOfficeNodeRequestDto requestDto)
        {
            var sqlWhere = string.Empty;
            if (requestDto.Gender != null)
            {
                sqlWhere = "and u.gender=@Gender";
            }

            var sql = $@"SELECT
	                        doc.doctor_guid AS DoctorGuid,
	                        tbUser.user_name AS DoctorName,
	                        doc.hospital_guid AS HospitalGuid,
	                        hos.hos_name AS HospitalName,
	                        doc.office_guid AS OfficeGuid,
	                        doc.office_name AS OfficeName,
	                        jobTitle.config_name AS Title,
	                        doc.adept_tags AS AdeptTags,
	                        doc.honor AS Honor,
	                        CONCAT( picture.base_path, picture.relative_path ) AS Picture,
	                        CONCAT( hospicture.base_path, hospicture.relative_path ) AS HospitalPicture 
                        FROM
	                        t_doctor AS doc
                            inner join t_utility_user u on doc.doctor_guid=u.user_guid
	                        LEFT JOIN t_manager_dictionary AS jobTitle ON doc.title_guid = jobTitle.dic_guid 
	                        AND jobTitle.`enable` =
	                        TRUE LEFT JOIN t_utility_accessory AS picture ON accessory_guid = doc.portrait_guid 
	                        AND picture.`enable` =
	                        TRUE LEFT JOIN t_utility_user AS tbUser ON tbUser.user_guid = doc.doctor_guid 
	                        AND tbUser.`enable` =
	                        TRUE LEFT JOIN t_doctor_hospital AS hos ON hos.hospital_guid = doc.hospital_guid 
	                        AND hos.`enable` =
	                        TRUE LEFT JOIN t_utility_accessory AS hospicture ON hospicture.accessory_guid = hos.logo_guid 
                        WHERE
	                        doc.`enable` = TRUE 
	                        AND doc.`status` = 'approved' and hos.`enable`=1 {sqlWhere}
                            and doc.office_guid in @OfficeIds
                        ORDER BY
	                        doc.creation_date";
            return await MySqlHelper.QueryByPageAsync<GetDoctorListByParentOfficeNodeRequestDto, GetDoctorListByParentOfficeNodeResponseDto, GetDoctorListByParentOfficeNodeItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 获取医生咨询量
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetDocotrConsultationVolumeAsync(string doctorGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select sum(times) from t_doctor_consult_statistic where doctor_guid=@doctorGuid and `enable`=1";
                return (await conn.QueryFirstOrDefaultAsync<int?>(sql, new { doctorGuid })) ?? 0;
            }
        }

        /// <summary>
        /// 获取医院下推荐医生列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetHospitalRecommendDoctorListResponseDto> GetHospitalRecommendDoctorListAsync(GetHospitalRecommendDoctorListRequestDto requestDto)
        {
            var sql = @"SELECT
	                        a.doctor_guid,
	                        b.user_name AS doctor_name,
	                        a.hospital_guid,
	                        a.hospital_name,
	                        a.office_guid,
	                        a.office_name,
	                        a.adept_tags,
                            dic.config_name as title,
	                        CONCAT( c.base_path, c.relative_path ) AS portrait,
	                        CONCAT( d.base_path, d.relative_path ) AS hospital_logo 
                        FROM
	                        t_doctor a
	                        INNER JOIN t_utility_user b ON a.doctor_guid = b.user_guid
	                        INNER JOIN t_doctor_hospital h ON h.hospital_guid = a.hospital_guid
	                        LEFT JOIN t_utility_accessory c ON c.accessory_guid = a.portrait_guid
	                        LEFT JOIN t_utility_accessory d ON d.accessory_guid = h.logo_guid 
                            left join t_manager_dictionary dic on dic.dic_guid=a.title_guid
                        WHERE
	                        a.`enable` = 1 
	                        AND a.is_recommend = 1 
	                        AND a.hospital_guid = @HospitalGuid";
            return await MySqlHelper.QueryByPageAsync<GetHospitalRecommendDoctorListRequestDto, GetHospitalRecommendDoctorListResponseDto, GetHospitalRecommendDoctorListItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 获取患者信息
        /// </summary>
        /// <param name="userId">患者Id</param>
        /// <param name="doctorId">医生Id,选填，用于获取对用户的备注名称</param>
        /// <returns></returns>
        public async Task<GetPatientInfoResponseDto> GetPatientInfoAsync(string userId, string doctorId = "")
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.user_guid,
	                            a.gender,
                                a.nick_name,
	                            IFNULL( c.alias_name, a.nick_name ) AS alias_name,
	                            CONCAT( b.base_path, b.relative_path ) AS PortraitUrl 
                            FROM
	                            t_utility_user a
	                            LEFT JOIN t_utility_accessory b ON a.portrait_guid = b.accessory_guid
	                            LEFT JOIN t_utility_alias c ON c.user_guid = @doctorId 
	                            AND c.target_guid = a.user_guid 
                            WHERE
	                            a.user_guid = @userId;";
                return await conn.QueryFirstOrDefaultAsync<GetPatientInfoResponseDto>(sql, new { userId, doctorId });
            }
        }

        /// <summary>
        /// 根据一级科室名称获取医生分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetDoctorByFirstLevelOfficeNameResponseDto> GetDoctorByFirstLevelOfficeNameAsync(GetDoctorByFirstLevelOfficeNameRequestDto requestDto)
        {
            var sql = $@"SELECT
	                        doc.doctor_guid AS DoctorGuid,
	                        tbUser.user_name AS DoctorName,
	                        doc.hospital_guid AS HospitalGuid,
	                        hos.hos_name AS HospitalName,
	                        doc.office_guid AS OfficeGuid,
	                        doc.office_name AS OfficeName,
	                        jobTitle.config_name AS Title,
	                        doc.adept_tags AS AdeptTags,
	                        doc.honor AS Honor,
	                        CONCAT( picture.base_path, picture.relative_path ) AS Picture,
	                        CONCAT( hospicture.base_path, hospicture.relative_path ) AS HospitalPicture 
                        FROM
	                        t_doctor AS doc
                            inner join t_utility_user u on doc.doctor_guid=u.user_guid
	                        LEFT JOIN t_manager_dictionary AS jobTitle ON doc.title_guid = jobTitle.dic_guid 
	                        AND jobTitle.`enable` =
	                        TRUE LEFT JOIN t_utility_accessory AS picture ON accessory_guid = doc.portrait_guid 
	                        AND picture.`enable` =
	                        TRUE LEFT JOIN t_utility_user AS tbUser ON tbUser.user_guid = doc.doctor_guid 
	                        AND tbUser.`enable` =
	                        TRUE LEFT JOIN t_doctor_hospital AS hos ON hos.hospital_guid = doc.hospital_guid 
	                        AND hos.`enable` =
	                        TRUE LEFT JOIN t_utility_accessory AS hospicture ON hospicture.accessory_guid = hos.logo_guid 
                        WHERE
	                        doc.`enable` = TRUE 
	                        AND doc.`status` = 'approved' and hos.`enable`=1
                            and doc.office_guid in @OfficeIds
                        ORDER BY
	                        doc.creation_date";
            return await MySqlHelper.QueryByPageAsync<GetDoctorByFirstLevelOfficeNameRequestDto, GetDoctorByFirstLevelOfficeNameResponseDto, GetDoctorByFirstLevelOfficeNameItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 获取医生粉丝列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<GetDoctorFansListItemDto>> GetDoctorFansListAsync(GetDoctorFansListRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var keywordWhere = "";
                if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
                {
                    keywordWhere = $" and b.nick_name like @Keyword";
                    requestDto.Keyword = $"%{requestDto.Keyword}%";
                }
                var sql = $@"SELECT
	                            b.user_guid as UserGuid,
	                            b.user_name as UserName,
	                            IFNULL(d.alias_name,b.nick_name) AS NickName,
	                            CONCAT( c.base_path, c.relative_path ) AS PortraitUrl 
                            FROM
	                            t_consumer_collection a
	                            INNER JOIN t_utility_user b ON a.user_guid = b.user_guid
	                            LEFT JOIN t_utility_accessory c ON b.portrait_guid = c.accessory_guid 
	                            AND c.`enable` = 1 
                                left join t_utility_alias d on d.user_guid=a.target_guid and d.target_guid=a.user_guid
                            WHERE
	                            a.target_guid = @DoctorGuid 
	                            AND a.`enable` = 1 
	                            AND b.`enable` = 1 
                                {keywordWhere}
                                order by NickName";
                var result = await conn.QueryAsync<GetDoctorFansListItemDto>(sql, requestDto);
                return result?.ToList();
            }
        }

        /// <summary>
        /// 获取名医推荐医生列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetFamousRecommendDoctorListResponseDto> GetFamousRecommendDoctorListAsync(GetFamousRecommendDoctorListRequestDto requestDto)
        {
            var sql = @"SELECT
	                        a.doctor_guid,
	                        b.user_name AS doctor_name,
	                        a.hospital_guid,
	                        a.hospital_name,
	                        a.office_guid,
	                        a.office_name,
	                        a.adept_tags,
                            dic.config_name as title,
	                        CONCAT( c.base_path, c.relative_path ) AS portrait 
                        FROM
	                        t_doctor a
	                        INNER JOIN t_utility_user b ON a.doctor_guid = b.user_guid
	                        INNER JOIN t_doctor_hospital h ON h.hospital_guid = a.hospital_guid
	                        LEFT JOIN t_utility_accessory c ON c.accessory_guid = a.portrait_guid
	                        LEFT JOIN t_utility_accessory d ON d.accessory_guid = h.logo_guid 
                            left join t_manager_dictionary dic on dic.dic_guid=a.title_guid
                        WHERE
	                        a.`enable` = 1 
	                        AND a.is_recommend = 1 
	                        order by a.recommend_sort desc";
            return await MySqlHelper.QueryByPageAsync<GetFamousRecommendDoctorListRequestDto, GetFamousRecommendDoctorListResponseDto, GetFamousRecommendDoctorListItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 获取科室下的医生列表
        /// </summary>
        /// <param name="officeGuid"></param>
        /// <returns></returns>
        public async Task<List<KeyValueDto<string, string>>> GetDoctorSelectListOfOfficeAsync(string officeGuid)
        {
            var sql = @"SELECT
	                        a.doctor_guid as `key`,
	                        b.user_name as `value`
                        FROM
	                        t_doctor a
	                        LEFT JOIN t_utility_user b ON a.doctor_guid = b.user_guid
                        where a.office_guid=@officeGuid and a.`enable`=1 and a.`status`='approved'	
                        order by b.user_name ;";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<KeyValueDto<string, string>>(sql, new { officeGuid });
                return result.ToList();
            }
        }

        /// <summary>
        /// 获取医院下的医生列表
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <returns></returns>
        public async Task<List<KeyValueDto<string, string>>> GetDoctorSelectListOfHospitalAsync(string hospitalGuid)
        {
            var sql = @"SELECT
	                        a.doctor_guid as `key`,
	                        b.user_name as `value`
                        FROM
	                        t_doctor a
							inner join t_doctor_office o on o.office_guid=a.office_guid and o.`enable`=a.`enable`
	                        LEFT JOIN t_utility_user b ON a.doctor_guid = b.user_guid
                        where a.hospital_guid=@hospitalGuid and a.`enable`=1 and a.`status`='approved'	
                        order by b.user_name ;";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<KeyValueDto<string, string>>(sql, new { hospitalGuid });
                return result.ToList();
            }
        }
    }
}
