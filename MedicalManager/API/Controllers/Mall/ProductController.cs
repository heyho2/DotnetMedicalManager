using GD.Common;
using GD.Dtos.Mall.Mall;
using GD.Manager.Mall;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GD.API.Controllers.Mall
{
    /// <summary>
    /// 商品控制器
    /// </summary>
    public class ProductController : MallBaseController
    {
        /// <summary>
        /// 搜索商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<SearchProductResponseDto>))]
        public async Task<IActionResult> SearchProductAsync([FromBody]SearchProductRequestDto request)
        {
            var response = await new ProductBiz().SearchProductAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 商品中心
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<ProductCenterResponseDto>))]
        public async Task<IActionResult> ProductCenterAsync([FromQuery]ProductCenterRequestDto request)
        {
            var response = await new ProductBiz().ProductCenterAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 修改是上下架
        /// </summary>
        /// <param name="productGuid"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateOnlieCategory(string productGuid)
        {
            if (string.IsNullOrEmpty(productGuid))
            {
                return Failed(ErrorCode.UserData, "参数不正确");
            }

            var productBiz = new ProductBiz();

            var model = await productBiz.GetAsync(productGuid);

            if (model is null)
            {
                return Failed(ErrorCode.UserData, "商品不存在");
            }

            model.PlatformOnSale = !model.PlatformOnSale;
            //如何后台启用商品则不修改商户端状态
            if (!model.PlatformOnSale)
            {
                model.OnSale = model.PlatformOnSale;
            }
            var result = await productBiz.UpdateAsync(model);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新失败！");
        }
        /// <summary>
        /// 修改推荐到首页
        /// </summary>
        /// <param name="productGuid"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateRecommend(string productGuid)
        {
            if (string.IsNullOrEmpty(productGuid))
            {
                return Failed(ErrorCode.UserData, "参数不正确");
            }

            var productBiz = new ProductBiz();

            var model = await productBiz.GetAsync(productGuid);

            if (model is null)
            {
                return Failed(ErrorCode.UserData, "商品不存在");
            }

            model.Recommend = !model.Recommend;

            var result = await productBiz.UpdateAsync(model);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新失败！");
        }
        /// <summary>
        /// 修改推荐排序
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateRecommendSort([FromBody]UpdateProductSortRequestDto requestDto)
        {
            if (string.IsNullOrEmpty(requestDto.ProductGuid))
            {
                return Failed(ErrorCode.UserData, "参数不正确");
            }

            var productBiz = new ProductBiz();

            var model = await productBiz.GetAsync(requestDto.ProductGuid);

            if (model is null)
            {
                return Failed(ErrorCode.UserData, "商品不存在");
            }

            model.Sort = requestDto.Sort;

            var result = await productBiz.UpdateAsync(model);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新失败！");
        }
    }
}
