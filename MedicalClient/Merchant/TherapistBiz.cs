using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Mall.Category;
using GD.Dtos.Merchant.Merchant;
using GD.Dtos.Merchant.Therapist;
using GD.Models.Merchant;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Merchant
{
    /// <summary>
    /// 服务人员业务类
    /// </summary>
    public class TherapistBiz : BaseBiz<TherapistModel>
    {
        /// <summary>
        /// 通过服务人员id获取服务人员model
        /// </summary>
        /// <param name="therapistId"></param>
        /// <returns></returns>
        public async Task<TherapistModel> GetModelAsync(string therapistId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<TherapistModel>("select * from t_merchant_therapist where therapist_guid=@therapistId and `enable`=1", new { therapistId });
            }
        }


        /// <summary>
        /// 获取商户某天某项目的服务人员和排班详情
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<TherapistsScheduleByProjectIdOneDayResponseDto> GetTherapistsScheduleByProjectIdOneDayAsync(GetTherapistsScheduleByProjectIdOneDayRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"DROP TEMPORARY TABLE
                            IF
	                            EXISTS tmp_merchant_therapist;
                            DROP TEMPORARY TABLE
                            IF
	                            EXISTS tmp_merchant_therapist_1;
                            CREATE TEMPORARY TABLE tmp_merchant_therapist AS SELECT distinct
                            b.therapist_guid,
                            b.therapist_name,
                            CONCAT( c.base_path, c.relative_path ) AS PortraitUrl 
                            FROM
	                            t_merchant_therapist_project a
	                            INNER JOIN t_merchant_therapist b ON a.therapist_guid = b.therapist_guid 
	                            AND a.`enable` = b.`enable`
	                            LEFT JOIN t_utility_accessory c ON b.portrait_guid = c.accessory_guid 
                            WHERE
	                            a.project_guid = @projectGuid 
	                            and b.merchant_guid=@merchantGuid
	                            AND a.`enable` = 1;
                            CREATE TEMPORARY TABLE tmp_merchant_therapist_1 SELECT
                            * 
                            FROM
	                            tmp_merchant_therapist;
                            SELECT
	                            * 
                            FROM
	                            tmp_merchant_therapist;
	
                            SELECT
                              t.therapist_guid,
                                sche.schedule_guid,
	                            detail.schedule_detail_guid,
	                            detail.consumption_guid,
	                            detail.start_time,
	                            detail.end_time
                            FROM
	                            tmp_merchant_therapist_1 t
	                            INNER JOIN t_merchant_schedule sche ON t.therapist_guid = sche.target_guid
	                            left JOIN t_merchant_schedule_detail detail ON sche.schedule_guid = detail.schedule_guid and sche.`enable`=detail.`enable`
                            WHERE
	                            sche.schedule_date = @scheduleDate 
	                            AND sche.`enable` = 1 ;
                            DROP TEMPORARY TABLE tmp_merchant_therapist;
                            DROP TEMPORARY TABLE tmp_merchant_therapist_1;";

                var reader = await conn.QueryMultipleAsync(sql, new { projectGuid = requestDto.ProjectGuid, merchantGuid = requestDto.MerchantGuid, scheduleDate = requestDto.ScheduleDate.Date });

                var therapists = (await reader.ReadAsync<TherapistsByProjectIdOneDayDto>())?.ToList();

                var scheduleDetails = (await reader.ReadAsync<TherapistsScheduleByProjectIdOneDayDto>())?.ToList();

                if (!therapists.Any())
                {
                    return null;
                }

                return new TherapistsScheduleByProjectIdOneDayResponseDto
                {
                    Therapists = therapists,
                    ScheduleDetails = scheduleDetails
                };

            }
        }

        /// <summary>
        /// 通过服务人员手机号获取服务人员model
        /// </summary>
        /// <param name="phone">服务人员登录手机号</param>
        /// <returns></returns>
        public async Task<TherapistModel> GetModelByPhoneAsync(string phone)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryFirstOrDefaultAsync<TherapistModel>("select * from t_merchant_therapist where therapist_phone=@phone and `enable`=1", new { phone }));
            }
        }

        /// <summary>
        /// 获取服务人员详情
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="therapistGuid"></param>
        /// <returns></returns>
        public async Task<GetTherapistDetailInfoResponseDto> GetTherapistDetailInfoAsync(
            string merchantGuid,
            string therapistGuid)
        {
            var sql = @"SELECT
	                            a.therapist_guid as TherapistGuid,
	                            a.therapist_name AS TherapistName,
                                a.therapist_phone AS TherapistPhone,
	                            a.portrait_guid AS PortraitGuid,
	                            CONCAT(b.base_path,b.relative_path) as PortraitUrl,
                                a.introduction AS Introduction,
                                a.job_title AS JobTitle,
                                a.tag,
	                            a.creation_date AS CreationDate
                            FROM
	                            t_merchant_therapist a
	                            LEFT JOIN t_utility_accessory b ON a.portrait_guid = b.accessory_guid
	                            AND a.`enable` = b.`enable`
                          WHERE a.therapist_guid = @TherapistGuid AND
                                a.merchant_guid = @MerchantGuid AND a.`enable`= 1";

            var parameters = new DynamicParameters();
            parameters.Add("@TherapistGuid", therapistGuid);
            parameters.Add("@MerchantGuid", merchantGuid);

            var response = (GetTherapistDetailInfoResponseDto)null;

            using (var conn = MySqlHelper.GetConnection())
            {
                //获取服务人员基础数据
                response = (await conn.QueryFirstOrDefaultAsync<GetTherapistDetailInfoResponseDto>(sql, commandType: CommandType.Text, param: parameters));

                if (response is null)
                {
                    return null;
                }

                sql = $@"
                    SELECT DISTINCT
                          b.classify_guid as ClassifyGuid,
                          b.classify_name  as ClassifyName
                    FROM t_merchant_therapist_classify as a
	                    INNER JOIN t_merchant_category_extension as b ON a.classify_guid = b.classify_guid
                    WHERE a.therapist_guid = '{therapistGuid}' AND b.merchant_guid = '{merchantGuid}'";

                //获取服务人员所关联的分类
                response.Classifies = (await conn.QueryAsync<TherapistClassify>(sql, commandType: CommandType.Text)).ToList();

                sql = @"SELECT
	                                       tp.project_guid as ProjectGuid,
                                           proj.project_name as ProjectName
                                        FROM
	                                        t_merchant_therapist a
	                                        LEFT JOIN t_merchant_therapist_project AS tp ON tp.therapist_guid = a.therapist_guid 
	                                        AND tp.ENABLE =
	                                        TRUE INNER JOIN t_mall_project AS proj ON proj.project_guid = tp.project_guid 
	                                        AND proj.ENABLE = TRUE 
                                        WHERE
	                                        a.therapist_guid = @TherapistGuid 
	                                        AND a.`enable` = TRUE 
                                        ORDER BY
	                                        a.Creation_Date DESC ";

                //获取服务人员所关联分类下的项目
                response.Projects = (await conn.QueryAsync<ClassifyProject>(sql, new { therapistGuid })).ToList();

                return response;
            }
        }

        /// <summary>
        /// 分页获取服务人员列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetTherapistListResponseDto> GetTherapistListAsync(GetTherapistListRequestDto requestDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@MerchantGuid", requestDto.MerchantGuid);

            var sql = $@"SELECT
	                            a.therapist_guid as TherapistGuid,
	                            a.therapist_name AS TherapistName,
                                a.therapist_phone AS TherapistPhone,
	                            a.creation_date AS CreationDate
                            FROM
	                            t_merchant_therapist a
                          WHERE a.merchant_guid = @MerchantGuid AND a.`enable`= 1";

            if (!string.IsNullOrEmpty(requestDto.KeyWord))
            {
                sql += $" and a.therapist_name like '%{requestDto.KeyWord}%' OR a.therapist_phone like '%{requestDto.KeyWord}%'";
            }

            sql += " order by a.creation_date desc";

            var rows = (List<GetTherapistListItemDto>)null;

            var classifyItems = (List<TherapistClassifyItem>)null;

            using (var conn = MySqlHelper.GetConnection())
            {
                rows = (await conn.QueryAsync<GetTherapistListItemDto>(sql, commandType: CommandType.Text, param: parameters)).ToList();

                if (rows?.Count() <= 0)
                {
                    return null;
                }

                var ids = string.Join(",", rows.Select(x => "'" + x.TherapistGuid + "'").ToArray());

                sql = $@"
                    SELECT DISTINCT
                           a.therapist_guid AS TherapistGuid, 
                           b.classify_guid AS ClassifyGuid,
                           b.classify_name  AS ClassifyName
                    FROM t_merchant_therapist_classify as a
	                    INNER JOIN t_merchant_category_extension as b ON a.classify_guid = b.classify_guid
                    WHERE a.therapist_guid in ({ids}) AND b.merchant_guid = '{requestDto.MerchantGuid}'";

                classifyItems = (await conn.QueryAsync<TherapistClassifyItem>(sql, commandType: CommandType.Text)).ToList();
            }

            foreach (var row in rows.ToList())
            {
                row.Items = classifyItems.Where(d => d.TherapistGuid ==
                    row.TherapistGuid)?.ToList();

                if (!string.IsNullOrEmpty(requestDto.ClassifyGuid))
                {
                    //通过平台分类Id过滤数据
                    row.Items = row.Items.Where(d => d.ClassifyGuid.Equals(requestDto.ClassifyGuid))?.ToList();

                    //若查询分类不存在，则将基础数据行给移除
                    if (row.Items?.Count <= 0)
                    {
                        rows.Remove(row);
                    }
                }
            }

            var total = rows.Count();

            var offset = (requestDto.PageIndex - 1) * requestDto.PageSize;

            rows = rows.Skip(offset).Take(requestDto.PageSize).ToList();

            return new GetTherapistListResponseDto()
            {
                CurrentPage = rows,
                Total = total
            };
        }

        /// <summary>
        /// 检查服务人员手机号码是否已注册                                                               
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IsTherapistPhoneExist(string phone, bool enable = true,
            string therapistGuid = null)
        {
            var sql = "where therapist_phone = @phone and enable = @enable";

            if (!string.IsNullOrEmpty(therapistGuid))
            {
                sql += " and therapist_guid<> @therapistGuid";
            }

            return MySqlHelper.Count<TherapistModel>(sql, new { phone, enable, therapistGuid }) > 0;
        }

        /// <summary>
        /// 新增服务人员及其关联服务项目
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public bool AddNewTherapist(TherapistModel model, List<TherapistProjectModel> therapistProjectModels, List<MerchantTherapistClassifyModel> classifyModels)
        {
            var result = MySqlHelper.Transaction((conn, tran) =>
            {
                //服务人员信息
                if (string.IsNullOrWhiteSpace(model.Insert(conn)))
                {
                    return false;
                }

                //服务人员与分类关联
                if (classifyModels.Any())
                {
                    foreach (var classifyModel in classifyModels)
                    {
                        if (string.IsNullOrWhiteSpace(classifyModel.Insert(conn)))
                        {
                            return false;
                        }
                    }
                }

                //服务人员与服务项目关联
                if (therapistProjectModels.Any())
                {
                    foreach (var item in therapistProjectModels)
                    {
                        if (string.IsNullOrWhiteSpace(item.Insert(conn)))
                        {
                            return false;
                        }
                    }
                }
                return true;
            });
            return result;
        }

        /// <summary>
        /// 编辑服务人员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateTherapist(TherapistModel model,
            List<TherapistProjectModel> tpModelList,
            List<MerchantTherapistClassifyModel> classifyModels)
        {
            var result = MySqlHelper.Transaction((conn, tran) =>
            {
                //删除服务人员之前关联项目
                if (tpModelList.Any())
                {
                    var deleteResult = conn.Execute($"DELETE FROM t_merchant_therapist_project WHERE therapist_guid = '{model.TherapistGuid}'");

                    if (deleteResult <= 0)
                    {
                        return false;
                    }
                }

                //删除服务人员与分类并更新
                if (classifyModels.Any())
                {
                    var deleteResult = conn.Execute($"DELETE FROM t_merchant_therapist_classify WHERE therapist_guid = '{model.TherapistGuid}'");

                    if (deleteResult <= 0)
                    {
                        return false;
                    }

                    foreach (var classifyModel in classifyModels)
                    {
                        if (string.IsNullOrWhiteSpace(classifyModel.Insert(conn)))
                        {
                            return false;
                        }
                    }
                }

                //更新服务人员信息
                if (model.Update(conn) < 1)
                {
                    return false;
                }

                //服务人员与服务项目的关联
                if (tpModelList.Any())
                {
                    foreach (var item in tpModelList)
                    {
                        if (string.IsNullOrWhiteSpace(item.Insert(conn)))
                        {
                            return false;
                        }
                    }
                }
                return true;
            });
            return result;
        }

        /// <summary>
        /// 获取指定商户分类下服务人员列表
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <returns></returns>
        public async Task<List<GetMerchantClassifyTherapistListResponseDto>> GetMerchantClassifyTherapists(string merchantGuid)
        {
            var classifies = await new MerchantCategoryBiz().GetClassifies(merchantGuid, true);

            if (classifies?.Count() <= 0)
            {
                return null;
            }

            //var ids = string.Join(",", classifies.Select(x => "'" + ((dynamic)x).id + "'").ToArray());

            //     var sql = $@"SELECT c.classify_guid,
            //                         t.therapist_guid,
            //                         t.therapist_name 
            //                     FROM t_merchant_therapist as t 
            //                         INNER JOIN (SELECT therapist_guid,classify_guid FROM t_merchant_therapist_classify
            //WHERE classify_guid in ({ids}) and enable = 1) as c
            // ON t.therapist_guid = c.therapist_guid ";

            var items = (List<MerchantClassifyTherapistItem>)null;

            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                            b.classify_guid,
	                            a.therapist_guid,
	                            a.therapist_name 
                            FROM
	                            t_merchant_therapist a
	                            INNER JOIN t_merchant_therapist_classify b ON a.therapist_guid = b.therapist_guid 
	                            AND a.`enable` = b.`enable` 
                            WHERE
	                            b.classify_guid IN @ids 
	                            AND a.merchant_guid = @merchantGuid";
                items = (await conn.QueryAsync<MerchantClassifyTherapistItem>(sql,
                            new
                            {
                                ids = classifies.Select(x => ((dynamic)x).id).ToList(),
                                merchantGuid
                            })).ToList();
            }

            if (items?.Count() <= 0)
            {
                return null;
            }

            var classifyTherapists = new List<GetMerchantClassifyTherapistListResponseDto>();

            foreach (var classify in classifies)
            {
                var classifyTherapist = new GetMerchantClassifyTherapistListResponseDto()
                {
                    Id = ((dynamic)classify).id,
                    Name = ((dynamic)classify).name
                };

                classifyTherapist.Items = items.Where(d => d.ClassifyGuid == classifyTherapist.Id).ToList();

                classifyTherapists.Add(classifyTherapist);
            }
            return classifyTherapists;
        }

        /// <summary>
        /// 类目的团队介绍
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetTreamIntroduceResponseDto> GetTreamIntroduceList(GetTreamIntroduceListRequest request)
        {
            var sql = $@"SELECT
	                        mt.therapist_guid,
	                        mt.therapist_name,
	                        mt.job_title,
	                        mt.therapist_phone,
	                        CONCAT( acc.base_path, acc.relative_path ) AS PortraitURL,
	                        mt.tag,
	                        mt.introduction,
	                        mt.sort 
                        FROM
	                        t_merchant_therapist AS mt
	                        INNER JOIN t_merchant_therapist_classify AS tc ON tc.therapist_guid = mt.therapist_guid 
	                        AND tc.ENABLE = 1
	                        LEFT JOIN t_utility_accessory AS acc ON mt.portrait_guid = acc.accessory_guid 
                        WHERE
	                        mt.merchant_guid = @MerchantGuid 
	                        AND tc.classify_guid = @DicGuid 
	                        AND mt.ENABLE = 1 
                        ORDER BY
	                        mt.therapist_name ";

            return await MySqlHelper.QueryByPageAsync<GetTreamIntroduceListRequest, GetTreamIntroduceResponseDto, GetTreamIntroduceItemResponse>(sql, request);
        }

    }
}
