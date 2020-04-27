namespace GD.Models.Manager
{
    /// <summary>
    /// 字典类型
    /// </summary>
    public struct DictionaryType
    {
        /// <summary>
        /// 数据字典类型Guid
        /// </summary>
        public const string DicTypeConfig = "t_manager_dictionary_00000000000";

        /// <summary>
        /// 医生证书配置字典原数据Guid
        /// </summary>
        public const string DoctorDicConfig = "t_manager_dictionary_00000000005";

        /// <summary>
        /// 商户证书配置字典原数据Guid
        /// </summary>
        public const string MerchantDicConfig = "t_manager_dictionary_00000000006";

        /// <summary>
        /// 经营范围元数据字典Guid
        /// </summary>
        public const string BusinessScopeDic = "t_manager_dictionary_00000000003";

        /// <summary>
        /// 用户生活习惯/个人资料
        /// </summary>
        public const string UserPersonalInfo = "t_manager_dictionary_00000000001";

        #region 用户类型
        /// <summary>
        /// 用户类型配置Guid
        /// </summary>
        public const string UserTypeConfig = "t_manager_dictionary_00000000007";

        /// <summary>
        /// 用户类型--消费者
        /// </summary>
        public const string Consumer_UserType = "0edcdc64f79111e885ed00e04c01c721";
        #endregion


        /// <summary>
        /// 医生职称
        /// </summary>
        public const string DoctorTitle = "t_manager_dictionary_00000000009";

        /// <summary>
        /// 后台菜单
        /// </summary>
        public const string BackgroundMenu = "t_manager_dictionary_00000000010";

        #region 页面ID
        /// <summary>
        /// 页面ID元数据Guid
        /// </summary>
        public const string PageId = "t_manager_dictionary_00000000008";

        /// <summary>
        /// 智慧云医公众号首页ID
        /// </summary>
        public const string WechatOfficialAccountHome = "a37390bbf95111e8922700e04c01c721";

        /// <summary>
        /// 问医热点页面ID
        /// </summary>
        public const string AskedDoctorHot = "1a81c4a1f95011e8922700e04c01c721";

        /// <summary>
        /// 问医讲堂页面ID
        /// </summary>
        public const string AskedDoctorLectureHall = "5d0faebff95011e8922700e04c01c721";

        /// <summary>
        /// 健康管理页面ID
        /// </summary>
        public const string HealthManagementPage = "0c09d8361eea11e993b100163e0c4296";
        #endregion

        /// <summary>
        /// 常备药品
        /// </summary>
        public const string RegularMedicine = "t_manager_dictionary_00000000012";

        #region 文章类型
        /// <summary>
        /// 文章类型
        /// </summary>
        public const string ArticleTypeConfig = "t_manager_dictionary_00000000004";

        /// <summary>
        /// 课程文章类型
        /// </summary>
        public const string CourseArticle = "t_manager_dictionary_00000000011";

        /// <summary>
        /// 文章类型-职业病常识
        /// </summary>
        public const string OccupationalDiseaseKnowledge = "18176367f82d11e885ed00e04c01c721";

        /// <summary>
        /// 文章类型-职业病预防
        /// </summary>
        public const string OccupationalDiseasePrevent = "682772b1f82d11e885ed00e04c01c721";
        #endregion

        #region 医院资质

        /// <summary>
        /// 医院资质
        /// </summary>
        public const string HospitalQualification = "t_manager_dictionary_00000000002";


        /// <summary>
        /// 职业病资质
        /// </summary>
        public const string OccupationalDiseaseQualification = "e00a5a27f85b11e885ed00e04c01c721";

        #endregion

        #region 医美

        /// <summary>
        ///医美 产品分类 元数据字典Guid
        /// </summary>
        public const string CosmeticMedicineType = " ";
        /// <summary>
        ///医美-首页页面Guid
        /// </summary>
        public const string CosmeticMedicineHomePageGuid = " ";
        #endregion

        #region 生美
        /// <summary>
        /// 生美-产品分类 元数据字典Guid
        /// </summary>
        public const string LivingBeautyProductType = " ";
        /// <summary>
        ///生美-首页页面Guid
        /// </summary>
        public const string LivingBeautyProductHomePageGuid = " ";
        /// <summary>
        ///生美-明星产品BannerGuid
        /// </summary>
        public const string LivingBeautyStartProductHomePageGuid = " ";
        /// <summary>
        ///生美-超值优惠BannerGuid
        /// </summary>
        public const string LivingBeautySuperValuHomePageGuid = " ";
        /// <summary>
        ///生美-拼团聚划算BannerGuid
        /// </summary>
        public const string LivingBeautyGroupBuyHomePageGuid = " ";
        #endregion

        #region 服务类型
        /// <summary>
        /// 服务类型一级分类
        /// </summary>
        public const string ServiceClassifyGuid = "t_manager_dictionary_service_classify_01";
        #endregion


        #region MyRegion
        /// <summary>
        /// 问答设置
        /// </summary>
        public const string FaqsSetting = "t_manager_dictionary_00000000014";
        /// <summary>
        /// 问答设置 悬赏最少金额
        /// </summary>
        public const string FaqsSettingMixFee = "t_manager_dictionary_00000000015";
        /// <summary>
        /// 问答设置 悬赏最少积分
        /// </summary>
        public const string FaqsSettingMixIntegral = "t_manager_dictionary_00000000016";

        #endregion

        /// <summary>
        /// 平台客服电话
        /// </summary>
        public const string PlatformServiceTel = "t_manager_dictionary_00000000018";

        /// <summary>
        /// 关注云医公众号关自动回复消息内容
        /// </summary>
        public const string SubscribeWelcome = "t_manager_dictionary_00000000019";

        /// <summary>
        /// 关注云医公众号（若为医生端则推送医生个人中心链接地址）
        /// </summary>
        public const string DoctorPersonalCenterLink = "t_manager_dictionary_00000000023";


        #region 诊所参数

        /// <summary>
        /// 药品用法
        /// </summary>
        public const string DrugUsage = "t_manager_dictionary_00000000021";

        /// <summary>
        /// 药品频度单位
        /// </summary>
        public const string DrugFrequency = "t_manager_dictionary_00000000022";
        #endregion
    }
}
