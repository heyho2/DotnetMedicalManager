using Dapper;
using GD.Common.Base;
using GD.DataAccess;
using GD.Dtos.Consumer.Consumer;
using GD.Models.CommonEnum;
using GD.Models.Consumer;
using GD.Models.CrossTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GD.Dtos.Consumer.Collection;

namespace GD.Consumer
{
    /// <summary>
    /// 消息模块-我的关注业务等
    /// </summary>
    public class CollectionBiz
    {
        #region 查询
        /// <summary>
        /// 按ID查询
        /// </summary>
        /// <param name="collectionGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public CollectionModel GetModelByID(string collectionGuid, bool enable = true)
        {
            const string sql = "select * from t_consumer_collection  where collection_guid=@collectionGuid and enable=@enable";

            return MySqlHelper.SelectFirst<CollectionModel>(sql, new { collectionGuid, enable });
        }
        /// <summary>
        /// 获取用户对指定目标的收藏
        /// </summary>
        /// <param name="userGuid">用户guid</param>
        /// <param name="targetGuid">目标guid</param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public CollectionModel GetTheModelByUserId(string userGuid, string targetGuid, bool enable = true)
        {
            const string sql = "select * from t_consumer_collection  where target_guid=@targetGuid and user_guid=@userGuid and enable=@enable";

            return MySqlHelper.SelectFirst<CollectionModel>(sql, new { targetGuid, userGuid, enable });
        }

        /// <summary>
        /// 获取用户对指定目标的收藏记录（忽略enable状态）
        /// </summary>
        /// <param name="userGuid">用户guid</param>
        /// <param name="targetGuid">目标guid</param>
        /// <returns></returns>
        public CollectionModel GetOneModelByUserId(string userGuid, string targetGuid)
        {
            const string sql = "select * from t_consumer_collection  where target_guid=@targetGuid and user_guid=@userGuid";

            return MySqlHelper.SelectFirst<CollectionModel>(sql, new { targetGuid, userGuid });
        }

        /// <summary>
        /// 按targetGuid查询数量
        /// </summary>
        /// <param name="targetGuid">目标GUID</param>
        /// <param name="enable">是否启用</param>
        /// <returns></returns>
        public int GetListCountByTarget(string targetGuid, bool enable = true)
        {
            string sqlstring = "SELECT count(1) FROM t_consumer_collection where target_guid=@target_guid and enable=1";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("target_guid", targetGuid, System.Data.DbType.String);
            using (MySql.Data.MySqlClient.MySqlConnection conn = MySqlHelper.GetConnection())
            {
                return conn.QuerySingle<int?>(sqlstring, parameters) ?? 0;
            }
        }

        /// <summary>
        /// 按targetGuid查询
        /// </summary>
        /// <param name="targetGuid"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public List<CollectionModel> GetListByTarget(string targetGuid, bool enable = true)
        {
            const string sql = "select * from t_consumer_collection where target_guid=@targetGuid and enable=@enable ";
            return MySqlHelper.Select<CollectionModel>(sql, new { targetGuid, enable }).ToList();
        }

