using GD.DataAccess;
using GD.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using System.Linq;

namespace GD.Doctor
{
    /// <summary>
    /// 排班周期业务类
    /// </summary>
    public class DoctorScheduleCycleBiz : BizBase.BaseBiz<DoctorScheduleCycleModel>
    {
        /// <summary>
        /// 获取上个月上个月之后的周期记录
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <returns></returns>
        public async Task<List<DoctorScheduleCycleModel>> GetCycleListAfterLastMonthAsync(string hospitalGuid)
        {
            var sql = @"WHERE
	                        hospital_guid = @hospitalGuid and `enable`=1
	                        AND start_date >= concat( date_format( LAST_DAY( now() - INTERVAL 1 MONTH ), '%Y-%m-' ), '01' ) 
                        ORDER BY
	                        start_date;";
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.GetListAsync<DoctorScheduleCycleModel>(sql, new { hospitalGuid });
                return result.ToList();
            }
        }

        /// <summary>
        /// 通过医院guid和周期首次日期获取周期记录
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public async Task<DoctorScheduleCycleModel> GetHospitalCycleByStartDateAsync(string hospitalGuid, DateTime startDate)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = (await conn.GetListAsync<DoctorScheduleCycleModel>("where hospital_guid=@hospitalGuid and start_date=@startDate", new { hospitalGuid, startDate = startDate.Date })).FirstOrDefault();
                return result;
            }
        }
    }
}
