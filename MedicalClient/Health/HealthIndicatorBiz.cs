using Dapper;
using GD.DataAccess;
using GD.Dtos.Health;
using GD.Models.Consumer;
using GD.Models.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Health
{
    /// <summary>
    /// 健康指标业务类
    /// </summary>
    public class HealthIndicatorBiz : BizBase.BaseBiz<HealthIndicatorModel>
    {
        /// <summary>
        /// 查询指标项
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetHealthIndicatorResponseDto>> GetHealthIndicatorList()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
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
	                        LEFT JOIN t_health_indicator_option b ON a.indicator_guid = b.indicator_guid 
                        WHERE
	                        a.`enable` = 1 
	                        AND a.display = 1 
                        ORDER BY
	                        a.sort";
                var healthIndicatorList = await conn.QueryAsync<GetHealthIndicatorResponseDto>(sql);
                return healthIndicatorList?.ToList();
            }
        }
        /// <summary>
        /// 根据用户Id查找单项指标最近时间指标值
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="indicatorGuid"></param>
        /// <param name="optionGuid"></param>
        /// <returns></returns>
        public async Task<decimal?> GetHealthIndicatorValue(string userId, string indicatorGuid, string optionGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"	
                            SELECT
	                            b.indicator_value
                            FROM
	                            t_consumer_indicator a
	                            LEFT JOIN t_consumer_indicator_detail b ON a.indicator_record_guid = b.indicator_record_guid 
                            WHERE
	                            a.user_guid =@userId 
	                            AND a.indicator_guid =@indicatorGuid
                                AND b.indicator_option_guid=@optionGuid
	                            AND a.`enable` = 1 
	                            AND b.`enable` = 1 
                            ORDER BY
	                            a.creation_date DESC LIMIT 1";
                var indicatorValue = await conn.QueryFirstOrDefaultAsync<decimal?>(sql, new { userId, indicatorGuid, optionGuid });
                return indicatorValue;
            }
        }
        /// <summary>
        ///  查询最新一次建议
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetHealthIndicatorSuggestion(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"	
                            SELECT
	                            suggestion
                            FROM
	                            t_consumer_indicator_suggestion a
                            WHERE
	                            a.user_guid =@userId 
	                            AND a.`enable` = 1 
                            ORDER BY
	                            a.creation_date DESC LIMIT 1";
                var suggestion = await conn.QueryFirstOrDefaultAsync<string>(sql, new { userId });
                return suggestion;
            }
        }
        /// <summary>
        /// 获取指标选项
        /// </summary>
        /// <param name="indicatorGuid">健康指标id</param>
        /// <returns></returns>
        public async Task<List<HealthIndicatorOption>> GetHealthIndicatorOptionList(string indicatorGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = $@"SELECT
	                        b.option_name,
                            b.option_guid,
                            b.option_unit,
                            b.required,
                            b.min_value,
                            b.max_value
                        FROM
	                        t_health_indicator a
	                        LEFT JOIN t_health_indicator_option b ON a.indicator_guid = b.indicator_guid 
                        WHERE
	                        a.`enable` = 1 
                            AND b.`enable` = 1 
	                        AND a.display = 1 
                            AND a.indicator_guid=@indicatorGuid
                        ORDER BY
	                        a.sort DESC";
                var healthIndicatorDetailList = await conn.QueryAsync<HealthIndicatorOption>(sql, new { indicatorGuid });
                return healthIndicatorDetailList?.ToList();
            }
        }
        /// <summary>
        /// 获取指标选项详情
        /// </summary>
        /// <param name="indicatorGuid">健康指标id</param>
        /// <returns></returns>
        public async Task<List<HealthIndicatorDetail>> GetHealthIndicatorDetailList(string userId, GetHealthIndicatorDetailRequestDto requestDto)
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
	                            SELECT 
		                            a.creation_date,
		                            a.indicator_record_guid 
	                            FROM
		                            t_consumer_indicator a 
	                            WHERE
		                            a.user_guid =@userId 
		                            AND a.indicator_guid =@IndicatorGuid
		                            AND a.`enable` = 1 
	                            ORDER BY
		                            a.creation_date DESC 
		                            LIMIT @PageIndex,
		                            @PageSize
	                            ) c LEFT JOIN	t_consumer_indicator_detail b on c.indicator_record_guid=b.indicator_record_guid  and b.`enable`=1 inner join t_health_indicator_option f on b.indicator_option_guid=f.option_guid  ORDER BY c.creation_date DESC
	                            ";
                var healthIndicatorDetailList = await conn.QueryAsync<HealthIndicatorDetail>(sql, new { userId, requestDto.IndicatorGuid, requestDto.PageIndex, requestDto.PageSize });
                return healthIndicatorDetailList?.ToList();
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
        /// 创建健康指标数据
        /// </summary>
        /// <param name="healthIndicatorModel"></param>
        /// <param name="healthOptionList"></param>
        /// <returns></returns>
        public async Task<bool> CreateHealthIndicatorAsync(ConsumerIndicatorModel consumerIndicatorModel, List<ConsumerIndicatorDetailModel> consumerIndicatorDetailModelList)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                //保存用户健康指标
                await conn.InsertAsync<string, ConsumerIndicatorModel>(consumerIndicatorModel);
                //保存用户健康明细数据
                consumerIndicatorDetailModelList.InsertBatch(conn);
                return true;
            });
        }
        /// <summary>
        /// 修改建议
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
        /// 当前查询类使用
        /// </summary>
        public class HealthIndicatorDetail
        {
            public string OptionName { get; set; }
            public string OptionUnit { get; set; }
            public DateTime CreationDate { get; set; }
            public decimal? IndicatorValue { get; set; }
            public string IndicatorOptionGuid { get; set; }
            public decimal MaxValue { get; set; }
            public decimal MinValue { get; set; }
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
