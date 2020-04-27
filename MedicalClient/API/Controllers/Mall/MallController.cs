using GD.API.Code;
using GD.AppSettings;
using GD.Common;
using GD.Common.Helper;
using GD.Consumer;
using GD.Dtos.CommonEnum;
using GD.Dtos.Consumer.Consumer;
using GD.Dtos.Mall.Category;
using GD.Dtos.Mall.LivingBeautyMall;
using GD.Dtos.Mall.Mall;
using GD.Dtos.Mall.Product;
using GD.Dtos.Mall.WeiXinPay;
using GD.Dtos.MallPay.ControllerApi;
using GD.Dtos.MallPay.FangDiInterface;
using GD.FAQs;
using GD.Mall;
using GD.Manager;
using GD.Merchant;
using GD.Models.CommonEnum;
using GD.Models.Consumer;
using GD.Models.Mall;
using GD.Models.Manager;
using GD.Models.Merchant;
using GD.Models.Utility;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static GD.Models.Consumer.OrderProductCommentModel;
using static GD.Models.Mall.OrderModel;
using static GD.Models.Mall.ProductModel;

namespace GD.API.Controllers.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 云购商城
    /// </summary>
    public class MallController : MallBaseController
    {
        /// <summary>
        /// 获取商品一级分类
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<ClassifyResponseDto>>))]
        public IActionResult GetFirstClassifyList()
        {
            var dicBiz = new DictionaryBiz();
            var dicList = dicBiz.GetDictionaryFirstClassifyList();
            if (dicList == null)
            {
                return Failed(ErrorCode.Empty, "获取不到数据！");
            }
            //组装的dto
            var crdList = new List<ClassifyResponseDto>();
            foreach (var model in dicList)
            {
                var dtoModel = new ClassifyResponseDto
                {
                    ClassifyGuid = model.DicGuid,
                    ClassifyName = model.ConfigName,
                    ClassifyPic = model.ExtensionField,
                    Sort = model.Sort
                };
                crdList.Add(dtoModel);
            }
            return Success(crdList);
        }

        /// <summary>
        /// 获取经营范围/二级分类--根据ParentGuid
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<SubClassifyResponseDto>>))]
        public IActionResult GetSubClassifyList(GetSubClassifyListRequestDto requestDto)
        {
            var dicBiz = new DictionaryBiz();
            var dicList = string.IsNullOrWhiteSpace(requestDto.ParentGuid) ? dicBiz.GetDictionaryClassifyListByGuid() : dicBiz.GetDictionaryClassifyListByGuid(requestDto.ParentGuid);
            //组装的dto
            var crdList = new List<SubClassifyResponseDto>();
            if (dicList == null || dicList.Count < 1) return Success(crdList);

            foreach (var model in dicList)
            {
                var dtoModel = new SubClassifyResponseDto
                {
                    FirstClassifyGuid = model.ParentGuid,
                    SubClassifyGuid = model.DicGuid,
                    SubClassifyName = model.ConfigName,
                    SubClassifyPic = model.ExtensionField,
                    Sort = model.Sort
                };
                crdList.Add(dtoModel);
            }
            return Success(crdList.OrderByDescending(a => a.Sort));
        }

        /// <summary>
        /// 从分类找商家
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetProductPageListResponseDto>)), AllowAnonymous]
        public IActionResult GetProductListByCategoryAndSort([FromBody]GetProductListByCategoryAndSortRequestDto requestDto)
        {
            if (!string.IsNullOrWhiteSpace(requestDto.OrderBy))
            {
                string[] strArr = { "soldtotal", "creation_date", "price" };
                var isRight = strArr.Contains(requestDto.OrderBy.Trim().ToLower());
                if (!isRight)
                    requestDto.OrderBy = " creation_date ";
            }

            if (!string.IsNullOrWhiteSpace(requestDto.DescOrAsc))
            {
                string[] strArr = { "DESC", "ASC" };
                var isRight = strArr.Contains(requestDto.DescOrAsc.Trim().ToUpper());
                if (!isRight)
                    requestDto.DescOrAsc = " DESC ";
            }
            var productBiz = new ProductBiz();
            var productList = productBiz.GetProductListByCategory(requestDto);
            return Success(productList);
        }

        /// <summary>
        /// 商品banner简介
        /// </summary>
        /// <param name="proGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetProductDetailResponseDto>))]
        public IActionResult GetProductDetail(string proGuid)
        {
            if (string.IsNullOrWhiteSpace(proGuid))
            {
                return Failed(ErrorCode.Empty, "产品ID不能为空！");
            }

            var responseDto = new GetProductDetailResponseDto();
            var proModel = new ProductBiz().GetModelByGuid(proGuid);
            if (proModel == null)
            {
                return Failed(ErrorCode.Empty, "产品数据为空！");
            }

            responseDto.ProductGuid = proModel.ProductGuid;
            responseDto.ProductName = proModel.ProductName;
            responseDto.ProductTitle = proModel.ProductTitle;
            responseDto.MerchantGuid = proModel.MerchantGuid;
            responseDto.Standard = proModel.Standerd;
            responseDto.Price = proModel.Price;
            responseDto.Freight = proModel.Freight ?? 0M;//运费 缺少字段
            var bannerModelList = new BannerBiz().GetModelsByOwnerGuid(proModel.ProductGuid);
            if (bannerModelList == null)
            {
                return Success(responseDto);
            }

            var accModelList = new AccessoryBiz();
            var newProBannerInfo = new List<GetProductDetailResponseDto.BannerInfo>();
            //一个产品多个banner
            foreach (var bModel in bannerModelList)
            {
                var accModel = accModelList.GetAccessoryModelByGuid(bModel.PictureGuid);
                if (accModel == null)
                {
                    break;
                }
                var bannerInfo = new GetProductDetailResponseDto.BannerInfo
                {
                    BannerGuid = bModel.BannerGuid,
                    BannerPicUrl = accModel.BasePath + accModel.RelativePath,
                    BannerTargetUrl = bModel.TargetUrl
                };
                newProBannerInfo.Add(bannerInfo);
                responseDto.ProBannerInfo = newProBannerInfo;
            }
            return Success(responseDto);
        }

        /// <summary>
        /// 商品详细介绍(富文本)
        /// </summary>
        /// <param name="proGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetProDetailRichTextResponseDto>))]
        public IActionResult GetProductDetailText(string proGuid)
        {
            if (string.IsNullOrWhiteSpace(proGuid))
            {
                return Failed(ErrorCode.Empty, "产品ID不能为空！");
            }

            var proModel = new ProductBiz().GetModelByGuid(proGuid);
            if (proModel == null)
            {
                return Failed(ErrorCode.Empty, "产品数据为空！");
            }
            var richTextModel = new RichtextBiz().GetModel(proModel.IntroduceGuid);
            var specificationDto = new GetProDetailRichTextResponseDto { ProductGuid = proModel.ProductGuid };
            if (richTextModel == null)
            {
                return Failed(ErrorCode.Empty, "无商品详细介绍！");
            }
            specificationDto.ProductGuid = proModel.ProductGuid;
            specificationDto.TextGuid = richTextModel.TextGuid;
            specificationDto.Content = richTextModel.Content;
            specificationDto.CreatedBy = richTextModel.CreatedBy;
            specificationDto.CreationDate = richTextModel.CreationDate.ToString(CultureInfo.CurrentCulture);
            return Success(specificationDto);
        }

        /// <summary>
        /// 商品说明书(富文本)
        /// </summary>
        /// <param name="proGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetProDetailRichTextResponseDto>))]
        public IActionResult GetProductSpecification(string proGuid)
        {
            if (string.IsNullOrWhiteSpace(proGuid))
            {
                return Failed(ErrorCode.Empty, "产品ID不能为空！");
            }
            var proModel = new ProductBiz().GetModelByGuid(proGuid);
            if (proModel == null)
            {
                return Failed(ErrorCode.Empty, "产品数据为空！");
            }
            var specificationDto = new GetProDetailRichTextResponseDto { ProductGuid = proModel.ProductGuid };
            var richTextModel = new RichtextBiz().GetModel(proModel.ProDetailGuid);
            if (richTextModel == null)
            {
                return Failed(ErrorCode.Empty, "无商品说明书！");
            }
            specificationDto.ProductGuid = proModel.ProductGuid;
            specificationDto.TextGuid = richTextModel.TextGuid;
            specificationDto.Content = richTextModel.Content;
            specificationDto.CreatedBy = richTextModel.CreatedBy;
            specificationDto.CreationDate = richTextModel.CreationDate.ToString(CultureInfo.CurrentCulture);
            return Success(specificationDto);
        }

        /// <summary>
        /// 获取商户及其产品列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetProductListInMerchantResponseDto>))]
        public IActionResult GetProductListInMerchant([FromBody]GetProductListInMerchantRequestDto requestDto)
        {
            var productBiz = new ProductBiz();
            var proModelList = productBiz.GetProListByMerchantGuidAndCategoryGuid(requestDto);
            if (proModelList == null || proModelList.Count == 0)
            {
                return Failed(ErrorCode.DataBaseError, "无产品信息！");
            }
            var responseDto = new GetProductListInMerchantResponseDto
            {
                MerchantGuid = proModelList[0].MerchantGuid,
                MerchantName = proModelList[0].MerchantName,
                ClassifyGuid = proModelList[0].CategoryGuid,
                ClassifyName = proModelList[0].ConfigName
            };
            var responseProListDto = new List<GetProductListInMerchantResponseDto.ProductListInfo>();
            foreach (var model in proModelList)
            {
                var productModel = new GetProductListInMerchantResponseDto.ProductListInfo
                {
                    ProductGuid = model.ProductGuid,
                    ProductPicUrl = model.ProductPicUrl,
                    ProductName = model.ProductName,
                    Price = model.Price,
                    ProductForm = model.ProductForm,
                    SaleNum = productBiz.SumProductSoldNum(model.ProductGuid)
                };
                responseProListDto.Add(productModel);
            }
            responseDto.ProListInfo = responseProListDto;
            return Success(responseDto);
        }

        /// <summary>
        /// 常备药品
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<SubClassifyResponseDto>))]
        public IActionResult GetRegularMedicine()
        {
            var dicBiz = new DictionaryBiz();
            var dicList = dicBiz.GetDictionaryClassifyListByGuid(DictionaryType.RegularMedicine);
            if (dicList == null || dicList.Count < 1) return Failed(ErrorCode.Empty, "约定ID找不到数据！");
            //组装的dto
            var crdList = new List<SubClassifyResponseDto>();
            foreach (var model in dicList)
            {
                var dtoModel = new SubClassifyResponseDto
                {
                    FirstClassifyGuid = model.ParentGuid,
                    SubClassifyGuid = model.DicGuid,
                    SubClassifyName = model.ConfigName,
                    SubClassifyPic = model.ExtensionField,
                    Sort = model.Sort
                };
                crdList.Add(dtoModel);
            }
            if (crdList.Count < 1)
            {
                return Failed(ErrorCode.Empty, "无数据！");
            }
            return Success(crdList);
        }

        #region 服务类目相关

        /// <summary>
        /// 服务类目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<CategoryClassifyListResponse>))]
        public async Task<IActionResult> CategoryClassifyList(CategoryClassifyListRequest request)
        {
            var response = await new MerchantCategoryBiz().GetCategoryClassifyListAsync(request);
            return Success(response);
        }

        /// <summary>
        /// 类目详情介绍及Banner
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<CategoryClassifyIntroduceResponse>))]
        public async Task<IActionResult> CategoryClassifyIntroduce(CategoryClassifyIntroduceRequest request)
        {
            var categoryExtensionModel = await new MerchantCategoryBiz().GetAsync(request.CategoryGuid);
            if (categoryExtensionModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "该ID找不到类目信息！");
            }
            var response = categoryExtensionModel.ToDto<CategoryClassifyIntroduceResponse>();
            var accessoryBiz = new AccessoryBiz();
            var coverAccModel = await accessoryBiz.GetAccessoryListByOwnerGuid(categoryExtensionModel.CoverGuid);
            response.CoverURL = coverAccModel.FirstOrDefault()?.BasePath + coverAccModel.FirstOrDefault()?.RelativePath;

            var bannerModelList = new BannerBiz().GetModelsByOwnerGuid(categoryExtensionModel.CategoryGuid);
            var bannerInfoList = new List<CategoryClassifyIntroduceResponse.CategoryBannerInfo>();
            foreach (var bannerModel in bannerModelList)
            {
                var accessoryModel = await accessoryBiz.GetAsync(bannerModel.PictureGuid);
                var bannerInfo = new CategoryClassifyIntroduceResponse.CategoryBannerInfo
                {
                    BannerGuid = bannerModel.BannerGuid,
                    BannerName = bannerModel.BannerName,
                    Description = bannerModel.Description,
                    TargetUrl = bannerModel.TargetUrl,
                    Sort = bannerModel.Sort,
                    PictureURL = accessoryModel?.BasePath + accessoryModel?.RelativePath
                };
                bannerInfoList.Add(bannerInfo);
            }
            response.CategoryBannerInfoList = bannerInfoList;
            return Success(response);
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetClassifyProductListResponse>))]
        public async Task<IActionResult> GetClassifyProductList(GetClassifyProductListRequest request)
        {
            var responseList = await new ProductBiz().GetClassifyProductListAsync(request);
            return Success(responseList);
        }

        /// <summary>
        /// 团队介绍
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetTreamIntroduceResponseDto>))]
        public async Task<IActionResult> GetTreamIntroduceList(GetTreamIntroduceListRequest request)
        {
            var responseList = await new TherapistBiz().GetTreamIntroduceList(request);
            return Success(responseList);
        }

        #endregion 服务类目相关

        #region 商品详情

        /// <summary>
        /// （新）商品banner简介
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetCommonProductDetailBannerResponseDto>)), AllowAnonymous]
        public IActionResult GetCommonProductDetailBanner(GetCommonProductDetailBannerRequestDto requestDto)
        {
            var productBiz = new ProductBiz();
            var proModel = productBiz.GetModelByGuid(requestDto.ProductGuid);
            if (proModel == null)
            {
                return Failed(ErrorCode.Empty, "产品数据为空！");
            }
            var accModelList = new AccessoryBiz();
            var accModel = accModelList.GetAccessoryModelByGuid(proModel.PictureGuid);
            var responseDto = new GetCommonProductDetailBannerResponseDto
            {
                ProductGuid = proModel.ProductGuid,
                ProductName = proModel.ProductName,
                MerchantGuid = proModel.MerchantGuid,
                Standard = proModel.Standerd,
                Price = proModel.Price,
                Freight = proModel.Freight ?? 0M,
                ProductForm = proModel.ProductForm,
                ProductTitle = proModel.ProductTitle,
                ProductPicURL = accModel?.BasePath + accModel?.RelativePath,
                SoldNum = productBiz.SumProductSoldNum(proModel.ProductGuid)
            };
            var bannerModelList = new BannerBiz().GetModelsByOwnerGuid(proModel.ProductGuid);
            var newProBannerInfo = new List<GetCommonProductDetailBannerResponseDto.GetCommonProductDetailBannerItem>();
            //一个产品多个banner
            foreach (var bModel in bannerModelList)
            {
                var bannerAccModel = accModelList.GetAccessoryModelByGuid(bModel.PictureGuid);
                var bannerInfo = new GetCommonProductDetailBannerResponseDto.GetCommonProductDetailBannerItem
                {
                    BannerGuid = bModel.BannerGuid,
                    BannerPicUrl = bannerAccModel?.BasePath + bannerAccModel?.RelativePath,
                    BannerTargetUrl = bModel.TargetUrl
                };
                newProBannerInfo.Add(bannerInfo);
                responseDto.ProBannerInfo = newProBannerInfo;
            }
            return Success(responseDto);
        }

        /// <summary>
        /// （新）-商品包含产品项
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetCommonProductIncludingProjectListResponseDto>)), AllowAnonymous]
        public IActionResult GetCommonProductIncludingProjectList(GetCommonProductIncludingProjectListRequestDto requestDto)
        {
            var proModel = new ProductBiz().GetModelByGuid(requestDto.ProductGuid);
            if (proModel == null)
            {
                return Failed(ErrorCode.Empty, "产品数据为空！");
            }
            //关系查询
            var productProjectModelList = new ProductProjectBiz().GetModelsByProductGuidAsync(proModel.ProductGuid);
            var responseDto = new GetCommonProductIncludingProjectListResponseDto();
            responseDto.ProductGuid = proModel.ProductGuid;
            responseDto.ProductName = proModel.ProductName;
            if (productProjectModelList == null)
            {
                return Failed(ErrorCode.Empty, "该商品无子项信息，请检查！");
            }
            var responseItemList = new List<GetCommonProductIncludingProjectListResponseDto.GetProjectListItemDto>();
            var projectBiz = new ProjectBiz();
            //Parallel.ForEach(productProjectModelList.Result, productProjectModel =>
            //{
            foreach (var productProjectModel in productProjectModelList.Result)
            {
                var projectModel = projectBiz.GetModelAsync(productProjectModel.ProjectGuid);
                if (projectModel == null) { continue; }
                var responseItem = new GetCommonProductIncludingProjectListResponseDto.GetProjectListItemDto
                {
                    ProjectTimes = productProjectModel.ProjectTimes,//无限次
                    AllowPresent = productProjectModel.AllowPresent,
                    ProjectGuid = productProjectModel.ProjectGuid
                };
                //if (projectModel == null)
                //{
                //    responseItem.ProjectName = "";
                //    responseItem.ProjectNo = "";
                //    responseItem.Price = 0.00m;
                //}
                responseItem.ProjectName = projectModel.Result.ProjectName;
                //responseItem.ProjectNo = projectModel.Result.ProjectNo;
                responseItem.Price = projectModel.Result.Price;
                responseItemList.Add(responseItem);
            }
            responseDto.ProjectList = responseItemList;
            return Success(responseDto);
        }

        /// <summary>
        /// （新）-商品详细介绍(富文本)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetCommonProductDetailTextResponseDto>)), AllowAnonymous]
        public IActionResult GetCommonProductDetailText(GetCommonProductDetailTextRequestDto requestDto)
        {
            var proModel = new ProductBiz().GetModelByGuid(requestDto.ProductGuid);
            if (proModel == null)
            {
                return Failed(ErrorCode.Empty, "产品数据为空！");
            }
            var richTextModel = new RichtextBiz().GetModel(proModel.IntroduceGuid);
            var specificationDto = new GetCommonProductDetailTextResponseDto { ProductGuid = proModel.ProductGuid };
            if (richTextModel == null)
            {
                return Failed(ErrorCode.Empty, "无商品详细介绍！");
            }
            specificationDto.ProductGuid = proModel.ProductGuid;
            specificationDto.TextGuid = richTextModel.TextGuid;
            specificationDto.Content = richTextModel.Content;
            specificationDto.CreatedBy = richTextModel.CreatedBy;
            specificationDto.CreationDate = richTextModel.CreationDate.ToString(CultureInfo.CurrentCulture);
            return Success(specificationDto);
        }

        /// <summary>
        /// （新）商品购买须知(富文本)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetCommonProductSpecificationResponseDto>)), AllowAnonymous]
        public IActionResult GetCommonProductSpecification(GetCommonProductSpecificationRequestDto requestDto)
        {
            var proModel = new ProductBiz().GetModelByGuid(requestDto.ProductGuid);
            if (proModel == null)
            {
                return Failed(ErrorCode.Empty, "产品数据为空！");
            }
            var specificationDto = new GetCommonProductSpecificationResponseDto { ProductGuid = proModel.ProductGuid };
            var richTextModel = new RichtextBiz().GetModel(proModel.ProDetailGuid);
            if (richTextModel == null)
            {
                return Failed(ErrorCode.Empty, "无商品购买须知！");
            }
            specificationDto.ProductGuid = proModel.ProductGuid;
            specificationDto.TextGuid = richTextModel.TextGuid;
            specificationDto.Content = richTextModel.Content;
            specificationDto.CreatedBy = richTextModel.CreatedBy;
            specificationDto.CreationDate = richTextModel.CreationDate.ToString(CultureInfo.CurrentCulture);
            return Success(specificationDto);
        }

        /// <summary>
        /// 分页--获取双美全部评价
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetAllCLEvaluateResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetAllCLEvaluate(GetAllCLEvaluateRequestDto requestDto)
        {
            var response = await new CommentBiz().GetAllCLEvaluateAsync(requestDto);
            return Success(response);
        }

        #endregion 商品详情

        #region 购物车

        /// <summary>
        /// （新）加入购物车
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddShoppingCart([FromBody]AddShoppingCartRequestDto requestDto)
        {
            var productBiz = new ProductBiz();
            var shoppingCartBiz = new ShoppingCarBiz();
            var model = await shoppingCartBiz.GetShoppingCarProductByUserId(UserID, requestDto.ProductGuid);
            bool isSuccess;
            if (model == null)
            {
                var productModel = await productBiz.GetModelByGuidAsync(requestDto.ProductGuid);
                if (productModel == null)
                {
                    return Failed(ErrorCode.DataBaseError, "商品不存在！");
                }
                var shoppingCartModel = new ShoppingCarModel
                {
                    ItemGuid = Guid.NewGuid().ToString("N"),
                    UserGuid = UserID,
                    MerchantGuid = productModel.MerchantGuid,
                    ProductGuid = requestDto.ProductGuid,
                    ProductName = productModel.ProductName,
                    Count = requestDto.ProductNum,
                    LastUpdatedBy = UserID,
                    CreatedBy = UserID,
                    CreationDate = DateTime.Now,
                    PlatformType = productModel.PlatformType
                };
                isSuccess = !string.IsNullOrWhiteSpace(shoppingCartBiz.Add(shoppingCartModel));
            }
            else
            {
                model.Count += requestDto.ProductNum;
                //if (!productBiz.CheckProductInventory(model.ProductGuid, model.Count))
                //{  //购物车不计算库存
                //    model.IsValid = false;
                //}
                isSuccess = shoppingCartBiz.Update(model) == 1;
            }
            if (!isSuccess)
            {
                return Failed(ErrorCode.DataBaseError, "加入购物车失败！");
            }
            return Success();
        }

        /// <summary>
        /// 检查商品库存
        /// </summary>
        /// <param name="productId">商品Guid</param>
        /// <param name="inventory">数量</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult CheckProductInventory(string productId, int inventory)
        {
            var productModel = new ProductBiz().GetModelByGuid(productId);
            if (string.Equals(productModel.ProductForm, ProductFormEnum.Service.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Success(true);
            }
            var isEnougth = new ProductBiz().CheckProductInventory(productId, inventory);
            return Success(isEnougth);
        }

        /// <summary>
        /// 批量检查商品库存，返回结果集
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<CheckProductListInventoryResponse>>))]
        public async Task<IActionResult> CheckProductListInventoryAsync([FromBody]CheckProductListInventoryRequest request)
        {
            var productBiz = new ProductBiz();
            var responseList = new List<CheckProductListInventoryResponse>();
            foreach (var item in request.CheckProductinventoryList)
            {
                var productModel = await productBiz.GetModelByGuidAsync(item.ProductGuid);
                var message = productModel.OnSale ?
                    productModel.ProductForm.Equals(ProductFormEnum.Physical.ToString()) ?
                    productModel.Inventory > item.Num || productModel.Inventory == item.Num ?
                    "" : "商品库存不足！" : "" : "商品已下架！";
                var response = new CheckProductListInventoryResponse
                {
                    ProductGuid = item.ProductGuid,
                    ProductName = productModel.ProductName,
                    Num = item.Num,
                    IsRightStatus = !(message.Length > 0),
                    Message = message
                };
                responseList.Add(response);
                var isEnougth = new ProductBiz().GetModelByGuidAsync(item.ProductGuid);
            }
            return Success(responseList);
        }

        /// <summary>
        /// 获取我的购物车记录数
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult GetMyShoppingCarTotal()
        {
            var shoppingCarNum = new ShoppingCarBiz().GetMyShoppingCarTotal(UserID).ShoppingCarNum;
            return Success(shoppingCarNum);
        }

        /// <summary>
        /// 新-获取购物车列表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<ShowMyShoppingCartListOfCosmetologyResponseDto>>))]
        public async Task<IActionResult> ShowMyShoppingCartListOfCosmetologyAsync()
        {
            var shoppingCarItemList = await new ShoppingCarBiz().GetModelListAsyncBuUserGuid(UserID);
            var merchantBiz = new MerchantBiz();
            var productBiz = new ProductBiz();
            var projectBiz = new ProjectBiz();
            var responseList = new List<ShowMyShoppingCartListOfCosmetologyResponseDto>();
            if (shoppingCarItemList == null || shoppingCarItemList.Count < 1)
            {
                return Success(responseList, "购物车无数据");
            }
            var shoppingCarItemGroupByList = shoppingCarItemList.GroupBy(a => a.MerchantGuid);
            foreach (var groupByItem in shoppingCarItemGroupByList)
            {
                var merchantModel = await merchantBiz.GetModelAsyncNoEnable(groupByItem.Key ?? "");
                if (merchantModel == null) { continue; }
                var response = new ShowMyShoppingCartListOfCosmetologyResponseDto
                {
                    MerchantGuid = merchantModel.MerchantGuid,
                    MerchantName = merchantModel.MerchantName,
                    MerchantEnable = merchantModel.Enable
                };
                var responseProductList = new List<ShoppingCarListProductInfo>();
                foreach (var item in groupByItem)
                {
                    var productModel = await productBiz.GetModelByGuidAsyncNoEnable(item.ProductGuid ?? "");
                    if (productModel == null) { continue; }
                    if (!merchantModel.Enable || !productModel.PlatformOnSale || !productModel.OnSale || !productModel.Enable) { SetInvalidationFalseInShoppingCar(item); item.IsValid = false; }

                    var accModel = new AccessoryBiz().GetAccessoryModelByGuid(productModel.PictureGuid ?? "");
                    var responseProduct = new ShoppingCarListProductInfo
                    {
                        AdvancePaymentRate = productModel.AdvancePaymentRate,
                        OnSale = productModel.PlatformOnSale ? productModel.OnSale : productModel.PlatformOnSale,
                        AllowAdvancePayment = productModel.AllowAdvancePayment,
                        ItemGuid = item.ItemGuid,
                        IsValid = item.IsValid,
                        ProductCount = item.Count,
                        ProductForm = productModel.ProductForm,
                        ProductGuid = productModel.ProductGuid,
                        ProductName = productModel.ProductName,
                        ProductPicture = accModel?.BasePath + accModel?.RelativePath,
                        ProductPrice = productModel.Price,
                        Freight = productModel.Freight
                    };
                    var responseProjectList = new List<ShoppingCarListProductProjectInfo>();
                    if (productModel.ProductForm.Equals(ProductFormEnum.Service.ToString()))
                    {
                        var productProjectModelList = await new ProductProjectBiz().GetModelsByProductGuidAsync(productModel?.ProductGuid);
                        foreach (var relationItem in productProjectModelList)
                        {
                            var projectModel = await projectBiz.GetModelAsync(relationItem?.ProjectGuid);
                            if (projectModel == null) { continue; }
                            var responseProject = new ShoppingCarListProductProjectInfo
                            {
                                ProductGuid = productModel.ProductGuid,
                                ProjectGuid = projectModel.ProjectGuid,
                                ProjectName = projectModel.ProjectName,
                                ProjectTimes = relationItem.ProjectTimes
                            };
                            responseProjectList.Add(responseProject);
                        }
                    }
                    responseProduct.Projects = responseProjectList;
                    responseProductList.Add(responseProduct);
                    response.Products = responseProductList;
                }
                responseList.Add(response);
            }
            return Success(responseList);
        }

        private void SetInvalidationFalseInShoppingCar(ShoppingCarModel model)
        {
            model.IsValid = false;
            var isSucc = new ShoppingCarBiz().Update(model);
        }

        /// <summary>
        /// 删除购物车项
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteShoppingCarItemByItemGuid([FromBody]DeleteShoppingCarItemByItemGuidRequestDto requestDto)
        {
            if (!requestDto.ItemGuidListStr.Any())
            {
                return Failed(ErrorCode.Empty, "请传入正确的ID列表");
            }
            var isDeleteSucc = await new ShoppingCarBiz().DeleteListByIdsAsync(requestDto.ItemGuidListStr) > 0;
            return isDeleteSucc ? Success() : Failed(ErrorCode.DataBaseError, "删除失败！");
        }

        #endregion 购物车

        #region 微信支付/线下支付/流水/支付回调

        /// <summary>
        /// 用Code获取openid
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetOpenIDResponseDto>))]
        public async Task<IActionResult> GetOpenIDByCode([FromBody]GetOpenIDRequestDto requestDto)
        {
            var responseDto = await new FangDiPayBiz().GetOpenID(requestDto);
            return Success(responseDto);
        }

        /// <summary>
        /// 获取微信AppID
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<WeiXinConfigResponseDto>))]
        public async Task<WeiXinConfigResponseDto> GetWeiXinAppIDAsync()
        {
            var userModel = await new UserBiz().GetModelAsync(UserID);
            var isHaveOpenID = false;
            if (string.IsNullOrWhiteSpace(userModel.WechatOpenid))
            {
                isHaveOpenID = true;
            }
            Settings settings = Factory.GetSettings("host.json");
            return new WeiXinConfigResponseDto() { AppID = settings["WeChat:Client:AppId"], IsNeedToGetCode = isHaveOpenID, UserOpenID = userModel.WechatOpenid };
        }

        /// <summary>
        /// 微信支付
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> WeChatPay([FromBody]ChoosePaymentWayDoRequest request)
        {
            var fdPayBiz = new FangDiPayBiz();
            var userBiz = new UserBiz();
            var orderBiz = new OrderBiz();
            var transactionFlowingBiz = new TransactionFlowingBiz();
            var userModel = userBiz.GetUser(UserID);
            if (userModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "无法获取用户信息!");
            }
            //if (string.IsNullOrWhiteSpace(request.Code))
            //{
            //    if (string.IsNullOrWhiteSpace(userModel.WechatOpenid))
            //    {
            //        return Failed(ErrorCode.DataBaseError, "用户OpenId为空，请传Code参数!");
            //    }
            //}
            //else
            //{
            //    var fdResponse = await fdPayBiz.GetOpenID(new GetOpenIDRequestDto { Code = request.Code });
            //    if (string.IsNullOrWhiteSpace(fdResponse.Open_Id))
            //    {
            //        return Failed(ErrorCode.DataBaseError, "无法获取用户OpenID!");
            //    }
            //    if (string.IsNullOrWhiteSpace(fdResponse.Open_Id) || !userModel.WechatOpenid.Equals(fdResponse.Open_Id))
            //    {
            //        userModel.WechatOpenid = fdResponse.Open_Id;
            //        await userBiz.UpdateAsync(userModel);
            //    }
            //}

            if (string.IsNullOrWhiteSpace(userModel.WechatOpenid))
            {
                return Failed(ErrorCode.DataBaseError, "用户无OpenID!");
            }

            var orderModelList = await orderBiz.GetOrderListByOrderKey(request.OrderKey);
            if (!orderModelList.Any())
            {
                return Failed(ErrorCode.DataBaseError, "OrderKey无法找到订单，请检查!");
            }

            #region 流水相关

            //判断订单以前是否下过订单
            List<string> orderTransactionGuids = orderModelList.Where(n => !string.IsNullOrWhiteSpace(n.TransactionFlowingGuid)).Select(n => n.TransactionFlowingGuid).Distinct().ToList();
            //判断订单是否属于同一批支付
            if (orderTransactionGuids.Count > 1)
            {
                return Failed(ErrorCode.DataBaseError, "订单不属于同一批支付!");
            }
            //如果此订单已创建并尝试发起过支付
            if (orderTransactionGuids.Count == 1)
            {
                string orderTransactionGuid = orderTransactionGuids[0];
                //如果存在下单,判断上次下单是否有效
                var transactionFlowingModel = await transactionFlowingBiz.GetModelsById(orderTransactionGuid);
                //查询商户订单信息
                var merchantFlowingModelList = await new MerchantFlowingBiz().GetModelByTransactionFlowingGuid(orderTransactionGuid);

                if (!string.Equals(transactionFlowingModel.TransactionStatus, TransactionStatusEnum.WaitForPayment.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return Failed(ErrorCode.DataBaseError, "订单不是待支付状态!");
                }
                var outTradeNo = $"ZHYYOD_{GetRandomString(10, false, false, true, false, "")}{DateTime.Now.ToString("yyyyMMddHHmmss")}";
                transactionFlowingModel.OutTradeNo = outTradeNo;
                var updateTFlowing = await transactionFlowingBiz.UpdateAsync(new List<TransactionFlowingModel> { transactionFlowingModel });
                if (!updateTFlowing)
                {
                    return Failed(ErrorCode.DataBaseError, "更新流水失败!");
                }
                var fdXDResponse = await fdPayBiz.OrdersPay(new OrdersPayRequestDto { Trade_No = outTradeNo, Amount = (Convert.ToInt32(transactionFlowingModel.Amount)).ToString(), Open_Id = userModel.WechatOpenid });
                return Success(fdXDResponse);
            }

            #endregion 流水相关

            //新订单
            var newTFModel = new TransactionFlowingModel();
            var newMFModelList = new List<MerchantFlowingModel>();
            CreateNewFlowing(ref orderModelList, userModel, ref newTFModel, ref newMFModelList);
            newTFModel.Channel = "微信支付";
            newTFModel.PayAccount = userModel.WechatOpenid;
            var isSuccess = await transactionFlowingBiz.SaveTransactionFlowing(newTFModel, newMFModelList, orderModelList);
            if (!isSuccess)
            {
                return Failed(ErrorCode.DataBaseError, "流水生成失败!");
            }
            var fdXDResponsed = await fdPayBiz.OrdersPay(new OrdersPayRequestDto { Trade_No = newTFModel.OutTradeNo, Amount = (Convert.ToInt32(newTFModel.Amount)).ToString(), Open_Id = userModel.WechatOpenid });
            return Success(fdXDResponsed);
        }

        /// <summary>
        /// 线下支付
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> OffLinePay([FromBody]OffLinePayRequest request)
        {
            var fdPayBiz = new FangDiPayBiz();
            var userBiz = new UserBiz();
            var orderBiz = new OrderBiz();
            var transactionFlowingBiz = new TransactionFlowingBiz();
            var userModel = userBiz.GetUser(UserID);
            var orderModelList = await orderBiz.GetOrderListByOrderKey(request.OrderKey);
            if (!orderModelList.Any())
            {
                return Failed(ErrorCode.DataBaseError, "OrderKey无法找到订单，请检查!");
            }
            //判断订单以前是否下过订单
            List<string> orderTransactionGuids = orderModelList.Where(n => !string.IsNullOrWhiteSpace(n.TransactionFlowingGuid)).Select(n => n.TransactionFlowingGuid).Distinct().ToList();
            //判断订单是否属于同一批支付
            if (orderTransactionGuids.Count > 1)
            {
                return Failed(ErrorCode.DataBaseError, "订单不属于同一批支付!");
            }
            var newTFModel = new TransactionFlowingModel();
            var newMFModelList = new List<MerchantFlowingModel>();
            CreateNewFlowing(ref orderModelList, userModel, ref newTFModel, ref newMFModelList);
            //新订单
            newTFModel.Channel = "线下支付";
            newTFModel.PayAccount = string.Empty;
            var isSuccess = await transactionFlowingBiz.SaveTransactionFlowing(newTFModel, newMFModelList, orderModelList);
            if (!isSuccess)
            {
                return Failed(ErrorCode.DataBaseError, "流水生成失败!");
            }
            return Success(isSuccess);
        }

        /// <summary>
        /// 创建流水
        /// </summary>
        /// <param name="orderModelList"></param>
        /// <param name="userModel"></param>
        /// <param name="tfModel"></param>
        /// <param name="mfModelList"></param>
        /// <returns></returns>
        private void CreateNewFlowing(ref List<OrderModel> orderModelList, UserModel userModel, ref TransactionFlowingModel tfModel, ref List<MerchantFlowingModel> mfModelList)
        {
            //var dFlowing = new Dictionary<TransactionFlowingModel, List<MerchantFlowingModel>>();
            var transactionFlowingModel = new TransactionFlowingModel
            {
                //交易流水数据
                TransactionFlowingGuid = Guid.NewGuid().ToString("N"),
                TransactionNumber = $"ZHYYLS_{GetRandomString(10, false, false, true, false, "")}{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                OutTradeNo = $"ZHYYOD_{GetRandomString(10, false, false, true, false, "")}{DateTime.Now.ToString("yyyyMMddHHmmss")}",
                //Channel = "微信支付",
                ChannelNumber = "2",
                //PayAccount = userModel.WechatOpenid,
                Amount = (orderModelList.Where(a => a.OrderMark.Equals(OrderMarkEnum.Primary.ToString())).ToList().Select(a => new { a.OrderGuid, a.PaidAmount, a.Freight, a.OrderMark }).Distinct().Sum(a => a.PaidAmount + a.Freight)) * 100,
                OutRefundNo = string.Empty,
                TransactionStatus = TransactionStatusEnum.WaitForPayment.ToString(),
                Remarks = string.Empty,
                OrgGuid = string.Empty,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
            };
            var merchantFlowingModelList = new List<MerchantFlowingModel>();
            var orderModelWhereList = orderModelList.Where(a => a.OrderMark.Equals(OrderMarkEnum.Secondary.ToString()));
            foreach (var item in orderModelWhereList)
            {
                var merchantFlowingModel = new MerchantFlowingModel
                {
                    MerchantFlowingGuid = Guid.NewGuid().ToString("N"),
                    TransactionFlowingGuid = transactionFlowingModel.TransactionFlowingGuid,
                    OrderGuid = item.OrderGuid,
                    MerchantAccount = item.MerchantGuid,
                    FlowStatus = FlowStatus.PlatformNotReceived.ToString(),
                    Amount = (item.PaidAmount + item.Freight) * 100,
                    OrgGuid = string.Empty,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID
                };
                merchantFlowingModelList.Add(merchantFlowingModel);
            }
            tfModel = transactionFlowingModel;
            mfModelList = merchantFlowingModelList;
            orderModelList.ForEach(n => n.TransactionFlowingGuid = transactionFlowingModel.TransactionFlowingGuid);
            //dFlowing.Add(transactionFlowingModel, merchantFlowingModelList);
            //return dFlowing;
        }

        ///<summary>
        ///生成随机字符串
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        private string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }

        /// <summary>
        /// （新）提交订单(所有商品先添加购物车，然后提交商品)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<ResultOfSubmitOrderResponseDto>))]
        public async Task<IActionResult> SubmitOrderSettlement([FromBody]AddOrderInfoRequestDto requestDto)
        {
            var userModel = await new UserBiz().GetModelAsync(UserID);
            if (userModel == null || !userModel.Enable)
            {
                return Failed(ErrorCode.UserData, "无此用户");
            }
            if (string.IsNullOrWhiteSpace(userModel.WechatOpenid))
            {
                Logger.Error($"提交订单时-SubmitOrderSettlement-未获取到用户[{UserID}]的openId,需进行微信授权");
                return Failed(ErrorCode.Unauthorized, "未获取到用户的openId,需进行微信授权");
            }

            var payWayList = new List<string> { "offlinepay", "wechat" };
            if (!payWayList.Contains(requestDto.PayType.ToLower()))
            {
                return Failed(ErrorCode.Empty, "请输入正确的支付方式！");
            }
            var getShoppingCarModelList = await new ShoppingCarBiz().GetShoppingCarSelectedProductDetail(requestDto.ShoppingCarItemIDList);
            if (getShoppingCarModelList == null || !getShoppingCarModelList.Any())
            {
                return Failed(ErrorCode.Empty, "未获取到数据");
            }
            var checkMerchantNum = getShoppingCarModelList.ToList().Select(a => new { a.MerchantGuid }).Distinct();
            if (checkMerchantNum.ToList().Count > 1 && requestDto.PayType.ToLower().Equals("offlinepay"))
            {
                return Failed(ErrorCode.Empty, "跨店铺订单不能线下支付，请分开下单，谢谢！");
            }
            var newOrderInfoList = new List<Dictionary<OrderModel, List<OrderDetailModel>>>();

            var goodsModelListInfo = new List<List<Dictionary<GoodsModel, List<GoodsItemModel>>>>();
            var groupByList = getShoppingCarModelList.GroupBy(s => s.MerchantGuid);

            string orderKey = Guid.NewGuid().ToString("N");
            var primaryOrderList = new List<OrderModel>();
            var primaryOrderDetailList = new List<OrderDetailModel>();
            foreach (var item in groupByList)//一个商户
            {
                var oneOrder = new Dictionary<OrderModel, List<OrderDetailModel>>();
                var goodsModelInfo = new List<Dictionary<GoodsModel, List<GoodsItemModel>>>();
                var typeItemGroup = item.GroupBy(a => a.ProductForm);
                foreach (var IsPhysicalItem in typeItemGroup)//是否实体商品
                {
                    var merchant = item.FirstOrDefault();
                    var orderModel = new OrderModel
                    {
                        OrderGuid = Guid.NewGuid().ToString("N"),
                        OrderNo = Common.Helper.OrderNoCreater.Create((Common.EnumDefine.PlatformType)Enum.Parse(typeof(Common.EnumDefine.PlatformType), merchant.PlatformType)),
                        UserGuid = UserID,
                        MerchantGuid = item.Key,
                        OrderType = OrderTypeEnum.Normal.ToString(),
                        OrderCategory = IsPhysicalItem.Key.Equals(OrderCategoryEnum.Physical.ToString()) ? OrderCategoryEnum.Physical.ToString() : IsPhysicalItem.Key.Equals(OrderCategoryEnum.Service.ToString()) ? OrderCategoryEnum.Service.ToString() : null,
                        OrderMark = OrderMarkEnum.Secondary.ToString(),
                        OrderKey = orderKey,
                        // ProductCount = IsPhysicalItem.ToList().Select(a => new { a.MerchantGuid, a.ProductGuid, a.ProductCount }).Distinct().Sum(a => a.ProductCount),
                        OrderPhone = requestDto.Phone,
                        OrderAddress = requestDto.AddressStr,
                        OrderReceiver = requestDto.Receiver,
                        OrderStatus = OrderStatusEnum.Obligation.ToString(),// 默认待付款
                        //PaymentDate = null,
                        PayType = requestDto.PayType,
                        //PayablesAmount = IsPhysicalItem.ToList().Select(a => new { a.MerchantGuid, a.ProductGuid, a.ProductCount, a.ProductPrice }).Distinct().Sum(a => a.ProductCount * a.ProductPrice),
                        //PaidAmount = IsPhysicalItem.ToList().Select(a => new { a.MerchantGuid, a.ProductGuid, a.ProductCount, a.ProductPrice }).Distinct().Sum(a => a.ProductCount * a.ProductPrice),
                        Debt = 0,
                        DiscountAmout = 0,
                        //Freight= string.Empty,//流水
                        Remark = requestDto.Remark,
                        TransactionFlowingGuid = string.Empty,//流水
                        PlatformType = merchant.PlatformType,
                        OrgGuid = string.Empty,
                        CreatedBy = UserID,
                        CreationDate = DateTime.Now,
                        LastUpdatedBy = UserID,
                        LastUpdatedDate = DateTime.Now,
                        Enable = false
                    };
                    var orderDetailList = IsPhysicalItem.Key.Equals(OrderCategoryEnum.Physical.ToString()) ? CheckPhysicalItemStatus(IsPhysicalItem.ToList(), ref orderModel) : IsPhysicalItem.Key.Equals(OrderCategoryEnum.Service.ToString()) ? CheckNoPhysicalItemStatus(IsPhysicalItem.ToList(), orderModel, goodsModelInfo) : null;
                    oneOrder.Add(orderModel, orderDetailList);
                    primaryOrderList.Add(orderModel);
                    orderDetailList.ForEach(i => primaryOrderDetailList.Add(i));
                }
                newOrderInfoList.Add(oneOrder);
                goodsModelListInfo.Add(goodsModelInfo);
            }
            //建主单  提交订单   减库存  删购物车
            var result = await new OrderBiz().SubmitShoppingCartSeletedProductOfCosmetologyAsync(newOrderInfoList, goodsModelListInfo, CreatePrimaryOrder(primaryOrderList, primaryOrderDetailList), requestDto.ShoppingCarItemIDList);
            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, "产品存库不足或已下架！");
            }
            return Success(orderKey);
        }

        /// <summary>
        /// 生成主订单
        /// </summary>
        /// <returns></returns>
        private Dictionary<OrderModel, List<OrderDetailModel>> CreatePrimaryOrder(List<OrderModel> primaryOrderList, List<OrderDetailModel> primaryOrderDetailList)
        {
            var dictionaryOrderInfo = new Dictionary<OrderModel, List<OrderDetailModel>>();
            var orderDetailModelList = new List<OrderDetailModel>();
            var oneOrder = primaryOrderList.FirstOrDefault();
            var orderModel = new OrderModel
            {
                OrderGuid = Guid.NewGuid().ToString("N"),
                OrderNo = OrderNoCreater.Create((Common.EnumDefine.PlatformType)Enum.Parse(typeof(Common.EnumDefine.PlatformType), oneOrder.PlatformType)),
                UserGuid = UserID,
                MerchantGuid = oneOrder.MerchantGuid,
                OrderType = oneOrder.OrderType,
                OrderCategory = oneOrder.OrderCategory,
                OrderMark = OrderMarkEnum.Primary.ToString(),
                OrderKey = oneOrder.OrderKey,
                ProductCount = primaryOrderList.ToList().Select(a => new { a.OrderGuid, a.ProductCount }).Distinct().Sum(a => a.ProductCount),
                OrderPhone = oneOrder.OrderPhone,
                OrderAddress = oneOrder.OrderAddress,
                OrderReceiver = oneOrder.OrderReceiver,
                OrderStatus = OrderStatusEnum.Obligation.ToString(),// 默认待付款
                PaymentDate = null,
                PayType = oneOrder.PayType,
                PayablesAmount = primaryOrderList.ToList().Select(a => new { a.OrderGuid, a.ProductCount, a.PayablesAmount }).Distinct().Sum(a => a.PayablesAmount),
                PaidAmount = primaryOrderList.ToList().Select(a => new { a.OrderGuid, a.ProductCount, a.PaidAmount }).Distinct().Sum(a => a.PaidAmount),
                Debt = 0,
                DiscountAmout = 0,
                Freight = primaryOrderList.ToList().Select(a => new { a.OrderGuid, a.Freight }).Distinct().Sum(a => a.Freight),//运费
                Remark = oneOrder.Remark,
                TransactionFlowingGuid = string.Empty,//流水
                PlatformType = oneOrder.PlatformType,
                OrgGuid = string.Empty,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now,
                Enable = true
            };
            foreach (var item in primaryOrderDetailList)
            {
                var orderDetailModel = new OrderDetailModel
                {
                    DetailGuid = Guid.NewGuid().ToString("N"),
                    OrderGuid = orderModel.OrderGuid,
                    ProductGuid = item.ProductGuid,
                    ProductName = item.ProductName,
                    ProductPrice = item.ProductPrice,
                    ProductCount = item.ProductCount,
                    CampaignGuid = string.Empty,//优惠Guid
                    CommentGuid = string.Empty,//评论Guid
                    OrgGuid = string.Empty,
                    CreatedBy = UserID,
                    CreationDate = DateTime.Now,
                    LastUpdatedBy = UserID,
                    LastUpdatedDate = DateTime.Now
                };
                orderDetailModelList.Add(orderDetailModel);
            }
            dictionaryOrderInfo.Add(orderModel, orderDetailModelList);
            return dictionaryOrderInfo;
        }

        /// <summary>
        /// 实体商品 计算
        /// </summary>
        /// <param name="itemList"></param>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        private List<OrderDetailModel> CheckPhysicalItemStatus(List<ShoppingCarProductDetailDto> itemList, ref OrderModel orderModel)
        {
            var orderDetailList = new List<OrderDetailModel>();
            var productBiz = new ProductBiz();
            var shoppingCarBiz = new ShoppingCarBiz();
            var freightArr = new List<decimal>();//订单运费
            foreach (var itemDetail in itemList)
            {
                //实体商品
                var productModel = productBiz.GetModelByGuid(itemDetail.ProductGuid);
                var shoppingCarModel = shoppingCarBiz.GetModelByGuid(itemDetail.ItemGuid);
                if (!productModel.OnSale)
                {
                    shoppingCarModel.IsValid = false;
                    var isSuccessUpdate = shoppingCarBiz.Update(shoppingCarModel);
                    //continue;
                }

                if (productModel.Inventory < shoppingCarModel.Count)
                {
                    shoppingCarModel.IsValid = false;
                    var isSuccessUpdate = shoppingCarBiz.Update(shoppingCarModel);
                    //continue;
                }
                freightArr.Add(productModel.Freight ?? 0M);
                var orderDetailModel = new OrderDetailModel
                {
                    DetailGuid = Guid.NewGuid().ToString("N"),
                    OrderGuid = orderModel.OrderGuid,
                    ProductGuid = productModel.ProductGuid,
                    ProductName = productModel.ProductName,
                    ProductPrice = productModel.Price,
                    ProductCount = itemDetail.ProductCount,
                    CampaignGuid = string.Empty,//优惠Guid
                    CommentGuid = string.Empty,//评论Guid
                    OrgGuid = string.Empty,
                    CreatedBy = UserID,
                    CreationDate = DateTime.Now,
                    LastUpdatedBy = UserID,
                    LastUpdatedDate = DateTime.Now
                };
                orderDetailList.Add(orderDetailModel);
            }
            orderModel.ProductCount = orderDetailList.Select(a => new { a.ProductGuid, a.ProductCount }).Distinct().Sum(a => a.ProductCount);
            orderModel.PayablesAmount = orderDetailList.Select(a => new { a.ProductGuid, a.ProductCount, a.ProductPrice }).Distinct().Sum(a => a.ProductCount * a.ProductPrice);
            orderModel.PaidAmount = orderDetailList.Select(a => new { a.ProductGuid, a.ProductCount, a.ProductPrice }).Distinct().Sum(a => a.ProductCount * a.ProductPrice);
            orderModel.Freight = freightArr.Count > 0 ? freightArr.Max() : 0;
            return orderDetailList;
        }

        /// <summary>
        /// 服务类型订单 计算
        /// </summary>
        /// <returns></returns>
        private List<OrderDetailModel> CheckNoPhysicalItemStatus(List<ShoppingCarProductDetailDto> itemList, OrderModel orderModel, List<Dictionary<GoodsModel, List<GoodsItemModel>>> goodsModelInfoList)
        {
            var orderDetailList = new List<OrderDetailModel>();
            var productBiz = new ProductBiz();
            var shoppingCarBiz = new ShoppingCarBiz();
            //var gitemList = new List<GoodsItemModel>();
            var orderDetailsDidstinct = itemList.Select(a => new { a.ProductGuid, a.ProductName, a.ProductPrice, a.ProductCount }).Distinct().Select(a => new OrderDetailModel
            {
                DetailGuid = Guid.NewGuid().ToString("N"),
                OrderGuid = orderModel.OrderGuid,
                ProductGuid = a.ProductGuid,
                ProductName = a.ProductName,
                ProductPrice = a.ProductPrice,
                ProductCount = a.ProductCount,
                CampaignGuid = string.Empty,//优惠Guid
                CommentGuid = string.Empty,//评论Guid
                OrgGuid = string.Empty,
                CreatedBy = UserID,
                CreationDate = DateTime.Now,
                LastUpdatedBy = UserID,
                LastUpdatedDate = DateTime.Now
            }).ToList();

            orderDetailList.AddRange(orderDetailsDidstinct);
            foreach (var orderDetail in orderDetailsDidstinct)
            {
                var dGoodsModel = new Dictionary<GoodsModel, List<GoodsItemModel>>();
                var product = itemList.Where(a => a.ProductGuid == orderDetail.ProductGuid).FirstOrDefault();
                for (int i = 0; i < orderDetail.ProductCount; i++)
                {
                    var goodsModel = new GoodsModel
                    {
                        GoodsGuid = Guid.NewGuid().ToString("N"),
                        UserGuid = UserID,
                        OrderGuid = orderDetail.OrderGuid,
                        DetailGuid = orderDetail.DetailGuid,
                        Available = true,
                        EffectiveStartDate = product.EffectiveDays == 0 ? null : (DateTime?)DateTime.Now,
                        EffectiveEndDate = product.EffectiveDays == 0 ? null : (DateTime?)DateTime.Now.AddDays(product.EffectiveDays),
                        ProductGuid = orderDetail.ProductGuid,
                        ProductName = orderDetail.ProductName,
                        MerchanGuid = product.MerchantGuid,
                        Price = product.ProductPrice,
                        SelectRule = "0",
                        ProjectThreshold = product.ProjectThreshold,
                        PlatformType = product.PlatformType,
                        CreatedBy = UserID,
                        LastUpdatedBy = UserID,
                        OrgGuid = "GuoDan"
                    };
                    var goodsItemsDidstinct = itemList.Where(a => a.ProductGuid == goodsModel.ProductGuid).Select(a => new
                    {
                        a.ProductGuid,
                        a.ProjectGuid,
                        a.ProjectName,
                        a.ProjectTimes,
                        a.ProjectPrice,
                        a.AllowPresent,
                        a.PlatformType
                    }).Distinct();

                    var goodsItemList = goodsItemsDidstinct.Select(a => new GoodsItemModel
                    {
                        GoodsItemGuid = Guid.NewGuid().ToString("N"),
                        GoodsGuid = goodsModel.GoodsGuid,
                        ProjectGuid = a.ProjectGuid,
                        Count = a.ProjectTimes,
                        Remain = a.ProjectTimes,
                        Used = 0,
                        Available = true,
                        Price = a.ProjectPrice,
                        AllowPresent = a.AllowPresent,
                        PlatformType = a.PlatformType,
                        CreatedBy = UserID,
                        LastUpdatedBy = UserID,
                        OrgGuid = "GuoDan"
                    }).Distinct().ToList();
                    //gitemList.AddRange(goodsItemList);
                    dGoodsModel.Add(goodsModel, goodsItemList);
                }
                goodsModelInfoList.Add(dGoodsModel);
            }

            orderModel.ProductCount = orderDetailList.Select(a => new { a.ProductGuid, a.ProductCount }).Distinct().Sum(a => a.ProductCount);
            orderModel.PayablesAmount = orderDetailList.Select(a => new { a.ProductGuid, a.ProductCount, a.ProductPrice }).Distinct().Sum(a => a.ProductCount * a.ProductPrice);
            orderModel.PaidAmount = orderDetailList.Select(a => new { a.ProductGuid, a.ProductCount, a.ProductPrice }).Distinct().Sum(a => a.ProductCount * a.ProductPrice);
            orderModel.Freight = 0;
            return orderDetailList;
        }

        /// <summary>
        /// 方迪支付回调
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<PaymentPushResponse>)), AllowAnonymous]
        public async Task<IActionResult> PaymentCallAsync([FromBody]PaymentPushRequest requestDto)
        {
            Logger.Info($"微信支付回调-PaymentCallAsync=>requestDto:{JsonConvert.SerializeObject(requestDto)}");
            string[] orderFlowingArr = { "ZHYYOD_" };//可以往后加
            string[] faqsFlowingArr = { "FAQSPQ_" };
            try
            {
                if (requestDto.trade_status.ToLower().Equals("pay_ok") && !string.IsNullOrWhiteSpace(requestDto.hosp_out_trade_no))
                {
                    //订单处理
                    var outTradeNO = requestDto.hosp_out_trade_no.Substring(0, 7);
                    if (orderFlowingArr.Contains(outTradeNO))
                    {
                        if (!await DealWithOrder(requestDto))
                        {
                            return Ok(new PaymentPushResponse { resultCode = "-1", resultMsg = "Failed" });
                        }
                    }
                    //问答处理
                    if (faqsFlowingArr.Contains(outTradeNO))
                    {
                        if (!await DealWithFAQs(requestDto))
                        {
                            return Ok(new PaymentPushResponse { resultCode = "-1", resultMsg = "Failed" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"微信支付回调-PaymentCallAsync=>数据处理异常：{ex.Message}!");
                return Ok(new PaymentPushResponse { resultCode = "-1", resultMsg = "Failed" });
            }

            return Ok(new PaymentPushResponse { resultCode = "0", resultMsg = "SUCCESS" });
        }

        /// <summary>
        /// 回调订单处理
        /// </summary>
        /// <returns></returns>
        private async Task<bool> DealWithOrder(PaymentPushRequest requestDto)
        {
            var transactionFlowingBiz = new TransactionFlowingBiz();
            var tfModel = await transactionFlowingBiz.GetModelByOutTradeNo(requestDto.hosp_out_trade_no);
            if (tfModel == null) { Logger.Info($"微信支付回调-PaymentCallAsync=>回调订单号找不到TransactionFlowing!"); return false; }
            var mfModelList = await new MerchantFlowingBiz().GetModelByTransactionFlowingGuid(tfModel.TransactionFlowingGuid);
            if (mfModelList == null || mfModelList.Count < 1) { Logger.Info($"微信支付回调-PaymentCallAsync=>流水找不到MerchantFlowing!"); return false; }
            var orderModelList = await new OrderBiz().GetGetModelsByTransactionFlowingGuid(tfModel.TransactionFlowingGuid);
            if (orderModelList == null || orderModelList.Count < 1) { Logger.Info($"微信支付回调-PaymentCallAsync=>流水找不到orderModelList!"); return false; }

            tfModel.TransactionStatus = TransactionStatusEnum.Success.ToString();
            mfModelList.ForEach(n => n.FlowStatus = FlowStatus.PlatformCollection.ToString());
            foreach (var item in orderModelList)
            {
                item.PaymentDate = DateTime.Now;
                if (item.OrderMark.Equals(OrderMarkEnum.Primary.ToString()))
                {
                    item.Enable = false;
                }
                if (item.OrderMark.Equals(OrderMarkEnum.Secondary.ToString()))
                {
                    item.Enable = true;
                }
                if (item.OrderCategory.Equals(OrderCategoryEnum.Physical.ToString()))
                {
                    item.OrderStatus = OrderStatusEnum.Received.ToString();//待收货
                }
                if (item.OrderCategory.Equals(OrderCategoryEnum.Service.ToString()))
                {
                    item.OrderStatus = OrderStatusEnum.Completed.ToString();
                }
            }
            var isSuccess = await new TransactionFlowingBiz().UpdateTransactionFlowing(tfModel, mfModelList, orderModelList);
            if (!isSuccess)
            {
                Logger.Info($"微信支付回调-PaymentCallAsync=>更新流水与订单状态失败!");
                return false;
            }
            //订单完成后生成订单下待评价商品记录
            var completedOrderIds = orderModelList.Where(a => a.OrderMark.ToLower() == OrderMarkEnum.Secondary.ToString().ToLower() && a.OrderStatus == OrderStatusEnum.Completed.ToString()).Select(a => a.OrderGuid).ToList();
            CreateOrderProductComment(completedOrderIds);//服务订单完成后，生成评价记录
            return true;
        }

        /// <summary>
        /// 回调问答处理
        /// </summary>
        /// <returns></returns>
        private async Task<bool> DealWithFAQs(PaymentPushRequest requestDto)
        {
            var transferFlowingModel = await new TransactionFlowingBiz().GetModelByOutTradeNo(requestDto.hosp_out_trade_no);
            if (transferFlowingModel == null) { Logger.Info($"微信支付回调-PaymentCallAsync=>问答回调找不到TransactionFlowing!{requestDto.hosp_out_trade_no}"); return false; }
            var questionBiz = new FaqsQuestionBiz();
            var questionModel = await questionBiz.GetModelByFlowingAsync(transferFlowingModel.TransactionFlowingGuid);
            if (transferFlowingModel == null) { Logger.Info($"微信支付回调-PaymentCallAsync=>问答回调找不到QuestionModel!{transferFlowingModel.TransactionFlowingGuid}"); return false; }

            questionModel.Enable = true;
            transferFlowingModel.TransactionStatus = TransactionStatusEnum.Success.ToString();
            if (!await questionBiz.UpdateDoubleModelAsync(questionModel, transferFlowingModel))
            {
                Logger.Info($"微信支付回调-PaymentCallAsync=>更新问题状态失败!{questionModel.QuestionGuid}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 订单完成后生成订单下待评价商品记录
        /// </summary>
        private void CreateOrderProductComment(List<string> orderIds)
        {
            Task.Run(() =>
            {
                try
                {
                    if (!orderIds.Any())
                    {
                        return;
                    }
                    var check = new OrderProductCommentBiz().GetModelsByOrderGuidAsync(orderIds.FirstOrDefault()).Result;
                    if (check.Any())
                    {
                        return;
                    }
                    var list = new List<OrderDetailModel>();
                    foreach (var orderGuid in orderIds)
                    {
                        var orderDetailModels = new OrderDetailBiz().GetModelsByOrderIdAsync(orderGuid).Result;
                        list.AddRange(orderDetailModels);
                    }
                    if (!list.Any())
                    {
                        return;
                    }
                    var oneOrderModel = new OrderBiz().GetAsync(orderIds.FirstOrDefault()).Result;
                    var orderProductCommentModels = list.Select(a => new OrderProductCommentModel
                    {
                        ProductCommentGuid = Guid.NewGuid().ToString("N"),
                        UserGuid = oneOrderModel.UserGuid,
                        OrderGuid = a.OrderGuid,
                        OrderDetailGuid = a.DetailGuid,
                        ProductGuid = a.ProductGuid,
                        ProductName = a.ProductName,
                        CommentStatus = CommentStatusEnum.NotEvaluate.ToString(),
                        CreatedBy = "PaymentCallAsync",
                        LastUpdatedBy = "PaymentCallAsync",
                        OrgGuid = string.Empty
                    }).ToList();
                    var result = new OrderProductCommentBiz().InsertAsync(orderProductCommentModels).Result;
                    if (result)
                    {
                        Logger.Debug($"订单完成后生成订单下待评价商品记录-OrderGuids:{JsonConvert.SerializeObject(orderIds)}");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"微信支付回调后创建订单商品评价记录失败 at {nameof(MallController)}.{nameof(CreateOrderProductComment)}({JsonConvert.SerializeObject(orderIds)}) {Environment.NewLine}错误信息 ：{ex.Message}");
                }
            });
        }

        #endregion 微信支付/线下支付/流水/支付回调

        #region 订单及详情/待评价

        /// <summary>
        /// 获取我的订单
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetMyOrderListResponseDto>>))]
        public async Task<IActionResult> GetMyOrderListAsync(GetMyOrderListRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.UserID))
            {
                requestDto.UserID = UserID;
            }
            var orderBiz = new OrderBiz();
            var models = orderBiz.GetMyOrderList(requestDto);


            var responseDtoList = new List<GetMyOrderListResponseItemDto>();
            foreach (var item in models.CurrentPage)
            {
                var orderDetailModelList = new OrderDetailBiz().GetModelsByOrderIdAsync(item.OrderGuid).Result;
                var orderDetailList = new List<GetMyOrderListResponseItemDto.OrderDetail>();

                var responseDto = item.MapTo<GetMyOrderListResponseItemDto, GetMyOrderListItemTmpDto>();
                #region 查看订单售后描述
                var orderDetialServiceInfo = await new OrderDetailBiz().GetOrderDetailAfterServiceInfoByOrderGuidAsync(item.OrderGuid);
                if (orderDetialServiceInfo.Where(a => a.ServiceStatus != null).Count() > 0)
                {
                    var allRefund = orderDetialServiceInfo.All(a => a.ServiceStatus == AfterSaleServiceStatusEnum.Completed
                        && (a.ServiceType == AfterSaleServiceTypeEnum.RefundWhithReturn || a.ServiceType == AfterSaleServiceTypeEnum.RefundWhitoutReturn));
                    if (allRefund)
                    {
                        responseDto.AfterSeriveDescription = $"已退款";
                    }
                    else if (orderDetialServiceInfo.FirstOrDefault(a => a.ServiceStatus == AfterSaleServiceStatusEnum.Applying
                    && (a.ServiceType == AfterSaleServiceTypeEnum.RefundWhithReturn || a.ServiceType == AfterSaleServiceTypeEnum.RefundWhitoutReturn)) != null)
                    {
                        responseDto.AfterSeriveDescription = $"退款审核中";
                    }
                    else if (orderDetialServiceInfo.FirstOrDefault(a => a.ServiceStatus == AfterSaleServiceStatusEnum.Reject
                    && (a.ServiceType == AfterSaleServiceTypeEnum.RefundWhithReturn || a.ServiceType == AfterSaleServiceTypeEnum.RefundWhitoutReturn)) != null)
                    {
                        responseDto.AfterSeriveDescription = $"退款被拒绝";
                    }

                    else if (orderDetialServiceInfo.Where(a => a.ServiceStatus == AfterSaleServiceStatusEnum.Completed
                     && (a.ServiceType == AfterSaleServiceTypeEnum.RefundWhithReturn || a.ServiceType == AfterSaleServiceTypeEnum.RefundWhitoutReturn)).Count() < orderDetialServiceInfo.Count())
                    {
                        responseDto.AfterSeriveDescription = $"部分已退款";
                    }
                }
                #endregion

                foreach (var orderDetailModel in orderDetailModelList)
                {
                    var productModel = await new ProductBiz().GetModelByGuidAsync(orderDetailModel.ProductGuid);
                    var accessoryModel = new AccessoryBiz().GetAccessoryModelByGuid(productModel.PictureGuid);
                    var orderDetail = new GetMyOrderListResponseItemDto.OrderDetail
                    {
                        DetailGuid = orderDetailModel.DetailGuid,
                        ProductGuid = orderDetailModel.ProductGuid,
                        ProductName = orderDetailModel.ProductName,
                        ProductPicUrl = accessoryModel?.BasePath + accessoryModel?.RelativePath,
                        Price = orderDetailModel.ProductPrice,
                        Count = orderDetailModel.ProductCount,
                        CommentGuid = orderDetailModel.CommentGuid,
                        OnSale = productModel.OnSale
                    };
                    orderDetailList.Add(orderDetail);
                }
                responseDto.OrderDetailList = orderDetailList;
                responseDtoList.Add(responseDto);
            }
            return Success(new GetMyOrderListResponseDto
            {
                Total = models.Total,
                CurrentPage = responseDtoList
            });
        }

        /// <summary>
        /// （新）-可使用订单列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<List<GetUseableOrderListOfCosmetologyResponseDto>>))]
        public async Task<IActionResult> GetUseableOrderListOfCosmetologyAsync([FromBody]GetUseableOrderListOfCosmetologyRequestDto requestDto)
        {
            (IEnumerable<GoodsItemDetailDto> pages, int count) = await new GoodsBiz().GetUseableGoodsListOfCosmetologyAsync(UserID, requestDto);
            IEnumerable<IGrouping<string, GoodsItemDetailDto>> goodsGroups = pages.GroupBy(a => a.GoodsGuid);
            var response = new GetUseableOrderListOfCosmetologyResponseDto
            {
                Total = count
            };
            var lst = new List<GetUseableOrderListOfCosmetologyItemDto>();
            foreach (IGrouping<string, GoodsItemDetailDto> item in goodsGroups)
            {
                if (lst.Where(a => a.GoodsGuid == item.Key).Count() > 0)
                {
                    continue;
                }
                var goods = new GetUseableOrderListOfCosmetologyItemDto
                {
                    GoodsGuid = item.Key,
                    Available = item.ToList().FirstOrDefault().Available,
                    ProductName = item.ToList().FirstOrDefault().ProductName,
                    ProjectThreshold = item.ToList().FirstOrDefault().ProjectThreshold,
                    ProductPicture = item.ToList().FirstOrDefault().ProductPicture,
                    OrderNo = item.ToList().FirstOrDefault().OrderNo,
                    OrderCategory = item.ToList().FirstOrDefault().OrderCategory,
                    OrderDate = item.ToList().FirstOrDefault().OrderDate,
                    EffectiveStartDate = item.ToList().FirstOrDefault().EffectiveStartDate,
                    EffectiveEndDate = item.ToList().FirstOrDefault().EffectiveEndDate,
                    IsEffective = item.ToList().FirstOrDefault().IsEffective,
                    GoodsItems = item.ToList().Select(a => new GoodsItemResponseDto
                    {
                        GoodsItemGuid = a.GoodsItemGuid,
                        ProjectGuid = a.ProjectGuid,
                        ProjectName = a.ProjectName,
                        OperationTime = a.OperationTime,
                        ItemCount = a.ItemCount,
                        ItemRemain = a.ItemRemain,
                        ItemAvailable = a.ItemAvailable
                    }).Distinct().ToList()
                };
                lst.Add(goods);
            }
            response.CurrentPage = lst.OrderByDescending(a => a.OrderDate).ToList();
            return Success(response);
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetOrderDetailResponse>))]
        public async Task<IActionResult> GetOrderDetailAsync(GetOrderDetailRequest request)
        {
            var orderBiz = new OrderBiz();
            var orderModel = orderBiz.Getmodel(request.OrderGuid);
            if (orderModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "找不到该订单数据，请检查！");
            }
            var response = orderModel.ToDto<GetOrderDetailResponse>();
            response.MerchantName = orderModel.OrderMark.Equals(OrderMarkEnum.Primary.ToString()) ? "智慧云医" : new MerchantBiz().GetModelNoEnable(orderModel.MerchantGuid)?.MerchantName;
            var orderDetailList = await new OrderDetailBiz().GetModelsByOrderIdAsync(orderModel.OrderGuid);
            var itemList = new List<GetOrderDetailResponse.OrderDetailInfo>();
            var accBiz = new AccessoryBiz();
            var productBiz = new ProductBiz();

            var settings = Factory.GetSettings("host.json");
            var orderDeadline = settings["OrderDeadline"];
            var iOrderDeadline = 24 * 60 + 1;
            int.TryParse(orderDeadline, out iOrderDeadline);

            foreach (var item in orderDetailList)
            {
                var productModel = productBiz.GetModelByGuid(item.ProductGuid);
                var AccModel = await accBiz.GetAsync(productModel == null ? "1" : string.IsNullOrWhiteSpace(productModel.PictureGuid) ? "1" : productModel.PictureGuid);
                var detail = new GetOrderDetailResponse.OrderDetailInfo
                {
                    DetailGuid = item.DetailGuid,
                    ProductGuid = item.ProductGuid,
                    ProductName = item.ProductName,
                    ProductCount = item.ProductCount,
                    ProductPicURL = AccModel?.BasePath + AccModel?.RelativePath,
                    ProductPrice = item.ProductPrice
                };
                itemList.Add(detail);
            }
            response.OrderDetailInfoList = itemList;
            //非线下付款的待付款订单，给予付款截止日期
            if (orderModel.OrderStatus == OrderStatusEnum.Obligation.ToString() && orderModel.PayType == PayTypeEnum.Wechat.ToString())
            {
                response.PaymentDeadline = orderModel.CreationDate.AddMinutes(iOrderDeadline);
            }
            return Success(response);
        }

        /// <summary>
        /// 再次购买( 即加入商品到购物车，成功跳转购物车)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> BuyProductListAgainAsync([FromBody]BuyProductListAgainRequest request)
        {
            var orderDetailBiz = new OrderDetailBiz();
            var orderDetailModelList = await orderDetailBiz.GetOrderDetailModelListByOrderGuidAsync(request.OrderGuid);
            if (orderDetailModelList == null)
            {
                return Failed(ErrorCode.Empty, "没有找到该订单数据！");
            }
            var shoppingCartBiz = new ShoppingCarBiz();
            var addShoppingCarModelList = new List<ShoppingCarModel>();
            var updateShoppingCarModelList = new List<ShoppingCarModel>();
            var productBiz = new ProductBiz();
            var checkOrderProduct = true;
            foreach (var item in orderDetailModelList)
            {
                var productModel = await productBiz.GetModelByGuidAsync(item.ProductGuid);
                if (!productModel.OnSale)
                {
                    checkOrderProduct = false;
                }
                if (string.Equals(productModel.ProductForm, ProductFormEnum.Physical) && productModel.Inventory < item.ProductCount)
                {
                    checkOrderProduct = false;
                }
                if (!checkOrderProduct)
                {
                    return Failed(ErrorCode.UserData, "商品库存不足或已下架");
                }

                var checkShoppingCart = await shoppingCartBiz.GetShoppingCarProductByUserId(UserID, item.ProductGuid);
                //若购物车不存在此商品，则新生成一条购物车记录;若购物车已存在，则跳过
                if (checkShoppingCart == null)
                {
                    var model = new ShoppingCarModel
                    {
                        ItemGuid = Guid.NewGuid().ToString("N"),
                        UserGuid = UserID,
                        MerchantGuid = productModel.MerchantGuid,
                        ProductGuid = productModel.ProductGuid,
                        ProductName = productModel.ProductName,
                        Count = item.ProductCount,
                        AdvancePayment = false,
                        IsValid = true,
                        CreatedBy = UserID,
                        LastUpdatedBy = UserID,
                        PlatformType = "CloudDoctor",
                        OrgGuid = string.Empty
                    };
                    addShoppingCarModelList.Add(model);
                }


            }
            if (!addShoppingCarModelList.Any())
            {
                return Success(true);
            }
            var isSuccess = await shoppingCartBiz.AddShoppingCarModelListAsnyc(addShoppingCarModelList);
            if (!isSuccess)
            {
                return Failed(ErrorCode.Empty, "添加购物车失败！");
            }
            return Success(isSuccess);
        }

        /// <summary>
        /// 待评价项目和产品
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        private async Task<IActionResult> NoConmentProductOrProjectListAsync(NoConmentProductOrProjectListAsyncRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserGuid))
            {
                request.UserGuid = UserID;
            }

            return Success();
        }

        #endregion 订单及详情/待评价

        #region /取消订单/订单收货/立即购买

        /// <summary>
        /// 取消订单（针对未支付订单）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CancelOrder(CancelOrderRequestDto requestDto)
        {
            var orderBiz = new OrderBiz();
            var orderModel = orderBiz.Getmodel(requestDto.OrderGuid);
            if (orderModel != null)
            {
                if (orderModel.OrderStatus.ToLower().Equals(OrderStatusEnum.Obligation.ToString().ToLower()))
                {
                    orderModel.OrderStatus = OrderStatusEnum.Canceled.ToString();
                    var isUpdateSuccess = await orderBiz.CancelOrderAsync(orderModel);
                    return isUpdateSuccess ? Success() : Failed(ErrorCode.DataBaseError, "取消订单失败！");
                }
                return Failed(ErrorCode.DataBaseError, "该订单状态不支持取消，请检查！");
            }
            return Failed(ErrorCode.Empty, "没有找到该订单数据！");
        }

        /// <summary>
        /// 订单收货
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult TakeDeliveryOfOrder(string orderGuid)
        {
            if (string.IsNullOrWhiteSpace(orderGuid))
            {
                return Failed(ErrorCode.Empty, "请输入订单号！");
            }
            var orderBiz = new OrderBiz();
            var orderModel = orderBiz.Getmodel(orderGuid);
            if (orderModel == null)
            {
                return Failed(ErrorCode.Empty, "请输入正确订单号！");
            }
            orderModel.OrderStatus = OrderStatusEnum.Completed.ToString();
            var isSucc = orderBiz.UpdateModel(orderModel);
            if (isSucc)
            {
                //订单完成后生成订单下待评价商品记录
                CreateOrderProductComment(new List<string> { orderGuid });//实体订单完成后，生成评价记录
                return Success(isSucc);
            }
            return Failed(ErrorCode.DataBaseError, "更新失败！");
        }

        /// <summary>
        /// 自动确认收货（发货超15天）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> TakeDeliveryAutomaticallyAsync()
        {
            var biz = new OrderBiz();
            var models = await biz.GetOrdersWithoutReceivingBeyondDeadlineAsync(UserID);
            if (!models.Any())
            {
                return Success();
            }
            Logger.Debug($"用户[{UserID}]自动确认收货(TakeDeliveryAutomaticallyAsync)-OrderIds-[{JsonConvert.SerializeObject(models.Select(a => a.OrderGuid))}]");
            models.ForEach(a =>
            {
                a.OrderStatus = OrderStatusEnum.Completed.ToString();
                a.LastUpdatedBy = UserID;
                a.LastUpdatedDate = DateTime.Now;
            });
            var result = await biz.UpdateModelsAsync(models);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "自动确认收货失败");
        }

        /// <summary>
        ///立即购买
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<BuyNowResponse>))]
        public async Task<IActionResult> BuyNow(BuyNowRequest request)
        {
            var productBiz = new ProductBiz();
            var productModel = await productBiz.GetModelByGuidAsync(request.ProductGuid);
            var isRightStatus = productModel.OnSale ? productModel.ProductForm.Equals(ProductFormEnum.Physical.ToString()) ? productModel.Inventory > request.ProductNum || productModel.Inventory == request.ProductNum ? true : false : true : false;
            var model = new ShoppingCarModel();
            if (isRightStatus)
            {
                model = new ShoppingCarModel
                {
                    ItemGuid = Guid.NewGuid().ToString("N"),
                    UserGuid = UserID,
                    MerchantGuid = productModel.MerchantGuid,
                    ProductGuid = productModel.ProductGuid,
                    ProductName = productModel.ProductName,
                    Count = request.ProductNum,
                    AdvancePayment = false,
                    IsValid = true,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    PlatformType = "CloudDoctor",
                    OrgGuid = string.Empty,
                    Enable = false
                };
                var isSuccess = await new ShoppingCarBiz().AddShoppingCarModelListAsnyc(new List<ShoppingCarModel> { model });
                if (!isSuccess)
                {
                    return Failed(ErrorCode.Empty, "添加购物车失败！");
                }
                var response = model.ToDto<BuyNowResponse>();
                var merchantModel = await new MerchantBiz().GetModelAsync(model.MerchantGuid);
                response.MerchantName = merchantModel?.MerchantName;
                response.Freight = productModel.Freight ?? 0M;
                response.ProductForm = productModel.ProductForm;
                var AccModel = await new AccessoryBiz().GetAsync(productModel == null ? "1" : string.IsNullOrWhiteSpace(productModel.PictureGuid) ? "1" : productModel.PictureGuid);
                response.ProductPicURL = AccModel?.BasePath + AccModel?.RelativePath;
                return Success(response);
            }
            return Failed(ErrorCode.Empty, "已下架或库存不够，请检查！");
        }

        /// <summary>
        ///更新该Item在购物车的状态
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult UpdateShoppingCarItemStatus(string itemGuid)
        {
            if (string.IsNullOrWhiteSpace(itemGuid))
            {
                return Failed(ErrorCode.Empty, "请填写正确的ItemGuid！");
            }
            var shoppingCarBiz = new ShoppingCarBiz();
            var shoppingCarModel = shoppingCarBiz.GetModelByGuid(itemGuid, false);
            if (shoppingCarModel != null && shoppingCarModel.Enable == false)
            {
                shoppingCarModel.Enable = true;
                var isSuccess = shoppingCarBiz.UpdateModels(new List<ShoppingCarModel> { shoppingCarModel });
                if (!isSuccess)
                {
                    return Failed(ErrorCode.Empty, "添加购物车失败！");
                }
                return Success(isSuccess);
            }
            return Failed(ErrorCode.Empty, "无法获取该Item数据！");
        }

        #endregion /取消订单/订单收货/立即购买

        /// <summary>
        /// 获取当前用户待使用的项目数量
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<int>))]
        public async Task<IActionResult> GetCloudUseProjectNumAsync()
        {
            var counNum = await new GoodsBiz().GetUseableGoodsItemNumAsync(UserID);
            return Success(counNum);
        }

        /// <summary>
        /// 订单售后（退款）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult UpdateOrderAfterSale(string productGuid)
        {
            return Failed(ErrorCode.Empty, "功能待开放！");
        }

        /// <summary>
        /// 搜索商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<SearchProductResponseDto>))]
        public async Task<IActionResult> SearchProductAsync([FromBody]SearchProductRequestDto request)
        {
            CommonBiz commonBiz = new CommonBiz();
            if (!string.IsNullOrEmpty(UserID))
            {
                commonBiz.SearchHistory(UserID, request.Keyword);
            }
            commonBiz.HotWordSearch(request.Keyword);
            var response = await new ProductBiz().SearchProductAsync(request);
            return Success(response);
        }

        /// <summary>
        /// 移除购物车商品记录
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult RemoveShoppingCarProduct([FromBody]string[] itemIds)
        {
            var models = new List<ShoppingCarModel>();
            var shoppingCarBiz = new ShoppingCarBiz();
            foreach (var item in itemIds)
            {
                var model = shoppingCarBiz.GetModelByGuid(item);
                model.Enable = false;
                models.Add(model);
            }
            var result = shoppingCarBiz.UpdateModels(models);
            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, "移除购物车商品记录失败");
            }
            return Success();
        }

        /// <summary>
        /// 修改购物车商品数量
        /// </summary>
        /// <param name="itemId">购物车记录Id</param>
        /// <param name="productNumber">商品数量</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto))]
        public IActionResult ChangeShoppingCarProductNumber(string itemId, int productNumber)
        {
            var shoppingCarBiz = new ShoppingCarBiz();

            var model = shoppingCarBiz.GetModelByGuid(itemId);
            if (model == null)
            {
                return Failed(ErrorCode.Empty, "没有查询到该记录！");
            }
            model.Count = productNumber;
            var result = shoppingCarBiz.Update(model) == 1;
            if (!result)
            {
                return Failed(ErrorCode.DataBaseError, "操作失败！");
            }
            return Success();
        }

        /// <summary>
        /// 获取首页热门商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetHomeHotProductItemDto>>))]
        public async Task<IActionResult> GetHomeHotProductAsync(GetHomeHotProductRequestDto request)
        {
            var response = await new ProductBiz().GetHomeHotProductAsync(request);
            return Success(response);
        }

        /// <summary>
        /// 普通待付款订单，若超过付款截止时间(默认为下单后24小时内)，则自动取消
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CloseDeadlineOrderAsync()
        {
            var orderBiz = new OrderBiz();
            var closeData = await orderBiz.GetDeadlineOrderAsync(UserID);
            if (!closeData.Any())
            {
                return Success();
            }
            var orderKeys = closeData.Select(a => a.OrderKey).Distinct().ToList();
            var result = await new OrderBiz().CloseDeadlineOrderAsync(closeData);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "未付款订单自动关闭失败");
        }


    }
}