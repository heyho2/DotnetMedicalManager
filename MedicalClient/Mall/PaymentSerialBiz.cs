using GD.DataAccess;
using GD.Models.Mall;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GD.Mall
{
    public class PaymentSerialBiz
    {
        /// <summary>
        /// 提交支付流水记录和支付订单数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="paymentOrders"></param>
        /// <returns></returns>
        public async Task<bool> InsertPaymentSerialWithOrderInfo(PaymentSerialModel model, List<PaymentOrderModel> paymentOrders, List<OrderModel> orders)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                if (string.IsNullOrEmpty(await conn.InsertAsync<string, PaymentSerialModel>(model))) { return false; }

                foreach (var item in paymentOrders)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, PaymentOrderModel>(item))) { return false; }
                }

                foreach (var item in orders)
                {
                    if ((await conn.UpdateAsync(item)) != 1) { return false; }
                }

                return true;
            });
        }
    }
}
