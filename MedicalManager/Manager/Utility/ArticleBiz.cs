using Dapper;
using GD.DataAccess;
using GD.Dtos.Article;
using GD.Models.Utility;
using System.Threading.Tasks;

namespace GD.Manager.Utility
{
    /// <summary>
    /// 文章业务类
    /// </summary>
    public class ArticleBiz : BaseBiz<ArticleModel>
    {

        public override async Task<ArticleModel> GetAsync(object id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<ArticleModel>(@"
SELECT
    article_guid,
    author_guid,
    source_type,
    json_extract( keyword, '$' ) AS keyword,
    content_guid,
    title,
    abstract,
	picture_guid,
	article_type_dic,
	visible,
	sort,
	actcle_release_status,
	created_by,
	creation_date,
	last_updated_by,
	last_updated_date,
	platform_type,
	org_guid,
    external_link,
	`ENABLE`
FROM
    t_utility_article WHERE article_guid=@id", new { id });
            }
        }

        /// <summary>
        /// 搜索文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SearchArticleResponseDto> SearchArticleAsync(SearchArticleRequestDto request)
        {
            var sqlWhere = $@"AND ENABLE = 1 AND visible = 1 AND actcle_release_status='{Models.Utility.ReleaseStatus.Release.ToString()}'";

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                sqlWhere = $"{sqlWhere} AND (title like @Keyword  OR abstract like @Keyword)";
            }
            var sql = $@"
SELECT * FROM(
	SELECT
		A.*,
		CONCAT( B.base_path, B.relative_path ) AS PictureUrl 
	FROM
		t_utility_article A
		LEFT JOIN t_utility_accessory B ON B.accessory_guid = A.picture_guid 
	) T 
WHERE
	1 = 1 {sqlWhere}
ORDER BY
	creation_date Desc
";
            request.Keyword = $"%{request.Keyword}%";
            return await MySqlHelper.QueryByPageAsync<SearchArticleRequestDto, SearchArticleResponseDto, SearchArticleItemDto>(sql, request);

        }
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetArticleListResponseDto> GetArticleListAsync(GetArticleListRequestDto request)
        {
            var validStatus = new Models.Utility.ReleaseStatus[] {
                Models.Utility.ReleaseStatus.Release,
                Models.Utility.ReleaseStatus.ReviewPass,
                Models.Utility.ReleaseStatus.Reject,
            };
            var orderbySql = "creation_date desc ";
            if (!string.IsNullOrWhiteSpace(request.SortField))
            {
                orderbySql = $"{request.SortField} {(request.IsAscending ? "asc" : "desc")}";
            }
            var sqlWhere = "1 = 1 ";
            //var sqlWhere = $@"AND actcle_release_status IN ({string.Join(",", validStatus.Select(a => $"'{a.ToString()}'"))})";
            sqlWhere = $"{sqlWhere} AND source_type = @SourceType";

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                request.Keyword = $"{request.Keyword}%";
                sqlWhere = $"{sqlWhere} AND (title like @Keyword  OR abstract like @Keyword)";
            }
            if (!string.IsNullOrWhiteSpace(request.ArticleType))
            {
                sqlWhere = $"{sqlWhere} AND article_type_dic = @ArticleType";
            }
            if (request.ReleaseStatus != null)
            {
                sqlWhere = $"{sqlWhere} AND actcle_release_status = @ReleaseStatus";
            }
            var sql = $@"
SELECT * FROM(
	SELECT
	    a.*,
	    b.nick_name AS AuthorName,-- 	c.like_count,
	    ( SELECT count( 1 ) FROM t_consumer_article_view __s WHERE a.article_guid = __s.target_guid GROUP BY target_guid ) AS visit_count,
	    ( SELECT count( 1 ) FROM t_consumer_collection __s WHERE a.article_guid = __s.target_guid GROUP BY target_guid ) AS collection 
    FROM
	    t_utility_article a
	    LEFT JOIN t_utility_user b ON a.author_guid = b.user_guid
) T 
WHERE
	{sqlWhere}
ORDER BY
	{orderbySql}
";
            return await MySqlHelper.QueryByPageAsync<GetArticleListRequestDto, GetArticleListResponseDto, GetArticleListItemDto>(sql, request);

        }
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="richtextModel"></param>
        /// <param name="articleModel"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(RichtextModel richtextModel, ArticleModel articleModel)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.InsertAsync<string, ArticleModel>(articleModel);
                await conn.InsertAsync<string, RichtextModel>(richtextModel);
                return true;
            });
            return result;
        }
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="richtextModel"></param>
        /// <param name="articleModel"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(RichtextModel richtextModel, ArticleModel articleModel)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.UpdateAsync(richtextModel);
                await conn.UpdateAsync(articleModel);
                return true;
            });
            return result;
        }
        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<bool> DeleteAsync(string id)
        {
            var entity = await base.GetAsync(id);
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.DeleteAsync<ArticleModel>(id);
                await conn.DeleteAsync<RichtextModel>(entity.ContentGuid);
                return true;
            });
            return result;
        }
    }
}
