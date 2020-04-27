using GD.DataAccess;
using GD.Models.Doctor;
using GD.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GD.Models.Manager;

namespace GD.Fushion.CompositeBiz
{
    /// <summary>
    /// 医生组合业务类
    /// </summary>
    public class DoctorCompositeBiz
    {
        /// <summary>
        /// 注册医生
        /// </summary>
        /// <param name="doctorModel">医生Model实例</param>
        /// <param name="certificates">证书项实例集合</param>
        /// <param name="accessories">附件实例集合</param>
        /// <param name="portraitAccessoryModel">一寸照附件</param>
        /// <returns></returns>
        public async Task<bool> RegisterDoctor(DoctorModel doctorModel, List<CertificateModel> certificates, List<AccessoryModel> accessories, UserModel userModel, bool isAdd = true)
        {
            if (doctorModel == null)
            {
                return false;
            }
            bool result = await MySqlHelper.TransactionAsync(async (conn, tran) =>
            {
                if (isAdd)
                {
                    //医生信息
                    if (string.IsNullOrWhiteSpace(await conn.InsertAsync<string, DoctorModel>(doctorModel)))
                    {
                        return false;
                    }
                }
                else
                {
                    //医生信息
                    if ((await conn.UpdateAsync(doctorModel)) != 1)
                    {
                        return false;
                    }
                }

                //用户信息
                if ((await conn.UpdateAsync(userModel)) != 1)
                {
                    return false;
                }
                //配置项证书附件信息
                if (certificates.Any() && accessories.Any())
                {
                    //附件
                    foreach (var accessory in accessories)
                    {
                        if ((await conn.UpdateAsync(accessory)) != 1)
                        {
                            return false;
                        }
                    }
                    if (!isAdd)
                    {
                        var sql = @"DELETE a 
                                    FROM
	                                    t_utility_certificate a
	                                    INNER JOIN t_manager_dictionary b ON a.dic_guid = b.dic_guid 
                                    WHERE
	                                    a.owner_guid = @doctorGuid 
	                                    AND b.parent_guid = @doctorDicConfig
                                        AND a.`enable`=1 AND b.`enable`=1 ";
                        await conn.ExecuteAsync(sql, new { doctorGuid = doctorModel.DoctorGuid, doctorDicConfig = DictionaryType.DoctorDicConfig });
                    }
                    //证书
                    foreach (var certificate in certificates)
                    {
                        if (string.IsNullOrEmpty(certificate.Insert(conn)))
                        {
                            return false;
                        }
                    }
                }

                return true;
            });
            return result;
        }
    }
}
