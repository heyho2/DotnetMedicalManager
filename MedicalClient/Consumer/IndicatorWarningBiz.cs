using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Health.HealthManager;
using GD.Models.Consumer;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Consumer
{
    public class IndicatorWarningBiz : BaseBiz<IndicatorWarningModel>
    {
        /// <summary>
        /// 获取预警记录分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="healthManagerId"></param>
        /// <returns></returns>
        public async Task<GetIndicatorWarningPageListResponseDto> GetIndicatorWarningPageListAsync(GetIndicatorWarningPageListRequestDto requestDto, string healthManagerId)
        {
            var sqlWhere = string.Empty;
            if (requestDto.Status.HasValue)
            {
                sqlWhere = $"AND a.`status` = @Status ";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                sqlWhere = $"{sqlWhere} AND (a.phone like CONCAT('%',@Keyword,'%') or a.`name` like CONCAT('%',@Keyword,'%')) ";
            }

            var sql = $@"SELECT
							a.warning_guid,
							a.`name`,
							a.age,
							a.creation_date AS warning_date,
							a.phone,
                            CONCAT(c.base_path,c.relative_path) as portrait,
							a.description,
							a.`status` 
						FROM
							t_consumer_indicator_warning a
                            inner join t_utility_user b on a.consumer_guid=b.user_guid
							left join t_utility_accessory c on c.accessory_guid=b.portrait_guid
						WHERE
							a.health_manager_guid = '{healthManagerId}' 
							AND a.`enable` = 1 
							{sqlWhere}
						ORDER BY
							a.creation_date DESC";
            var result = await MySqlHelper.QueryByPageAsync<GetIndicatorWarningPageListRequestDto, GetIndicatorWarningPageListResponseDto, GetIndicatorWarningPageListItemDto>(sql, requestDto);
            return result;
        }
        /// <summary>
        /// 根据用户和指标获取数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="option"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<IndicatorWarningModel> GetModelAsyncByUserAndOption(string userId, string option, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<IndicatorWarningModel>("SELECT *  FROM t_consumer_indicator_warning where consumer_guid=@UserId and indicator_option_guid=@OptionId and status='pending' and `enable`=@enable", new { UserId = userId, OptionId = option, enable });
            }
        }
        /// <summary>
        /// 发预警
        /// </summary>
        /// <param name="indicatorWarningUpdateModel"></param>
        /// <param name="indicatorWarningModel"></param>
        /// <returns></returns>
        public async Task<bool> CreateUpdataeWarningAsync(IndicatorWarningModel indicatorWarningUpdateModel, IndicatorWarningModel indicatorWarningModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                //修改
                if (indicatorWarningUpdateModel != null)
                {
                    await conn.UpdateAsync(indicatorWarningUpdateModel);
                }
                indicatorWarningModel.Insert(conn);
                return true;
            });
        }


        /// <summary>
        /// 获取健康管理师待处理的预警记录数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> GetNumberByHealthManagerIdAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync<int>("select count(1) from t_consumer_indicator_warning where health_manager_guid=@id and `Status`='Pending' and `enable`=1", new { id });
                return result;
            }
        }


    }
}
