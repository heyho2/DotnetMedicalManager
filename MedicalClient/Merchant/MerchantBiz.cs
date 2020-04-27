using Dapper;
using GD.Common.EnumDefine;
using GD.DataAccess;
using GD.Dtos.Admin.Merchant;
using GD.Dtos.Doctor.Doctor;
using GD.Models.Manager;
using GD.Models.Merchant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Merchant
{
    /// <summary>
    /// 商户模块业务类
    /// </summary>
    public class MerchantBiz
    {
        /// <summary>
        /// 通过商户账号获取商户model
        /// </summary>
        /// <param name="account">商户账号</param>
        /// <returns></returns>
        public async Task<MerchantModel> GetModelByAccountAsync(string account)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryFirstOrDefaultAsync<MerchantModel>("select * from t_merchant where account = @account and `enable` = 1", new { account }));
            }
        }

        /// <summary>
        /// 获取商户模型实例
        /// </summary>
        /// <param name="guid">主键guid</param>
        /// <returns></returns>
        public MerchantModel GetModel(string guid, bool enable = true)
        {
            var sql = "select * from t_merchant where merchant_guid=@guid and enable=@enable";
            var model = MySqlHelper.SelectFirst<MerchantModel>(sql, new { guid, enable });
            return model;
        }

        /// <summary>
        /// 异步获取商户唯一数据
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<MerchantModel> GetModelAsync(string guid, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select * from t_merchant where merchant_guid=@guid and enable=@enable";
                return await conn.QueryFirstOrDefaultAsync<MerchantModel>(sql, new { guid, enable });
            }
        }

        /// <summary>
        /// 获取订单唯一实例
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        public MerchantModel GetModelNoEnable(string merchantGuid)
        {
            return MySqlHelper.GetModelById<MerchantModel>(merchantGuid);
        }
        /// <summary>
        /// 异步获取商户唯一数据
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<MerchantModel> GetModelAsyncNoEnable(string guid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = "select * from t_merchant where merchant_guid=@guid ";
                return await conn.QueryFirstOrDefaultAsync<MerchantModel>(sql, new { guid });
            }
        }
        public async Task<int> RecordCountAsync()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.RecordCountAsync<MerchantModel>("where enable=@enable and Status=@Status", new { enable = true, Status = MerchantModel.StatusEnum.Approved.ToString() });
            }
        }

        public async Task<MerchantModel> GetAsync(string guid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<MerchantModel>(guid);
            }
        }
        /// <summary>
        /// 更新医生model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(MerchantModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.UpdateAsync(model)) > 0;
            }
        }
        #region 修改
        /// <summary>
        /// 新增商户
        /// </summary>
        /// <param name="merchantModel">商户模型实例</param>
        /// <returns>是否成功</returns>
        public bool InsertMerchant(MerchantModel merchantModel)
        {
            return !string.IsNullOrEmpty(merchantModel.Insert());
        }

        public bool UpdateModel(MerchantModel model)
        {
            return model.Update() == 1;
        }

        #endregion
        public async Task<GetReviewMerchantPageResponseDto> GetReviewMerchantPageAsync(GetReviewMerchantPageRequestDto request)
        {
            var sqlWhere = $@"1=1 ";//AND enable=1

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                sqlWhere = $"{sqlWhere} AND Merchant_Name like @Name";
            }
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                sqlWhere = $"{sqlWhere} AND status = @Status";
            }
            if (request.BeginDate != null)
            {
                request.BeginDate = request.BeginDate?.Date;
                sqlWhere = $"{sqlWhere} AND creation_date > @BeginDate";
            }
            if (request.EndDate != null)
            {
                request.EndDate = request.EndDate?.AddDays(1).Date;
                sqlWhere = $"{sqlWhere} AND creation_date < @EndDate";
            }
            var sql = $@"
SELECT * FROM(
    SELECT
	    A.*,
	    CONCAT( B.base_path, B.relative_path ) AS signatureUrl,
	    C.user_name,
	    D.scope 
    FROM
	    t_merchant A
	    LEFT JOIN t_utility_accessory B ON B.accessory_guid = A.signature_guid
	    LEFT JOIN t_utility_user C ON C.user_guid = A.merchant_guid
	    LEFT JOIN (
	        SELECT
		        a.merchant_guid,
		        GROUP_CONCAT( b.config_name ) scope 
	        FROM
		        t_merchant_scope a
		        LEFT JOIN t_manager_dictionary b ON a.scope_dic_guid = b.dic_guid 
	        GROUP BY
		        a.merchant_guid 
	    ) D ON A.merchant_guid = D.merchant_guid
) __t 
WHERE
	{sqlWhere}
