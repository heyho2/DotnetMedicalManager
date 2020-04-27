using GD.DataAccess;
using GD.Dtos.Decoration;
using GD.Models.Decoration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GD.Manager.Decoration
{
    /// <summary>
    /// 装修记录业务类
    /// </summary>
    public class DecorationBiz : BaseBiz<DecorationModel>
    {
        public async Task<GetDecorationPageListResponseDto> GetDecorationPageListAsync(GetDecorationPageListRequestDto requestDto)
        {
            var sqlWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(requestDto.Keyword))
            {
                sqlWhere = "and a.decoration_name like @Keyword";
                requestDto.Keyword = $"%{requestDto.Keyword}%";
            }
            if (!string.IsNullOrWhiteSpace(requestDto.ClassificationGuid))
            {
                sqlWhere = $"{sqlWhere} and a.classification_guid = @ClassificationGuid";
            }
            var sql = $@"SELECT
	                        a.decoration_guid,
	                        a.decoration_name,
	                        b.classification_name,
	                        a.sort,
	                        a.creation_date ,
	                        a.enable 
                        FROM
	                        t_decoration a
	                        INNER JOIN t_decoration_classification b ON a.classification_guid = b.classification_guid 
                        WHERE
	                        a.`enable` = 1 { sqlWhere } 
                        ORDER BY
	                        a.sort,
	                        a.creation_date DESC";
            var result = await MySqlHelper.QueryByPageAsync<GetDecorationPageListRequestDto, GetDecorationPageListResponseDto, GetDecorationPageListItemDto>(sql, requestDto);
            return result;
        }

        /// <summary>
        /// 获取装修记录json内容
        /// </summary>
        /// <param name="decorationGuid"></param>
        /// <returns></returns>
        public async Task<string> GetDecorationContentAsync(string decorationGuid)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync<string>("select JSON_EXTRACT(content,'$') as content from t_decoration where decoration_guid=@decorationGuid", new { decorationGuid });
                return result;
            }
        }
    }
}
