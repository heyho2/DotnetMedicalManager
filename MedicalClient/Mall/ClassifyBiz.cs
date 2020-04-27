using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GD.DataAccess;
using GD.Dtos.Doctor.Doctor;
using GD.Dtos.Mall.Groupbuy;
using GD.Dtos.Mall.Mall;
using GD.Dtos.Manager.Banner;
using GD.Models.CommonEnum;
using GD.Models.Mall;
using GD.Models.Manager;

namespace GD.Mall
{
    /// <summary>
    /// 分类
    /// </summary>
    public class ClassifyBiz
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Add(ClassifyModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return !string.IsNullOrWhiteSpace(await conn.InsertAsync<string, ClassifyModel>(model));
            }
        }
        /// <summary>
        ///修改
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Update(ClassifyModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.UpdateAsync<ClassifyModel>(model) > 0;
            }
        }
        /// <summary>
        ///删除
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Delete(ClassifyModel model)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                return await conn.DeleteAsync<ClassifyModel>(model) > 0;
            }
        }


        /// <summary>
        /// 生美-获取明星首页商品列表
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ClassifyModel>> GetClassifyList(string classifyName, string platformType, int pageIndex = 1, int pageSize = 10, bool recommend = true, bool enable = true)
        {
            var sqlWhere = " 1=1 AND recommend=@recommend AND  `enable`=@enable ";
            if (!string.IsNullOrWhiteSpace(classifyName))
            {
                sqlWhere = $"{sqlWhere} AND  classify_name = @classifyName";
            }
            if (!string.IsNullOrWhiteSpace(platformType))
            {
                sqlWhere = $"{sqlWhere} AND  platform_type=@platformType";
            }
            var sql = $@"SELECT
	                                `classify_guid`,
	                                `classify_name`,
	                                `target_guid`,
	                                `recommend`,
	                                `sort`,
	                                `created_by`,
	                                `creation_date`,
	                                `last_updated_by`,
	                                `last_updated_date`,
	                                `platform_type`,
	                                `org_guid`,
	                                `enable` 
                                FROM
	                                t_mall_classify 
                                WHERE
	                                {sqlWhere}
	                                 ORDER BY sort DESC
	                                 LIMIT @pageIndex, @pageSize  ";
            var parameters = new
            {
                enable,
                recommend,
                classifyName,
                pageIndex = (pageIndex - 1) * pageSize,
                pageSize
            };
            using (var conn = MySqlHelper.GetConnection())
            {
                var result = await conn.QueryAsync<ClassifyModel>(sql, parameters);
                return result;
            }
        }
        /// <summary>
        /// 生美-获取明星医生推荐
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetStartDoctorListResponseDto> GetDoctorClassifyListAsync(GetStartDoctorListRequestDto requestDto)
        {
            string sql = @"SELECT
	                                    clas.`classify_guid` AS ClassifyGuid,
	                                    clas.`recommend` AS Recommend,
                                        clas.`target_guid` AS TargetGuid,
	                                    tbUser.user_name AS TargetName,
	                                    jobTitle.config_name AS TargetTitle,
	                                    CONCAT( picture.base_path, picture.relative_path ) AS TargetPicture
                                    FROM
	                                    t_mall_classify AS clas
	                                    LEFT JOIN t_doctor AS doc ON clas.target_guid = doc.doctor_guid
	                                    LEFT JOIN t_utility_user AS tbUser ON tbUser.user_guid = doc.doctor_guid
	                                    LEFT JOIN t_manager_dictionary AS jobTitle ON doc.title_guid = jobTitle.dic_guid 
	                                    LEFT JOIN t_utility_accessory AS picture ON accessory_guid = doc.portrait_guid 
                                    WHERE
	                                    1=1
	                                    AND clas.`classify_name` = @ClassifyName 
	                                    AND clas.`platform_type` = @PlatformType 
	                                    AND clas.`enable` = @Enable
                                    ORDER BY
	                                    clas.sort ";
            return await MySqlHelper.QueryByPageAsync<GetStartDoctorListRequestDto, GetStartDoctorListResponseDto, GetStartDDoctorListItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 生美-明星产品列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetStartProductItemListResponseDto> GetStartProductItemAsync(GetStartProductItemListRequestDto requestDto)
        {
            string sql = @"SELECT
	                                    clas.`classify_guid` AS ClassifyGuid,
	                                    clas.`target_guid` AS TargetGuid,
	                                    clas.`recommend` AS Recommend,
	                                    tbUser.user_name AS TargetName,
	                                    pro.price AS TargetPrice,
	                                    clas.relation_doctor_guid as DoctorGuid,
	                                    tbUser.user_name as DoctorName,
	                                    399 as SoldTotal
                                    FROM
	                                    t_mall_classify AS clas
	                                    LEFT JOIN t_doctor AS doc ON clas.relation_doctor_guid = doc.doctor_guid
	                                    LEFT JOIN t_utility_user AS tbUser ON tbUser.user_guid = clas.relation_doctor_guid
	                                    LEFT JOIN t_mall_product AS pro ON clas.classify_guid = pro.product_guid
                                    WHERE
	                                    1=1
	                                    AND clas.`classify_name` = @ClassifyName 
	                                    AND clas.`platform_type` = @PlatformType 
	                                    AND clas.`enable` = @Enable
                                    ORDER BY
	                                    clas.sort ";
            return await MySqlHelper.QueryByPageAsync<GetStartProductItemListRequestDto, GetStartProductItemListResponseDto, GetStartProductItemItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 生美-推荐产品-通用方法
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetRecommendProductResponseDto> GetRecommendProductListAsync(GetRecommendProductRequestDto requestDto)
        {
            string sql = @"SELECT
	                                    clas.`classify_guid` AS ClassifyGuid,
	                                    clas.`recommend` AS Recommend,
	                                    clas.target_guid AS TargetGuid,
	                                    pro.product_name AS TargetName,
	                                    CONCAT( acc.base_path, acc.relative_path ) AS TargetPicUrl 
                                    FROM
	                                    t_mall_classify AS clas
	                                    LEFT JOIN t_mall_product AS pro ON clas.target_guid = pro.product_guid
	                                    LEFT JOIN t_utility_accessory AS acc ON pro.picture_guid = acc.accessory_guid 
                                    WHERE
	                                    clas.`recommend` = @Recommend 
	                                    AND clas.`classify_name` = @ClassifyName 
	                                    AND clas.`platform_type` = @Platform_type 
	                                    AND clas.`enable` = @Enable 
                                    ORDER BY
	                                    clas.sort ";
            return await MySqlHelper.QueryByPageAsync<GetRecommendProductRequestDto, GetRecommendProductResponseDto, GetRecommendProductItem>(sql, requestDto);
        }


        /// <summary>
        /// 生美-超值优惠列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<GetSuperValuSuitProductListResponseDto> GetSuperValuListAsync(GetSuperValuSuitProductListRequestDto requestDto)
        {
            string sql = @"SELECT
	                                    clas.`classify_guid` AS ClassifyGuid,
	                                    clas.`target_guid` AS TargetGuid,
	                                    clas.`recommend` AS Recommend,
	                                    pro.product_name AS TargetName,
	                                    CONCAT( acc.base_path, acc.relative_path ) AS TargetPicUrl,
	                                    cam.campaign_name AS CampaignTitle,
	                                    399 AS SoldTotal 
                                    FROM
	                                    t_mall_classify AS clas
	                                    LEFT JOIN t_mall_product AS pro ON clas.target_guid = pro.product_guid
	                                    LEFT JOIN t_utility_accessory AS acc ON pro.picture_guid = acc.accessory_guid 
	                                    left join t_mall_campaign as cam on pro.product_guid=cam.product_guid
                                    WHERE
	                                    1 = 1 
	                                    AND clas.`classify_name` = @ClassifyName 
	                                    AND clas.`platform_type` = @PlatformType 
	                                    AND clas.`enable` = @ENABLE 
                                    ORDER BY
	                                    clas.sort";
            return await MySqlHelper.QueryByPageAsync<GetSuperValuSuitProductListRequestDto, GetSuperValuSuitProductListResponseDto, GetSuperValuSuitProductItem>(sql, requestDto);
        }

       

    }
}
