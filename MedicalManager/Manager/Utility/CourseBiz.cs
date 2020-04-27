using Dapper;
using GD.DataAccess;
using GD.Dtos.Course;
using GD.Models.Utility;
using System.Threading.Tasks;

namespace GD.Manager.Utility
{
    /// <summary>
    /// 课程 业务类
    /// </summary>
    public class CourseBiz : BaseBiz<CourseModel>
    {

        public async Task<GetCourseListResponseDto> GetCourseListAsync(GetCourseListRequestDto request)
        {
            var orderbySql = "creation_date desc";
            if (!string.IsNullOrWhiteSpace(request.SortField))
            {
                orderbySql = $"{request.SortField} {(request.IsAscending ? "asc" : "desc")}";
            }
            var sqlWhere = $@"1 = 1";

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                sqlWhere = $"{sqlWhere} AND (title like @Keyword  OR summary like @Keyword)";
            }
            var sql = $@"
SELECT * FROM(
	SELECT
	a.*,
	b.nick_name,
	( SELECT count( 1 ) FROM t_consumer_article_view __s WHERE a.course_guid = __s.target_guid GROUP BY target_guid ) AS visit_count,
	( SELECT count( 1 ) FROM t_consumer_collection __s where a.course_guid = __s.target_guid  GROUP BY target_guid ) AS collection 
FROM
	t_utility_course a
	LEFT JOIN t_utility_user b ON a.author_guid = b.user_guid
) T 
WHERE
	{sqlWhere}
ORDER BY
	{orderbySql}
";
            request.Keyword = $"%{request.Keyword}%";
            return await MySqlHelper.QueryByPageAsync<GetCourseListRequestDto, GetCourseListResponseDto, GetCourseListItemDto>(sql, request);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(RichtextModel richtextModel, CourseModel courseModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.InsertAsync<string, CourseModel>(courseModel);
                await conn.InsertAsync<string, RichtextModel>(richtextModel);
                return true;
            });

        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(RichtextModel richtextModel, CourseModel model)
        {
            return await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.UpdateAsync(richtextModel);
                await conn.UpdateAsync(model);
                return true;
            });
        }

        /// <summary>
        /// 搜索文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SearchCourseResponseDto> SearchCourseAsync(SearchCourseRequestDto request)
        {
            var sqlWhere = $@"AND ENABLE = 1 AND visible = 1";

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                sqlWhere = $"{sqlWhere} AND (title like @Keyword  OR summary like @Keyword)";
            }
            var sql = $@"
SELECT * FROM(
	SELECT
		A.*,
		CONCAT( B.base_path, B.relative_path ) AS PictureUrl 
	FROM
		t_utility_course A
		LEFT JOIN t_utility_accessory B ON B.accessory_guid = A.logo_guid 
	) T 
WHERE
	1 = 1 {sqlWhere}
ORDER BY
	creation_date Desc
";
            request.Keyword = $"%{request.Keyword}%";
            return await MySqlHelper.QueryByPageAsync<SearchCourseRequestDto, SearchCourseResponseDto, SearchCourseItemDto>(sql, request);

        }

    }
}
