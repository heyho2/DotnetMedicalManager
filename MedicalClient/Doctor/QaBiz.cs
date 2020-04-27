using GD.DataAccess;
using GD.Models.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD.Doctor
{
    /// <summary>
    /// 医生问答业务类
    /// </summary>
    public class QaBiz
    {
        #region 查询
        /// <summary>
        /// 通过主键guid获取医生问答Model
        /// </summary>
        /// <param name="guid">主键guid</param>
        /// <returns></returns>
        public QaModel GetModel(string guid)
        {
            return MySqlHelper.GetModelById<QaModel>(guid);
        }

        /// <summary>
        /// 医生问答列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">单页条数</param>
        /// <param name="strWhere">条件 where condition=@condition</param>
        /// <param name="orderBy">col desc</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<QaModel> GetQaModels(int pageIndex, int pageSize, string strWhere, string orderBy, object parameters = null)
        {
            return MySqlHelper.Select<QaModel>(pageIndex, pageSize, strWhere, orderBy, parameters)?.ToList();
        }

        /// <summary>
        /// 通过医生guid获取医生问答列表
        /// </summary>
        /// <param name="doctorGuid">医生guid</param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public List<QaModel> GetQaModelsByDoctorGuid(string doctorGuid,bool enable=true)
        {
            return MySqlHelper.Select<QaModel>("select * from t_doctor_qa where doctor_guid=@doctorGuid and enable=@enable", new { doctorGuid, enable })?.ToList();
        } 
        #endregion
    }
}
