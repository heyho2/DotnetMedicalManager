using System;
using System.Collections.Generic;
using Dapper;
using GD.DataAccess;
using GD.Models.Consumer;
using GD.Models.CrossTable;
using GD.Models.Manager;
using GD.Models.Utility;

namespace GD.Utility
{
    public class AccountBiz
    {
        /// <summary>
        /// 手机验证号KEY在Redis中的前缀
        /// </summary>
        private const string Prefix = "VerificationCode:";

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="consumerModel"></param>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        /// <remarks>
        /// 返回空，主键重复
        /// </remarks>
        public bool? Register(UserModel userModel, ConsumerModel consumerModel, RegisterModel registerModel)
        {
            bool? result = false; // 默认为失败

            MySqlHelper.Transaction((conn, tran) =>
            {
                try
                {
                    result = string.Equals(userModel.Insert(conn), userModel.UserGuid) && string.Equals(consumerModel.Insert(conn), consumerModel.ConsumerGuid);
                    return result.Value;
                }
                catch (MySql.Data.MySqlClient.MySqlException e) when (e.Message.Contains("Duplicate"))
                {
                    result = null; // 返回空，主键重复
                    return false;
                }
            });

            return result;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateUser(UserModel model)
        {
            model.LastUpdatedDate = DateTime.Now;
            return model.Update() == 1;
        }

        /// <summary>
        /// 按ID获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public UserModel GetUserById(string userId, bool enable = true)
        {
            var model = MySqlHelper.GetModelById<UserModel>(userId);
            return model?.Enable == enable ? model : null;
        }

        /// <summary>
        /// 按电话号码获取用户
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public IEnumerable<UserModel> GetUserByPhone(string phone, bool enable = true)
        {
            const string sql = "select * from t_utility_user where phone = @phone and enable = @enable";
            return MySqlHelper.Select<UserModel>(sql, new { phone, enable });
        }

        /// <summary>
        /// 生成6位手机验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="expiry">验证码有效期分钟数</param>
        /// <returns></returns>
        /// <remarks>
        /// 手机验证码一定为6位，从111111到999999，验证的时候会使用到
        /// </remarks>
        public int CreateVerificationCode(string phone, int expiry)
        {
            var key = Prefix + phone;
            var random = new Random(unchecked((int)DateTime.Now.Ticks));
            var code = random.Next(111111, 999999);

            RedisHelper.Database.StringSet(key, code, new TimeSpan(0, expiry, 3)); // 考虑到网络传输开销，有效期增加3秒钟

            return code;
        }

        /// <summary>
        /// 校验手机验证码有效性
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool VerifyCode(string phone, int code)
        {
            var key = Prefix + phone;

            if (!RedisHelper.Database.KeyExists(key))
            {
                return false;
            }

            var result = RedisHelper.Database.StringGet(key) == code;

            if (result)
            {
                RedisHelper.Database.KeyDelete(key);
            }

            return result;
        }

        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="account">管理员账号</param>
        /// <returns></returns>
        public IEnumerable<AccountModel> GetAdministrator(string account, bool enable = true)
        {
            const string sql = "select * from t_manager_account where account = @account and enable = @enable";
            return MySqlHelper.Select<AccountModel>(sql, new { account, enable });
        }
    }
}