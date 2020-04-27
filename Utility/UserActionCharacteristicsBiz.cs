using System;
using System.Threading.Tasks;
using Dapper;
using GD.Common.EnumDefine;
using GD.DataAccess;
using GD.Models.Utility;

namespace GD.Utility
{
    /// <summary>
    /// 用户操作逻辑处理类
    /// </summary>
    public class UserActionCharacteristicsBiz
    {
        /// <summary>
        /// 获取操作model
        /// </summary>
        /// <returns></returns>
        public async Task<UserActionCharacteristics> GetOperateModel(UserType userType, ActionEnum operateType, ActionCharacteristicsEnum actionCharacteristics = ActionCharacteristicsEnum.Action)
        {
            string sqlstring = $@"select 
                                    ua.user_action_guid,
                                    ua.user_type_guid,
                                    ua.action_guid,
                                    ac.action_characteristics_code,
                                    ac.action_characteristics_name,
                                    ua.enable,
                                    ua.created_by,
                                    ua.creation_date,
                                    ua.last_updated_by,
                                    ua.last_updated_date
                                from t_utility_user_action as ua  
                                     left join t_utility_action_characteristics as ac on ac.action_characteristics_guid=ua.action_guid
                                where ac.action_characteristics_guid=@action_characteristics_guid 
                                      and ac.action_characteristics_type=@action_characteristics_type 
                                      and ua.user_type_guid=@user_type_guid";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("action_characteristics_guid", operateType.GetDescription(), System.Data.DbType.String);
            parameters.Add("action_characteristics_type", actionCharacteristics.ToString(), System.Data.DbType.String);
            parameters.Add("user_type_guid", userType.ToString(), System.Data.DbType.String);

            using (MySql.Data.MySqlClient.MySqlConnection conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstAsync<UserActionCharacteristics>(sqlstring, parameters);
            }
        }
    }
}
