using GD.API.Code;
using GD.Common;
using GD.Dtos.Mall.Mall;
using GD.Dtos.Mall.Product;
using GD.Dtos.Manager.Banner;
using GD.Dtos.Merchant.Merchant;
using GD.Mall;
using GD.Manager;
using GD.Merchant;
using GD.Models.Mall;
using GD.Models.Manager;
using GD.Models.Utility;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GD.Models.Mall.ProductModel;

namespace GD.API.Controllers.Mall
{
    /// <summary>
    /// 商品控制器
    /// </summary>
    public class ProductController : MallBaseController
    {
        /// <summary>
        /// 获取商家端商品列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetProductListForMerchantManagementResponseDto>))]
        public async Task<IActionResult> GetProductListForMerchantManagementAsync([FromBody]GetProductListForMerchantManagementRequestDto requestDto)
        {
            requestDto.MerchantGuid = UserID;
            var response = await new ProductBiz().GetProductListForMerchantManagementAsync(requestDto);
            return Success(response);
        }
        /// <summary>
        /// 检查商品编码是否被占用
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns>是否被占用</returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> CheckProductCodeAsync([FromBody]CheckProductCodeRequestDto requestDto)
        {
            var result = true;
            var model = await new ProductBiz().GetByCodeAsync(requestDto.MerchantGuid, requestDto.ProductCode);
            if (model == null)
            {
                result = false;
            }
            else if (model.ProductGuid == requestDto.ProductGuid)
            {
                result = false;
            }
            return Success(result, null);
        }

        /// <summary>
        /// 获取实物产品一级分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<DictionaryModel>>))]
        public IActionResult GetPhysicalFirstClassifies()
        {
            var dictionaryBiz = new DictionaryBiz();

            var list = dictionaryBiz.GetDictionaryPhysicalFirstClassifies(UserID);
            return Success(list.Select(d => new
            {
                id = d.DicGuid,
                name = d.ConfigName
            }));
        }

        /// <summary>
        /// 获取实物产品二级分类列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<DictionaryModel>>))]
        public IActionResult GetDictionaryClassifyListByGuid(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            var dictionaryBiz = new DictionaryBiz();

            var list = dictionaryBiz.GetDictionaryClassifyListByGuid(id);

            return Success(list.Select(d => new
            {
                id = d.DicGuid,
                name = d.ConfigName
            }));
        }

        #region 服务类产品

