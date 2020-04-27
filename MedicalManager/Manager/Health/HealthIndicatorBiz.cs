using Dapper;
using GD.DataAccess;
using GD.Dtos.Health;
using GD.Manager.Common;
using GD.Models.Consumer;
using GD.Models.Health;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager.Health
{
    public class HealthIndicatorBiz : BaseBiz<HealthIndicatorModel>
    {
        /// <summary>
        /// 查询指标项
        /// </summary>
        /// <returns></returns>
        public async Task<GetHealthIndicatorResponseDto> GetHealthIndicatorList(
            string userGuid)
        {
            var response = (GetHealthIndicatorResponseDto)null;

            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT * 
                            FROM t_consumer_indicator_suggestion
                            WHERE user_guid = @userGuid
                            ORDER BY creation_date DESC LIMIT 1";

                var suggestionModel = (await conn.QueryFirstOrDefaultAsync<ConsumerIndicatorSuggestionModel>(sql, new { userGuid }));

                sql = @"SELECT
	                        a.indicator_guid,
	                        a.indicator_name,
	                        a.indicator_type,
	                        b.min_value,
	                        b.max_value,
	                        b.option_name,
                            b.option_guid,
                            b.option_unit,
                            b.required
                        FROM
	                        t_health_indicator a
	                        INNER JOIN t_health_indicator_option b ON a.indicator_guid = b.indicator_guid 
                        WHERE
	                        a.`enable` = 1 
	                        AND a.display = 1
                        ORDER BY a.sort";

                var indicators = (await conn.QueryAsync<GetHealthIndicatorItem>(sql, new { userGuid }))
                    .ToList();

                if (indicators == null || indicators.Count <= 0)
                {
                    return response;
                }

                response = new GetHealthIndicatorResponseDto()
                {
                    Suggestion = suggestionModel?.Suggestion
                };

                indicators = indicators.GroupBy(s => s.IndicatorGuid).Select(s => new GetHealthIndicatorItem
                {
                    IndicatorGuid = s.Key,
                    IndicatorName = s.FirstOrDefault().IndicatorName,
                    IndicatorType = s.FirstOrDefault().IndicatorType,
                    MaxValue = s.FirstOrDefault().MaxValue,
                    MinValue = s.FirstOrDefault().MinValue,
                    OptionName = s.FirstOrDefault().OptionName,
                    OptionUnit = s.FirstOrDefault().OptionUnit,
                    OptionGuid = s.FirstOrDefault().OptionGuid
                }).ToList();

                foreach (var indicator in indicators)
                {
                    if (!indicator.IndicatorType)
                    {
                        indicator.IndicatorName = indicator.OptionName;

                        var consumerIndicatorDetail = await GetHealthIndicatorValue(userGuid, indicator.IndicatorGuid, indicator.OptionGuid);

                        indicator.ResultVale = consumerIndicatorDetail?.IndicatorValue;
                    }
                }

                response.Items = indicators;

                return response;
            }
        }

        /// <summary>
        /// 根据用户Id查找单项指标最近时间指标值
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="indicatorGuid"></param>
        /// <param name="optionGuid"></param>
        /// <returns></returns>
        async Task<ConsumerIndicatorDetailModel> GetHealthIndicatorValue(string userId, string indicatorGuid, string optionGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"	
                            SELECT
	                            b.indicator_value,
                                b.record_detail_guid
                            FROM
	                            t_consumer_indicator a
	                            INNER JOIN t_consumer_indicator_detail b ON a.indicator_record_guid = b.indicator_record_guid 
                            WHERE
	                            a.user_guid = @userId 
	                            AND a.indicator_guid =@indicatorGuid
                                AND b.indicator_option_guid=@optionGuid
	                            AND a.`enable` = 1 
	                            AND b.`enable` = 1 
                            ORDER BY
	                            a.creation_date DESC LIMIT 1";

                return await conn.QueryFirstOrDefaultAsync<ConsumerIndicatorDetailModel>(sql, new { userId, indicatorGuid, optionGuid });
            }
        }
        /// <summary>
        /// 获取指标详情总条数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> GetHealthIndicatorDetailCount(string userId, GetHealthIndicatorDetailRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT 
		                           count(1)
	                            FROM
		                            t_consumer_indicator a 
	                            WHERE
		                            a.user_guid =@userId 
		                            AND a.indicator_guid =@IndicatorGuid
		                            AND a.`enable` = 1 ";
                var count = await conn.QueryFirstOrDefaultAsync<int>(sql, new { userId, requestDto.IndicatorGuid });
                return count;
            }
        }
        /// <summary>
        /// 获取指标选项
        /// </summary>
        /// <param name="indicatorGuid">健康指标id</param>
        /// <returns></returns>
        public async Task<List<ConsumerHealthIndicatorOption>> GetHealthIndicatorOptionList(string indicatorGuid, string userGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                        b.option_name,
                            b.option_guid,
                            b.option_unit,
                            b.required,
                            b.min_value,
                            b.max_value
                        FROM
	                        t_health_indicator_option b
                        WHERE b.`enable` = 1 AND b.indicator_guid = @indicatorGuid
                        ORDER BY b.sort DESC";

                var options = (await conn.QueryAsync<ConsumerHealthIndicatorOption>(sql, new { indicatorGuid })).ToList();

                if (options is null || options.Count <= 0)
                {
                    return null;
                }

                foreach (var option in options)
                {
                    var consumerIndicatorDetail = await GetHealthIndicatorValue(userGuid, indicatorGuid, option.OptionGuid);

                    option.ResultVale = consumerIndicatorDetail?.IndicatorValue;
                }

                return options;
            }
        }

        /// <summary>
        /// 获取指标选项详情
        /// </summary>
        /// <param name="indicatorGuid">健康指标id</param>
        /// <returns></returns>
        public async Task<List<GetHealthIndicatorOptionDetailResponseDto>> GetHealthIndicatorDetailList(GetHealthIndicatorDetailRequestDto requestDto)
        {
            if (requestDto.PageIndex < 1)
            {
                requestDto.PageIndex = 1;
            }

            if (requestDto.PageSize < 1)
            {
                requestDto.PageSize = 5;
            }

            requestDto.PageIndex = (requestDto.PageIndex - 1) * requestDto.PageSize;

            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"	SELECT 
	                            c.creation_date,
	                            b.indicator_value,
	                            b.indicator_option_guid,
                                f.option_name,
                                f.option_unit,
                                f.min_value,
	                            f.max_value
	                            FROM  (
	                            SELECT DISTINCT
		                            a.creation_date,
		                            a.indicator_record_guid 
	                            FROM
		                            t_consumer_indicator a 
	                            WHERE
		                            a.user_guid = @UserGuid 
		                            AND a.indicator_guid = @IndicatorGuid
		                            AND a.`enable` = 1 
	                            ORDER BY
		                            a.creation_date DESC 
		                            LIMIT @PageIndex,
		                            @PageSize
	                            ) c LEFT JOIN	t_consumer_indicator_detail b on c.indicator_record_guid=b.indicator_record_guid  and b.`enable`=1 left join t_health_indicator_option f on b.indicator_option_guid=f.option_guid  ORDER BY c.creation_date DESC";

                return (await conn.QueryAsync<GetHealthIndicatorOptionDetailResponseDto>(sql, new { requestDto.UserGuid, requestDto.IndicatorGuid, requestDto.PageIndex, requestDto.PageSize })).ToList();
            }
        }

        /// <summary>
        /// 查找对应的健康指标选项
        /// </summary>
        /// <param name="indicatorOptionGuid"></param>
        /// <returns></returns>
        public async Task<List<HealthIndicatorOptionModel>> GetHealthIndicatorOptionAsync(string indicatorOptionGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<HealthIndicatorOptionModel>("where indicator_guid=@indicatorOptionGuid", new { indicatorOptionGuid });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 创建用户健康指标数据
        /// </summary>
        /// <param name="healthIndicatorModel"></param>
        /// <param name="healthOptionList"></param>
        /// <returns></returns>
        public async Task<bool> CreateConsumerHealthIndicatorAsync(ConsumerIndicatorModel consumerIndicatorModel, List<ConsumerIndicatorDetailModel> consumerIndicatorDetailModelList)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                await conn.InsertAsync<string, ConsumerIndicatorModel>(consumerIndicatorModel);

                consumerIndicatorDetailModelList.InsertBatch(conn);

                return true;
            });
        }

        /// <summary>
        /// 保存日常健康指标数据
        /// </summary>
        /// <param name="healthIndicatorModel"></param>
        /// <param name="healthOptionList"></param>
        /// <returns></returns>
        public async Task<bool> SaveHealthIndicators(HealthIndicatorContext context)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                if (context.DeleteIndicatorGuids.Count > 0)
                {
                    var result = await conn.DeleteListAsync<HealthIndicatorModel>(
                         @"where indicator_guid in @DeleteIndicatorGuids", new { context.DeleteIndicatorGuids });

                    await conn.DeleteListAsync<HealthIndicatorOptionModel>(@"where indicator_guid in @DeleteIndicatorGuids", new { context.DeleteIndicatorGuids });
                }

                if (context.InsertIndicatorModels.Count > 0)
                {
                    context.InsertIndicatorModels.InsertBatch(conn);
                }

                if (context.InsertIndicatorOptionModels.Count > 0)
                {
                    context.InsertIndicatorOptionModels.InsertBatch(conn);
                }

                if (context.UpdateIndicatorModels.Count > 0)
                {
                    await BatchUpdateIndicators(conn, context.UpdateIndicatorModels);
                }

                if (context.UpdateIndicatorOptionModels.Count > 0)
                {
                    await BatchUpdateIndicatorOptions(conn, context.UpdateIndicatorOptionModels);
                }

                return true;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="healthIndicators"></param>
        /// <returns></returns>
        public async Task<bool> BatchUpdateIndicators(MySql.Data.MySqlClient.
            MySqlConnection conn, List<HealthIndicatorModel> healthIndicators)
        {
            var sql = @"update t_health_indicator 
                        set sort = @Sort,
                            display = @Display,
                            last_updated_by = @LastUpdatedBy,
                            last_updated_date = @LastUpdatedDate
                        where indicator_guid = @IndicatorGuid";

            return await conn.ExecuteAsync(sql, healthIndicators) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="healthIndicatorOptions"></param>
        /// <returns></returns>
        public async Task<bool> BatchUpdateIndicatorOptions(MySql.Data.MySqlClient.
            MySqlConnection conn, List<HealthIndicatorOptionModel> healthIndicatorOptions)
        {
            var sql = @"update t_health_indicator_option 
                        set sort = @Sort,
                            option_unit = @OptionUnit,
                            min_value = @MinValue,
                            max_value = @MaxValue,
                            required = @Required,
                            last_updated_by = @LastUpdatedBy,
                            last_updated_date = @LastUpdatedDate
                        where option_guid = @OptionGuid";

            return await conn.ExecuteAsync(sql, healthIndicatorOptions) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateConsumerHealthIndicatorSuggestion(ConsumerIndicatorSuggestionModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, ConsumerIndicatorSuggestionModel>(model);

                return !string.IsNullOrEmpty(result);
            }
        }

        /// <summary>
        /// 获取日常指标列表
        /// </summary>
        /// <returns></returns>
        public async Task<GetHealthIndicatorListResponseDto> GetHealthIndicators()
        {
            var response = (GetHealthIndicatorListResponseDto)null;

            var sql = @"SELECT
	                h.indicator_guid as IndicatorGuid,	
	                h.indicator_name as IndicatorName,
	                h.indicator_type as IndicatorType,
                    h.sort as Sort,
                    h.display as Display
                FROM t_health_indicator AS h
                where h.`enable`=1 
                ORDER BY h.sort";

            using (var conn = MySqlHelper.GetConnection())
            {
                var indicators = (await conn.QueryAsync<HealthIndicator>(sql)).ToList();

                if (indicators is null || indicators.Count <= 0)
                {
                    return response;
                }

                var indicatorGuids = indicators.Select(d => d.IndicatorGuid).ToArray();

                response = new GetHealthIndicatorListResponseDto()
                {
                    HealthIndicators = indicators
                };

                sql = @"SELECT 
	                o.option_guid as OptionGuid,
                    o.indicator_guid as IndicatorGuid,
	                o.option_unit OptionUnit,
	                o.option_name as `OptionName`,
                    o.required as `Required`,
                    o.min_value AS `MinValue`,
                    o.max_value as `MaxValue`,
                    o.sort
                FROM t_health_indicator_option as o
                WHERE o.`enable`=1 and  o.indicator_guid in @indicatorGuids
                ORDER BY o.sort";

                var options = (await conn.QueryAsync
                    <HealthIndicatorOption>(sql, new
                    {
                        indicatorGuids
                    })).GroupBy(d => d.IndicatorGuid);

                foreach (var indicator in response.HealthIndicators.ToList())
                {
                    indicator.Options = options.FirstOrDefault(d => d.Key == indicator.IndicatorGuid)?.ToList();
                }
            }

            return response;
        }

        public async Task<List<HealthIndicatorOptionModel>> GetConsumerOptions(string consumerGuid)
        {
            var sql = @"SELECT * 
                        FROM t_health_indicator_option
						WHERE indicator_guid IN 
                            (SELECT DISTINCT indicator_guid 
                            FROM t_consumer_indicator WHERE user_guid = @consumerGuid)";

            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<HealthIndicatorOptionModel>(sql, new { consumerGuid })).ToList();
            }
        }
    }
}
