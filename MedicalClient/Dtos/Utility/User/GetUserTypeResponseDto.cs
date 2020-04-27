using GD.Common.Base;

namespace GD.Dtos.Utility.User
{
    ///<summary>
    ///系统字典
    ///</summary>
    public class GetUserTypeResponseDto : BaseDto
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
        public object ValueRange { get; set; }

        ///<summary>
        ///父GUID
        ///</summary>
        public string ParentGuid { get; set; }

        ///<summary>
        ///排序(理论上相同TYPE_CODE的排序不能相同）
        ///</summary>
        public int Sort { get; set; }
    }
}