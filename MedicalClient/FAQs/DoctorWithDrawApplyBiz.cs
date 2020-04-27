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
    public class DoctorWithDrawApplyBiz : BaseBiz<DoctorWithDrawApplyModel>
    {

        /// <summary>
        /// 分页获取model
        /// </summary>
        /// <param name="ids">ids</param>
        /// <returns></returns>
        public async Task<List<DoctorWithDrawApplyModel>> GetPageModelListByIDAsync(string userID, int pageIndex, int pageSize)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlWhere = " where doctor_guid=@userID and enable=1 order by creation_date desc limit @pageIndex,@pageSize  ";
                return (await conn.GetListAsync<DoctorWithDrawApplyModel>(sqlWhere, new { userID, pageIndex = (pageIndex - 1) * pageSize, pageSize })).ToList();
            }
        }

        /// <summary>
        /// 获取单个model
        /// </summary>
        /// <param name="questionId">问题Guid</param>
        /// <returns></returns>
        public async Task<int> GetModelDataInTimeAsync(string userID)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sqlStr = " select count(*) from t_doctor_earings_detail where TO_DAYS(creation_date) = TO_DAYS(NOW()) and doctor_guid=@userID and `enable`=1  ";
                return await conn.QueryFirstOrDefaultAsync<int>(sqlStr, new { userID });
            }
        }


        /// <summary>
        /// 异步批量插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> InsertApplyModelAndFlowingModelAsync(DoctorWithDrawApplyModel withdrawModel, TransferFlowingModel flowingModel, DoctorBalanceModel balanceModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, DoctorWithDrawApplyModel>(withdrawModel))) { return false; }
                if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, TransferFlowingModel>(flowingModel))) { return false; }
                await conn.UpdateAsync(balanceModel);
                return true;
            });
        }
    }
}
