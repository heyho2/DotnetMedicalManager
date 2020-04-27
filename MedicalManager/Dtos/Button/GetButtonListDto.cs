using GD.Common.Base;

namespace GD.Dtos.Button
{
    /// <summary>
    /// 按钮
    /// </summary>
    public class GetButtonItemDto : BaseDto
    {
        ///<summary>
        ///GUID
        ///</summary>
        public string ButtonGuid { get; set; }

        ///<summary>
        ///菜单GUID
        ///</summary>
        public string MenuGuid { get; set; }

        ///<summary>
        ///菜单编码（CONTROLLER/ACTION）
        ///</summary>
        public string ButtonCode { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        public string ButtonName { get; set; }

        ///<summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
    }
}
