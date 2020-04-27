using Dapper;
using GD.DataAccess;
using GD.Dtos.Health;
using GD.Models.Consumer;
using GD.Models.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.Manager.Consumer
{
    /// <summary>
    /// 消费者表模型
    /// </summary>
    public class ConsumerBiz : BaseBiz<ConsumerModel>
    {
        public async Task<bool> CreateConsumerHealthInfo(UserModel user, ConsumerModel consumerModel,
            List<ConsumerHealthInfoModel> infosModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                var userResult = await conn.InsertAsync<string, UserModel>(user);

                if (string.IsNullOrEmpty(userResult))
                {
                    return false;
                }

                var consumerResult = await conn.InsertAsync<string, ConsumerModel>(consumerModel);

                if (string.IsNullOrEmpty(consumerResult))
                {
                    return false;
                }

                var infoResult = infosModel.InsertBatch(conn);
                if (infoResult <= 0)
                {
                    return false;
                }

                return true;
            });

        }

        public async Task<GetConsumerListResponseDto> GetConsumersPageList(GetConsumerListRequestDto requestDto)
        {
            var sqlWhere = string.Empty;

            if (!string.IsNullOrEmpty(requestDto.KeyWord))
            {
                sqlWhere = $"AND (u.user_name LIKE '%{requestDto.KeyWord}%' OR u.phone LIKE '%{requestDto.KeyWord}%')";
            }

            if (requestDto.RegistrationTime.HasValue && requestDto.EndTime.HasValue)
            {
                requestDto.EndTime = requestDto.EndTime.Value.AddDays(1);

                sqlWhere = $"{sqlWhere} and c.creation_date >= @RegistrationTime and c.creation_date < @EndTime";
            }

            var sql = $@"SELECT
                        u.user_guid as UserGuid,
	                    u.user_name as `Name`, 
                        u.gender as 'Gender',
                        u.phone AS Phone,
                        (
                            IF(birthday IS NULL, '-', TIMESTAMPDIFF(YEAR, birthday, CURDATE())) 
                        ) AS Age,
                        u.last_updated_date AS UpdatedDate,
                        m.user_name as ManagerName,
                        m.manager_guid as ManagerGuid,
                        m.phone as ManagerPhone,
                        c.creation_date as CreationDate
                    FROM t_utility_user as u
                        inner join t_consumer as c on u.user_guid = c.consumer_guid
                        left join t_health_manager as m on c.health_manager_guid = m.manager_guid
                    WHERE u.`enable` = 1 {sqlWhere}
                    ORDER BY u.last_updated_date DESC";

            return await MySqlHelper.QueryByPageAsync<GetConsumerListRequestDto,
                 GetConsumerListResponseDto,
                 GetConsumerItem>(sql, requestDto);
        }


        public async Task<GetConsumerListResponseDto> GetConsumers(GetConsumerListRequestDto requestDto)
        {
            var sqlWhere = string.Empty;

            if (!string.IsNullOrEmpty(requestDto.KeyWord))
            {
                sqlWhere = $"AND (u.user_name LIKE '%{requestDto.KeyWord}%' OR u.phone LIKE '%{requestDto.KeyWord}%')";
            }

            if (requestDto.RegistrationTime.HasValue)
            {
                requestDto.EndTime = requestDto.RegistrationTime.Value.AddDays(1);

                sqlWhere = $"{sqlWhere} and m.creation_date >= @RegistrationTime and m.creation_date < @EndTime";
            }

            var sql = $@"SELECT
                        u.user_guid as UserGuid,
	                    u.user_name as `Name`, 
                        u.gender as 'Gender',
                        u.phone AS Phone,
                        (
                            IF(birthday IS NULL, '-', TIMESTAMPDIFF(YEAR, birthday, CURDATE())) 
                        ) AS Age,
                        u.last_updated_date AS UpdatedDate,
                        m.user_name as ManagerName,
                        m.manager_guid as ManagerGuid,
                        m.phone as ManagerPhone,
                        c.creation_date as CreationDate
                    FROM t_utility_user as u
                        inner join t_consumer as c on u.user_guid = c.consumer_guid
                        left join t_health_manager as m on c.health_manager_guid = m.manager_guid
                    WHERE u.`enable` = 1 {sqlWhere}
                    ORDER BY u.last_updated_date DESC";

            return await MySqlHelper.QueryByPageAsync<GetConsumerListRequestDto,
                 GetConsumerListResponseDto,
                 GetConsumerItem>(sql, requestDto);
        }
    }
}
