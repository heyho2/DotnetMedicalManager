using Dapper;
using GD.DataAccess;
using GD.Dtos.Hospital;
using GD.Models.Doctor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GD.Manager.Doctor
{
    /// <summary>
    /// 科室业务类
    /// </summary>
    public class OfficeBiz : BaseBiz<OfficeModel>
    {
        /// <summary>
        /// 获取医院下所有科室
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="parentOfficeGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<IEnumerable<OfficeModel>> GetHospitalOfficeAllAsync(string hospitalGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var offices = await conn.GetListAsync<OfficeModel>("where hospital_guid=@hospitalGuid and enable=@enable", new
                {
                    hospitalGuid,
                    enable = true
                });
                return offices;
            }
        }
        public async Task<IEnumerable<OfficeModel>> GetAllAsync(bool? enable = null)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var whereSql = "where 1=1";
                if (enable != null)
                {
                    whereSql = $"{whereSql} and enable=@enable";
                }
                var result = await conn.GetListAsync<OfficeModel>($"{whereSql} ORDER BY sort DESC,creation_date DESC  ", new { enable });
                return result;
            }
        }
        public async Task<IEnumerable<OfficeModel>> GetAllAsync2(bool? enable = null)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var hospitalGuid = conn.QueryFirstOrDefault<string>("SELECT hospital_guid FROM t_doctor_hospital LIMIT 1");
                var whereSql = $"where 1=1 and hospital_guid = '{hospitalGuid}'";
                if (enable != null)
                {
                    whereSql = $"{whereSql} and enable=@enable";
                }
                var result = await conn.GetListAsync<OfficeModel>($"{whereSql} ORDER BY sort DESC,creation_date DESC  ", new { enable });
                return result;
            }
        }
        public async Task<IEnumerable<GetOfficeTreeItemDto>> GetOfficeListAsync(GetOfficeListRequestDto request)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var hospitalGuid = conn.QueryFirstOrDefault<string>("SELECT hospital_guid FROM t_doctor_hospital LIMIT 1");
                var whereSql = $"where 1=1 ";
                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    whereSql = $"{whereSql} and (office_name=@Name or parentName=@Name)";
                }
                var sql = $@"
SELECT * FROM (
    SELECT 
	    a.office_name,
	    a.sort,
	    a.ENABLE,
        a.Picture_Guid,
	    b.office_name AS parentName 
    FROM
	    t_doctor_office a
	    LEFT JOIN t_doctor_office b ON a.parent_office_guid = b.office_guid 
    Where a.hospital_guid='{hospitalGuid}'
    ORDER BY a.sort DESC,a.creation_date DESC 
)___T
{whereSql}
";
                var result = await conn.QueryAsync<GetOfficeTreeItemDto>(sql, new { request.Name });
                return result;
            }
        }
        public async Task<SearchOfficeResponseDto> SearchOfficeAsync(SearchOfficeRequestDto request)
        {
            var whereSql = "where 1=1 AND ENABLE=1";
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                whereSql = $"{whereSql} and (office_name like @Keyword or parentName like @Keyword or hospital_name like @Keyword)";
            }
            var sql = $@"
SELECT * FROM (
    SELECT DISTINCT
	    a.office_guid,
	    a.office_name,
	    a.hospital_name,
	    a.sort,
	    a.ENABLE,
        a.creation_date,
	    b.office_name AS parentName
    FROM
	    t_doctor_office a
	    LEFT JOIN t_doctor_office b ON a.parent_office_guid = b.office_guid
)___T
{whereSql}
ORDER BY sort DESC";
            request.Keyword = $"%{request.Keyword}%";
            return await MySqlHelper.QueryByPageAsync<SearchOfficeRequestDto, SearchOfficeResponseDto, SearchOfficeItemDto>(sql, request);
        }
        public async Task<bool> UpdateAsync(List<OfficeModel> models, string oldOfficeName, string newOfficeName)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                foreach (var item in models)
                {
                    await conn.UpdateAsync(item);
                }
                if (oldOfficeName != newOfficeName)
                {
                    await conn.ExecuteAsync(@"update t_doctor set office_name=@newOfficeName where office_name=@oldOfficeName", new
                    {
                        oldOfficeName,
                        newOfficeName
                    });
                }
                return true;
            });
            return result;
        }
    }
}
