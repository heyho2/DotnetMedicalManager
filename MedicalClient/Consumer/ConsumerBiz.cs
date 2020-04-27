using Dapper;
using GD.DataAccess;
using GD.Dtos.Admin.Article;
using GD.Dtos.Doctor.Doctor;
using GD.Dtos.Utility.Article;
using GD.Dtos.Utility.Course;
using GD.Dtos.Utility.Message;
using GD.Models.Consumer;
using GD.Models.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Consumer
{
    /// <summary>
    /// 消费者表模型
    /// </summary>
    public class ConsumerBiz
    {
        /// <summary>
        /// 异步获取唯一实例
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<ConsumerModel> GetModelAsync(string userId, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"select * from t_consumer where consumer_guid=@userId";
                var sumScore = await conn.QueryFirstOrDefaultAsync<ConsumerModel>(sql, new { userId, enable });
                return sumScore;
            }

        }

        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ConsumerModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, ConsumerModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }
        /// <summary>
        /// 异步修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ConsumerModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var count = await conn.UpdateAsync(model);
                return count == 1;
            }
        }

        /// <summary>
        /// 搜索文章
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SearchArticleResponseDto> SearchArticleAsync(SearchArticleRequestDto request)
        {
            var sqlWhere = $@"AND ENABLE = 1 AND visible = 1";

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
        public async Task<GetArticleListResponseDto> GetArticleListAsync(string userId, GetArticleListRequestDto request)
        {
            var orderbySql = "creation_date desc";
            if (!string.IsNullOrWhiteSpace(request.SortField))
            {
                orderbySql = $"{request.SortField} {(request.IsAscending ? "asc" : "desc")}";
            }
            var sqlWhere = $@"AND ENABLE = 1 AND  created_by='{userId}'";

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                sqlWhere = $"{sqlWhere} AND (title like @Keyword  OR abstract like @Keyword)";
            }
            if (string.IsNullOrWhiteSpace(request.ActcleReleaseStatus))
            {
                request.ActcleReleaseStatus = ReleaseStatus.Release.ToString();
            }
            if (request.ActcleReleaseStatus != "All")
            {
                sqlWhere = $"{sqlWhere} AND (actcle_release_status=@ActcleReleaseStatus)";
            }
            var sql = $@"
SELECT * FROM(
	SELECT
	a.*,
	b.nick_name,
	c.like_count,
	c.visit_count,
	( SELECT count( 1 ) FROM t_consumer_collection __s where a.article_guid = __s.target_guid  GROUP BY target_guid ) AS collection 
FROM
	t_utility_article a
	LEFT JOIN t_utility_user b ON a.author_guid = b.user_guid
	LEFT JOIN t_utility_hot c ON a.article_guid = c.owner_guid
) T 
WHERE
	1 = 1 {sqlWhere}
ORDER BY
	{orderbySql}
";
            request.Keyword = $"%{request.Keyword}%";
            return await MySqlHelper.QueryByPageAsync<GetArticleListRequestDto, GetArticleListResponseDto, GetArticleListItemDto>(sql, request);
        }

        /// <summary>
        /// 获取首页头条
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<GetHomeHeadlineItemDto>> GetHomeHeadlineAsync(GetHomeHeadlineRequestDto request)
        {
            var sql = $@"
SELECT
	A.* 
FROM
	t_manager_headline A
WHERE 1=1 AND A.enable=@enable
LIMIT {request.Take}";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetHomeHeadlineItemDto>(sql, new { enable = true });
                return result;
            }
        }

        /// <summary>
        /// 获取消息记录列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">单页条数</param>
        /// <param name="userOneId">用户一guid</param>
        /// <param name="userTwoId">用户二guid</param>
        /// <param name="topicAboutType">主题相关类型</param>
        public async Task<GetMessageListByFromAndToResponseDto> GetMessageListByFromAndToAsync(GetMessageListByFromAndToRequestDto requestDto)
        {
            var whereSql = "";
            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                requestDto.Keyword = $"%{requestDto.Keyword}%";
                whereSql = "and a.context like @Keyword";
            }
            var sql = $@"SELECT
	                    a.from_guid,
	                    a.to_guid,
	                    a.context,
	                    a.creation_date 
                    FROM
	                    t_utility_message a
	                    INNER JOIN t_utility_topic topic ON a.topic_guid = topic.topic_guid 
                    WHERE
	                    topic.about_type = @TopicAbountType 
                        AND a.`enable`=1 and topic.`enable`=1
	                    AND (( from_guid = @UserOneId AND to_guid = @UserTwoId ) 
	                    OR ( from_guid = @UserTwoId AND to_guid = @UserOneId ) )
                        {whereSql}
                    ORDER BY
	                    a.creation_date DESC ";
            return await MySqlHelper.QueryByPageAsync<GetMessageListByFromAndToRequestDto, GetMessageListByFromAndToResponseDto, GetMessageListByFromAndToItemDto>(sql, requestDto);

        }

        /// <summary>
        /// 通过接收者用户guid获取发送者用户列表
        /// </summary>
        /// <param name="toUserGuid">接收者用户guid</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">单页条数</param>
        /// <param name="topicAboutType">主题相关类型</param>
        /// <returns></returns>
        public async Task<List<MessageUserDto>> GetFromUserListByToUserGuidAsync(string toUserGuid, int pageIndex, int pageSize, string topicAboutType)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            from_guid AS UserGuid,
	                            b.user_name AS UserName,
	                            b.nick_name AS NickName,
	                            CONCAT( c.base_path, c.relative_path ) AS PortraitUrl,
	                            max( a.creation_date ) AS CreationDate 
                            FROM
	                            t_utility_message a
                                INNER JOIN t_utility_topic topic ON a.topic_guid = topic.topic_guid 
	                            INNER JOIN t_utility_user b ON a.from_guid = b.user_guid
	                            LEFT JOIN t_utility_accessory c ON b.portrait_guid = c.accessory_guid 
	                            AND c.`enable` = 1 
                            WHERE
	                            a.to_guid = @toUserGuid 
                                and topic.about_type=@topicAboutType
	                            AND a.`enable` = 1 
	                            AND b.`enable` = 1 
                            GROUP BY
	                            from_guid,
	                            b.user_name,
	                            b.nick_name,
	                            c.base_path,
	                            c.relative_path 
                            ORDER BY
	                            CreationDate DESC 
	                            LIMIT @pageIndex,
	                            @pageSize;";
                var result = await conn.QueryAsync<MessageUserDto>(sql, new { toUserGuid, pageIndex = (pageIndex - 1) * pageSize, pageSize, topicAboutType });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 获取首页热门课程
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<GetHotCourseItemDto>> GetHotCourseAsync(GetHotCourseRequestDto request)
        {
            var sqlWhere = string.Empty;
            if (request.BeginDate != null)
            {
                request.BeginDate = request.BeginDate.Value.Date;
                sqlWhere = $" AND a.creation_date>=@BeginDate";
            }
            #region MyRegion
            /*
            var sql = $@"SELECT
	                        a.article_guid AS course_guid,
	                        a.title,
	                        a.abstract AS summary,
	                        CONCAT( b.base_path, b.relative_path ) AS LogoUrl,
	                        a.creation_date,
	                        count( DISTINCT c.like_guid ) AS LikeCount,
	                        count( DISTINCT d.view_guid ) AS VisitCount 
                        FROM
	                        t_utility_article a
	                        LEFT JOIN t_utility_accessory b ON a.picture_guid = b.accessory_guid
	                        LEFT JOIN t_consumer_like c ON a.article_guid = c.target_guid 
	                        AND c.`enable` = 1
	                        LEFT JOIN t_consumer_article_view d ON a.article_guid = d.target_guid 
	                        AND d.`enable` = 1 
                        WHERE
	                        a.`enable` = 1 
	                        AND a.`actcle_release_status` = 'Release' 
	                        AND a.visible = 1 
	                        AND a.source_type = 'Manager' {sqlWhere}
                        GROUP BY
	                        a.article_guid,
	                        a.title,
	                        a.abstract,
	                        LogoUrl,
	                        a.creation_date,
	                        a.last_updated_date 
                        ORDER BY
	                        LikeCount DESC,
	                        VisitCount DESC,
	                        creation_date DESC 
	                        limit @pageIndex,@pageSize;";
            */
            #endregion

            var sql = $@"SELECT
	                        a.article_guid AS course_guid,
	                        a.title,
	                        a.abstract AS summary,
	                        CONCAT( b.base_path, b.relative_path ) AS LogoUrl,
	                        a.creation_date,
	                        ifnull(hot.like_count,0) as LikeCount,
                            ifnull(hot.visit_count,0) as VisitCount
                        FROM
	                        t_utility_article a
	                        LEFT JOIN t_utility_accessory b ON a.picture_guid = b.accessory_guid
	                        LEFT JOIN t_utility_hot hot ON hot.owner_guid = a.article_guid
                                AND hot.`enable` = 1
                        WHERE
	                        a.`enable` = 1 
	                        AND a.`actcle_release_status` = 'Release' 
	                        AND a.visible = 1 
	                        AND a.source_type = 'Manager' {sqlWhere}
                        ORDER BY
	                        LikeCount DESC,
	                        VisitCount DESC,
	                        creation_date DESC 
	                        limit @pageIndex,@pageSize;";

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetHotCourseItemDto>(sql, new { pageIndex = (request.PageIndex - 1) * request.Take, pageSize = request.Take, request.BeginDate, enable = true, Visible = true });
                return result;
            }
        }

        /// <summary>
        /// 获取问医热点文章列表
        /// </summary>
        /// <param name="daysWeight">时间权重</param>
        /// <param name="pageViewWeight">浏览量权重</param>
        /// <returns></returns>
        public async Task<List<GetAskedDoctorHotArticleResponseDto>> GetAskedDoctorHotArticlesAsync(decimal daysWeight, decimal pageViewWeight)
        {
            var sql = $@"SELECT
	                                article.article_guid AS ArticleGuid,
	                                article.title AS Title,
	                                CONCAT( acc.base_path, acc.relative_path ) AS Picture,
	                                dic.config_name AS ArticleType,
	                                article.last_updated_date AS LastUpdatedDate,
	                                ifnull(
	                                ( datediff( article.last_updated_date, now( ) ) * @daysWeight + hot.visit_count * @pageViewWeight ) / ( @daysWeight + @pageViewWeight ),
	                                0 
	                                ) AS OrderSeed,
	                                IFNULL(hot.like_count, 0) AS LikeTotal,
                                    IFNULL(hot.visit_count, 0) AS PageView
                                FROM
	                                t_utility_article article
	                                LEFT JOIN t_utility_accessory acc ON article.picture_guid = acc.accessory_guid 
	                                AND acc.`enable` = 1
	                                LEFT JOIN t_utility_hot hot ON article.article_guid = hot.owner_guid 
	                                AND hot.`enable` = 1
	                                LEFT JOIN t_manager_dictionary dic ON article.article_type_dic = dic.dic_guid
                                WHERE
	                                article.`enable` = 1 and
                                    article.`actcle_release_status` = 'Release'
                                ORDER BY
	                                OrderSeed DESC 
	                                LIMIT 0,
	                                10 ; ";

            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetAskedDoctorHotArticleResponseDto>(sql, new { daysWeight, pageViewWeight });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 获取问医讲堂文章
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public GetAskedDoctorLectureHallArticleResponseDto GetAskedDoctorLectureHallArticle(GetAskedDoctorLectureHallArticleRequestDto requestDto)
        {
            var sql = @"SELECT
	                        article.article_guid AS ArticleGuid,
	                        article.title AS Title,
	                        article.last_updated_date AS LastUpdatedDate,
	                        CONCAT( acce.base_path, acce.relative_path ) AS Picture,
	                        articleType.config_name AS ArticleType,
	                        IFNULL(hot.like_count, 0) AS LikeTotal,
                            IFNULL(hot.visit_count, 0) AS PageView
                        FROM
	                        t_utility_article AS article
	                        LEFT JOIN t_utility_accessory AS acce ON acce.accessory_guid = article.picture_guid 
	                        AND acce.`enable` = 1
	                        LEFT JOIN t_manager_dictionary AS articleType ON articleType.dic_guid = article.article_type_dic 
	                        AND articleType.`enable` = 1
	                        LEFT JOIN t_utility_hot hot ON hot.owner_guid = article.article_guid
                                AND hot.`enable` = 1
                        WHERE
	                        article.`enable` = TRUE and 
                            article.`actcle_release_status` = 'Release'
                        ORDER BY
	                        article.last_updated_date DESC ";
            var response = MySqlHelper.QueryByPage<GetAskedDoctorLectureHallArticleRequestDto, GetAskedDoctorLectureHallArticleResponseDto, GetAskedDoctorLectureHallArticleItemDto>(sql, requestDto);
            return response;

        }

        /// <summary>
        /// 获取用户模型实例
        /// </summary>
        /// <param name="guid">主键guid</param>
        /// <returns></returns>
        public async Task<UserModel> GetModelByPhone(string phone, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select * from t_utility_user  where phone=@phone and enable=@enable  ";
                return await conn.QueryFirstAsync<UserModel>(sql, new { phone, enable });
            }
        }
        public async Task<bool> CreateConsumerHealthInfo(UserModel user, ConsumerModel consumerModel,
           List<ConsumerHealthInfoModel> infosModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                var userResult = await conn.InsertAsync<string, UserModel>(user);

                if (string.IsNullOrEmpty(userResult))
                {
                    return false;
                }

                var consumerResult = await conn.InsertAsync<string, ConsumerModel>(consumerModel);

                if (string.IsNullOrEmpty(consumerResult))
                {
                    return false;
                }

                var infoResult = infosModel.InsertBatch(conn);
                if (infoResult <= 0)
                {
                    return false;
                }

                return true;
            });

        }
    }
}