        /// <summary>
        /// 创建服务类产品
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CreateServiceProductAsync([FromBody]CreateServiceProductRequestDto requestDto)
        {
            if (string.IsNullOrEmpty(requestDto.PictureGuid))
            {
                return Failed(ErrorCode.Empty, "封面图片未上传");
            }

            var merchantModel = await new MerchantBiz().GetModelAsync(UserID);
            if (merchantModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到此商铺数据");
            }

            var productBiz = new ProductBiz();

            var checkProductName = await productBiz.CheckProductNameRepeatAsync(requestDto.ProductName, UserID);

            if (checkProductName)
            {
                return Failed(ErrorCode.UserData, $"已存在“{requestDto.ProductName}”同名产品，请检查");
            }

            List<ProductModel> productModels = new List<ProductModel>();

            List<ProductProjectModel> productProjectModels = new List<ProductProjectModel>();

            List<RichtextModel> richtextModels = new List<RichtextModel>();

            List<BannerModel> bannerModels = new List<BannerModel>();

            var productGuid = Guid.NewGuid().ToString("N");

            List<RichtextModel> richtexts = new List<RichtextModel>();

            //商品介绍富文本
            var introduceTextModel = new RichtextModel
            {
                TextGuid = Guid.NewGuid().ToString("N"),
                OwnerGuid = productGuid,
                Content = requestDto.Introduce,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };

            //商品详情富文本
            var detailTextModel = new RichtextModel
            {
                TextGuid = Guid.NewGuid().ToString("N"),
                OwnerGuid = productGuid,
                Content = requestDto.ProductDetail,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };

            richtexts.AddRange(new List<RichtextModel> { introduceTextModel, detailTextModel });
            richtextModels.AddRange(richtexts);

            List<BannerModel> banners = new List<BannerModel>();

            var sort = 1;

            foreach (var item in requestDto.Banners)
            {
                banners.Add(new BannerModel
                {
                    BannerGuid = Guid.NewGuid().ToString("N"),
                    OwnerGuid = productGuid,
                    PictureGuid = item.PictureGuid,
                    TargetUrl = item.TargetUrl,
                    Sort = sort,
                    BannerName = string.IsNullOrWhiteSpace(item.BannerName) ? $"banner{sort}" : item.BannerName,
                    Description = item.Description,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                });
                sort++;
            }

            bannerModels.AddRange(banners);

            var productModel = new ProductModel
            {
                ProductGuid = productGuid,
                ProductCode = Guid.NewGuid().ToString("N"),
                ProductTitle = requestDto.ProductTitle,
                MerchantGuid = UserID,
                CategoryGuid = requestDto.CategoryGuid,
                CategoryName = requestDto.CategoryName,
                PictureGuid = requestDto.PictureGuid,
                ProductName = requestDto.ProductName,
                Price = requestDto.Price,
                IntroduceGuid = introduceTextModel.TextGuid,
                ProDetailGuid = detailTextModel.TextGuid,
                OnSale = requestDto.OnSale,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = "",
                ProductForm = ProductFormEnum.Service.ToString(),
                PlatformType = merchantModel.PlatformType,
                EffectiveDays = requestDto.EffectiveDays,
                ProjectThreshold = requestDto.ProjectThreshold,
                Recommend = requestDto.Recommend
            };

#warning 因未开通线上支付，现所有商品均为可预付款，预付款比例为0，即所有商品金额均到店支付

            productModels.Add(productModel);
            List<ProductProjectModel> productProjects = new List<ProductProjectModel>();
            if (requestDto.ProjectGuids != null)
            {
                productProjects.AddRange(requestDto.ProjectGuids.Select(a => new ProductProjectModel
                {
                    ProductProjectGuid = Guid.NewGuid().ToString("N"),
                    ProductGuid = productModel.ProductGuid,
                    ProjectGuid = a.ProjectGuid,
                    ProjectTimes = a.Infinite ? 999 : a.ProjectTimes,
                    PlatformType = merchantModel.PlatformType,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = ""
                }).ToList());
            }

            productProjectModels.AddRange(productProjects);

            var result = await productBiz.CreateProductAsync(productModels, richtextModels, bannerModels, productProjectModels);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "创建产品失败");
        }

        /// <summary>
        /// 获取商品下的服务项目列表
        /// </summary>
        /// <param name="productGuid">商品guid</param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<ProjectInfoDto>>)), AllowAnonymous]
        public async Task<IActionResult> GetProjectsOfProductAsync(string productGuid)
        {
            var response = await new ProjectBiz().GetModelsByProductIdAsync(productGuid);
            response.ForEach(a =>
            {
                a.ProjectTimes = a.Infinite ? 1 : a.ProjectTimes;
            });
            return Success(response);
        }

        /// <summary>
        /// 修改服务类产品基础信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ModifyServiceProductBasicInfoAsync([FromBody]ModifyPhysicalProductBasicInfoRequestDto requestDto)
        {
            if (string.IsNullOrEmpty(requestDto.PictureGuid))
            {
                return Failed(ErrorCode.Empty, "封面图片未上传");
            }

            if (requestDto.Price <= 0)
            {
                return Failed(ErrorCode.Empty, "价格必须大于0");
            }

            var productBiz = new ProductBiz();

            var productModel = await productBiz.GetModelByGuidAsync(requestDto.ProductGuid);

            if (productModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到此商品信息");
            }

            var checkProductName = await productBiz.CheckProductNameRepeatAsync(requestDto.ProductName, UserID, requestDto.ProductGuid);
            if (checkProductName)
            {
                return Failed(ErrorCode.UserData, "门店下已存在同名商品，请检查");
            }

            var category = (await new MerchantCategoryBiz().GetModelByClassifyGuidAsync(requestDto.CategoryGuid, UserID));
            if (category is null)
            {
                return Failed(ErrorCode.Empty, "产品分类不存在");
            }

            productModel.Recommend = requestDto.Recommend;
            productModel.ProductName = requestDto.ProductName;
            productModel.ProductTitle = requestDto.ProductTitle;
            productModel.Price = requestDto.Price;
            productModel.EffectiveDays = requestDto.EffectiveDays;
            productModel.PictureGuid = requestDto.PictureGuid;
            productModel.OnSale = requestDto.OnSale;
            productModel.CategoryGuid = requestDto.CategoryGuid;
            productModel.CategoryName = category.ClassifyName;
            productModel.LastUpdatedBy = UserID;
            productModel.LastUpdatedDate = DateTime.Now;

            var response = await productBiz.UpdateAsync(productModel);
            return response ? Success() : Failed(ErrorCode.DataBaseError, "修改商品基础信息失败");
        }

