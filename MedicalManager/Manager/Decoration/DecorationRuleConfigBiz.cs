using GD.DataAccess;
using GD.Models.Decoration;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager.Decoration
{
    /// <summary>
    /// 装修规则配置业务类
    /// </summary>
    public class DecorationRuleConfigBiz : BaseBiz<DecorationRuleConfigModel>
    {
        /// <summary>
        /// 通过装修记录类型guid获取规则集合
        /// </summary>
        /// <param name="decorationGuid"></param>
        /// <returns></returns>
        public async Task<List<DecorationRuleModel>> GetRulesByClassificationAsync(string classificationGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
	                            a.* 
                            FROM
	                            t_decoration_rule a
	                            INNER JOIN t_decoration_rule_config b ON a.rule_guid = b.rule_guid 
	                            AND a.`enable` = b.`enable` 
                            WHERE
	                            b.classification_guid = @classificationGuid
	                            AND a.`enable` =1";
                var result = await conn.QueryAsync<DecorationRuleModel>(sql, new { classificationGuid });
                return result?.ToList();
            }
        }
    }
}
