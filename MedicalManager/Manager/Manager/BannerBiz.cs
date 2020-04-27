using Dapper;
using GD.DataAccess;
using GD.Dtos.Banner;
using GD.Models.Manager;
using System.Linq;
using System.Threading.Tasks;

namespace GD.Manager.Manager
{
    /// <summary>
    /// Banner业务类
    /// </summary>
    public class BannerBiz : BaseBiz<BannerModel>
    {
        public async Task<GetBannerPageResponseDto> GetBannerPageAsync(GetBannerPageRequestDto request)
        {
            var whereSql = $@" 1=1";
            if (!string.IsNullOrWhiteSpace(request.Type))
            {
                whereSql = $"{whereSql} AND owner_guid=@Type";
            }
            else
            {
                using (var conn = MySqlHelper.GetConnection())
                {
                    var list = await conn.QueryAsync<string>("select dic_guid from t_manager_dictionary where parent_guid=@parentGuid and enable=1", new { parentGuid = DictionaryType.PageId });
                    var guids = list.Select(a => $"'{a}'").ToList();

                    var list2 = await conn.QueryAsync<string>("select hospital_guid from t_doctor_hospital where enable=1");
                    guids.AddRange(list2.Select(a => $"'{a}'"));

                    whereSql = $"{whereSql} AND owner_guid in ({string.Join(',', guids)})";
                }
            }
            var sortFields = new string[] { "sort".ToLower(), "creation_Date".ToLower() };
            var orderbySql = "sort DESC";
            if (!string.IsNullOrWhiteSpace(request.SortField))
            {
                orderbySql = $"{(sortFields.Contains(request.SortField.ToLower()) ? request.SortField : sortFields[0])} {(request.IsAscending ? "asc" : "desc")}";
            }
            var sql = $@"
SELECT * FROM(
    SELECT
	    A.*,
	    B.accessory_guid,
	    B.base_path,
	    B.relative_path,
CONCAT( B.base_path, B.relative_path ) as Picture
    FROM
	    t_manager_banner A
	    LEFT JOIN t_utility_accessory B ON A.picture_guid = B.accessory_guid 
) ____T
WHERE
	{whereSql}
ORDER BY 
	enable desc,{orderbySql}";
            return await MySqlHelper.QueryByPageAsync<GetBannerPageRequestDto, GetBannerPageResponseDto, GetBannerPageItemDto>(sql, request);
        }
    }
}