        /// <summary>
        /// 修改服务类商品下包含的服务项目
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ModifyProductProjectsRelationAsync([FromBody]ModifyProductProjectsRelationRequestDto requestDto)
        {
            var productBiz = new ProductBiz();

            var productModel = await productBiz.GetModelByGuidAsync(requestDto.ProductGuid);

            if (productModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到此商品信息");
            }

            //商品项目共可使用次数
            productModel.ProjectThreshold = requestDto.ProjectThreshold;
            productModel.LastUpdatedBy = UserID;
            productModel.LastUpdatedDate = DateTime.Now;

            List<ProductProjectModel> productProjectRelations = new List<ProductProjectModel>();

            List<ProductModel> prodcutModels = new List<ProductModel>
            {
                productModel
            };
            var models = requestDto.Projects.Select(a => new ProductProjectModel
            {
                ProductProjectGuid = Guid.NewGuid().ToString("N"),
                ProductGuid = requestDto.ProductGuid,
                ProjectGuid = a.ProjectGuid,
                ProjectTimes = a.Infinite ? 999 : a.ProjectTimes,
                PlatformType = productModel.PlatformType,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = ""
            }).ToList();

            productProjectRelations.AddRange(models);

            var result = await new ProductProjectBiz().CreateProductProjectRelation(prodcutModels, productProjectRelations);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "修改商品下包含的服务项目失败");
        }

        /// <summary>
        /// 修改服务类产品的Banner
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ModifyProductBannersAsync([FromBody]ModifyBannerInfoRequestDto requestDto)
        {
            var productBiz = new ProductBiz();
            var productModel = await productBiz.GetModelByGuidAsync(requestDto.OwnerGuid);
            if (productModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到此商品信息");
            }

            List<string> productIds = new List<string>
            {
                productModel.ProductGuid
            };

            List<BannerModel> banners = new List<BannerModel>();

            var sort = 1;

            var bannerModels = requestDto.Banners.Select(a => new BannerModel
            {
                BannerGuid = Guid.NewGuid().ToString("N"),
                OwnerGuid = requestDto.OwnerGuid,
                Sort = a.Sort ?? (a.Sort = sort++).Value,
                BannerName = string.IsNullOrWhiteSpace(a.BannerName) ? $"banner{a.Sort}" : a.BannerName,
                PictureGuid = a.PictureGuid,
                TargetUrl = a.TargetUrl,
                Description = a.Description,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            }).ToList();

            banners.AddRange(bannerModels);

            var result = await new BannerBiz().ModifyBannerInfoAsync(productIds, banners);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "修改banner数据出错");
        }
        #endregion

        /// <summary>
        /// 发布实体商品（智慧云医）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> CreatePhysicalProductAsync([FromBody]CreateProductOfDoctorCloudRequestDto requestDto)
        {
            if (string.IsNullOrEmpty(requestDto.PictureGuid))
            {
                return Failed(ErrorCode.Empty, "封面图片未上传");
            }

            requestDto.MerchantGuid = UserID;

            var merchantModel = await new MerchantBiz().GetModelAsync(requestDto.MerchantGuid);

            if (merchantModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到此商铺数据");
            }

            if (!string.IsNullOrEmpty(requestDto.SearchKey?.Trim()))
            {
                if (requestDto.SearchKey.Length > 30)
                {
                    return Failed(ErrorCode.Empty, "搜索关键词超过最大长度限制");
                }
            }

            if (!string.IsNullOrEmpty(requestDto.ProductTitle?.Trim()))
            {
                if (requestDto.ProductTitle.Length > 255)
                {
                    return Failed(ErrorCode.Empty, "商品副标题超过最大长度限制");
                }
            }

            if (requestDto.ApprovalNumber.Length > 30)
            {
                return Failed(ErrorCode.Empty, "批准文号超过最大长度限制");
            }

            if (requestDto.ProductCode.Length > 50)
            {
                return Failed(ErrorCode.Empty, "商品编码超过最大长度限制");
            }

            if (requestDto.Standerd.Length > 30)
            {
                return Failed(ErrorCode.Empty, "规格超过最大长度限制");
            }

            var productBiz = new ProductBiz();

            var checkProductName = await productBiz.CheckProductNameRepeatAsync(requestDto.ProductName, UserID);

            if (checkProductName)
            {
                return Failed(ErrorCode.UserData, $"已存在“{requestDto.ProductName}”同名产品，请检查");
            }

            var model = await productBiz.GetByCodeAsync(requestDto.MerchantGuid, requestDto.ProductCode);

            if (model != null)
            {
                return Failed(ErrorCode.UserData, $"商品编码“{requestDto.ProductCode}”被占用");
            }

            var productGuid = Guid.NewGuid().ToString("N");
            List<RichtextModel> richtexts = new List<RichtextModel>();
            //商品介绍富文本
            var introduceTextModel = new RichtextModel
            {
                TextGuid = Guid.NewGuid().ToString("N"),
                OwnerGuid = productGuid,
                Content = requestDto.Introduce,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };
            //商品详情富文本
            var detailTextModel = new RichtextModel
            {
                TextGuid = Guid.NewGuid().ToString("N"),
                OwnerGuid = productGuid,
                Content = requestDto.ProductDetail,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = string.Empty
            };
            richtexts.AddRange(new List<RichtextModel> { introduceTextModel, detailTextModel });

            List<BannerModel> banners = new List<BannerModel>();
            var sort = 1;
            foreach (var item in requestDto.Banners)
            {
                banners.Add(new BannerModel
                {
                    BannerGuid = Guid.NewGuid().ToString("N"),
                    OwnerGuid = productGuid,
                    PictureGuid = item.PictureGuid,
                    TargetUrl = item.TargetUrl,
                    Sort = item.Sort ?? (item.Sort = sort++).Value,
                    BannerName = string.IsNullOrWhiteSpace(item.BannerName) ? $"banner{item.Sort}" : item.BannerName,
                    Description = item.Description,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = string.Empty
                });
            }
            var categoryName = "";
            var categoryDicModel = await new DictionaryBiz().GetAsync(requestDto.CategoryGuid);
            categoryName = categoryDicModel?.ConfigName;
            var productModel = new ProductModel
            {
                ProductGuid = productGuid,
                ProductCode = requestDto.ProductCode,
                ProductTitle = requestDto.ProductTitle,
                MerchantGuid = requestDto.MerchantGuid,
                CategoryGuid = requestDto.CategoryGuid,
                CategoryName = categoryName ?? "",
                PictureGuid = requestDto.PictureGuid,
                ProductName = requestDto.ProductName,
                ProductLabel = requestDto.SearchKey,
                Brand = requestDto.BrandGuid,
                Standerd = requestDto.Standerd,
                RetentionPeriod = requestDto.RetentionPeriod,
                Manufacture = requestDto.Manufacture,
                ApprovalNumber = requestDto.ApprovalNumber,
                Price = requestDto.Price,
                CostPrice = requestDto.CostPrice,
                MarketPrice = requestDto.MarketPrice,
                Freight = requestDto.Freight,
                IntroduceGuid = introduceTextModel.TextGuid,
                ProDetailGuid = detailTextModel.TextGuid,
                Inventory = requestDto.Inventory,
                WarningInventory = requestDto.WarningInventory,
                OnSale = requestDto.OnSale,
                OperationTime = requestDto.OperationTime,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                ProductForm = ProductFormEnum.Physical.ToString(),
                OrgGuid = "",
                PlatformType = merchantModel.PlatformType
            };
            var result = await productBiz.CreateProductOfDoctorCloudAsync(productModel, richtexts, banners);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "发布商品失败");
        }

        /// <summary>
        /// 修改商品基础信息(智慧云医)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ModifyPhysicalProductBasicInfoAsync([FromBody]ModifyProductBasicInfoOfDoctorCloudRequestDto requestDto)
        {
            if (string.IsNullOrEmpty(requestDto.PictureGuid))
            {
                return Failed(ErrorCode.Empty, "封面图片未上传");
            }

            var productBiz = new ProductBiz();

            var productModel = await productBiz.GetModelByGuidAsync(requestDto.ProductGuid);

            if (productModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到此商品信息");
            }

            if (!string.IsNullOrEmpty(requestDto.SearchKey?.Trim()))
            {
                if (requestDto.SearchKey.Length > 30)
                {
                    return Failed(ErrorCode.Empty, "搜索关键词超过最大长度限制");
                }
            }

            if (!string.IsNullOrEmpty(requestDto.ProductTitle?.Trim()))
            {
                if (requestDto.ProductTitle.Length > 255)
                {
                    return Failed(ErrorCode.Empty, "商品副标题超过最大长度限制");
                }
            }

            if (requestDto.ApprovalNumber.Length > 30)
            {
                return Failed(ErrorCode.Empty, "批准文号超过最大长度限制");
            }

            if (requestDto.ProductCode.Length > 50)
            {
                return Failed(ErrorCode.Empty, "商品编码超过最大长度限制");
            }

            if (requestDto.Standerd.Length > 30)
            {
                return Failed(ErrorCode.Empty, "规格超过最大长度限制");
            }

            var checkProductName = await productBiz.CheckProductNameRepeatAsync(requestDto.ProductName, UserID, requestDto.ProductGuid);

            if (checkProductName)
            {
                return Failed(ErrorCode.UserData, "门店下已存在同名商品，请检查");
            }

            var model = await productBiz.GetByCodeAsync(UserID, requestDto.ProductCode);

            if (model != null && !model.ProductGuid.Equals(requestDto.ProductGuid))
            {
                return Failed(ErrorCode.DataBaseError, $"商品编码“{requestDto.ProductCode}”已被占用");
            }

            productModel.ApprovalNumber = requestDto.ApprovalNumber;
            productModel.ProductTitle = requestDto.ProductTitle;
            productModel.ProductLabel = requestDto.SearchKey;
            productModel.Brand = requestDto.BrandGuid;
            productModel.Manufacture = requestDto.Manufacture;
            productModel.RetentionPeriod = requestDto.RetentionPeriod;
            productModel.ProductName = requestDto.ProductName;
            productModel.PictureGuid = requestDto.PictureGuid;
            productModel.ProductCode = requestDto.ProductCode;
            productModel.Standerd = requestDto.Standerd;

            var response = await productBiz.UpdateAsync(productModel);
            return response ? Success() : Failed(ErrorCode.DataBaseError, "修改商品基础信息失败");
        }
        /// <summary>
        /// 修改商品分类信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ModifyProductCategoryInfoOfDoctorCloudAsync([FromBody]ModifyProductCategoryInfoRequestDto requestDto)
        {
            var productBiz = new ProductBiz();
            var productModel = await productBiz.GetModelByGuidAsync(requestDto.ProductGuid);
            if (productModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到此商品信息");
            }
            productModel.CategoryGuid = requestDto.CategoryGuid;
            var category = (await new DictionaryBiz().GetAsync(requestDto.CategoryGuid))?.ConfigName;
            productModel.CategoryName = category ?? "";
            var response = await productBiz.UpdateAsync(productModel);
            return response ? Success() : Failed(ErrorCode.DataBaseError, "修改商品分类信息失败");
        }

        /// <summary>
        /// 修改商品销售属性
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ModifyProductSalePropertyOfDoctorCloudAsync([FromBody]ModifyProductSalePropertyOfDoctorCloudRequestDto requestDto)
        {
            var productBiz = new ProductBiz();
            var productModel = await productBiz.GetModelByGuidAsync(requestDto.ProductGuid);
            if (productModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到此商品信息");
            }
            productModel.OnSale = requestDto.OnSale;
            productModel.Freight = requestDto.Freight;
            productModel.MarketPrice = requestDto.MarketPrice;
            productModel.CostPrice = requestDto.CostPrice;
            productModel.Price = requestDto.Price;
            var response = await productBiz.UpdateAsync(productModel);
            return response ? Success() : Failed(ErrorCode.DataBaseError, "修改商品销售属性失败");
        }
        /// <summary>
        /// 修改商品库存信息：追加库存、修改警戒库存（智慧云医）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ModifyProductInventoryOfDoctorCloudAsync([FromBody]ModifyProductInventoryOfDoctorCloudRequestDto requestDto)
        {
            var productBiz = new ProductBiz();
            var productModel = await productBiz.GetModelByGuidAsync(requestDto.ProductGuid);
            if (productModel == null)
            {
                return Failed(ErrorCode.Empty, "未查询到此商品信息");
            }
            productModel.WarningInventory = requestDto.WarningInventory;
            var resultWarningInventory = await productBiz.UpdateAsync(productModel);
            if (!resultWarningInventory) return Failed(ErrorCode.DataBaseError, "更新警戒库存失败");

            if (requestDto.ReplenishInventory > 0)
            {
                var resultReplenishInventory = await productBiz.ReplenishProductInventoryAsync(requestDto.ProductGuid, requestDto.ReplenishInventory);
                if (!resultReplenishInventory) return Failed(ErrorCode.DataBaseError, "追加库存信息失败");
            }
            return Success();
        }

        /// <summary>
        /// 获取商铺回收站中的商品列表（智慧云医）
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<GetProductsOfMerchantRecycleBinResponseDto>))]
        public async Task<IActionResult> GetProductsOfMerchantRecycleBinAsync([FromBody]GetProductsOfMerchantRecycleBinRequestDto requestDto)
        {
            var response = await new ProductBiz().GetProductsOfMerchantRecycleBinAsync(requestDto);
            return Success(response);
        }

        /// <summary>
        /// 修改商品上下架状态
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> ChangeProductsOnSaleStatusAsync([FromBody]ChangeProductsOnSaleStatusRequestDto requestDto)
        {
            var status = requestDto.OnSale ? "上架" : "下架";
            requestDto.MerchantGuid = UserID;
            var result = await new ProductBiz().ChangeProductsOnSaleStatusAsync(requestDto);
            return result ? Success() : Failed(ErrorCode.DataBaseError, $"{status}商品失败");
        }

        /// <summary>
        /// 移除商品到回收站
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> RemoveProductToRecycleBinAsync([FromBody]RemoveProductToRecycleBinRequestDto requestDto)
        {
            var result = await new ProductBiz().RemoveOrRestoreProductsAsync(requestDto.MerchantGuid, requestDto.ProductIds.ToArray(), false);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "移除商品到回收站失败");
        }
        /// <summary>
        /// 从回收站恢复商品
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> RestoreProductFromRecycleBinAsync([FromBody]RemoveProductToRecycleBinRequestDto requestDto)
        {
            var result = await new ProductBiz().RemoveOrRestoreProductsAsync(requestDto.MerchantGuid, requestDto.ProductIds.ToArray(), true);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "从回收站恢复商品失败");
        }

        /// <summary>
        /// 从回收站彻底删除商铺的商品
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> DeleteProductsCompletelyAsync([FromBody]DeleteProductsCompletelyRequestDto requestDto)
        {
            var result = await new ProductBiz().DeleteProductsCompletelyAsync(requestDto);
            return result ? Success() : Failed(ErrorCode.DataBaseError, "彻底删除商品失败");
        }

        /// <summary>
        /// 获取商品详情（除富文本外的所有数据）
        /// </summary>
        /// <param name="productGuid"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetProductInfoResponseDto>))]
        public async Task<IActionResult> GetProductInfoAsync(string productGuid)
        {
            if (string.IsNullOrWhiteSpace(productGuid))
            {
                return Failed(ErrorCode.UserData, "商品guid必填");
            }

            var productModel = await new ProductBiz().GetModelByGuidAsync(productGuid);

            if (productModel == null)
            {
                return Failed(ErrorCode.Empty, "未查到该商品数据");
            }

            var response = productModel.ToDto<GetProductInfoResponseDto>();

            response.Banners = (await new BannerBiz().GetBannerInfoAsync(productModel.ProductGuid))?.ToList();

            var twoLevel = await new DictionaryBiz().GetAsync(response.CategoryGuid);

            var productPicture = await new AccessoryBiz().GetAsync(response.PictureGuid);
            response.PictureUrl = $"{productPicture?.BasePath}{productPicture?.RelativePath}";

            response.OneLevelCategoryGuid = twoLevel?.ParentGuid;
            return Success(response);
        }
        /// <summary>
        /// 获取推荐商品(明星产品数据)
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetProductListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetRecommendProductAsync(GetRecommendProductListRequestDto requestDto)
        {
            var result = await new ProductBiz().GetRecommendProductListAsync(requestDto);
            return Success(result);
        }
        /// <summary>
        /// 获取商品(根据一级产品类型)
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetProductListResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> GetByCategoryIdProductAsync(GetByCategoryProductListRequestDto requestDto)
        {
            var result = await new ProductBiz().GetProductListAsync(requestDto);
            return Success(result);
        }
    }
}
