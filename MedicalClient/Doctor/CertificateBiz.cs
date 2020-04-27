using Dapper;
using GD.DataAccess;
using GD.Dtos.Admin.Certificate;
using GD.Dtos.Utility.Utility;
using GD.Models.Manager;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Utility
{
    /// <summary>
    /// 证书业务表
    /// </summary>
    public class CertificateBiz
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(CertificateModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, CertificateModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(CertificateModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }
        /// <summary>
        /// 根据id获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CertificateModel> GetAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<CertificateModel>(id);
            }
        }
        public async Task<CertificateModel> GetAsync(string dicGuid ,string ownerGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<CertificateModel>("where dic_Guid=@dicGuid and owner_Guid=@ownerGuid AND enable =1 ",new { dicGuid, ownerGuid })).FirstOrDefault();
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.DeleteAsync<CertificateModel>(id);
                return result > 0;
            }
        }

        public async Task<IEnumerable<GetCertificateListItemDto>> GetCertificateListAsync(string id, string type)
        {
            var sqlWhere = $"1=1 And Owner_Guid =@id And enable=1 ";

            if (!string.IsNullOrEmpty(type))
            {
                sqlWhere = $"{sqlWhere} and parent_guid='{type}'";
            }
            var sql = $@"
SELECT * FROM(
    SELECT
	    a.*,
	    b.config_name as CertificateName,
        b.parent_guid,
	    CONCAT( c.base_path, c.relative_path ) AS picture_url 
    FROM
	    t_utility_certificate a
	    LEFT JOIN t_manager_dictionary b ON a.dic_guid = b.dic_guid
	    LEFT JOIN t_utility_accessory c ON c.accessory_guid = a.picture_guid
)___T
Where {sqlWhere}
";
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryAsync<GetCertificateListItemDto>(sql, new { id });
            }
        }
        /// <summary>
        /// 获取证书项明细
        /// </summary>
        /// <param name="dicType">证书项类型</param>
        /// <param name="userGuid">用户guid</param>
        /// <returns></returns>
        public async Task<List<CertificateDetailDto>> GetCertificateDetailAsync(string dicType, string userGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.dic_guid AS DicGuid,
	                            a.config_name AS ConfigName,
                                c.accessory_guid AS AccessoryGuid,
                                b.certificate_guid as CertificateGuid,
	                            CONCAT( c.base_path, c.relative_path ) AS CertificateUrl 
                            FROM
	                            t_manager_dictionary a
	                            LEFT JOIN t_utility_certificate b ON a.dic_guid = b.dic_guid AND b.owner_guid = @userGuid and b.`enable`=1
	                            LEFT JOIN t_utility_accessory c ON b.picture_guid = c.accessory_guid and c.`enable`=1
                            WHERE
	                            a.parent_guid = @dicType 
	                            AND a.`enable` =1 ";
                var result = await conn.QueryAsync<CertificateDetailDto>(sql, new { dicType, userGuid });
                return result?.ToList();
            }
        }
    }
}
