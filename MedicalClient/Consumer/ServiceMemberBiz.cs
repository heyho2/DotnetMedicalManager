using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Consumer.Consumer;
using GD.Models.Consumer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Consumer
{
    public class ServiceMemberBiz : BaseBiz<ServiceMemberModel>
    {
        /// <summary>
        /// 获取用户的服务对象成员列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ServiceMemberModel>> GetServiceMemberListAsync(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<ServiceMemberModel>("where user_guid=@userId and `enable`=1", new { userId });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 创建/修改model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isCreate"></param>
        /// <returns></returns>
        public async Task<bool> CreateEidtServiceMemberAsync(ServiceMemberModel model, bool isCreate)
        {
            if (isCreate)
            {
                return await InsertAsync(model);
            }
            return await UpdateAsync(model);
        }

        /// <summary>
        /// 获取用户账号下服务对象列表
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task<List<GetUserServiceMembersResponseDto>> GetUserServiceMembersAsync(string phone)
        {
            var sql = @"
             SELECT 
                c.service_member_guid as id, 
                c.`name` 
             FROM t_utility_user as u
	            INNER JOIN t_consumer_service_member as c ON u.user_guid = c.user_guid
             WHERE u.phone = @phone AND u.`enable` = 1 AND c.`enable` = 1";

            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<GetUserServiceMembersResponseDto>(sql, new { phone })).ToList();
            }
        }
    }
}
