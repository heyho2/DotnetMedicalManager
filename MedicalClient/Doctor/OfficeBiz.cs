using Dapper;
using GD.DataAccess;
using GD.Dtos.Admin.Hospital;
using GD.Dtos.Doctor.Office;
using GD.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Doctor
{
    /// <summary>
    /// 科室业务类
    /// </summary>
    public class OfficeBiz
    {
        /// <summary>
        /// 通过主键获取科室实例
        /// </summary>
        /// <param name="officeGuid">科室guid</param>
        /// <returns></returns>
        public OfficeModel GetModel(string officeGuid)
        {
            return MySqlHelper.GetModelById<OfficeModel>(officeGuid);
        }

        /// <summary>
        /// 获取下属科室
        /// </summary>
        /// <param name="hospitalGuid">医院guid</param>
        /// <param name="parentOfficeGuid">上级科室guid</param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public List<OfficeModel> GetHospitalOffice(string hospitalGuid, string parentOfficeGuid, bool enable = true)
        {
            var sql = "select * from t_doctor_office where hospital_guid=@hospitalGuid and parent_office_guid=@parentOfficeGuid and enable=@enable";
            if (string.IsNullOrWhiteSpace(parentOfficeGuid))
            {
                sql = "select * from t_doctor_office where hospital_guid=@hospitalGuid and parent_office_guid is null and enable=@enable";
            }
            var offices = MySqlHelper.Select<OfficeModel>(sql, new { hospitalGuid, parentOfficeGuid, enable });
            return offices?.ToList();
        }


        /// <summary>
        /// 获取下属科室Dto List
        /// </summary>
        /// <param name="hospitalGuid">医院guid</param>
        /// <param name="parentOfficeGuid">上级科室guid</param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task<List<OfficeDto>> GetHospitalOfficeDtoAsync(string hospitalGuid, string parentOfficeGuid, bool enable = true)
        {
            var whereSql = "and a.parent_office_guid=@parentOfficeGuid ";

            if (string.IsNullOrWhiteSpace(parentOfficeGuid))
            {
                whereSql = "and a.parent_office_guid is null";
            }
            var sql = $"select a.*,CONCAT(acc.base_path,acc.relative_path) as PictureUrl from t_doctor_office a LEFT JOIN t_utility_accessory acc ON a.picture_guid = acc.accessory_guid where a.hospital_guid=@hospitalGuid and a.enable=@enable {whereSql}";
            using (var conn = MySqlHelper.GetConnection())
            {
                var offices = await conn.QueryAsync<OfficeDto>(sql, new { hospitalGuid, parentOfficeGuid, enable });
                return offices?.ToList();

            }


        }

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

        /// <summary>
        /// 获取指定医院下二级科室
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <returns></returns>
        public async Task<IEnumerable<OfficeModel>> GetHospitalFirstLevelOfficesAsync(string hospitalGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var offices = await conn.QueryAsync<OfficeModel>(
                    @"select * from t_doctor_office 
                      where parent_office_guid 
                            in(select office_guid FROM `t_doctor_office` where hospital_guid = @hospitalGuid and parent_office_guid is null and `enable` = 1) 
                      and `enable` = 1", new
                    {
                        hospitalGuid
                    });

                return offices;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<OfficeModel>> GetAllAsync(bool? enable = null)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var whereSql = "where 1=1";
                if (enable != null)
                {
                    whereSql = $"{whereSql} and enable=@enable";
                }
                var result = await conn.GetListAsync<OfficeModel>($"{whereSql}", new { enable });

                return result;
            }
        }
        public async Task<IEnumerable<GetOfficeListItemDto>> GetOfficeListAsync(GetOfficeListRequestDto request)
        {
            var whereSql = "where 1=1";
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                whereSql = $"{whereSql} and (office_name=@Name or parentName=@Name)";
            }
            var sql = $@"
SELECT * FROM (
    SELECT DISTINCT
	    a.office_name,
	    a.sort,
	    a.ENABLE,
	    b.office_name AS parentName 
    FROM
	    t_doctor_office a
	    LEFT JOIN t_doctor_office b ON a.parent_office_guid = b.office_guid
)___T
{whereSql}
ORDER BY sort DESC";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetOfficeListItemDto>(sql, new { request.Name });
                return result;
            }
        }
        public async Task<SearchOfficeResponseDto> SearchOfficeAsync(SearchOfficeRequestDto request)
        {
            var whereSql = "where 1=1";
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

        public async Task<IEnumerable<DoctorModel>> GetDoctorByOfficeNameAsync(string officeName)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<DoctorModel>("where office_name=@officeName", new { officeName });
                return result;
            }
        }
        /// <summary>
        /// 获取医院详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OfficeModel> GetAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<OfficeModel>(id);
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(OfficeModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, OfficeModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }
        public async Task<bool> AddAsync(List<OfficeModel> models)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                foreach (var item in models)
                {
                    await conn.InsertAsync<string, OfficeModel>(item);
                }
                return true;
            });
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(OfficeModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
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
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.DeleteAsync<OfficeModel>(id);
                return result > 0;
            }
        }

        /// <summary>
        /// 获取科室本身和下属所有的科室
        /// </summary>
        /// <param name="officeGuid"></param>
        /// <param name="offices"></param>
        /// <returns></returns>
        public List<string> GetOfficeListByParentOfficeNode(string officeGuid, IEnumerable<OfficeModel> offices)
        {
            List<string> officeIds = new List<string>();
            officeIds.Add(officeGuid);
            GetOfficeByRecursion(officeGuid, offices, ref officeIds);
            return officeIds;
        }
        /// <summary>
        /// 科室递归
        /// </summary>
        /// <param name="officeGuid"></param>
        /// <param name="offices"></param>
        /// <param name="officeIds"></param>
        public void GetOfficeByRecursion(string officeGuid, IEnumerable<OfficeModel> offices, ref List<string> officeIds)
        {
            var tmpOffices = offices.Where(a => a.ParentOfficeGuid == officeGuid).Select(a => a.OfficeGuid).ToList();
            if (tmpOffices.Any())
            {
                officeIds.AddRange(tmpOffices);
            }
            else
            {
                return;
            }
            foreach (var item in tmpOffices)
            {
                GetOfficeByRecursion(item, offices, ref officeIds);
            }
        }

        /// <summary>
        /// 根据科室名称查询科室
        /// </summary>
        /// <param name="officeName"></param>
        /// <returns></returns>
        public async Task<List<OfficeModel>> GetModelByNameAsync(string officeName)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<OfficeModel>("where office_name=@officeName", new { officeName });
                return result?.ToList();
            }
        }

        /// <summary>
        /// 获取科室下存在医生的科室列表
        /// </summary>
        /// <param name="hospitalGuid">医院guid</param>
        /// <returns></returns>
        public async Task<List<OfficeModel>> GetOfficeListWithDoctorOfHospitalAsync(string hospitalGuid)
        {
            var sql = @"SELECT DISTINCT
	                        a.* 
                        FROM
	                        t_doctor_office a
	                        INNER JOIN t_doctor b ON a.office_guid = b.office_guid 
                            AND a.hospital_guid = b.hospital_guid
	                        AND a.`enable` = b.`enable` 
                        WHERE
	                        b.hospital_guid = @hospitalGuid 
	                        AND b.`enable` = 1 
                        ORDER BY
	                        a.office_name";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<OfficeModel>(sql, new { hospitalGuid });
                return result.ToList();
            }
        }


    }
}
