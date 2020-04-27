using System.Threading.Tasks;
using Dapper;
using GD.DataAccess;
using GD.Models.Utility;

namespace GD.Utility
{
    public class RichtextBiz
    {
        /// <summary>
        /// 获取富文本唯一Model
        /// </summary>
        /// <param name="guid">主键Guid</param>
        /// <returns></returns>
        public RichtextModel GetModel(string guid)
        {
            return MySqlHelper.GetModelById<RichtextModel>(guid);
        }

        /// <summary>
        /// 按ID查询
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<RichtextModel> GetAsync(string guid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<RichtextModel>(guid);
            }
        }

        /// <summary>
        /// 修改富文本model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(RichtextModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result == 1;
            }
        }
    }
}
