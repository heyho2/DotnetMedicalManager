using GD.Common.Base;

namespace GD.Dtos.Mall.Project
{
    /// <summary>
    /// 获取商户服务项目列表请求类
    /// </summary>
    public class GetMerchantProjectListRequestDto : BasePageRequestDto
    {
        /// <summary>
        /// 商户Id
        /// </summary>
        public string MerchantGuid { get; set; }
        /// <summary>
        ///分类
        /// </summary>
        public string ClassifyGuid { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
    }


    /// <summary>
    ///获取服务项目列表响应类
    /// </summary>
    public class GetMerchantProjectResponseDto : BasePageResponseDto<MerchantProjectItem>
    {

    }


    /// <summary>
    /// 服务项目项具体信息
    /// </summary>
    public class MerchantProjectItem : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string ProjectGuid { get; set; }
        /// <summary>
        /// 服务类型一级分类名称
        /// </summary>
        public string ClassifyName { get; set; }
        /// <summary>
        /// 服务项目名称
        /// </summary>
        public string ProjectName { get; set; }
        ///<summary>
        ///服务时长(分钟)
        ///</summary>
        public int OperationTime { get; set; }

        ///<summary>
        ///项目价格
        ///</summary>
        public decimal Price { get; set; }
    }
}
