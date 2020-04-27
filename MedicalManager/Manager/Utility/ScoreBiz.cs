using Dapper;
using GD.Common.EnumDefine;
using GD.DataAccess;
using GD.Dtos.Score;
using GD.Dtos.User;
using GD.Models.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.Manager.Utility
{
    /// <summary>
    /// 积分模块业务类
    /// </summary>
    public class ScoreBiz : BaseBiz<ScoreModel>
    {
        /// <summary>
        /// 获取user积分
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UserPointsItemDto>> GetUserPointsAsync(string[] users)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"
SELECT
	user_guid,
	sum( variation ) as  variation
FROM
	t_utility_score 
WHERE
	user_guid IN @users AND `enable`=1 and user_type_guid='{UserType.Doctor.ToString()}'
GROUP BY
	user_guid";
                return await conn.QueryAsync<UserPointsItemDto>(sql, new { users });
            }
        }
        public async Task<IEnumerable<UserPointsItemDto>> GetUserEarnPointsAsync(string[] users)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"
SELECT
	user_guid,
	sum( variation ) as  variation
FROM
	t_utility_score 
WHERE
	user_guid IN @users AND `enable`=1 AND variation>0  and user_type_guid='{UserType.Doctor.ToString()}'
GROUP BY
	user_guid";
                return await conn.QueryAsync<UserPointsItemDto>(sql, new { users });
            }
        }
        public async Task<IEnumerable<UserPointsItemDto>> GetUserUsePointsAsync(string[] users)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"
SELECT
	user_guid,
	sum( variation ) as  variation
FROM
	t_utility_score 
WHERE
	user_guid IN @users AND `enable`=1 AND variation<0  and user_type_guid='{UserType.Doctor.ToString()}'
GROUP BY
	user_guid";
                return await conn.QueryAsync<UserPointsItemDto>(sql, new { users });
            }
        }

        public async Task<GetIntegralInfoPageResponseDto> GetIntegralInfoPageAsync(GetIntegralInfoPageRequestDto request)
        {
            var sqlWhere = $@"1 = 1 and enable=1 and user_guid=@UserGuid and user_type_guid='{UserType.Doctor.ToString()}'";
            var sqlOrderBy = "creation_date desc";
            var sql = $@"
SELECT * FROM(
    SELECT * FROM t_utility_score 
)___T
 WHERE
	{sqlWhere}
ORDER BY
	{sqlOrderBy}";
            return await MySqlHelper.QueryByPageAsync<GetIntegralInfoPageRequestDto, GetIntegralInfoPageResponseDto, GetIntegralInfoPageItemDto>(sql, request);
        }
        public async Task<IEnumerable<GetIntegralInfoPageItemDto>> ExportIntegralInfoAsync(ExportIntegralInfoRequestDto request)
        {
            var sqlWhere = $@"1 = 1 and enable=1 and user_guid=@UserGuid  and user_type_guid='{UserType.Doctor.ToString()}'";
            var sqlOrderBy = "creation_date desc";
            var sql = $@"
SELECT Score_Guid,User_Guid,Variation,Reason ,Creation_Date FROM t_utility_score
 WHERE
	{sqlWhere}
ORDER BY
	{sqlOrderBy}";
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryAsync<GetIntegralInfoPageItemDto>(sql, request);
            }
        }
    }
}
