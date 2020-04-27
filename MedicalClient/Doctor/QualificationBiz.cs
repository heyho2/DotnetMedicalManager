using GD.DataAccess;
using GD.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD.Doctor
{
    /// <summary>
    /// 医院资质表
    /// </summary>
    public class QualificationBiz
    {
        /// <summary>
        /// 获取指定医院的资质列表
        /// </summary>
        /// <param name="hospitalGuid">医院Guid</param>
        /// <param name="enable">可选参数，标示位</param>
        /// <returns></returns>
        public List<QualificationModel> GetQualificationByHospitalGuid(string hospitalGuid, bool enable = true)
        {
            var sql = "select * from t_doctor_hsopital_qualification where hospital_guid=@hospitalGuid and enable=@enable";
            var characterModels = MySqlHelper.Select<QualificationModel>(sql, new { hospitalGuid, enable });
            return characterModels?.ToList();
        }
        /// <summary>
        /// 判断医院是否有某项资质
        /// </summary>
        /// <param name="hospitalGuid">医院guid</param>
        /// <param name="ualificationDic">资质配置项guid</param>
        /// <param name="enable">可选参数，标示位</param>
        /// <returns>是否有资质</returns>
        public bool CheckHospitalQualification(string hospitalGuid, string ualificationDic, bool enable = true)
        {
            var sql = "where hospital_guid=@hospitalGuid and conf_guid=@ualificationDic and conf_value='1' and enable=@enable";
            var characterModels = MySqlHelper.Select<QualificationModel>(sql, new { hospitalGuid, ualificationDic, enable });
            return MySqlHelper.Count<QualificationModel>(sql, new { hospitalGuid, ualificationDic, enable }) > 0;
        }

        /// <summary>
        /// 通过条件获取资质列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<QualificationModel> GetQualifications(string condition, object parameters = null)
        {
            var sql = "select * from t_doctor_hsopital_qualification " + condition;
            var characterModels = MySqlHelper.Select<QualificationModel>(sql, parameters);
            return characterModels?.ToList();
        }
    }
}
