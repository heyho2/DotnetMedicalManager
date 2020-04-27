using GD.DataAccess;
using GD.Dtos.Utility.Article;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Module
{
    /// <summary>
    /// 综合文章(普通文章+健康管理文章)Biz
    /// </summary>
    public class UnionArticleBiz
    {
        /// <summary>
        /// 获取客户端综合文章(普通文章+健康管理文章)分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        public async Task<GetClientArticlePageListResponseDto> GetClientArticlePageListAsync(GetClientArticlePageListRequestDto requestDto)
        {
            var whereSql = string.Empty;
            if (!string.IsNullOrWhiteSpace(requestDto.ArticleTypeDic))
            {
                whereSql = "and article.article_type_dic=@ArticleTypeDic";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.AuthorGuid))
            {
                whereSql = $"{whereSql} and article.author_guid=@AuthorGuid";
            }

            #region MyRegion
            //var sql = $@"SELECT
	           //             article.article_guid AS ArticleGuid,
	           //             article.title AS Title,
	           //             article.last_updated_date AS LastUpdatedDate,
	           //             article.author_guid AS AuthorGuid,
	           //             u.user_name AS AuthorName,
	           //             CONCAT( acce.base_path, acce.relative_path ) AS Picture,
	           //             article.article_type_dic AS ArticleTypeDic,
	           //             articleType.config_name AS ArticleType,
	           //             article.source_type AS ArticleSource,
	           //             count( DISTINCT l.like_guid ) AS LikeTotal,
	           //             count( DISTINCT av.view_guid ) AS PageView 
            //            FROM
	           //             t_utility_article AS article
	           //             LEFT JOIN t_utility_user u ON u.user_guid = article.author_guid
	           //             LEFT JOIN t_utility_accessory AS acce ON acce.accessory_guid = article.picture_guid 
	           //             AND acce.`enable` = 1
	           //             LEFT JOIN t_manager_dictionary AS articleType ON articleType.dic_guid = article.article_type_dic 
	           //             AND articleType.`enable` = 1
	           //             LEFT JOIN t_consumer_like l ON article.article_guid = l.target_guid 
	           //             AND l.`enable` = 1
	           //             LEFT JOIN t_consumer_article_view av ON article.article_guid = av.target_guid 
	           //             AND av.`enable` = 1 
            //            WHERE
	           //             article.`enable` = 1 
	           //             AND article.`actcle_release_status` = 'Release' 
	           //             AND article.visible = 1 {whereSql}
            //            GROUP BY
	           //             article.article_guid,
	           //             article.title,
	           //             article.last_updated_date,
	           //             u.user_name,
	           //             Picture,
	           //             article.article_type_dic,
	           //             articleType.config_name ,
            //                article.source_type
            //            ORDER BY
	           //             LastUpdatedDate DESC";
            #endregion
            var sql = $@"SELECT
	                        article.article_guid AS ArticleGuid,
	                        article.title AS Title,
                            article.external_link as ExternalLink,
	                        article.last_updated_date AS LastUpdatedDate,
	                        article.author_guid AS AuthorGuid,
	                        u.user_name AS AuthorName,
	                        CONCAT( acce.base_path, acce.relative_path ) AS Picture,
	                        article.article_type_dic AS ArticleTypeDic,
	                        articleType.config_name AS ArticleType,
	                        article.source_type AS ArticleSource,
	                        IFNULL(hot.like_count, 0) AS LikeTotal,
                            IFNULL(hot.visit_count, 0) AS PageView
                        FROM
	                        t_utility_article AS article
	                        LEFT JOIN t_utility_user u ON u.user_guid = article.author_guid
	                        LEFT JOIN t_utility_accessory AS acce ON acce.accessory_guid = article.picture_guid 
	                        AND acce.`enable` = 1
	                        LEFT JOIN t_manager_dictionary AS articleType ON articleType.dic_guid = article.article_type_dic 
	                        AND articleType.`enable` = 1
	                        LEFT JOIN t_utility_hot hot ON hot.owner_guid = article.article_guid
                                AND hot.`enable` = 1
                        WHERE
	                        article.`enable` = 1 
	                        AND article.`actcle_release_status` = 'Release' 
	                        AND article.visible = 1 {whereSql}
                        ORDER BY
	                        LastUpdatedDate DESC";
            return await MySqlHelper.QueryByPageAsync<GetClientArticlePageListRequestDto, GetClientArticlePageListResponseDto, GetClientArticlePageListItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 获取首页推荐综合文章分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetClientRecommandArticleListResponseDto> GetClientRecommandArticleListAsync(GetClientRecommandArticleListRequestDto requestDto)
        {
            var sql = $@"SELECT
	                        article.article_guid AS ArticleGuid,
	                        article.title AS Title,
                            article.external_link as ExternalLink,
	                        article.last_updated_date AS LastUpdatedDate,
	                        article.author_guid AS AuthorGuid,
	                        u.user_name AS AuthorName,
	                        CONCAT( acce.base_path, acce.relative_path ) AS Picture,
	                        article.article_type_dic AS ArticleTypeDic,
	                        articleType.config_name AS ArticleType,
	                        article.source_type AS ArticleSource,
	                        IFNULL(hot.like_count, 0) AS LikeTotal,
                            IFNULL(hot.visit_count, 0) AS PageView
                        FROM
	                        t_utility_article AS article
	                        LEFT JOIN t_utility_user u ON u.user_guid = article.author_guid
	                        LEFT JOIN t_utility_accessory AS acce ON acce.accessory_guid = article.picture_guid 
	                        AND acce.`enable` = 1
	                        LEFT JOIN t_manager_dictionary AS articleType ON articleType.dic_guid = article.article_type_dic 
	                        AND articleType.`enable` = 1
	                        LEFT JOIN t_utility_hot hot ON hot.owner_guid = article.article_guid
                                AND hot.`enable` = 1
                        WHERE
	                        article.`enable` = 1 
	                        AND article.`actcle_release_status` = 'Release' 
	                        AND article.visible = 1 
                        ORDER BY
	                        PageView DESC ";

            return await MySqlHelper.QueryByPageAsync<GetClientRecommandArticleListRequestDto, GetClientRecommandArticleListResponseDto, GetClientRecommandArticleListItemDto>(sql, requestDto);
        }

        
    }
}
