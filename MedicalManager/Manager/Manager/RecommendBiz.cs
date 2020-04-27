using Dapper;
using GD.DataAccess;
using GD.Dtos.Recommend;
using GD.Models.Doctor;
using GD.Models.Manager;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager.Manager
{
    public class RecommendBiz : BaseBiz<RecommendModel>
    {
        public async Task<bool> DeleteRecommendAsync(string id)
        {
            var response = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.DeleteAsync<RecommendModel>(id);
                await conn.DeleteListAsync<RecommendDetailModel>("where recommend_guid = @recommendGuid", new { recommendGuid = id });
                return true;
            });
            return response;
        }
        /// <summary>
        /// 获取医生推荐列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetRecommendDoctorListResponseDto> GetRecommendDoctorListAsync(GetRecommendDoctorListRequestDto request)
        {
            var sqlWhere = $@" 1=1 AND A.type = @type AND A.ENABLE = @enable and status='{DoctorModel.StatusEnum.Approved.ToString()}'";
            if (!string.IsNullOrWhiteSpace(request.RecommendGuid))
            {
                sqlWhere = $"{sqlWhere} AND A.recommend_guid=@RecommendGuid";
            }
            if (!string.IsNullOrWhiteSpace(request.HospitalGuid))
            {
                sqlWhere = $"{sqlWhere} AND C.hospital_guid=@HospitalGuid";
            }
            var sql = $@"
SELECT
	A.type,
	A.recommend_guid AS RecommendGuid,
	B.owner_guid,
	B.detail_guid,
    C.hospital_guid AS HospitalGuid,
	C.doctor_guid AS DoctorGuid,
	C.work_city AS WorkCity,
	C.office_guid AS OfficeGuid,
	C.office_name AS OfficeName,
	C.hospital_name,
	C.adept_tags AS AdeptTags,
    C.title_guid AS DocTitleGuid,
    C.status,
	C.honor AS Honors,
	CONCAT( E.base_path, E.relative_path ) AS DoctorLogo,
	CONCAT( F.base_path, F.relative_path ) AS PortraitUrl ,
	G.user_name 
FROM
	t_manager_recommend A
	INNER JOIN t_manager_recommend_detail B ON A.recommend_guid = B.recommend_guid
	INNER JOIN t_doctor C ON B.owner_guid = C.doctor_guid AND C.ENABLE = 1
	LEFT JOIN t_doctor_hospital D ON D.hospital_guid = C.hospital_guid
	LEFT JOIN t_utility_accessory E ON E.accessory_guid = D.logo_guid
	LEFT JOIN t_utility_accessory F ON F.accessory_guid = C.portrait_guid 
	LEFT JOIN t_utility_user G ON G.user_guid = C.doctor_guid 
WHERE
	{sqlWhere}
ORDER BY
	C.creation_date";
            var parameters = new
            {
                type = RecommendModel.TypeEnum.Doctor.ToString(),
                enable = true,
                request.HospitalGuid,
                request.RecommendGuid,
            };
            var pageSql = $"{sql} limit {(request.PageIndex - 1) * request.PageSize},{request.PageSize}";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetDoctorRecommendListItemDto>(pageSql, parameters);
                var count = await conn.QueryFirstOrDefaultAsync<int>($@"SELECT COUNT(1) AS count FROM({sql}) AS T", parameters);
                return new GetRecommendDoctorListResponseDto
                {
                    Total = count,
                    CurrentPage = result
                };
            }
        }

        /// <summary>
        /// 获取医生推荐列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetRecommendHospitalListResponseDto> GetRecommendHospitalListAsync(GetRecommendHospitalListRequestDto request)
        {
            var sqlWhere = $@" 1=1 AND A.type = @type AND A.ENABLE = @enable";
            if (!string.IsNullOrWhiteSpace(request.RecommendGuid))
            {
                sqlWhere = $"{sqlWhere} AND A.recommend_guid=@RecommendGuid";
            }

            var sql = $@"
SELECT
	A.type,
	A.recommend_guid,
	B.owner_guid,
	B.detail_guid,
	C.hospital_guid,
	C.logo_guid,
	C.hos_name,
	C.hos_abstract,
	C.hos_level,
	C.location,
	CONCAT( D.base_path, D.relative_path ) AS LogoUrl 
FROM
	t_manager_recommend A
	INNER JOIN t_manager_recommend_detail B ON A.recommend_guid = B.recommend_guid
	INNER JOIN t_doctor_hospital C ON B.owner_guid = C.hospital_guid AND C.ENABLE = 1
	LEFT JOIN t_utility_accessory D ON D.accessory_guid = C.logo_guid 
WHERE
	{sqlWhere}
ORDER BY
	C.creation_date ";
            var parameters = new
            {
                type = RecommendModel.TypeEnum.Hostpital.ToString(),
                enable = true,
                request.RecommendGuid,
            };
            var pageSql = $"{sql} limit {(request.PageIndex - 1) * request.PageSize},{request.PageSize}";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetHospitalRecommendListItemDto>(pageSql, parameters);
                var count = await conn.QueryFirstOrDefaultAsync<int>($@"SELECT COUNT(1) AS count FROM({sql}) AS T", parameters);
                return new GetRecommendHospitalListResponseDto
                {
                    Total = count,
                    CurrentPage = result
                };
            }
        }
        /// <summary>
        /// 获取医生推荐列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetRecommendOfficeListResponseDto> GetRecommendOfficeListAsync(GetRecommendOfficeListAsyncRequestDto request)
        {

            var sqlWhere = $@" 1=1 AND A.type = @type AND A.ENABLE = @enable";
            if (!string.IsNullOrWhiteSpace(request.RecommendGuid))
            {
                sqlWhere = $"{sqlWhere} AND A.recommend_guid=@RecommendGuid";
            }
            var sql = $@"
SELECT
	A.type,
	A.recommend_guid,
	B.detail_guid,
	C.office_guid,
	C.office_name,
	C.hospital_guid,
	C.hospital_name,
	C.picture_guid,
	CONCAT( D.base_path, D.relative_path ) AS PictureUrl 
FROM
	t_manager_recommend A
	INNER JOIN t_manager_recommend_detail B ON A.recommend_guid = B.recommend_guid
	INNER JOIN t_doctor_office C ON B.owner_guid = C.office_guid  AND C.ENABLE = 1
	LEFT JOIN t_utility_accessory D ON D.accessory_guid = C.picture_guid 
WHERE
	{sqlWhere}
ORDER BY
	C.creation_date ";
            var parameters = new
            {
                type = RecommendModel.TypeEnum.Office.ToString(),
                enable = true,
                request.RecommendGuid,
                request.HospitalGuid
            };
            var pageSql = $"{sql} limit {(request.PageIndex - 1) * request.PageSize},{request.PageSize}";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetRecommendOfficeListItemDto>(pageSql, parameters);
                var count = await conn.QueryFirstOrDefaultAsync<int>($@"SELECT COUNT(1) AS count FROM({sql}) AS T", parameters);
                return new GetRecommendOfficeListResponseDto
                {
                    Total = count,
                    CurrentPage = result
                };
            }
        }
        /// <summary>
        /// 获取文章推荐列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetRecommendArticleListResponseDto> GetRecommendArticleListAsync(GetRecommendArticleListRequestDto request)
        {
            var sqlWhere = $@" 1=1 AND type = @type AND ENABLE = 1";
            if (!string.IsNullOrWhiteSpace(request.RecommendGuid))
            {
                sqlWhere = $"{sqlWhere} AND recommend_guid=@RecommendGuid";
            }
            var sql = $@"
SELECT * FROM(
    SELECT
	    A.type,
	    A.recommend_guid,
        A.creation_date,
	    B.owner_guid,
	    B.detail_guid,
	    C.article_guid,
	    C.author_guid,
	    E.nick_name AS author_name,
        A.ENABLE,
	    C.title,
	    C.abstract,
	    C.picture_guid,
	    C.article_type_dic,
	    CONCAT( D.base_path, D.relative_path ) AS PictureUrl 
    FROM
	    t_manager_recommend A
	    INNER JOIN t_manager_recommend_detail B ON A.recommend_guid = B.recommend_guid
	    INNER JOIN t_utility_article C ON B.owner_guid = C.article_guid AND C.ENABLE=1
	    LEFT JOIN t_utility_accessory D ON D.accessory_guid = C.picture_guid
	    LEFT JOIN t_utility_user E ON E.user_guid = C.author_guid 
    WHERE 1=1 AND C.actcle_release_status='{Models.Utility.ReleaseStatus.Release.ToString()}'
) ____T
WHERE
	{sqlWhere}
ORDER BY
	creation_date desc";
            var parameters = new
            {
                type = RecommendModel.TypeEnum.Article.ToString(),
                request.RecommendGuid,
            };
            var pageSql = $"{sql} limit {(request.PageIndex - 1) * request.PageSize},{request.PageSize}";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetRecommendArticleListItemDto>(pageSql, parameters);
                var count = await conn.QueryFirstOrDefaultAsync<int>($@"SELECT COUNT(1) AS count FROM({sql}) AS T", parameters);
                return new GetRecommendArticleListResponseDto
                {
                    Total = count,
                    CurrentPage = result
                };
            }
        }
        /// <summary>
        /// 获取商品推荐列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetRecommendProductListResponseDto> GetRecommendProductListAsync(GetRecommendProductListRequestDto request)
        {
            var sqlWhere = $@" 1=1 AND A.type = '{RecommendModel.TypeEnum.Product.ToString()}' AND A.ENABLE = 1";
            if (!string.IsNullOrWhiteSpace(request.RecommendGuid))
            {
                sqlWhere = $"{sqlWhere} AND A.recommend_guid=@RecommendGuid";
            }
            var sql = $@"
SELECT
	A.type,
	A.recommend_guid,
	B.owner_guid,
	B.detail_guid,
	C.*,
	CONCAT( D.base_path, D.relative_path ) AS PictureUrl 
FROM
	t_manager_recommend A
	INNER JOIN t_manager_recommend_detail B ON A.recommend_guid = B.recommend_guid
	INNER JOIN t_mall_product C ON B.owner_guid = C.product_guid AND C.ENABLE = 1
	LEFT JOIN t_utility_accessory D ON D.accessory_guid = C.picture_guid
WHERE
	{sqlWhere}
ORDER BY
	C.creation_date ";
            var result = await MySqlHelper.QueryByPageAsync<GetRecommendProductListRequestDto, GetRecommendProductListResponseDto, GetRecommendProductListItemDto>(sql, request);
            return result;
        }
        /// <summary>
        /// 推荐课程
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetRecommendCourseListResponseDto> GetRecommendCourseListAsync(GetRecommendCourseListRequestDto request)
        {
            var sqlWhere = $@" 1=1 AND A.type = '{RecommendModel.TypeEnum.Course.ToString()}' AND A.ENABLE = 1";
            if (!string.IsNullOrWhiteSpace(request.RecommendGuid))
            {
                sqlWhere = $"{sqlWhere} AND A.recommend_guid=@RecommendGuid";
            }
            var sql = $@"
SELECT
	A.type,
	A.recommend_guid,
	B.owner_guid,
	B.detail_guid,
	C.*,
	CONCAT( D.base_path, D.relative_path ) AS LogoUrl 
FROM
	t_manager_recommend A
	INNER JOIN t_manager_recommend_detail B ON A.recommend_guid = B.recommend_guid
	INNER JOIN t_utility_course C ON B.owner_guid = C.course_guid AND C.ENABLE = 1
	LEFT JOIN t_utility_accessory D ON D.accessory_guid = C.logo_guid
WHERE
	{sqlWhere}
ORDER BY
	C.creation_date ";
            var result = await MySqlHelper.QueryByPageAsync<GetRecommendCourseListRequestDto, GetRecommendCourseListResponseDto, GetRecommendCourseListItemDto>(sql, request);
            return result;
        }

        public async Task<GetRecommendPageResponseDto> GetRecommendPageAsync(GetRecommendPageRequestDto request)
        {
            var whereSql = $@"1=1";
            if (!string.IsNullOrWhiteSpace(request.Type))
            {
                whereSql = $"{whereSql} AND type=@Type";
            }
            var sortFields = new string[] { "sort".ToLower(), "creation_date".ToLower() };
            var orderbySql = $"{sortFields.First()} DESC";
            if (sortFields.Contains(request.SortField?.ToLower()))
            {
                orderbySql = $"{request.SortField} {(request.IsAscending ? "asc" : "desc")}";
            }
            var sql = $@"
SELECT * FROM(
    SELECT
	    A.recommend_guid,
	    A.NAME,
	    A.target,
	    A.remark,
	    A.type,
	    A.Sort ,
        A.ENABLE,
        A.Picture_Guid,
        A.creation_date,
	    B.accessory_guid,
	    B.base_path,
	    B.relative_path,
	    CONCAT( B.base_path, B.relative_path ) AS PictureUrl 
    FROM
	    t_manager_recommend A
	    LEFT JOIN t_utility_accessory B ON A.picture_guid = B.accessory_guid 
) ____T
WHERE
	{whereSql}
ORDER BY
	{orderbySql}";

            return await MySqlHelper.QueryByPageAsync<GetRecommendPageRequestDto, GetRecommendPageResponseDto, GetRecommendPageItemDto>(sql, request);

        }
    }
}
