using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Models.FAQs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.FAQs
{
    /// <summary>
    /// 
    /// </summary>
    public class DoctorEaringsDetailBiz : BaseBiz<DoctorEaringsDetailModel>
    {

        /// <summary>
        /// 分页获取model
        /// </summary>
        /// <param name="ids">ids</param>
        /// <returns></returns>
        public async Task<List<DoctorEaringsDetailModel>> GetPageModelListByIDAsync(string userID, int pageIndex, int pageSize)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = " where doctor_guid=@userID and enable=1 order by creation_date desc limit @pageIndex,@pageSize  ";
                return (await conn.GetListAsync<DoctorEaringsDetailModel>(sqlWhere, new { userID, pageIndex = (pageIndex - 1) * pageSize, pageSize })).ToList();
            }
        }


    }
}
