using Dapper;
using GD.DataAccess;
using GD.Dtos.Doctor.Office;
using GD.Models.Doctor;
using GD.Models.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Doctor
{
    /// <summary>
    /// 医院科室
    /// </summary>
    public class DoctorOfficeBiz
    {
        /// <summary>
        /// 获取科室信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OfficeModel> GetModelAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<OfficeModel>(id);
            }
        }

        /// <summary>
        /// 获取医院科室信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isRecommend"></param>
        /// <returns></returns>
        public async Task<List<OfficeModel>> GetOfficeListByHospitalId(string hospitalId, bool? isRecommend = null)
        {
            string strWhere = "where hospital_guid=@hospital_guid and enable=@enable";
            if (isRecommend != null)
            {
                strWhere = $"{strWhere} and recommend=@recommend";
            }
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<OfficeModel>(strWhere, new
                {
                    hospital_guid = hospitalId,
                    enable = true,
                    recommend = isRecommend
                });
                return result.ToList();
            }
        }

        /// <summary>
        /// 获取医院科室信息列表（包含科室扩展信息）
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="isRecommend"></param>
        /// <returns></returns>
        public async Task<List<OfficeDto>> GetOfficeDtoListByHospitalId(string hospitalId, bool? isRecommend = null)
        {
            string sql = @"SELECT
	                                a.*,
	                                CONCAT(acc.base_path,acc.relative_path) as PictureUrl
                                FROM
	                                t_doctor_office a
	                                LEFT JOIN t_utility_accessory acc ON a.picture_guid = acc.accessory_guid
	                                where a.hospital_guid=@hospitalId and a.enable=@enable";
            if (isRecommend != null)
            {
                sql = $"{sql} and a.recommend=@recommend";
            }
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<OfficeDto>(sql, new
                {
                    hospital_guid = hospitalId,
                    enable = true,
                    recommend = isRecommend
                });
                return result.ToList();
            }
        }

        /// <summary>
        /// 科室分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetOfficeListPagingResponseDto> GetOfficeListPaging(GetOfficeListPagingRequestDto request)
        {
            var sqlWerre = $@" 1=1 AND A.ENABLE = @enable";
            if (!string.IsNullOrWhiteSpace(request.OfficeName))
            {
                sqlWerre = $"{sqlWerre} AND A.office_name like @OfficeName";
            }
            if (!string.IsNullOrWhiteSpace(request.HospitalName))
            {
                sqlWerre = $"{sqlWerre} AND C.hospital_name like @HospitalName";
            }
            var sql = $@"
SELECT
	A.*,
	B.accessory_guid,
	B.base_path,
	B.relative_path 
FROM
	t_doctor_office A
	LEFT JOIN t_utility_accessory B ON A.picture_guid = B.accessory_guid 
WHERE
	{sqlWerre}
ORDER BY
	A.creation_date DESC";
            var parameters = new
            {
                enable = true,
                HospitalName = $"%{request.HospitalName}%",
                OfficeName = $"%{request.OfficeName}%",
            };
            var pageSql = $"{sql} limit {(request.PageIndex - 1) * request.PageSize},{request.PageSize}";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetOfficeListPagingItemDto, AccessoryModel, GetOfficeListPagingItemDto>(pageSql, (a, b) =>
                {
                    a.PictureUrl = $"{b?.BasePath}{b?.RelativePath}";
                    return a;
                }, new { enable = true }, splitOn: "accessory_guid");
                var count = await conn.QueryFirstOrDefaultAsync<int>($@"SELECT COUNT(1) AS count FROM({sql}) AS T", parameters);
                return new GetOfficeListPagingResponseDto
                {
                    Total = count,
                    CurrentPage = result
                };
            }
        }

        /// <summary>
        /// 获取去重科室
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetDistinctOfficeResponseDto>> GetDistinctOfficeAsync()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT DISTINCT
	                            office_name,
	                            CONCAT(b.base_path,b.relative_path) office_picture 
                            FROM
	                            t_doctor_office a
	                            LEFT JOIN t_utility_accessory b ON a.picture_guid = b.accessory_guid 
                            WHERE
	                            a.`enable` = 1 
	                            AND parent_office_guid IS NULL;";
                var response = await conn.QueryAsync<GetDistinctOfficeResponseDto>(sql);
                return response?.ToList();
            }
        }
    }
}
