using Dapper;
using GD.DataAccess;
using GD.Models.Consumer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GD.Consumer
{
    /// <summary>
    /// 文章浏览量相关
    /// </summary>
    public class ArticleViewBiz
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ArticleViewModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, ArticleViewModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ArticleViewModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }

        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <param enable="enable"></param>
        /// <returns></returns>
        public async Task<ArticleViewModel> GetModelAsyncByID(string id, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<ArticleViewModel>("select * from t_consumer_article_view where view_guid=@id and `enable`=@enable", new { id, enable });
            }
        }

        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <param enable="enable"></param>
        /// <returns></returns>
        public async Task<ArticleViewModel> GetModelAsyncBySqlWhere(string id, string userID, bool isToday = true, bool enable = true)
        {
            var sqlWhere = " where target_guid=@id and `enable`=@enable  ";
            if (!string.IsNullOrWhiteSpace(userID))
            {
                sqlWhere += " and  created_by =@userID ";
            }
            if (isToday)
            {
                sqlWhere += $" and to_days(creation_date) = to_days(now())  ";
            }

            string sql = $@"select * from t_consumer_article_view  {sqlWhere} ";
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<ArticleViewModel>(sql, new { id, enable, userID });
            }
        }

        /// <summary>
        /// 根据id获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ArticleViewModel> GetModelAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<ArticleViewModel>(id);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.DeleteAsync<ArticleViewModel>(id);
                return result > 0;
            }
        }

        /// <summary>
        /// 根据id获取List
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<ArticleViewModel>> GetListModelAsyncByTargetID(string targetID)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<List<ArticleViewModel>>(targetID);
            }
        }



        /// <summary>
        /// 通过targetId获取浏览量
        /// </summary>
        /// <param name="targetID"></param>
        /// <returns></returns>
        public async Task<int> CountNumByTargetIDAsync(string targetID)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.RecordCountAsync<ArticleViewModel>("where target_guid=@targetID ", new { targetID });
            }
        }

        /// <summary>
        /// 通过targetId获取是否浏览
        /// </summary>
        /// <param name="targetID"></param>
        /// <returns></returns>
        public async Task<bool> IsExistThisTargetIDRecord(string targetID,string userID)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = " where target_guid=@targetID AND to_days( creation_date ) = to_days( now( ) ) and created_by=@userID ";
                return await conn.RecordCountAsync<ArticleViewModel>(sqlWhere, new { targetID, userID }) > 0;
            }
        }


    }
}
