using System.Threading.Tasks;
using Dapper;
using GD.DataAccess;
using GD.Models.Utility;

namespace GD.Utility
{
    /// <summary>
    /// 课程 业务类
    /// </summary>
    public class CourseBiz
    {
        /// <summary>
        /// 按ID查询
        /// </summary>
        /// <param name="dicGuid"></param>
        /// <returns></returns>
        public async Task<CourseModel> GetAsync(string guid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<CourseModel>(guid);
            }
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
        public async Task<bool> UpdateAsync(CourseModel model)
        {
            return await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.UpdateAsync(model);
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
    }
}
