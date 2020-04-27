using GD.BizBase;
using GD.DataAccess;
using GD.Models.Consumer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GD.Dtos.Consumer.Consumer;
using System.Linq;

namespace GD.Consumer
{
    /// <summary>
    /// 就诊档案人业务类
    /// </summary>
    public class PatientMemberBiz : BaseBiz<PatientMemberModel>
    {
        /// <summary>
        /// 添加就诊人
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="patientMember"></param>
        /// <returns></returns>
        public async Task<bool> AddPatientMemberAsync(string userId, PatientMemberModel patientMember)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                //修改默认人
                if (patientMember.IsDefault)
                {
                    await conn.ExecuteAsync("UPDATE t_consumer_patient_member set is_default=FALSE where user_guid=@userId", new { userId });
                }
                //新增删除标记表
                await conn.InsertAsync<string, PatientMemberModel>(patientMember);
                return true;
            });
        }
        /// <summary>
        /// 修改就诊人
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="patientMember"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePatientMemberAsync(string userId, PatientMemberModel patientMember)
        {
            return await MySqlHelper.TransactionAsync(async (conn, trans) =>
            {
                //修改默认人
                if (patientMember.IsDefault)
                {
                    await conn.ExecuteAsync("UPDATE t_consumer_patient_member set is_default=FALSE where user_guid=@userId", new { userId });
                }
                //修改删除标记表
                await conn.UpdateAsync(patientMember);
                return true;
            });
        }
        /// <summary>
        /// 获取就诊人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<GetPatientResponDto>> GetPatientMemberModelAsync(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetPatientResponDto>("select * from t_consumer_patient_member where user_guid=@userId and `enable`= 1 order by is_default desc,creation_date desc", new { userId });
                return result?.ToList();
            }
        }
        /// <summary>
        /// 获取自己就诊人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<GetPatientResponDto> GetOwnPatientMemberModelAsync(string userId)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<GetPatientResponDto>("select * from t_consumer_patient_member where user_guid=@userId and `enable`= 1 and relationship='Own' ", new { userId });
                return result?.FirstOrDefault();
            }
        }
    }
}
