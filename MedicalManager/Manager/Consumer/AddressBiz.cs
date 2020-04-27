using GD.DataAccess;
using GD.Models.Consumer;

namespace GD.Manager.Consumer
{
    /// <summary>
    /// 消费者地址业务类
    /// </summary>
    public class AddressBiz : BaseBiz<AddressModel>
    {
        /// <summary>
        /// 获取用户默认地址
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public AddressModel GetUserDefaultAddress(string userGuid, bool enable = true)
        {
            var sql = "select * from t_consumer_address where user_guid=@userGuid and enable=@enable order by is_default desc ";
            return MySqlHelper.SelectFirst<AddressModel>(sql, new { userGuid, enable });
        }

    }
}