ORDER BY
	creation_date";
            request.Name = $"%{request.Name}%";
            return await MySqlHelper.QueryByPageAsync<GetReviewMerchantPageRequestDto, GetReviewMerchantPageResponseDto, GetReviewMerchantPageItemDto>(sql, request);
        }

        public async Task<IEnumerable<GetBusinessScopeLicenseItemDto>> GetBusinessScopeLicenseListAsync(string merchantGuid)
        {
            var sql = $@"
SELECT
	a.merchant_guid,
	b.config_name as name,
	a.picture_guid,
	CONCAT( c.base_path, c.relative_path ) AS picture_url 
FROM
	t_merchant_scope a
	LEFT JOIN t_manager_dictionary b ON a.scope_dic_guid = b.dic_guid
	LEFT JOIN t_utility_accessory c ON c.accessory_guid = a.picture_guid
WHERE 
	a.merchant_guid = @merchantGuid
";
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryAsync<GetBusinessScopeLicenseItemDto>(sql, new { merchantGuid });
            }
        }

        public async Task<bool> ReviewMerchantAsync(MerchantModel model, string rejectReason)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.UpdateAsync(model);
                ReviewRecordModel reviewRecordModel = new ReviewRecordModel
                {
                    OwnerGuid = model.MerchantGuid,
                    CreatedBy = model.LastUpdatedBy,
                    Enable = true,
                    LastUpdatedBy = model.LastUpdatedBy,
                    OrgGuid = string.Empty,
                    Status = model.Status,
                    RejectReason = rejectReason,
                    ReviewGuid = Guid.NewGuid().ToString("N"),
                    Type = ReviewRecordModel.TypeEnum.Merchant.ToString()
                };
                await conn.InsertAsync<string, ReviewRecordModel>(reviewRecordModel);
                return true;
            });
            return result;
        }

        /// <summary>
        /// 生美-获取美疗师介绍
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<List<GetBeautyDoctorIntroduceResponseDto>> GetBeautyDoctorIntroduceAsync(GetBeautyDoctorIntroduceRequestDto requestDto)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                //需要新添数据库表字段
                string sql = "select * from t_merchant_therapist  ";

                var result = await conn.QueryAsync<GetBeautyDoctorIntroduceResponseDto>(sql,
                        new { requestDto.Enable, requestDto.TherapistGuid });
                return result?.ToList();
            }
        }
        public async Task<GetMerchantPageResponseDto> GetMerchantPageAsync(GetMerchantPageRequestDto request)
        {
            var sqlWhere = $@"1=1 ";

            sqlWhere = $"{sqlWhere} AND status = '{MerchantModel.StatusEnum.Approved.ToString()}'";

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                sqlWhere = $"{sqlWhere} AND Merchant_Name like @Name";
            }
            if (!string.IsNullOrWhiteSpace(request.Scope))
            {
                sqlWhere = $"{sqlWhere} AND scope like @Scope";
            }
            if (request.BeginDate != null)
            {
                request.BeginDate = request.BeginDate?.Date;
                sqlWhere = $"{sqlWhere} AND creation_date > @BeginDate";
            }
            if (request.EndDate != null)
            {
                request.EndDate = request.EndDate?.AddDays(1).Date;
                sqlWhere = $"{sqlWhere} AND creation_date < @EndDate";
            }
            if (!string.IsNullOrWhiteSpace(request.PlatformType))
            {
                if (request.PlatformType.ToLower() == PlatformType.CloudDoctor.ToString().ToLower())
                {
                    sqlWhere = $"{sqlWhere} and platform_type = @PlatformType";
                }
                else
                {
                    sqlWhere = $"{sqlWhere} and platform_type != '{PlatformType.CloudDoctor.ToString()}'";
                }
            }
            var sql = $@"
