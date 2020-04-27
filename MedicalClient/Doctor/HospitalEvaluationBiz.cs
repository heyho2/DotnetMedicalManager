using Dapper;
using GD.DataAccess;
using GD.Dtos.Doctor.Hospital;
using GD.Models.Doctor;
using System.Threading.Tasks;

namespace GD.Doctor
{
    public class HospitalEvaluationBiz : BizBase.BaseBiz<HospitalEvaluationModel>
    {
        public async Task<GetHospitalEvaluvationResponseDto> GetEvaluationAsync(string evaluationGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<GetHospitalEvaluvationResponseDto>
                    (@"SELECT 
                        e.user_guid as UserGuid,
	                    e.office_guid as OfficeGuid,
	                    o.office_name as OfficeName,
	                    e.evaluation_tag as Tags,
	                    e.anonymous as Anonymous,
	                    e.condition_detail as ConditionDetail,
	                    e.score as Score
                    FROM `t_doctor_hospital_evaluation` as e
		                    INNER JOIN t_doctor_office as o ON e.office_guid = o.office_guid
                    WHERE e.evaluation_guid = @evaluationGuid
                    LIMIT 1", new { evaluationGuid });
            }
        }
    }
}
