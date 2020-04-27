using Dapper;
using GD.Models.Utility;
using GD.DataAccess;
using GD.Dtos.Consumer.Consumer;
using GD.Mall;
using GD.Models.CommonEnum;
using GD.Models.Consumer;
using GD.Models.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Fushion.CompositeBiz
{
    public class CreateScoreBiz
    {
        /// <summary>
        /// 产生分销积分
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateDistributionScore(string[] orderIds, decimal oneLevelDistributionRate, decimal twoLevelDistributionRate)
        {
            List<DistributionForOrderDto> distributionForOrderDtos = new List<DistributionForOrderDto>();
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"DROP TEMPORARY TABLE
                            IF
	                            EXISTS tmpOrder;
                            CREATE TEMPORARY TABLE tmpOrder AS SELECT
                            * 
                            FROM
	                            t_mall_order 
                            WHERE
	                            order_guid IN @orderIds 
	                            AND debt = 0 
	                            AND `enable` = 1;
                            SELECT
	                            o.order_guid AS OrderGuid,
                                o.user_guid as UserGuid,
                                o.platform_type as PlatformType,
	                            o.paid_amount AS PaidAmount,
	                            r1.consumer_guid AS OneLevelDistribution,
	                            r2.consumer_guid AS TwoLevelDistribution 
                            FROM
	                            tmpOrder o
	                            INNER JOIN t_consumer cons ON o.user_guid = cons.consumer_guid
	                            LEFT JOIN t_consumer r1 ON cons.recommend_guid = r1.consumer_guid 
	                            AND r1.distribution_enable = 1
	                            LEFT JOIN t_consumer r2 ON r1.recommend_guid = r2.consumer_guid 
	                            AND r2.distribution_enable = 1;";
                distributionForOrderDtos = (await conn.QueryAsync<DistributionForOrderDto>(sql, new { orderIds }))?.ToList();
            }
            if (distributionForOrderDtos == null)
            {
                return false;
            }
            List<ScoreRecordModel> lstScoreRecordModel = new List<ScoreRecordModel>();
            List<ScoreModel> lstScoreModel = new List<ScoreModel>();
            foreach (var item in distributionForOrderDtos)
            {
                if (item.OneLevelDistribution != null)
                {
                    var scoreRecord = new ScoreRecordModel
                    {
                        ScoreRecordGuid = Guid.NewGuid().ToString("N"),
                        UserGuid = item.OneLevelDistribution,
                        TargetUserGuid = item.UserGuid,
                        Ammount = item.PaidAmount,
                        Rate = oneLevelDistributionRate,
                        Score = (int)(oneLevelDistributionRate * item.PaidAmount),
                        ScoreSourceType = ScoreSourceTypeEnum.Distribution.ToString(),
                        CreatedBy = "system",
                        LastUpdatedBy = "system"
                    };
                    lstScoreRecordModel.Add(scoreRecord);
                    var score = new ScoreModel
                    {
                        ScoreGuid = Guid.NewGuid().ToString("N"),
                        UserGuid = item.OneLevelDistribution,
                        UserTypeGuid = DictionaryType.Consumer_UserType,
                        Variation = (int)(oneLevelDistributionRate * item.PaidAmount),
                        Reason = "一级分销积分提成",
                        PlatformType = item.PlatformType,
                        ScoreLock = true,
                        ScoreRecordGuid = scoreRecord.ScoreRecordGuid,
                        CreatedBy = "system",
                        LastUpdatedBy = "system"
                    };
                    lstScoreModel.Add(score);
                }
                if (item.TwoLevelDistribution != null)
                {
                    var scoreRecord = new ScoreRecordModel
                    {
                        ScoreRecordGuid = Guid.NewGuid().ToString("N"),
                        UserGuid = item.TwoLevelDistribution,
                        TargetUserGuid = item.UserGuid,
                        Ammount = item.PaidAmount,
                        Rate = oneLevelDistributionRate,
                        Score = (int)(oneLevelDistributionRate * item.PaidAmount),
                        ScoreSourceType = ScoreSourceTypeEnum.Distribution.ToString(),
                        CreatedBy = "system",
                        LastUpdatedBy = "system"
                    };
                    lstScoreRecordModel.Add(scoreRecord);
                    var score = new ScoreModel
                    {
                        ScoreGuid = Guid.NewGuid().ToString("N"),
                        UserGuid = item.TwoLevelDistribution,
                        UserTypeGuid = DictionaryType.Consumer_UserType,
                        Variation = (int)(twoLevelDistributionRate * item.PaidAmount),
                        Reason = "二级分销积分提成",
                        PlatformType = item.PlatformType,
                        ScoreLock = true,
                        ScoreRecordGuid = scoreRecord.ScoreRecordGuid,
                        CreatedBy = "system",
                        LastUpdatedBy = "system"
                    };
                    lstScoreModel.Add(score);
                }

            }
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                foreach (var item in lstScoreRecordModel)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, ScoreRecordModel>(item)))
                    {
                        return false;
                    }
                }
                foreach (var item in lstScoreModel)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, ScoreModel>(item)))
                    {
                        return false;
                    }
                }
                return true;
            });

        }
        /// <summary>
        /// 产生消费积分
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateConsumptionScore(string[] orderIds,decimal consumptionRate)
        {
            var orders = await new OrderBiz().GetModelsByIds(orderIds);
            if (orders == null)
            {
                return false;
            }
            var lstOrders = orders.Where(a => a.Debt == 0).ToList();
            if (!lstOrders.Any())
            {
                return false;
            }
            List<ScoreRecordModel> lstScoreRecordModel = new List<ScoreRecordModel>();
            List<ScoreModel> lstScoreModel = new List<ScoreModel>();
            foreach (var item in lstOrders)
            {
                var scoreRecord = new ScoreRecordModel
                {
                    ScoreRecordGuid = Guid.NewGuid().ToString("N"),
                    UserGuid = item.UserGuid,
                    TargetUserGuid = item.UserGuid,
                    Ammount = item.PaidAmount,
                    Rate = consumptionRate,
                    Score = (int)(consumptionRate * item.PaidAmount),
                    ScoreSourceType = ScoreSourceTypeEnum.Consumption.ToString(),
                    CreatedBy = "system",
                    LastUpdatedBy = "system"
                };
                lstScoreRecordModel.Add(scoreRecord);
                var score = new ScoreModel
                {
                    ScoreGuid = Guid.NewGuid().ToString("N"),
                    UserGuid = item.UserGuid,
                    UserTypeGuid = DictionaryType.Consumer_UserType,
                    Variation = (int)(consumptionRate * item.PaidAmount),
                    Reason = "订单消费提成",
                    PlatformType = item.PlatformType,
                    ScoreLock = false,
                    ScoreRecordGuid = scoreRecord.ScoreRecordGuid,
                    CreatedBy = "system",
                    LastUpdatedBy = "system"
                };
                lstScoreModel.Add(score);
            }
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                foreach (var item in lstScoreRecordModel)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, ScoreRecordModel>(item)))
                    {
                        return false;
                    }
                }
                foreach (var item in lstScoreModel)
                {
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, ScoreModel>(item)))
                    {
                        return false;
                    }
                }
                return true;
            });
        }
    }
}
