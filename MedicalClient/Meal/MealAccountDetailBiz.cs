using Dapper;
using GD.DataAccess;
using GD.Dtos.Meal.MealAdmin;
using GD.Models.Meal;
using GD.Dtos.Meal.MealClient;
using System;
using System.Threading.Tasks;

namespace GD.Meal
{
    /// <summary>
    /// 钱包账户明细业务类
    /// </summary>
    public class MealAccountDetailBiz
    {

        /// <summary>
        /// 获取指定医院用户钱包账户明细流水
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetMealAccountDetailListResponseDto> GetDetailsByHospitalGuid(GetMealAccountDetailListRequestDto requestDto)
        {
            var sql = $@"SELECT 
	                    u.phone,
                        a.user_type,
                        a.account_type,
	                    d.account_detail_fee as fee,
                        d.account_detail_income_type as income_type,
	                    d.account_detail_type as type,
	                    d.account_detail_after_fee as total_fee,
	                    d.creation_date
                    FROM t_meal_account_detail as d
	                    INNER JOIN t_meal_account as a on d.account_guid = a.account_guid
	                    INNER JOIN t_utility_user as u on u.user_guid  = a.user_guid
                    WHERE a.hospital_guid = @HospitalGuid";

            if (!string.IsNullOrEmpty(requestDto.Phone))
            {
                sql += $" and u.phone = @Phone";
            }

            if (requestDto.StartTime.HasValue)
            {
                sql += " and d.creation_date >= @StartTime";
            }

            if (requestDto.EndTime.HasValue)
            {
                requestDto.EndTime = requestDto.EndTime.Value.AddDays(1);
                sql += " and d.creation_date <= @EndTime";
            }

            sql += " order by d.creation_date desc";

            return await MySqlHelper.QueryByPageAsync<GetMealAccountDetailListRequestDto, GetMealAccountDetailListResponseDto, MealAccountDetailItem>(sql, requestDto);
        }

        /// <summary>
        /// 创建账户明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> CreateAccountDetail(MealAccountDetailModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.InsertAsync<string, MealAccountDetailModel>(model);
            }
        }

        /// <summary>
        /// 查询个人钱包账户交易流水
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetMealWalletRecordResponseDto> GetMealWalletRecordAsync(GetMealWalletRecordRequestDto requestDto)
        {
            var sql = @"SELECT
	                        b.account_type,
	                        a.account_detail_description,
	                        a.account_detail_fee,
	                        a.creation_date	,
	                        a.account_detail_income_type,
	                        a.remark
                        FROM
	                        t_meal_account_detail a
	                        INNER JOIN t_meal_account b ON a.account_guid = b.account_guid 
                        WHERE
	                        b.hospital_guid = @HospitalGuid 
	                        AND b.user_guid = @UserGuid
	                        order by a.creation_date desc ";
            return await MySqlHelper.QueryByPageAsync<GetMealWalletRecordRequestDto, GetMealWalletRecordResponseDto, GetMealWalletRecordItemDto>(sql, requestDto);
        }

        
    }
}
