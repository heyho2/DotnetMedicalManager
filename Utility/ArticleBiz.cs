using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GD.DataAccess;
using GD.Models.Utility;

namespace GD.Utility
{
    /// <summary>
    /// 文章业务类
    /// </summary>
    public class ArticleBiz
    {
        /// <summary>
        /// 获取唯一文章model
        /// </summary>
        /// <param name="guid">主键guid</param>
        /// <returns></returns>
        public ArticleModel GetModel(string guid)
        {
            return MySqlHelper.GetModelById<ArticleModel>(guid);
        }
        /// <summary>
        ///  获取唯一文章model
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<ArticleModel> GetAsync(string guid, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<ArticleModel>("select * from t_utility_article where article_guid=@guid and `enable`=@enable", new { guid, enable });
            }
        }
        /// <summary>
        /// 通过条件查询文章(分页)
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="pageSize">单页条数</param>
        /// <param name="condition">条件,例如 where condition=@condition</param>
        /// <param name="orderby">排序,例如 col desc</param>
        /// <param name="parameters">匿名类参数</param>
        /// <returns></returns>
        public List<ArticleModel> GetArticles(int pageNumber, int pageSize, string condition, string orderby, object parameters = null)
        {
            var models = MySqlHelper.Select<ArticleModel>(pageNumber, pageSize, condition, orderby, parameters);
            return models?.ToList();
        }

        /// <summary>
        /// 按条件查询文章
        /// </summary>
        /// <param name="condition">条件 where condition=@condition</param>
        /// <param name="orderby">col desc</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<ArticleModel> GetArticles(string condition = null, string orderby = null, object parameters = null)
        {
            if (!string.IsNullOrEmpty(orderby))
            {
                orderby = $"order by {orderby}";
            }
            var sql = $"select * from t_utility_article {condition} {orderby} ";
            return MySqlHelper.Select<ArticleModel>(sql, parameters)?.ToList();
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
        public async Task<int> UpdateArticleAsync(ArticleModel articleModel)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.UpdateAsync(articleModel);

            }
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="articleModel"></param>
        /// <param name="physicalDelete">是否物理删除</param>
        /// <returns></returns>
        public async Task<bool> DeleteArticleAsync(ArticleModel articleModel, bool physicalDelete = false)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var affect = 0;
                if (physicalDelete)
                {
                    affect = await conn.DeleteAsync(articleModel);
                }
                else
                {
                    articleModel.Enable = false;
                    affect = await conn.UpdateAsync(articleModel);
                }
                return affect == 1;
            }
        }
    }
}