        /// <summary>
        /// 分页查询我的关注
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="pageDto"></param>
        /// <param name="enable"></param>
        /// <param name="targetType"></param>
        /// <returns>我关注的文章</returns>
        public async Task<GetMyArticlesResponseDto> GetMyArticleListAsync(GetMyArticlesRequestDto requestDto)
        {
            const string sql = @"SELECT
	                                art.article_guid,
	                                CONCAT( acc.base_path, acc.relative_path ) AS article_pic,
	                                art.title,
                                    art.source_type,
	                                dic.dic_guid,
	                                dic.config_name,
	                                art.creation_date,
                                    art.last_updated_date,
	                                u.user_name AS UserName,
	                                IFNULL(hot.like_count, 0) AS thumbup_num,
                                    IFNULL(hot.visit_count, 0) AS view_num
                                FROM
	                                t_consumer_collection AS col
	                                INNER JOIN t_utility_article AS art ON col.target_guid = art.article_guid 
	                                    AND art.`enable` = 1
	                                LEFT JOIN t_utility_richtext AS richT ON art.content_guid = richT.text_guid 
	                                    AND richT.`enable` = 1
	                                LEFT JOIN t_utility_accessory AS acc ON art.picture_guid = acc.accessory_guid 
	                                    AND acc.`enable` = 1
	                                LEFT JOIN t_manager_dictionary AS dic ON art.article_type_dic = dic.dic_guid 
	                                    AND dic.`enable` = 1
	                                LEFT JOIN t_utility_user AS u ON u.user_guid = art.author_guid 
	                                    AND u.`enable` = 1
	                                LEFT JOIN t_utility_hot hot ON hot.owner_guid = col.target_guid
                                        AND hot.`enable` = 1
                                WHERE
	                                col.user_guid = @UserId
	                                AND col.target_type = 'article'
	                                AND col.`enable` = 1 
                                ORDER BY
	                                art.creation_date DESC ";
            return await MySqlHelper.QueryByPageAsync<GetMyArticlesRequestDto, GetMyArticlesResponseDto, GetMyArticlesItemDto>(sql, requestDto);
        }

        /// <summary>
        /// 分页查询我的医生
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="pageDto"></param>
        /// <param name="enable"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public GetMyDoctorsResponseDto GetMyDoctorList(string userGuid, GetMyDoctorsRequestDto pageDto)
        {
            string sql = $@"SELECT * FROM(
	                            SELECT
		                            doc.doctor_guid,
		                            doc.hospital_guid,
                                    doc.adept_tags AS AdeptTags,
		                            CONCAT( acc.base_path, acc.relative_path ) AS doc_portrait,
		                            u.user_name,
		                            CONCAT( acc2.base_path, acc2.relative_path ) AS hospital_pic,
		                            hos.hos_name,
		                            of.office_name,
		                            dic.config_name,
		                            col.creation_date 
	                            FROM
		                            t_consumer_collection AS col
		                            INNER JOIN t_doctor AS doc ON col.target_guid = doc.doctor_guid
		                            LEFT JOIN t_utility_accessory AS acc ON doc.portrait_guid = acc.accessory_guid
		                            LEFT JOIN t_utility_user AS u ON doc.doctor_guid = u.user_guid
		                            LEFT JOIN t_doctor_hospital AS hos ON doc.hospital_guid = hos.hospital_guid
		                            LEFT JOIN ( SELECT base_path, relative_path, accessory_guid FROM t_utility_accessory ) AS acc2 ON hos.logo_guid = acc2.accessory_guid
		                            LEFT JOIN t_doctor_office AS of ON doc.office_guid = of.office_guid
		                            LEFT JOIN t_manager_dictionary AS dic ON doc.title_guid = dic.dic_guid 
	                            WHERE
		                            col.user_guid = '{userGuid}'
		                            AND col.target_type = '{CollectType.doctor.ToString()}'
		                            AND doc.ENABLE = 1 
                                    and col.`enable`=1
	                            ) AS T 
                            ORDER BY
	                            creation_date DESC";
            return MySqlHelper.QueryByPage<BasePageRequestDto, GetMyDoctorsResponseDto, GetMyDoctorsItemDto>(sql, pageDto);
        }

        /// <summary>
        /// 获取我关注的产品列表
        /// </summary>
        /// <param name="userGuid">用户id</param>
        /// <param name="pageDto">请求参数</param>
        /// <returns></returns>
        public GetMyProductsResponseDto GetMyProductsList(string userGuid, BasePageRequestDto pageDto, string platformType = "CloudDoctor")
        {
            var sql = $@"SELECT
	                                product.product_guid AS ProductGuid,
	                                product.product_name AS ProductName,
	                                CONCAT( acce.base_path, acce.relative_path ) AS ProductPicture,
	                                product.price AS ProductPrice,
	                                product.market_price AS MarketPrice,
	                                product.product_form,
	                                collection.last_updated_date AS CollectionDate 
                                FROM
	                                t_consumer_collection AS collection
	                                LEFT JOIN t_mall_product AS product ON collection.target_guid = product.product_guid
	                                left join t_merchant as mer on product.merchant_guid=mer.merchant_guid
	                                LEFT JOIN t_utility_accessory AS acce ON acce.accessory_guid = product.picture_guid 
	                                AND acce.`enable` = 1 
                                WHERE
	                                collection.`enable` = 1 
	                                AND product.`enable` = 1 
	                                AND product.on_sale = 1 
	                                AND mer.`enable` = 1 
	                                AND collection.target_type = 'product' 
	                                AND collection.user_guid = '{userGuid}'
                                    AND collection.platform_type='{platformType}'
                                ORDER BY
	                                collection.creation_date DESC";
            return MySqlHelper.QueryByPage<BasePageRequestDto, GetMyProductsResponseDto, GetMyProductsItemDto>(sql, pageDto);

        }

