using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Doctor
{
    public class HospitalPaymentConfigBiz : BaseBiz<HospitalPaymentConfigModel>
    {
        /// <summary>
        /// 根据商户id获取model
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public async Task<List<HospitalPaymentConfigModel>> GetModelsByMerchantIdAsync(string merchantId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<HospitalPaymentConfigModel>("where merchant_id=@merchantId and `enable`=1", new { merchantId });
                return result.ToList();
            }
        }
    }
}
