using Dapper;
using GD.DataAccess;
using GD.Dtos.Admin.Hospital;
using GD.Dtos.Doctor.Hospital;
using GD.Models.Doctor;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Doctor
{
    /// <summary>
    /// 医院模块业务类
    /// </summary>
    public class HospitalBiz
    {
        /// <summary>
        /// 获取所有医院List
        /// </summary>
        /// <param name="visibility">可选参数，医院是否可查询</param>
        /// <returns></returns>
        public List<HospitalModel> GetAllHospital(int visibility = 1, bool enable = true)
        {
            var sql = "select * from t_doctor_hospital where visibility=@visibility and enable=@enable";
            var hospitals = MySqlHelper.Select<HospitalModel>(sql, new { visibility, enable });
            return hospitals?.ToList();
        }
        /// <summary>
        /// 获取医院详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<HospitalModel>> GetAllAsync(int? visibility = 1, bool? enable = null)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var whereSql = "where 1=1";
                if (enable != null)
                {
                    whereSql = $"{whereSql} and enable=@enable";
                }
                if (visibility != null)
                {
                    whereSql = $"{whereSql} and visibility=@visibility";
                }
                var result = await conn.GetListAsync<HospitalModel>($"{whereSql}", new
                {
                    visibility,
                    enable
                });
                return result.ToList();
            }
        }
        /// <summary>
        /// 获取医院详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HospitalModel> GetAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<HospitalModel>(id);
            }
        }
        /// <summary>
        /// 获取医院详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HospitalModel GetModel(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return conn.Get<HospitalModel>(id);
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(HospitalModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, HospitalModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }
        public async Task<bool> AddAsync(HospitalModel model, RichtextModel richtextModel, List<OfficeModel> offices)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.InsertAsync<string, HospitalModel>(model);
                await conn.InsertAsync<string, RichtextModel>(richtextModel);
                foreach (var item in offices)
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
        public async Task<bool> UpdateAsync(HospitalModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }
        public async Task<bool> UpdateAsync(HospitalModel model, RichtextModel richtextModel, bool richtextIsAdd = false)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.UpdateAsync(model);
                if (richtextIsAdd)
                {
                    await conn.InsertAsync<string, RichtextModel>(richtextModel);
                }
                else
                {
                    await conn.UpdateAsync(richtextModel);

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
                var result = await conn.DeleteAsync<HospitalModel>(id);
                return result > 0;
            }
        }

        /// <summary>
        /// 获取拥有某种资质的医院
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="qualificationId"></param>
        /// <returns></returns>
        public List<HospitalModel> GetQualificationHospital(int pageIndex, int pageSize, string qualificationId)
        {
            int startIndex = (pageIndex - 1) * pageSize;
            var sql = @"SELECT
                            a.*
                        FROM
                            t_doctor_hospital a
                            INNER JOIN t_doctor_hsopital_qualification b ON a.hospital_guid = b.hospital_guid
                        WHERE
                            b.conf_guid = @conf_guid
                            AND a.ENABLE = TRUE
                            AND b.conf_value = TRUE
                            LIMIT @startIndex,
                            @pageSize";
            return MySqlHelper.Select<HospitalModel>(sql, new { conf_guid = qualificationId, startIndex, pageSize })?.ToList();
        }
        /// <summary>
        /// 搜索医院
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SearchHospitalResponseDto> SearchHospitalAsync(SearchHospitalRequestDto request)
        {
            var sqlWhere = $@"AND ENABLE = 1 AND visibility = 1";

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                sqlWhere = $"{sqlWhere} AND (hos_name like @Keyword  OR hos_tag like @Keyword OR hos_abstract like @Keyword)";
            }
            var sql = $@"
SELECT * FROM(
    SELECT
	    A.*,
	    CONCAT( B.base_path, B.relative_path ) AS LogoUrl 
    FROM
	    t_doctor_hospital A
	    LEFT JOIN t_utility_accessory B ON B.accessory_guid = A.logo_guid
)___T
WHERE
	1 = 1 {sqlWhere}
ORDER BY
	creation_date desc";
            request.Keyword = $"%{request.Keyword}%";

            return await MySqlHelper.QueryByPageAsync<SearchHospitalRequestDto, SearchHospitalResponseDto, SearchHospitalItemDto>(sql, request);
        }

        /// <summary>
        /// 搜索医生(后台管理)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetHospitalPageResponseDto> GetHospitalPageAsync(GetHospitalPageRequestDto request)
        {
            var sqlWhere = $@"1 = 1";
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                sqlWhere = $"{sqlWhere} AND (hos_name like @Name)";
            }
            if (request.RegisteredBeginDate != null)
            {
                request.RegisteredBeginDate = request.RegisteredBeginDate?.Date;
                sqlWhere = $"{sqlWhere} AND Registered_Date > @RegisteredBeginDate";
            }
            if (request.RegisteredEndDate != null)
            {
                request.RegisteredEndDate = request.RegisteredEndDate?.AddDays(1).Date;
                sqlWhere = $"{sqlWhere} AND Registered_Date < @RegisteredEndDate";
            }
            var sql = $@"
SELECT * FROM(
    SELECT
	    A.*,
	    CONCAT( B.base_path, B.relative_path ) AS LogoUrl 
    FROM
	    t_doctor_hospital A
	    LEFT JOIN t_utility_accessory B ON B.accessory_guid = A.logo_guid
)____T
WHERE
	 {sqlWhere}
ORDER BY
	creation_date";
            request.Name = $"%{request.Name}%";

            return await MySqlHelper.QueryByPageAsync<GetHospitalPageRequestDto, GetHospitalPageResponseDto, GetHospitalPageItemDto>(sql, request);
        }

        /// <summary>
        /// 分页获取医院列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetHospitalPageListResponseDto> GetHospitalPageListAsync(GetHospitalPageListRequestDto requestDto)
        {
            var sql = @"SELECT
	                        a.hospital_guid,
	                        a.hos_name,
	                        a.hos_abstract,
	                        a.hos_tag,
                            a.external_link,
	                        CONCAT(b.base_path,b.relative_path) as logo_url
                        FROM
	                        t_doctor_hospital a
	                        LEFT JOIN t_utility_accessory b ON a.logo_guid = b.accessory_guid
                        where a.`enable`=1 order by a.sort desc";
            var result= await MySqlHelper.QueryByPageAsync<GetHospitalPageListRequestDto, GetHospitalPageListResponseDto, GetHospitalPageListItemDto>(sql, requestDto);
            return result;
        }


        /// <summary>
        /// 通过医院账号获取医院model
        /// </summary>
        /// <param name="account">商户账号</param>
        /// <returns></returns>
        public async Task<HospitalModel> GetModelByAccountAsync(string account)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryFirstOrDefaultAsync<HospitalModel>("select * from t_doctor_hospital where account = @account and `enable` = 1", new { account }));
            }
        }
    }
}
