using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GD.DataAccess;
using GD.Dtos.Utility.User;
using GD.Models.Utility;

namespace GD.Utility
{
    /// <summary>
    /// 用户模块业务类
    /// </summary>
    public class UserBiz
    {
        #region 查询
        /// <summary>
        /// 通过用户Guid获取唯一用户模型对象实例
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <returns>返回唯一用户模型对象实例</returns>
        public UserModel GetUser(string userGuid, bool enable = true)
        {
            var sql = "select * from t_utility_user where user_guid=@userGuid  and enable=@enable ";
            var userModel = MySqlHelper.SelectFirst<UserModel>(sql, new { userGuid, enable });

            return userModel;
        }

        public UserModel GetModelByPhoneAsync(string phone)
        {
            var sql = "select * from t_utility_user where phone = @phone and enable =1 ";
            return MySqlHelper.SelectFirst<UserModel>(sql, new { phone });
        }

        public async Task<UserModel> GetModelAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<UserModel>(id);
            }
        }
        public async Task<UserModel> GetModelAsync(string userId, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select * from t_utility_user where user_guid=@userId  and enable=@enable ";
                var sumScore = await conn.QueryFirstOrDefaultAsync<UserModel>(sql, new { userId, enable });
                return sumScore;
            }

        }

        /// <summary>
        /// 通过用户手机号获取用户唯一实例
        /// </summary>
        /// <param name="phone">用户手机号</param>
        /// <returns>唯一用户实例</returns>
        public UserModel GetUserByPhone(string phone, bool enable = true)
        {
            var sql = "select * from t_utility_user where phone=@phone  and enable=@enable ";
            var userModel = MySqlHelper.SelectFirst<UserModel>(sql, new { phone, enable });
            return userModel;
        }

        public async Task<dynamic> GetConsumerAsync(string userGuid)
        {
            var sql = $@"
SELECT
	user_guid,
	count( 1 ) AS OrderQty,
	SUM( IFNULL( paid_amount, 0 ) ) AS OrderTotalAmount,
	AVG( paid_amount ) AS OrderAverage,
	MAX( creation_date ) AS LastBuyDate 
FROM
	t_mall_order 
WHERE
	order_status IN ( 'Shipped', 'Received', 'Completed' ) and user_Guid=@userGuid
GROUP BY
	user_guid 
";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync(sql, new { userGuid });
                return result;
            }
        }

        #endregion

        #region 修改

        public bool InsertUser(UserModel userModel)
        {
            return string.IsNullOrEmpty(userModel.Insert());
        }

        /// <summary>
        /// 更新单个用户数据记录
        /// </summary>
        /// <param name="userModel">用户模型对象实例</param>
        /// <returns>返回bool值，表示是否更新成功</returns>
        public bool UpdateUser(UserModel userModel)
        {
            bool result = true;

            if (userModel == null)
            {
                return false;
            }
            userModel.LastUpdatedDate = DateTime.Now;
            result = userModel.Update() == 1;
            return result;
        }

        /// <summary>
        /// 批量更新用户记录（事物执行）
        /// </summary>
        /// <param name="userModels">用户实例List集合</param>
        /// <returns>返回是否执行成功</returns>
        public bool UpdateUser(List<UserModel> userModels)
        {
            if (!userModels.Any())
            {
                return false;
            }

            return MySqlHelper.Transaction((conn, tran) =>
            {
                foreach (var userModel in userModels)
                {
                    userModel.LastUpdatedDate = DateTime.Now;
                    var res = userModel.Update(conn);
                    if (res != 1)//有记录未执行成功则回滚退出
                    {
                        return false;
                    }
                }

                return true;
            });
        }

        /// <summary>
        /// 修改用户 异步
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(UserModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var count = await conn.UpdateAsync(model);
                return count == 1;
            }
        }

        public async Task<int> RecordCountAsync()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.RecordCountAsync<UserModel>("where enable=1");
            }
        }

        #endregion

        /// <summary>
        /// 通过用户id集合获取用户信息列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<GetUsersInfoResponseDto>> GetUsersInfoAsync(GetUsersInfoRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.user_guid,
	                            a.user_name,
	                            a.nick_name,
	                            a.gender,
	                            a.birthday,
	                            a.phone,
	                            CONCAT( b.base_path, b.relative_path ) AS portrait 
                            FROM
	                            t_utility_user a
	                            LEFT JOIN t_utility_accessory b ON a.portrait_guid = b.accessory_guid 
                            WHERE
	                            a.user_guid IN @ids";
                var result = await conn.QueryAsync<GetUsersInfoResponseDto>(sql, new { ids = requestDto.UserIds });
                return result?.ToList();
            }
        }
    }
}
