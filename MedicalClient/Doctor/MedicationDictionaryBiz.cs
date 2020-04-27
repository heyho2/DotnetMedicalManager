using GD.DataAccess;
using GD.Dtos.Doctor.Doctor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GD.Models.Doctor;
using Dapper;

namespace GD.Doctor
{
    /// <summary>
    /// 药品使用业务类
    /// </summary>
    public class MedicationDictionaryBiz : BizBase.BaseBiz<MedicationDictionaryModel>
    {
        public async Task<GetMedicationDictionaryPageListResponseDto> GetMedicationDictionaryPageListAsync(GetMedicationDictionaryRequestDto requestDto)
        {
            var sqlWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(requestDto.Name))
            {
                sqlWhere = $" and a.name like @Name";
                requestDto.Name = $"%{requestDto.Name}%";
            }
            var sql = $@"SELECT
                            a.medication_guid,
	                        a.name,
                            a.creation_date
                        FROM
	                        t_doctor_medication_dictionary a
	                        WHERE 1=1 {sqlWhere} and a.medication_type=@PrescriptionEnum and a.`enable`=1";
            return await MySqlHelper.QueryByPageAsync<GetMedicationDictionaryRequestDto, GetMedicationDictionaryPageListResponseDto, GetMedicationDictionaryItemDto>(sql, requestDto);
        }
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<MedicationDictionaryModel> GetModelByNameAsync(string name)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryFirstOrDefaultAsync<MedicationDictionaryModel>("select * from t_doctor_medication_dictionary where name = @name and `enable` = 1", new { name }));
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(List<string> ids)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.DeleteListAsync<MedicationDictionaryModel>("where medication_guid in @ids", new { ids });
                return result > 0;
            }
        }
    }
}
