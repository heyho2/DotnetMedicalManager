using Dapper;
using GD.DataAccess;
using GD.Dtos.Common;
using GD.Dtos.Merchant;
using GD.Models.Manager;
using GD.Models.Merchant;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.Manager.Merchant
{
    /// <summary>
    /// 商户模块业务类
    /// </summary>
    public class MerchantBiz : BaseBiz<MerchantModel>
    {
        public async Task<int> RecordCountAsync()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.RecordCountAsync<MerchantModel>("where enable=@enable and Status=@Status", new { enable = true, Status = MerchantModel.StatusEnum.Approved.ToString() });
            }
        }
        public async Task<bool> AnyAccountAsync(string account)
        {
            var sql = $"SELECT COUNT(1) FROM t_merchant WHERE account=@account";
            using (var conn = MySqlHelper.GetConnection())
            {
                int result = await conn.QueryFirstOrDefaultAsync<int>(sql, new { account });
                return result > 0;
            }
        }
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
        C.Phone,
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
    a.scope_guid,
	b.config_name as name,
    b.dic_guid as scope_dic_guid,
    c.accessory_guid,
	CONCAT( c.base_path, c.relative_path ) AS certificateUrl 
FROM
	t_merchant_scope a
	LEFT JOIN t_manager_dictionary b ON a.scope_dic_guid = b.dic_guid
	LEFT JOIN t_utility_accessory c ON c.accessory_guid = a.picture_guid
WHERE 
	a.merchant_guid = @merchantGuid
ORDER BY a.creation_date asc
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

        public async Task<GetMerchantPageResponseDto> GetMerchantPageAsync(GetMerchantPageRequestDto request)
        {
            var sqlWhere = $@"1=1 ";
            sqlWhere = $"{sqlWhere} AND status = '{MerchantModel.StatusEnum.Approved.ToString()}'";

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                request.Name = $"{request.Name}%";
                sqlWhere = $"{sqlWhere} AND merchant_name like @Name";
            }
            if (!string.IsNullOrWhiteSpace(request.Scope))
            {
                request.Scope = $"%{request.Scope}%";
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

            var sql = $@"
SELECT * FROM(
    SELECT
	    A.merchant_name,
	    A.merchant_guid,
        A.hospital_guid,
	    A.account,
	    A.province,
	    A.city,
	    A.area,
	    A.street,
	    A.telephone,
	    A.STATUS,
	    A.creation_date,
        A.enable,
	    CONCAT( B.base_path, B.relative_path ) AS signatureUrl,
	    D.scope,
	    C.hos_name
    FROM
	    t_merchant A
	    LEFT JOIN t_utility_accessory B ON B.accessory_guid = A.signature_guid
	    LEFT JOIN t_doctor_hospital C ON A.hospital_guid = C.hospital_guid
	    LEFT JOIN (
	        SELECT
		        a.merchant_guid,
		        GROUP_CONCAT( b.config_name ) scope 
	        FROM
		        t_merchant_scope a
		        LEFT JOIN t_manager_dictionary b ON a.scope_dic_guid = b.dic_guid 
            WHERE b.enable=1
	        GROUP BY
	        a.merchant_guid 
	    ) D ON A.merchant_guid = D.merchant_guid
) __t 
WHERE
	{sqlWhere}
ORDER BY
	creation_date desc";

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
    where c.enable=1
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

        /// </summary>
        /// <param name="merchantModel">商户model实例</param>
        /// <param name="scopes">经营范围model实例集合</param>
        /// <param name="certificates">证书项实例集合</param>
        /// <param name="accessories">附件实例集合</param>
        /// <param name="bannsrs"></param>
        /// <returns></returns>
        public async Task<bool> RegisterMerchantAsync(MerchantModel merchantModel, List<ScopeModel> scopes, IEnumerable<CertificateModel> certificates, List<AccessoryModel> accessories)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                await conn.InsertAsync<string, MerchantModel>(merchantModel);
                //经营范围
                foreach (var scope in scopes)
                {
                    if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, ScopeModel>(scope)))
                    {
                        return false;
                    }
                }
                //商户配置项证书信息 & 配置项证书附件信息
                foreach (var accessory in accessories)
                {
                    if ((await conn.UpdateAsync(accessory)) != 1)
                    {
                        return false;
                    }
                }
                //证书
                foreach (var certificate in certificates)
                {
                    if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, CertificateModel>(certificate)))
                    {
                        return false;
                    }
                }
                return true;
            });
        }
        /// <summary>
        /// 修改商户
        /// </summary>
        /// <param name="merchantModel"></param>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<bool> UpdateMerchantAsync(MerchantModel merchantModel,IEnumerable<ScopeModel> scopes, IEnumerable<CertificateModel> certificates)
        {
            return await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                await conn.UpdateAsync(merchantModel);
                await conn.DeleteListAsync<ScopeModel>("where merchant_guid=@MerchantGuid", new { merchantModel.MerchantGuid });
                //经营范围
                foreach (var scope in scopes)
                {
                    if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, ScopeModel>(scope)))
                    {
                        return false;
                    }
                }
                await conn.DeleteListAsync<CertificateModel>("where owner_guid=@MerchantGuid", new { merchantModel.MerchantGuid });
                //证书
                foreach (var certificate in certificates)
                {
                    if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, CertificateModel>(certificate)))
                    {
                        return false;
                    }
                }
                return true;
            });
        }
        public async Task<IEnumerable<SelectItemDto>> GetMerchantSelectAsync()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                string sqlWhere = "where enable=1 and Status=@Status";
                return await conn.QueryAsync<SelectItemDto>($"select merchant_guid as guid,merchant_name as Name  from t_merchant {sqlWhere }", new
                {
                    enable = true,
                    Status = MerchantModel.StatusEnum.Approved.ToString(),
                });
            }
        }
    }
}

