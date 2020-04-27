using Dapper;
using GD.DataAccess;
using GD.Dtos.Dictionary;
using GD.Models.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager.Manager
{
    /// <summary>
    /// 字典表操作
    /// </summary>
    public class DictionaryBiz : BaseBiz<DictionaryModel>
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
        public List<DictionaryModel> GetDictionaryMatedata()
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return conn.GetList<DictionaryModel>("where enable=1 and type_code=@matedata", new { matedata = "matedata" }).ToList();
            }
        }
        public async Task<IEnumerable<DictionaryModel>> GetListByCodeAsync(string configCode)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<DictionaryModel>("where config_Code=@configCode", new { configCode }));
            }
        }
        public async Task<DictionaryModel> GetDictionaryByNameAsync(string name)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return (await conn.GetListAsync<DictionaryModel>("where config_name =@name", new { name })).FirstOrDefault();
            }
        }
        /// <summary>
        /// 判断 父子节点 相互冲突
        /// </summary>
        /// <param name="dicGuid"></param>
        /// <param name="parentGuid"></param>
        /// <returns></returns>
        public async Task<int> GetDictionaryChildrenAsync(string dicGuid, string parentGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<int>(@"
SELECT Count(1)
    FROM t_manager_dictionary 
where parent_guid = @dicGuid AND dic_guid = @parentGuid", new { dicGuid, parentGuid });
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




        public async Task<IEnumerable<DictionaryModel>> GetListAsync(string parentGuid = null, bool? enable = null)
        {
            var sqlWhere = $@"1 = 1";
            if (enable != null)
            {
                sqlWhere = $"{sqlWhere} AND enable = @enable";
            }
            if (!string.IsNullOrEmpty(parentGuid))
            {
                sqlWhere = $"{sqlWhere} AND parent_guid = @parentGuid";
            }
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
                sql = $"{sql} where {sqlWhere} order by {sqlOrderBy}";
                return await conn.QueryAsync<DictionaryModel>(sql, new { parentGuid, enable });
            }
        }
        /// <summary>
        /// 获取子集guid
        /// </summary>
        /// <param name="dictionarys"></param>
        /// <param name="pid"></param>
        /// <param name="lists"></param>
        public void GetAllSubsetGuids(IEnumerable<DictionaryModel> dictionarys, string pid, ref List<string> lists)
        {
            lists = lists ?? new List<string>();
            var pdictionarys = dictionarys.Where(a => a.ParentGuid == pid);
            lists.AddRange(pdictionarys.Select(a => a.DicGuid));
            foreach (var item in pdictionarys)
            {
                GetAllSubsetGuids(dictionarys, item.DicGuid, ref lists);
            }
        }
        /// <summary>
        /// 获取子集
        /// </summary>
        /// <returns></returns>
        public List<DictionaryModel> GetAllSubset(IEnumerable<DictionaryModel> organizations, string pid)
        {
            List<DictionaryModel> tdictionary = new List<DictionaryModel>();
            var os = organizations.Where(a => a.ParentGuid == pid);
            tdictionary.AddRange(os);
            foreach (var item in os)
            {
                tdictionary.AddRange(GetAllSubset(organizations, item.DicGuid));
            }
            return tdictionary;
        }
    }
}
