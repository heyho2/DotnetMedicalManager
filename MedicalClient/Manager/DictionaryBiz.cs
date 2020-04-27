using Dapper;
using GD.DataAccess;
using GD.Dtos.Admin.Dictionary;
using GD.Models.Manager;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager
{
    /// <summary>
    /// 字典表操作
    /// </summary>
    public class DictionaryBiz
    {
        /// <summary>
        /// 元数据
        /// </summary>
        //private static readonly IEnumerable<DictionaryModel> _dictionaryModels;
        static DictionaryBiz()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var _dictionaryModels = conn.GetList<DictionaryModel>("where enable=@enable", new { enable = true });
                foreach (var item in _dictionaryModels)
                {
                    SetRedis(item);
                }
            }
        }
        private static void SetRedis(DictionaryModel model)
        {
            var key = string.Format(Dtos.RedisKeys.Dictionary, model.DicGuid);
            if (model == null)
            {
                return;
            }
            RedisHelper.Database.HashSet(key, nameof(model.ConfigCode), model.ConfigCode);
            RedisHelper.Database.HashSet(key, nameof(model.ConfigName), model.ConfigName);
            RedisHelper.Database.HashSet(key, nameof(model.ValueType), model.ValueType);
            RedisHelper.Database.HashSet(key, nameof(model.ValueRange), model.ValueRange);
            RedisHelper.Database.HashSet(key, nameof(model.ParentGuid), model.ParentGuid);
        }
        /// <summary>
        /// 获取字典名称（redis缓存）
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string GetNameById(string guid)
        {
            var key = string.Format(Dtos.RedisKeys.Dictionary, guid);
            var name = RedisHelper.Database.HashGet(key, nameof(DictionaryModel.ConfigName));
            if (string.IsNullOrWhiteSpace(name))
            {
                using (var conn = MySqlHelper.GetConnection())
                {
                    var model = conn.Get<DictionaryModel>(guid);
                    SetRedis(model);
                    return model?.ConfigName;
                }
            }
            return name;
        }
        /// <summary>
        /// 获取字典code（redis缓存）
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string GetCodeById(string guid)
        {
            var key = string.Format(Dtos.RedisKeys.Dictionary, guid);
            var code = RedisHelper.Database.HashGet(key, nameof(DictionaryModel.ConfigCode));
            if (string.IsNullOrWhiteSpace(code))
            {
                using (var conn = MySqlHelper.GetConnection())
                {
                    var model = conn.Get<DictionaryModel>(guid);
                    SetRedis(model);
                    return model?.ConfigCode;
                }
            }
            return code;
        }


        /// <summary>
        /// 按ID查询
        /// </summary>
        /// <param name="dicGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public DictionaryModel GetModelById(string dicGuid, bool enable = true)
        {
            const string sql = "select * from t_manager_dictionary where dic_guid=@dicGuid and enable=@enable";

            return MySqlHelper.SelectFirst<DictionaryModel>(sql, new { dicGuid, enable });
        }

        /// <summary>
        /// 按typeCode查询
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public List<DictionaryModel> GetDictionaryListByCode(string typeCode, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"
                            SELECT
	                            `dic_guid`,
	                            `type_code`,
	                            `type_name`,
	                            `config_code`,
	                            `config_name`,
	                            `value_type`,
	                            json_extract( `value_range`, '$' ) AS value_range,
	                            `parent_guid`,
	                            `sort`,
	                            `created_by`,
	                            `creation_date`,
	                            `last_updated_by`,
	                            `last_updated_date`,
	                            `extension_field`,
	                            `org_guid`,
	                            `enable` 
                            FROM
	                            t_manager_dictionary 
                            WHERE
	                            ENABLE = @ENABLE 
	                            AND type_code = @typeCode";
                return conn.Query<DictionaryModel>(sql, new { enable, typeCode }).ToList();
            }
        }
        /// <summary>
        /// 根据父级Guid取列表
        /// </summary>
        /// <returns></returns>
        public List<DictionaryModel> GetListByParentGuid(string parentGuid, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"SELECT
                                        `dic_guid`,
                                        `type_code`,
                                        `type_name`,
                                        `config_code`,
                                        `config_name`,
                                        `value_type`,
                                         json_extract(`value_range`, '$') as value_range,
                                        `parent_guid`,
                                        `sort`,
                                        `created_by`,
                                        `creation_date`,
                                        `last_updated_by`,
                                        `last_updated_date`,
                                        `extension_field`,
                                        `org_guid`,
                                        `enable` 
                                    FROM
                                        t_manager_dictionary
                                    WHERE
                                        ENABLE = @enable
                                       AND parent_guid = @parentGuid order by sort";
                return  conn.Query<DictionaryModel>(sql, new { enable, parentGuid }).ToList();
            }
        }

        /// <summary>
        /// 根据父级guid数组获取列表
        /// </summary>
        /// <param name="parentGuids"></param>
        /// <returns></returns>
        public async Task<List<DictionaryModel>> GetListByParentGuidsAsync(string[] parentGuids, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<DictionaryModel>("where enable=@enable and parent_guid in @parentGuids", new { enable, parentGuids })).ToList();
            }
        }

        /// <summary>
        /// 根据父级guid获取列表
        /// </summary>
        /// <param name="parentGuid"></param>
        /// <returns></returns>
        public async Task<List<DictionaryModel>> GetListByParentGuidAsync(string parentGuid, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.QueryAsync<DictionaryModel>("select * from t_manager_dictionary where enable=@enable and parent_guid = @parentGuid", new { enable, parentGuid })).ToList();
            }
        }


        public List<DictionaryModel> GetDictionaryMatedata()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return conn.GetList<DictionaryModel>("where enable=1 and type_code=@matedata", new { matedata = "matedata" }).ToList();
            }
        }
        /// <summary>
        /// 获取经营范围/二级分类--根据ParentGuid
        /// 默认查经营范围 
        /// </summary>
        /// <returns></returns>
        public List<DictionaryModel> GetDictionaryClassifyListByGuid(string menuGuid = DictionaryType.BusinessScopeDic, bool enable = true)
        {
            const string sqlStr = @"SELECT
	                                            dic.`dic_guid`,
                                                dic.`type_code`,
                                                dic.`type_name`,
                                                dic.`config_code`,
                                                dic.`config_name`,
                                                dic.`value_type`,
                                                dic.`value_range`,
                                                dic.`parent_guid`,
                                                dic.`sort`,
                                                dic.`created_by`,
                                                dic.`creation_date`,
                                                dic.`last_updated_by`,
                                                dic.`last_updated_date`,
                                                CONCAT( acc.base_path, acc.relative_path ) AS `extension_field`,
                                                dic.`org_guid`,
                                                dic.`enable`
                                            FROM
	                                            t_manager_dictionary AS dic
	                                            LEFT JOIN t_utility_accessory AS acc ON dic.extension_field = acc.accessory_guid 
                                            WHERE
                                                 dic.`enable` = @enable
                                                AND  dic.parent_guid in (@menuGuid) GROUP BY dic_guid ";
            return MySqlHelper.Select<DictionaryModel>(sqlStr, new { enable, menuGuid }).ToList();
        }

        /// <summary>
        /// 获取商品一级分类
        /// </summary>
        /// <returns></returns>
        public List<DictionaryModel> GetDictionaryFirstClassifyList(string menuGuid = DictionaryType.BusinessScopeDic, bool enable = true)
        {
            const string sqlStr = @"SELECT
	                                                    dic.`dic_guid`,
	                                                    dic.`type_code`,
	                                                    dic.`type_name`,
	                                                    dic.`config_code`,
	                                                    dic.`config_name`,
	                                                    dic.`value_type`,
	                                                    dic.`value_range`,
	                                                    dic.`parent_guid`,
	                                                    dic.`sort`,
	                                                    dic.`created_by`,
	                                                    dic.`creation_date`,
	                                                    dic.`last_updated_by`,
	                                                    dic.`last_updated_date`,
	                                                    CONCAT( acc.base_path, acc.relative_path ) AS `extension_field`,
	                                                    dic.`org_guid`,
	                                                    dic.`enable` 
                                                    FROM
	                                                    t_manager_dictionary AS dic
	                                                    LEFT JOIN t_utility_accessory AS acc ON dic.extension_field = acc.accessory_guid
	                                                    LEFT JOIN t_manager_dictionary AS dic2 ON dic2.dic_guid = dic.parent_guid 
                                                    WHERE
                                                        dic.`enable` = @enable
	                                                    And  dic2.parent_guid = @menuGuid
                                                    GROUP BY
	                                                    dic_guid  ";
            return MySqlHelper.Select<DictionaryModel>(sqlStr, new { enable, menuGuid }).ToList();
        }

        /// <summary>
        /// 获取指定商户经验范围下实体类产品一级分类列表
        /// </summary>
        /// <param name="merchantGuid"></param>
        /// <param name="menuGuid"></param>
        /// <param name="serviceClassify"></param>
        /// <returns></returns>
        public List<DictionaryModel> GetDictionaryPhysicalFirstClassifies(string merchantGuid, string menuGuid = DictionaryType.BusinessScopeDic, string serviceClassify = DictionaryType.ServiceClassifyGuid)
        {
            const string sqlStr = @"SELECT
	                    dic.`dic_guid`,
	                    dic.`config_name` 
                    FROM
	                    t_manager_dictionary AS dic
	                    LEFT JOIN t_merchant_scope AS c ON dic.parent_guid = c.scope_dic_guid
	                    LEFT JOIN t_manager_dictionary AS dic2 ON dic2.dic_guid = dic.parent_guid 
                    WHERE
	                    dic.`enable` = 1 
	                    AND dic2.parent_guid = @menuGuid 
	                    AND c.merchant_guid = @merchantGuid
	                    AND dic.dic_guid <> @serviceClassify 
                    GROUP BY
	                    dic.dic_guid";

            return MySqlHelper.Select<DictionaryModel>(sqlStr, new { menuGuid, merchantGuid, serviceClassify }).ToList();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(DictionaryModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.InsertAsync<string, DictionaryModel>(model);
                return !string.IsNullOrWhiteSpace(result);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(DictionaryModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.UpdateAsync(model);
                return result > 0;
            }
        }
        /// <summary>
        /// 根据id获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DictionaryModel> GetAsync(string id, bool enable = true)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<DictionaryModel>("select * from t_manager_dictionary where dic_guid=@id and `enable`=@enable", new { id, enable });
            }
        }

        public async Task<DictionaryModel> GetModelAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.GetAsync<DictionaryModel>(id);
            }
        }

        public async Task<IEnumerable<DictionaryModel>> GetListByCodeAsync(string configCode)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<DictionaryModel>("where config_Code=@configCode", new { configCode }));
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.DeleteAsync<DictionaryModel>(id);
                return result > 0;
            }
        }

        public async Task<GetDictionaryPageResponseDto> GetDictionaryPageAsync(GetDictionaryPageRequestDto request)
        {
            var sqlWhere = $@"1 = 1";
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                sqlWhere = $"{sqlWhere} and (type_name like @Name ro config_name like @Name )";
            }
            var sqlOrderBy = "creation_date desc";
            var sql = $@"
SELECT * FROM
    t_manager_dictionary
 WHERE
	{sqlWhere}
ORDER BY
	{sqlOrderBy}";
            request.Name = $"%{request.Name}%";
            return await MySqlHelper.QueryByPageAsync<GetDictionaryPageRequestDto, GetDictionaryPageResponseDto, GetDictionaryPageItemDto>(sql, request);
        }
        public async Task<IEnumerable<DictionaryModel>> GetListAsync()
        {
            var sqlWhere = $@"1 = 1";
            var sqlOrderBy = "sort desc";
            using (var conn = MySqlHelper.GetConnection())
            {
                var sql = @"
SELECT
	dic_guid,
	type_code,
	type_name,
	config_code,
	config_name,
	value_type,
	json_extract( value_range, '$' ) AS value_range,
	parent_guid,
	sort,
	created_by,
	creation_date,
	last_updated_by,
	last_updated_date,
	extension_field,
	org_guid,
    ENABLE 
FROM
    t_manager_dictionary";
                return await conn.QueryAsync<DictionaryModel>(sql, $"where {sqlWhere} order by {sqlOrderBy}");
            }
        }
    }
}