        /// <summary>
        /// 获取双美我收藏的产品列表
        /// </summary>
        /// <param name="userGuid">用户id</param>
        /// <param name="pageDto">请求参数</param>
        /// <returns></returns>
        public async Task<GetMyProductOfCosmetologyResponseDto> GetMyProductsListOfCosmetologyAsync(string userGuid, BasePageRequestDto pageDto)
        {
            var sql = $@"SELECT
	                        product.product_guid AS ProductGuid,
	                        product.product_name AS ProductName,
	                        CONCAT( acce.base_path, acce.relative_path ) AS ProductPicture,
	                        product.price AS ProductPrice,
                            product.market_price AS MarketPrice,
	                        collection.creation_date AS CollectionDate 
                        FROM
	                        t_consumer_collection AS collection
	                        LEFT JOIN t_mall_product AS product ON collection.target_guid = product.product_guid 
	                        AND product.`enable` = 1
	                        LEFT JOIN t_utility_accessory AS acce ON acce.accessory_guid = product.picture_guid 
	                        AND acce.`enable` = 1 
                        WHERE
	                        collection.`enable` = 1 
	                        AND collection.target_type = 'product' 
	                        AND collection.user_guid = '{userGuid}'
                            AND collection.platform_type BETWEEN 2 and 3 
                        ORDER BY
	                        collection.creation_date DESC";
            return await MySqlHelper.QueryByPageAsync<BasePageRequestDto, GetMyProductOfCosmetologyResponseDto, GetMyProductOfCosmetologyItemDto>(sql, pageDto);

        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<CollectionModel> GetPageList(string strWhere, int pageSize, int pageIndex, string orderBy)
        {
            //组装where
            return MySqlHelper.Select<CollectionModel>(pageSize, pageIndex, strWhere, orderBy).ToList();
        }
        #endregion

        /// <summary>
        /// 更新收藏记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateModel(CollectionModel model)
        {
            model.LastUpdatedDate = DateTime.Now;
            return model.Update() == 1;
        }

        /// <summary>
        /// 收藏/取消收藏目标
        /// </summary>
        /// <param name="targetGuid">目标guid</param>
        /// <param name="userGuid">用户guid</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="collectionState">收藏状态：收藏/取消收藏</param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public TargetCollectResponseDto CollectTargetResult(string targetGuid, string userGuid, string targetType, string platformType = "")
        {
            TargetCollectResponseDto result = new TargetCollectResponseDto();
            var tmpModel = GetOneModelByUserId(userGuid, targetGuid);
            //如果以前没有收藏过,则为第一次收藏
            result.first = tmpModel == null;
            if (tmpModel != null)
            {
                //老数据为成功状态时,返回取消
                if (tmpModel.Enable)
                {
                    result.collectionState = CollectionStateEnum.Cancel;
                }
                else
                {
                    result.collectionState = CollectionStateEnum.Establish;
                }
                tmpModel.Enable = !tmpModel.Enable;
                tmpModel.LastUpdatedDate = DateTime.Now;
                tmpModel.LastUpdatedBy = userGuid;
                result.result = tmpModel.Update() == 1;
            }
            else
            {
                result.collectionState = CollectionStateEnum.Establish;
                CollectionModel model = new CollectionModel
                {
                    CollectionGuid = Guid.NewGuid().ToString("N"),
                    UserGuid = userGuid,
                    TargetGuid = targetGuid,
                    TargetType = targetType,
                    CreatedBy = userGuid,
                    LastUpdatedBy = userGuid
                };
                if (!string.IsNullOrWhiteSpace(platformType))
                {
                    model.PlatformType = platformType;
                }
                result.result = !string.IsNullOrEmpty(model.Insert());
            }
            #region 更新hot表收藏量
            if (result.result)
            {
                UpdateCollectTotal(targetGuid, result.collectionState == CollectionStateEnum.Establish);
            }
            #endregion
            return result;
        }

        /// <summary>
        /// 更新hot表收藏量
        /// </summary>
        /// <param name="targetGuid"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        private bool UpdateCollectTotal(string targetGuid, bool isAdd = true)
        {
            try
            {
                var modifyStr = isAdd ? "+1" : "-1";
                using (var conn = MySqlHelper.GetConnection())
                {
                    var sql = $"insert into t_utility_hot (owner_guid,collect_count) values('{targetGuid}',1)  ON DUPLICATE KEY UPDATE collect_count=collect_count{modifyStr},last_updated_date=NOW();";
                    var result = conn.Execute(sql);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Common.Helper.Logger.Error($"更新hot表收藏量失败 at {nameof(UpdateCollectTotal)}:{Environment.NewLine} {ex.Message}");
                return false;
            }

        }

        /// <summary>
        /// 收藏/取消收藏目标
        /// </summary>
        /// <param name="targetGuid">目标guid</param>
        /// <param name="userGuid">用户guid</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="collectionState">收藏状态：收藏/取消收藏</param>
        /// <returns></returns>
        public bool CollectTarget(string targetGuid, string userGuid, string targetType, out string collectionState, string platformType = "")
        {
            TargetCollectResponseDto result = CollectTargetResult(targetGuid, userGuid, targetType, platformType);
            collectionState = result.collectionState.GetDescription();
            return result.result;
        }

        /// <summary>
        /// 获取收藏目标的用户列表
        /// </summary>
        /// <param name="targetGuid">目标guid</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">单页条数</param>
        /// <returns></returns>
        public async Task<List<GetTheUserListOfCollectionTargetResponseDto>> GetTheUserListOfCollectionTargetAsync(string targetGuid, string keyword, int pageIndex, int pageSize)
        {
            using (var conn = MySqlHelper.GetConnection())
            {
                var keywordWhere = "";
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    keywordWhere = $" and b.nick_name like '%{keyword}%'";
                }
                var sql = $@"SELECT
	                            b.user_guid as UserGuid,
	                            b.user_name as UserName,
	                            IFNULL(d.alias_name,b.nick_name) AS NickName,
	                            CONCAT( c.base_path, c.relative_path ) AS PortraitUrl 
                            FROM
	                            t_consumer_collection a
	                            INNER JOIN t_utility_user b ON a.user_guid = b.user_guid
	                            LEFT JOIN t_utility_accessory c ON b.portrait_guid = c.accessory_guid 
	                            AND c.`enable` = 1 
                                left join t_utility_alias d on d.user_guid=a.target_guid and d.target_guid=a.user_guid
                            WHERE
	                            a.target_guid = @targetGuid 
	                            AND a.`enable` = 1 
	                            AND b.`enable` = 1 
                                {keywordWhere}
                                order by NickName
	                            LIMIT @pageIndex,
	                            @pageSize";
                var result = await conn.QueryAsync<GetTheUserListOfCollectionTargetResponseDto>(sql, new { targetGuid, pageIndex = (pageIndex - 1) * pageSize, pageSize });

                return result?.ToList();
            }
        }

        /// <summary>
        /// 今日是否收藏或收藏已取消
        /// </summary>
        /// <returns></returns>
        public bool IsCollectTheTarget(string userID, string targetGuid, bool enable = true)
        {
            const string sqlWhere = "where user_guid=@userID and target_guid=@targetGuid and enable=@enable  and  to_days( creation_date ) = to_days( now( ) )  ";
            return MySqlHelper.Count<CollectionModel>(sqlWhere, new { userID, targetGuid, enable }) > 0;
        }


    }
}