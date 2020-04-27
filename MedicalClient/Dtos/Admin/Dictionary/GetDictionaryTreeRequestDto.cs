using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Admin.Dictionary
{
    /// <summary>
    /// 字典树
    /// </summary>
    public class GetDictionaryTreeRequestDto : BaseDto
    {
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 元数据
        /// </summary>
        public string Matedata { get; set; }
    }
    /// <summary>
    /// 字典树
    /// </summary>
    public class GetDictionaryTreeResponseDto : BaseDto
    {
        ///<summary>
        ///系统字典GUID
        ///</summary>
        public string DicGuid { get; set; }

        ///<summary>
        ///字典类型CODE(如HABBIT表示生活习惯)
        ///</summary>
        public string TypeCode { get; set; }

        ///<summary>
        ///字段类型值（如生活习惯）
        ///</summary>
        public string TypeName { get; set; }

        ///<summary>
        ///配置项CODE，如SMOKE表示抽烟
        ///</summary>
        public string ConfigCode { get; set; }

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

        /// <summary>
        /// 创建时间，默认为系统当前时间   
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 使能标志，默认为 true
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 子集
        /// </summary>
        public List<GetDictionaryTreeResponseDto> Children { get; set; }
    }
}
