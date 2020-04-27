using GD.DataAccess;
using GD.Models.Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD.Consumer
{
    /// <summary>
    /// 消费者地址业务类
    /// </summary>
    public class AddressBiz
    {
        #region 查询
        /// <summary>
        /// 通过消费者地址Guid获取唯一地址实例
        /// </summary>
        /// <param name="addressGuid">消费者地址Guid</param>
        /// <returns>唯一消费者地址实例</returns>
        public AddressModel GetAddress(string addressGuid, bool enable = true)
        {
            var sql = "select * from t_consumer_address where address_guid=@addressGuid and enable=@enable";
            var addressModel = MySqlHelper.SelectFirst<AddressModel>(sql, new { addressGuid, enable });

            return addressModel;
        }

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

        /// <summary>
        /// 通过用户Guid获取地址实例集合
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <returns>消费者地址实例集合</returns>
        public List<AddressModel> GetAddresses(string userGuid, bool enable = true)
        {
            var sql = "select * from t_consumer_address where user_guid=@userGuid and enable=@enable";
            var characterModels = MySqlHelper.Select<AddressModel>(sql, new { userGuid, enable });

            return characterModels?.ToList();
        }
        #endregion

        #region 修改
        /// <summary>
        /// 新增消费者地址记录
        /// </summary>
        /// <param name="addressModel">消费者地址实例</param>
        /// <returns>是否成功</returns>
        public bool InsertAddress(AddressModel addressModel)
        {
            return !string.IsNullOrEmpty(addressModel.Insert());
        }

        /// <summary>
        /// 修改消费者地址记录
        /// </summary>
        /// <param name="addressModel">消费者地址实例</param>
        /// <returns>是否成功</returns>
        public bool UpdateAddress(AddressModel addressModel)
        {
            addressModel.LastUpdatedDate = DateTime.Now;
            return addressModel.Update() == 1;
        }
        /// <summary>
        /// 批量更新地址
        /// </summary>
        /// <param name="addressModels"></param>
        /// <returns></returns>
        public bool UpdateAddress(List<AddressModel> addressModels)
        {
            return MySqlHelper.Transaction((con, tran) =>
             {
                 if (!addressModels.Any())
                 {
                     return false;
                 }
                 foreach (var item in addressModels)
                 {
                     item.LastUpdatedDate = DateTime.Now;
                     if (item.Update(con) != 1)
                     {
                         return false;
                     }
                 }
                 return true;
             });
        }

        /// <summary>
        /// 删除消费者地址记录
        /// </summary>
        /// <param name="addressGuid">消费者地址Guid</param>
        /// <returns>是否成功</returns>
        public bool DeleteAddress(string addressGuid, bool physical = false)
        {
            bool result = true;

            var addressModel = GetAddress(addressGuid);
            if (addressModel != null)
            {
                result = addressModel.Delete(MySqlHelper.GetConnection(), physical) == 1;
            }
            return result;
        }
        #endregion
    }
}
