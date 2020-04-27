using Dapper;
using GD.BizBase;
using GD.DataAccess;
using GD.Dtos.Consumer.Consumer;
using GD.Dtos.Merchant.Appointment;
using GD.Models.Consumer;
using GD.Models.Merchant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Consumer
{
    /// <summary>
    /// 预约消费记录实体业务类
    /// </summary>
    public class ConsumptionBiz : BaseBiz<ConsumptionModel>
    {
        /// <summary>
        /// 消费预约
        /// </summary>
        /// <param name="consumptionModel">消费预约model</param>
        /// <param name="merchantScheduleDetailModel">预约时间明细model</param>
        /// <param name="item">个人卡项model</param>
        /// <param name="lockScheduleDetailModel">预约锁定时间明细model</param>
        /// <returns></returns>
        public async Task<bool> MakeAnAppointmentWithConsumption(ConsumptionModel consumptionModel, MerchantScheduleDetailModel merchantScheduleDetailModel, GoodsItemModel item, MerchantScheduleDetailModel lockScheduleDetailModel = null)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                var lockUpdate = lockScheduleDetailModel;
                var sql = @"UPDATE t_merchant_schedule_detail a
                            LEFT JOIN t_merchant_schedule_detail b ON a.schedule_guid = b.schedule_guid
                            AND b.`enable` = 1
                            AND a.schedule_detail_guid <> b.schedule_detail_guid
                            AND b.last_updated_date < a.last_updated_date
                            AND (
                           ( a.start_time >= b.start_time AND a.start_time<b.end_time )
                            OR ( a.end_time > b.start_time AND a.end_time<=b.end_time )
                            )
                            SET a.consumption_guid = @consumptionGuid $lockUpdate$
                            WHERE
	                            a.schedule_detail_guid = @scheduleDetailGuid
                                AND a.`enable` = 1
	                            AND b.schedule_detail_guid IS NULL;";

                if (lockScheduleDetailModel != null)
                {
                    //插入预约排班明细(项目间隔锁定时间)
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, MerchantScheduleDetailModel>(lockScheduleDetailModel))) return false;

                    //检查预约时间是否可行(项目间隔锁定时间，有无时间交叉？)
                    if ((await conn.ExecuteAsync(sql.Replace("$lockUpdate$", ""), new { scheduleDetailGuid = lockScheduleDetailModel.ScheduleDetailGuid, consumptionGuid = "Lock" })) == 0) return false;
                }

                //插入预约排班明细
                if (string.IsNullOrEmpty(await conn.InsertAsync<string, MerchantScheduleDetailModel>(merchantScheduleDetailModel))) return false;

                //检查预约时间是否可行(有无时间交叉？)
                if ((await conn.ExecuteAsync(sql.Replace("$lockUpdate$", $",a.lock_detail_guid='{lockScheduleDetailModel?.ScheduleDetailGuid}'"), new { scheduleDetailGuid = merchantScheduleDetailModel.ScheduleDetailGuid, consumptionGuid = consumptionModel.ConsumptionGuid })) == 0) return false;

                //插入消费记录
                if (string.IsNullOrEmpty(await conn.InsertAsync<string, ConsumptionModel>(consumptionModel))) return false;

                if ((await conn.UpdateAsync(item) != 1)) return false;

                return true;
            });
        }

        /// <summary>
        /// 变更预约时间
        /// </summary>
        /// <param name="consumptionModel"></param>
        /// <param name="deleteTimes"></param>
        /// <param name="merchantScheduleDetailModel"></param>
        /// <param name="lockScheduleDetailModel"></param>
        /// <returns></returns>
        public async Task<bool> AmendAppointmentAsync(ConsumptionModel consumptionModel, List<string> deleteTimes, MerchantScheduleDetailModel merchantScheduleDetailModel, MerchantScheduleDetailModel lockScheduleDetailModel = null)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                var sql = @"UPDATE t_merchant_schedule_detail a
                            LEFT JOIN t_merchant_schedule_detail b ON a.schedule_guid = b.schedule_guid
                            AND b.`enable` = 1
                            AND a.schedule_detail_guid <> b.schedule_detail_guid
                            AND b.last_updated_date < a.last_updated_date
                            AND (
                           ( a.start_time >= b.start_time AND a.start_time<b.end_time )
                            OR ( a.end_time > b.start_time AND a.end_time<=b.end_time )
                            )
                            SET a.consumption_guid = @consumptionGuid $lockUpdate$
                            WHERE
	                            a.schedule_detail_guid = @scheduleDetailGuid
                                AND a.`enable` = 1
	                            AND b.schedule_detail_guid IS NULL;";

                if (lockScheduleDetailModel != null)
                {
                    //插入预约排班明细(项目间隔锁定时间)
                    if (string.IsNullOrEmpty(await conn.InsertAsync<string, MerchantScheduleDetailModel>(lockScheduleDetailModel))) return false;

                    //检查预约时间是否可行(项目间隔锁定时间，有无时间交叉？)
                    if ((await conn.ExecuteAsync(sql.Replace("$lockUpdate$", ""), new { scheduleDetailGuid = lockScheduleDetailModel.ScheduleDetailGuid, consumptionGuid = "Lock" })) == 0) return false;
                }

                //插入预约排班明细
                if (string.IsNullOrEmpty(await conn.InsertAsync<string, MerchantScheduleDetailModel>(merchantScheduleDetailModel))) return false;

                //检查预约时间是否可行(有无时间交叉？)
                if ((await conn.ExecuteAsync(sql.Replace("$lockUpdate$", $",a.lock_detail_guid='{lockScheduleDetailModel?.ScheduleDetailGuid}'"), new { scheduleDetailGuid = merchantScheduleDetailModel.ScheduleDetailGuid, consumptionGuid = consumptionModel.ConsumptionGuid })) == 0) return false;

                //更新消费记录

                if ((await conn.UpdateAsync(consumptionModel) != 1)) return false;

                //删除旧的时间占用记录
                var delRes = await conn.DeleteListAsync<MerchantScheduleDetailModel>("where schedule_detail_guid in @deleteTimes", new { deleteTimes });
                return true;
            });
        }

        /// <summary>
        /// 按条件查询排班列表 List
        /// </summary>
        /// <param name="fromId"></param>
        /// <param name="projectGuid"></param>
        /// <returns></returns>
        public async Task<GetScheduleListByConditionPageResponseDto> SelectScheduleListByCondition(GetScheduleListByConditionRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var wherePhone = " ";
                var whereMerchantGuid = " ";
                var whereProjectGuid = " ";
                var whereDate = " ";
                var whereTherapistGuid = " ";
                if (!string.IsNullOrWhiteSpace(requestDto.Phone))
                {
                    wherePhone += $"  AND g.phone = '{requestDto.Phone}' ";
                }

                if (!string.IsNullOrWhiteSpace(requestDto.MerchantGuid))
                {
                    whereMerchantGuid += $" And a.merchant_guid ='{requestDto.MerchantGuid}' ";
                }
                if (!string.IsNullOrWhiteSpace(requestDto.ProjectGuid))
                {
                    whereProjectGuid += $" AND a.project_guid = '{requestDto.ProjectGuid}' ";
                }
                if (requestDto.Date != null)
                {
                    whereDate += $" AND d.schedule_date='{requestDto.Date.Value.ToString("yyyy/MM/dd")}' ";
                }
                if (!string.IsNullOrWhiteSpace(requestDto.TherapistGuid))
                {
                    whereTherapistGuid += $" And b.therapist_guid='{requestDto.TherapistGuid}' ";
                }

                var sql = $@"DROP TEMPORARY TABLE
                                        IF
	                                        EXISTS tmp_schedule;
                                        DROP TEMPORARY TABLE
                                        IF
	                                        EXISTS tmp_goods;
                                        DROP TEMPORARY TABLE
                                        IF
	                                        EXISTS tmp_result;
                                        DROP TEMPORARY TABLE
                                        IF
	                                        EXISTS tmp_result_1;
                                        CREATE TEMPORARY TABLE tmp_schedule AS SELECT distinct
                                        d.schedule_date,
                                        d.schedule_guid,
                                        a.project_guid,
                                        b.therapist_guid,
                                        b.therapist_name
                                        FROM
	                                        t_mall_project_merchant a
	                                        INNER JOIN t_merchant_therapist b ON a.merchant_guid = b.merchant_guid
	                                        INNER JOIN t_merchant_therapist_project c ON c.therapist_guid = b.therapist_guid
	                                        AND c.project_guid = a.project_guid
	                                        INNER JOIN t_merchant_schedule AS d ON d.target_guid = b.therapist_guid
                                        WHERE 1=1
	                                        {whereMerchantGuid}
	                                        {whereProjectGuid}
	                                        {whereDate}
	                                        {whereTherapistGuid}
                                        ORDER BY
	                                        schedule_date;
                                        CREATE TEMPORARY TABLE tmp_goods AS SELECT
                                        a.project_guid,
                                        g.user_guid,
                                        a.goods_item_guid
                                        FROM
	                                        t_consumer_goods_item a
	                                        INNER JOIN t_consumer_goods f ON f.goods_guid = a.goods_guid
	                                        INNER JOIN t_utility_user g ON g.user_guid = f.user_guid
                                        WHERE f.available=1 and  a.remain > 0
                                            AND
                                                    (
	                                                    ( DATE_FORMAT( '{requestDto.Date.Value.ToString("yyyy/MM/dd")}', '%Y-%m-%d' )
                                                BETWEEN
                                                    DATE_FORMAT( f.effective_start_date, '%Y-%m-%d' )
                                                AND
                                                    DATE_FORMAT( f.effective_end_date, '%Y-%m-%d' ) )
	                                            OR ( f.effective_start_date IS NULL AND effective_end_date IS NULL )
	                                                    )
	                                        {whereProjectGuid}
	                                        {wherePhone}
	                                        LIMIT 1;
                                        CREATE TEMPORARY TABLE tmp_result AS SELECT
                                        a.schedule_guid,
                                        a.schedule_date,
                                        a.therapist_guid,
                                        a.therapist_name,
                                        b.goods_item_guid
                                        FROM
	                                        tmp_schedule a
	                                        LEFT JOIN tmp_goods b ON a.project_guid = b.project_guid
                                        ORDER BY
	                                        a.schedule_date;
                                        CREATE TEMPORARY TABLE tmp_result_1 SELECT
                                        *
                                        FROM
	                                        tmp_result;
                                        SELECT
	                                        count( 1 ) as TotalNum
                                        FROM
	                                        tmp_result_1;
                                        SELECT
	                                        *
                                        FROM
	                                        tmp_result
                                        order by schedule_date
	                                        LIMIT {(requestDto.PageIndex - 1) * requestDto.PageSize},
	                                        {requestDto.PageSize};   ";

                var reader = await conn.QueryMultipleAsync(sql);
                int total = (await reader.ReadAsync<TotalNumDto>()).FirstOrDefault().TotalNum;
                var result = await reader.ReadAsync<GetScheduleListByConditionResponseDto>();
                GetScheduleListByConditionPageResponseDto responseDto = new GetScheduleListByConditionPageResponseDto
                {
                    Total = total,
                    CurrentPage = result
                };
                return responseDto;
            }
        }

        /// <summary>
        ///获取今日/全部预约列表
        /// </summary>
        /// <param name="targetGuid">目标guid</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">单页条数</param>
        /// <returns></returns>
        public async Task<GetAppointmentListResponseDto> GetAppointmentList(GetAppointmentListRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                                    c.consumption_guid,
										p.classify_name,/*大类名称*/
	                                    c.appointment_date,/*预约时间*/
	                                    u.user_name, /*预约人姓名*/
	                                    u.phone,/*联系电话*/
	                                    p.project_name,/*项目名称*/
	                                    c.consumption_status/*预约状态*/,
                                        c.creation_date /*创建时间*/,
                                        c.remark,
                                        t.therapist_name AS TherapistName,/*服务人员*/
                                        s.`name` AS ServerMemberName,
                                        s.sex AS ServerMemberGender,
                                        s.`age_year` AS ServerMemberAgeYear,
	                                    s.age_month AS ServerMemberAgeMonth
                                    FROM
	                                    t_consumer_consumption AS c
                                        INNER JOIN t_merchant_therapist AS t ON t.therapist_guid = c.operator_guid
                                        LEFT JOIN  t_consumer_service_member AS s ON c.service_member_guid = s.service_member_guid
	                                    LEFT JOIN t_utility_user AS u ON c.user_guid = u.user_guid
	                                    LEFT JOIN t_mall_project AS p ON c.project_guid = p.project_guid
                                    WHERE
	                                    c.merchant_guid = @UserGuid
                                        AND c.`enable`= @Enable";

                if (requestDto.Type <= 0)
                {
                    requestDto.BeginDate = DateTime.Today;
                    requestDto.EndDate = requestDto.BeginDate.Value.AddDays(1);
                }
                else
                {
                    if (requestDto.BeginDate.HasValue && requestDto.EndDate.HasValue)
                    {
                        requestDto.EndDate = requestDto.EndDate.Value.AddDays(1);
                    }
                }

                sql += " AND (c.appointment_date >= @BeginDate AND c.appointment_date < @EndDate)";

                if (!string.IsNullOrEmpty(requestDto.ClassifyGuid))
                {
                    sql += " AND p.classify_guid = @ClassifyGuid";
                }

                sql += " ORDER BY c.appointment_date DESC, c.consumption_status DESC";

                return await MySqlHelper.QueryByPageAsync<GetAppointmentListRequestDto, GetAppointmentListResponseDto, GetAppointmentItemDto>(sql, requestDto);
            }
        }

        /// <summary>
        /// 根据消费ID获取消费预约唯一实例
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        public ConsumptionModel GetModel(string consumptionGuid)
        {
            //return MySqlHelper.GetModelById<ConsumptionModel>(consumptionGuid);
            using (var conn = MySqlHelper.GetConnection())
            {
                return conn.Get<ConsumptionModel>(consumptionGuid);
            }
        }

        /// <summary>
        /// 更新ConsumptionModel
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        public async Task<bool> CancelOppointment(ConsumptionModel model, GoodsItemModel goodsItemModel, MerchantScheduleDetailModel msdModel)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                //消费预约信息
                if (await conn.UpdateAsync(model) < 1) { return false; }

                //撤回已扣除数量
                if (await conn.UpdateAsync(goodsItemModel) < 1) { return false; }

                //取消预约后，若个人商品处于不可用状态时，更新个人商品为可用状态
                await conn.ExecuteAsync("update t_consumer_goods set available=1 where goods_guid=@goodsGuid and available=0", new { goodsGuid = goodsItemModel.GoodsGuid });

                //去除预约所占的时间
                await conn.DeleteAsync<MerchantScheduleDetailModel>(msdModel.LockDetailGuid);
                await conn.DeleteAsync(msdModel);

                return true;
            });
            return result;
        }

        /// <summary>
        /// 我的预约列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetMyBookedItemListOfCosmetologyResponseDto> GetMyBookedItemListOfCosmetologyAsync(GetMyBookedItemListOfCosmetologyRequestDto requestDto)
        {
            string sqlConsumptionStatus = "";
            if (requestDto.ConsumptionStatus != null)
            {
                sqlConsumptionStatus = "AND consumption_status = @ConsumptionStatus";
            }
            string sql = $@"SELECT
	                            a.consumption_guid,
	                            a.consumption_no,
	                            a.appointment_date,
	                            a.consumption_status,
	                            a.project_guid,
                                a.merchant_remark,
	                            d.project_name,
	                            d.operation_time,
	                            b.merchant_guid,
	                            c.merchant_name,
	                            c.merchant_address,
	                            a.operator_guid AS therapist_guid,
	                            b.therapist_name,
	                            b.therapist_phone ,
	                            CONCAT(acc.base_path,acc.relative_path) as therapist_portrait
                            FROM
	                            t_consumer_consumption a
	                            LEFT JOIN t_merchant_therapist b ON a.operator_guid = b.therapist_guid
	                            LEFT JOIN t_merchant c ON b.merchant_guid = c.merchant_guid
	                            LEFT JOIN t_mall_project d ON a.project_guid = d.project_guid
	                            left join t_utility_accessory acc on acc.accessory_guid=b.portrait_guid
                                WHERE
	                                a.user_guid = @UserGuid
	                                AND a.`enable` = 1
	                                {sqlConsumptionStatus}
                                ORDER BY a.consumption_status,
	                                a.appointment_date";
            return await MySqlHelper.QueryByPageAsync<GetMyBookedItemListOfCosmetologyRequestDto, GetMyBookedItemListOfCosmetologyResponseDto, GetMyBookedItemListOfCosmetologyItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 获取已完成的消费记录分页列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetCompletedConsumptionPageListResponseDto> GetCompletedConsumptionPageListAsync(GetCompletedConsumptionPageListRequestDto requestDto)
        {
            var sql = @"SELECT
	                                a.consumption_guid,
	                                a.project_guid,
	                                b.project_name,
	                                CONCAT( acc.base_path, acc.relative_path ) AS project_picture,
	                                merchant.merchant_name,
	                                a.consumption_date,
	                                a.comment_guid,
	                                merchant.merchant_address,
	                                mt.therapist_guid,
	                                mt.therapist_name,
                                CASE

	                                WHEN ISNULL( comment_guid ) THEN
                                FALSE ELSE TRUE
	                                END AS is_comment
                                FROM
	                                t_consumer_consumption a
	                                INNER JOIN t_mall_project b ON a.project_guid = b.project_guid
	                                LEFT JOIN t_utility_accessory acc ON acc.accessory_guid = b.picture_guid
	                                LEFT JOIN t_merchant merchant ON merchant.merchant_guid = a.merchant_guid
	                                left join t_merchant_therapist as mt on a.operator_guid=mt.therapist_guid
                                WHERE
	                                consumption_status = 'Completed'
	                                AND user_guid = @ConsumerGuid
                                ORDER BY
	                                is_comment,
	                                consumption_date DESC ";
            return await MySqlHelper.QueryByPageAsync<GetCompletedConsumptionPageListRequestDto, GetCompletedConsumptionPageListResponseDto, GetCompletedConsumptionPageListItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 异步获取唯一实例
        /// </summary>
        /// <param name="consumptionGuid"></param>
        /// <returns></returns>
        public async Task<ConsumptionModel> GetModelAsync(string consumptionGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<ConsumptionModel>("select * from t_consumer_consumption where consumption_guid=@consumptionGuid and `enable`=1", new { consumptionGuid });
            }
        }

        /// <summary>
        /// 检测美疗师是否有进行中的预约服务
        /// </summary>
        /// <param name="therapistId">美疗师Id</param>
        /// <returns></returns>
        public async Task<bool> CheckHasWorkingConsumptionAsync(string therapistId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync<ConsumptionModel>("select * from t_consumer_consumption where operator_guid=@therapistId and `enable`=1 and consumption_status='Arrive' limit 1", new { therapistId });
                return result != null;
            }
        }

        /// <summary>
        /// 评价消费
        /// </summary>
        /// <param name="model"></param>
        /// <param name="commentModel"></param>
        /// <returns></returns>
        public async Task<bool> CommentConsumptionAsync(ConsumptionModel model, CommentModel commentModel)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                if (string.IsNullOrEmpty(await conn.InsertAsync<string, CommentModel>(commentModel))) return false;

                await conn.UpdateAsync(model);
                return true;
            });
        }

        /// <summary>
        /// 获取实际已过期，但状态仍为已预约的预约记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ConsumptionModel>> GetMissedConsumptionAsync(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"WHERE user_guid = @userId 
	                       AND consumption_status = 'Booked' 
	                       AND appointment_date < DATE_SUB( NOW( ), INTERVAL 60 MINUTE ) 
                           order by appointment_date";
                var result = await conn.GetListAsync<ConsumptionModel>(sql, new { userId });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 预约记录过期不扣除可用项目次数
        /// </summary>
        /// <returns></returns>
        public async Task<bool> MissConsumptionWithoutAbatementAsync(ConsumptionModel model, GoodsItemModel goodsItemModel)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                //消费预约信息
                if (await conn.UpdateAsync(model) < 1) { return false; }

                //撤回已扣除数量
                if (await conn.UpdateAsync(goodsItemModel) < 1) { return false; }

                //取消预约后，若个人商品处于不可用状态时，更新个人商品为可用状态
                await conn.ExecuteAsync("update t_consumer_goods set available=1 where goods_guid=@goodsGuid and available=0", new { goodsGuid = goodsItemModel.GoodsGuid });

                return true;
            });
            return result;
        }

        /// <summary>
        /// 检测用户当月在门店爽约次数是否已经超过限制
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<bool> IsMissedConsumptionExceedTheLimitAsync(string userId, string merchantGuid, DateTime date)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.RecordCountAsync<ConsumptionModel>("where user_guid = @userId and merchant_guid = @merchantGuid and consumption_status='Miss' and `enable`=1 and appointment_date BETWEEN @startDate and @endDate",
                    new
                    {
                        userId,
                        merchantGuid,
                        startDate = new DateTime(date.Year, date.Month, 1), // 2019-01-01 00:00:00
                        endDate = Convert.ToDateTime(date.ToShortDateString()).AddDays(1 - date.Day).AddMonths(1).AddMinutes(-1)//2019-01-31 23:59:59
                    });
                return result >= 3;
            }
        }


        /// <summary>
        /// 消费预约自动过期（默认超过预约时间一小时后），单个用户范围
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> MissConsumptionAutomaticAsync(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"UPDATE t_consumer_consumption
                            SET consumption_status = 'Miss'
                            WHERE
	                            user_guid = @userId
	                            AND consumption_status = 'Booked'
                                AND `enable` = 1
	                            AND appointment_date < DATE_SUB( NOW( ), INTERVAL 60 MINUTE )";
                var result = await conn.ExecuteAsync(sql, new { userId });
                return true;
            }
        }

        /// <summary>
        /// 消费预约自动过期（默认超过预约时间一小时后）,整个门店范围
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public async Task<bool> MissConsumptionAutomaticAsyncOfMerchant(string merchantId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"UPDATE t_consumer_consumption
                            SET consumption_status = 'Miss'
                            WHERE
	                            merchant_guid = @merchantId
	                            AND consumption_status = 'Booked'
                                AND `enable` = 1
	                            AND appointment_date < DATE_SUB( NOW( ), INTERVAL 60 MINUTE )";
                var result = await conn.ExecuteAsync(sql, new { merchantId });
                return true;
            }
        }

        /// <summary>
        /// 获取用户预约记录状态为已预约的记录数量
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public async Task<int> GetMyBookedItemCountAsync(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select count(consumption_guid) from t_consumer_consumption where user_guid=@userId and consumption_status='Booked' and `enable`=1";
                var result = await conn.QueryFirstOrDefaultAsync<int>(sql, new { userId });
                return result;
            }
        }
    }
}