using GD.Common.Base;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 获取班次模板分页数据响应Dto
    /// </summary>
    public class GetWorkShiftTemplatePageListResponseDto : BasePageResponseDto<GetWorkShiftTemplatePageListItemDto>
    {

    }

    /// <summary>
    /// 获取班次模板分页数据Item Dto
    /// </summary>
    public class GetWorkShiftTemplatePageListItemDto : BaseDto
    {
        /// <summary>
        /// 班次模板guid
        /// </summary>
        public string TemplateGuid { get; set; }

        /// <summary>
        /// 班次模板名称
        /// </summary>
        public string TemplateName { get; set; }
    }
}
