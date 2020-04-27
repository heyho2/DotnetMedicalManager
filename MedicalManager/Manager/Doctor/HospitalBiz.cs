using Dapper;
using GD.DataAccess;
using GD.Dtos.Hospital;
using GD.Dtos.Doctor.Hospital;
using GD.Models.Doctor;
using GD.Models.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GD.Dtos.Common;

namespace GD.Manager.Doctor
{
    /// <summary>
    /// 医院模块业务类
    /// </summary>
    public class HospitalBiz : BaseBiz<HospitalModel>
    {

        /// <summary>
        /// 获取医院详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<HospitalModel>> GetAllAsync(bool? enable = null)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var whereSql = "where 1=1";
                if (enable != null)
                {
                    whereSql = $"{whereSql} and enable=@enable";
                }
                var result = await conn.GetListAsync<HospitalModel>($"{whereSql}", new
                {
                    enable
                });
                return result.ToList();
            }
        }
        public async Task<IList<SelectItemDto>> GetHospitalSelectAsync(bool? enable = null)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var whereSql = "where 1=1";
                if (enable != null)
                {
                    whereSql = $"{whereSql} and enable=@enable";
                };
                var result = await conn.QueryAsync<SelectItemDto>($@"select hospital_guid as Guid, hos_name as Name from t_doctor_hospital {whereSql}", new
                {
                    enable
                });
                return result.ToList();
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
        public async Task<bool> Delete2Async(string id)
        {
            var result = await MySqlHelper.TransactionAsync(async (conn, t) =>
            {
                await conn.DeleteAsync<HospitalModel>(id);
                await conn.DeleteListAsync<OfficeModel>("where hospital_guid=@id", new { id });
                return true;
            });
            return result;

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
        /// 搜索医生
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

        public async Task<bool> AnyAccountAsync(string account)
        {
            var sql = $"SELECT COUNT(1) FROM t_doctor_hospital WHERE account=@account";
            using (var conn = MySqlHelper.GetConnection())
            {
                int result = await conn.QueryFirstOrDefaultAsync<int>(sql, new { account });
                return result > 0;
            }
        }
    }
}
