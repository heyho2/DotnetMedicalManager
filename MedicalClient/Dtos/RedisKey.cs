namespace GD.Dtos
{
    /// <summary>
    /// redis key
    /// 标注好redis 的数据类型
    /// 标注好并接参数
    /// </summary>
    public static class RedisKeys
    {
        /// <summary>
        /// 用户搜索记录（Sorted Set）
        /// {0}用户id
        /// </summary>
        public const string UserSearchHistory = "SearchHistory:User:{0}";
        /// <summary>
        /// 搜索词（Hash）
        /// </summary>
        public const string HotSearchHistoryDate = nameof(HotSearchHistoryDate);
        /// <summary>
        /// 字典（）
        /// {0}字典ID
        /// </summary>
        public const string Dictionary = "Dictionary:{0}";

        /// <summary>
        /// 生美首页搜索
        /// {0}
        /// </summary>
        public const string LivingBeautyUserSearchRecord = "LivingBeautyUser:SearchRecord:{0}";


    }
}
