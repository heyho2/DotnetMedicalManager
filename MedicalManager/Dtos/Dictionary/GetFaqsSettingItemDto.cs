using GD.Common.Base;

namespace GD.Dtos.Dictionary
{
    /// <summary>
    /// 获取设置dto
    /// </summary>
    public class GetFaqsSettingItemDto : BaseDto
    {
        ///<summary>
        ///系统字典GUID
        ///</summary>
        public string DicGuid { get; set; }

        ///<summary>
        ///配置项名称
        ///</summary>
        public string ConfigName { get; set; }

        ///<summary>
        ///取值类型，默认为字符类型
        ///</summary>
        public string ValueType { get; set; }

        ///<summary>
        ///取值范围
        ///</summary>
        public string ValueRange { get; set; }

        ///<summary>
        ///父GUID
        ///</summary>
        public string ParentGuid { get; set; }

        ///<summary>
        ///排序(理论上相同TYPE_CODE的排序不能相同）
        ///</summary>
        public int Sort { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        public string ExtensionField { get; set; }

    }
    /// <summary>
    /// 保存问答设置
    /// </summary>
    public class SaveFaqsSettingRequestDto : BaseDto
    {
        ///<summary>
        ///系统字典GUID
        ///</summary>
        public string DicGuid { get; set; }


        /// <summary>
        /// 扩展字段
        /// </summary>
        public string ExtensionField { get; set; }
    }
}
