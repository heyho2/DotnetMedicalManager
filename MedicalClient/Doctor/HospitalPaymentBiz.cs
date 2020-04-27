using GD.BizBase;
using GD.DataAccess;
using GD.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Doctor
{
    /// <summary>
    /// 医院扫码微信公众号支付实体业务类
    /// </summary>
    public class HospitalPaymentBiz : BaseBiz<HospitalPaymentModel>
    {
        /// <summary>
        /// 根据支付单号获取model
        /// </summary>
        /// <param name="outTradeNo"></param>
        /// <returns></returns>
        public async Task<HospitalPaymentModel> GetModelByOutTradeNoAsync(string outTradeNo)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = (await conn.GetListAsync<HospitalPaymentModel>("where out_trade_no=@outTradeNo and `enable`=1",new { outTradeNo})).FirstOrDefault();
                return result;
            }
        }
    }
}