SELECT * FROM(
    SELECT
	    A.*,
	    CONCAT( B.base_path, B.relative_path ) AS signatureUrl,
	    C.user_name,
	    D.scope,
	    E.MonthlySales,
	    E.TotalMonthlyAmount 
    FROM
	    t_merchant A
	    LEFT JOIN t_utility_accessory B ON B.accessory_guid = A.signature_guid
	    LEFT JOIN t_utility_user C ON C.user_guid = A.merchant_guid
	    LEFT JOIN (
	        SELECT
		        a.merchant_guid,
		        GROUP_CONCAT( b.config_name ) scope 
	        FROM
		        t_merchant_scope a
		        LEFT JOIN t_manager_dictionary b ON a.scope_dic_guid = b.dic_guid 
	        GROUP BY
		        a.merchant_guid 
	        ) D ON A.merchant_guid = D.merchant_guid
	    LEFT JOIN (
	        SELECT
		        yue,
		        merchant_guid,
		        count( 1 ) AS MonthlySales,
		        SUM( product_price * product_count ) AS TotalMonthlyAmount 
	        FROM
		        (
		        SELECT
			        DATE_FORMAT( a.creation_date, '%Y-%m' ) AS yue,
			        b.merchant_guid,
			        a.product_price,
			        a.product_count 
		        FROM
			        t_mall_order_detail a
			        LEFT JOIN t_mall_product b ON a.product_guid = b.product_guid 
		        WHERE
			        a.`enable` = 1 
		        ) t 
	        WHERE
		        yue = DATE_FORMAT( now( ), '%Y-%m' ) 
	        GROUP BY
		        yue,
	        merchant_guid 
	    ) E ON A.merchant_guid = E.merchant_guid
) __t 
WHERE
	{sqlWhere}
ORDER BY
	creation_date desc";
            request.Name = $"%{request.Name}%";
            request.Scope = $"%{request.Name}%";
            return await MySqlHelper.QueryByPageAsync<GetMerchantPageRequestDto, GetMerchantPageResponseDto, GetMerchantPageItemDto>(sql, request);
        }

        public async Task<GetMerchantOrderDetailPageResponseDto> GetMerchantOrderDetailPageAsync(GetMerchantOrderDetailPageRequestDto request)
        {
            var sqlWhere = $@"1=1 and Enable=1";
            if (!string.IsNullOrWhiteSpace(request.MerchantGuid))
            {
                sqlWhere = $"{sqlWhere} AND merchant_guid = @MerchantGuid";
            }
            if (request.BeginDate != null)
            {
                request.BeginDate = request.BeginDate?.Date;
                sqlWhere = $"{sqlWhere} AND creation_date > @BeginDate";
            }
            if (request.EndDate != null)
            {
                request.EndDate = request.EndDate?.AddDays(1).Date;
                sqlWhere = $"{sqlWhere} AND creation_date < @EndDate";
            }

            var sql = $@"
SELECT * FROM(
    SELECT
	    a.*,
	    b.merchant_guid,
	    b.category_name,
	    d.user_Name as UserName,
	    c.creation_date AS order_date,
        c.order_no,
        c.user_guid 
    FROM
	    t_mall_order_detail a
	    LEFT JOIN t_mall_product b ON a.product_guid = b.product_guid
	    LEFT JOIN t_mall_order c ON c.order_guid = a.order_guid
	    LEFT JOIN t_utility_user d ON c.user_guid = d.user_guid
) __t 
WHERE
	{sqlWhere}
ORDER BY
	creation_date desc";
            return await MySqlHelper.QueryByPageAsync<GetMerchantOrderDetailPageRequestDto, GetMerchantOrderDetailPageResponseDto, GetMerchantOrderDetailPageItemDto>(sql, request);
        }
        /// <summary>
        /// 获取用户购买商品次数
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="productGuid"></param>
        /// <returns></returns>
        public async Task<int> GetUserShopCountAsync(string userGuid, string productGuid)
        {
            var sql = $@"SELECT count(1) FROM t_mall_order_detail a LEFT JOIN t_mall_order c ON c.order_guid = a.order_guid
WHERE c.user_guid=@userGuid and a.product_guid=@productGuid";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync<int>(sql, new { userGuid, productGuid });
                return result;
            }
        }
    }
}

