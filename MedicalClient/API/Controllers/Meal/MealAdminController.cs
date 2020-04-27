using GD.API.Code;
using GD.Common;
using GD.Common.EnumDefine;
using GD.Dtos.Meal.MealAdmin;
using GD.Meal;
using GD.Models.Meal;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GD.API.Controllers.Meal
{
    /// <summary>
    /// 点餐后台管理员
    /// </summary>
    public class MealAdminController : MealBaseController
    {
        #region 管理员操作
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, Produces(typeof(ResponseDto<MealAdminLoginResponseDto>))]
        public async Task<IActionResult> Login([FromBody] MealAdminLoginRequestDto request)
        {
            var adminBiz = new MealAdminBiz();

            var model = await adminBiz.GetMealAdmin(request);

            if (model is null)
            {
                return Failed(ErrorCode.InvalidIdPassword, "账号或密码不正确");
            }

            var token = CreateToken(model.AdminGuid, UserType.Admin, request.Days);

            return Success(new MealAdminLoginResponseDto
            {
                HospitalName = model.HosName,
                HospitalId = model.HospitalGuid,
                UserName = model.AdminName,
                Token = token
            });
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Failed(ErrorCode.Empty, "参数为空");
            }

            if (string.IsNullOrEmpty(UserID))
            {
                return Failed(ErrorCode.Unauthorized);
            }

            var adminBiz = new MealAdminBiz();

            var result = await adminBiz.UpdateMealAdminPassword(UserID, password);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "密码修改失败！");
        }


        /// <summary>
        /// 重置管理员密码(运维专用)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RestMealAdminPassword(string userId, string newpassword)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newpassword))
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            var adminBiz = new MealAdminBiz();

            var result = await adminBiz.UpdateMealAdminPassword(userId, newpassword);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "密码修改失败！");
        }
        #endregion

        #region 餐别管理

        /// <summary>
        /// 获取餐别列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetCategoryListResponseDto>>))]
        public async Task<IActionResult> GetMealCategories([FromQuery]GetCategoryListRequestDto requestDto)
        {
            var categories = (List<GetCategoryListResponseDto>)null;

            var categoryBiz = new MealCategoryBiz();

            var modelCategories = await categoryBiz.GetMealCategories(requestDto);

            if (modelCategories is null || modelCategories.Count <= 0)
            {
                return Success(categories);
            }

            categories = modelCategories.Select(d => new GetCategoryListResponseDto()
            {
                CategoryGuid = d.CategoryGuid,
                CategoryName = d.CategoryName,
                MealStartTime = d.MealStartTime,
                MealEndTime = d.MealEndTime,
                CategoryAdvanceDay = d.CategoryAdvanceDay,
                CategoryScheduleTime = d.CategoryScheduleTime
            }).ToList();

            return Success(categories);
        }


        ResponseDto VerifyCategory(AddCategoryRequestDto requestDto)
        {
            TimeSpan.TryParse(requestDto.MealStartTime, out var startTime);

            if (startTime == TimeSpan.Zero)
            {
                return Failed(ErrorCode.Empty, "用餐开始时间格式不正确");
            }

            TimeSpan.TryParse(requestDto.MealEndTime, out var endTime);
            if (endTime == TimeSpan.Zero)
            {
                return Failed(ErrorCode.Empty, "用餐结束时间格式不正确");
            }

            if (startTime > endTime)
            {
                return Failed(ErrorCode.Empty, "用餐开始时间需小于用餐结束时间");
            }

            TimeSpan.TryParse(requestDto.CategoryScheduleTime, out var scheduleTime);
            if (scheduleTime == TimeSpan.Zero)
            {
                return Failed(ErrorCode.Empty, "用餐可预订时间格式不正确");
            }

            return Success();
        }

        /// <summary>
        /// 创建餐别
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> CreateCategory([FromBody]AddCategoryRequestDto category)
        {
            var verifyResult = VerifyCategory(category);
            if (verifyResult.Code != ErrorCode.Success) { return verifyResult; };

            var categoryBiz = new MealCategoryBiz();

            var exsitName = await categoryBiz.ExistCategoryName(category.HospitalGuid, category.CategoryName.Trim());

            if (exsitName)
            {
                return Failed(ErrorCode.Empty, $"餐别“{category.CategoryName}”已存在");
            }

            var model = category.ToModel<MealCategoryModel>();
            model.CategoryGuid = Guid.NewGuid().ToString("N");
            model.CreatedBy = UserID;
            model.LastUpdatedBy = UserID;
            model.OrgGuid = "";

            var result = await categoryBiz.CreateCategory(model);

            return !string.IsNullOrEmpty(result) ? Success()
                : Failed(ErrorCode.DataBaseError, "餐别创建失败！");
        }

        /// <summary>
        /// 更新餐别
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateCategory([FromBody] AddCategoryRequestDto category)
        {
            var verifyResult = VerifyCategory(category);
            if (verifyResult.Code != ErrorCode.Success) { return verifyResult; };

            if (string.IsNullOrEmpty(category.CategoryGuid) ||
                string.IsNullOrEmpty(category.HospitalGuid))
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            var categoryBiz = new MealCategoryBiz();

            var model = await categoryBiz.GetCategoryModelById(category.HospitalGuid, category.CategoryGuid);

            if (model is null)
            {
                return Failed(ErrorCode.Empty, $"餐别“{category.CategoryName}”不存在");
            }

            var exsitName = await categoryBiz.ExistCategoryName(category.HospitalGuid, category.CategoryName.Trim(), category.CategoryGuid);

            if (exsitName)
            {
                return Failed(ErrorCode.Empty, $"餐别“{category.CategoryName}”已存在,请修改");
            }

            model.CategoryAdvanceDay = category.CategoryAdvanceDay;
            model.CategoryName = category.CategoryName;
            model.CategoryScheduleTime = category.CategoryScheduleTime;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;
            model.MealStartTime = category.MealStartTime;
            model.MealEndTime = category.MealEndTime;

            var result = await categoryBiz.UpdateCategory(model);

            return result > 0 ? Success() : Failed(ErrorCode.DataBaseError, "餐别更新失败！");
        }

        /// <summary>
        /// 删除餐别
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="categoryGuid"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> DeleteCategory(string hospitalGuid, string categoryGuid)
        {
            if (string.IsNullOrEmpty(hospitalGuid) || string.IsNullOrEmpty(categoryGuid))
            {
                return Failed(ErrorCode.Empty, $"参数不正确");
            }

            var categoryBiz = new MealCategoryBiz();

            var model = await categoryBiz.GetCategoryModelById(hospitalGuid, categoryGuid);

            if (model is null)
            {
                return Failed(ErrorCode.Empty, "餐别不存在");
            }

            var exsitOrder = await categoryBiz.ExistOrder(categoryGuid);

            if (exsitOrder)
            {
                return Failed(ErrorCode.Empty, "该餐别下菜品已被下单,不支持删除");
            }

            model.Enable = false;

            var result = await categoryBiz.DeleteCategory(model);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "餐别删除失败！");
        }
        #endregion

        #region 菜品管理
        /// <summary>
        /// 获取菜品分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMealDishesListResponseDto>))]
        public async Task<IActionResult> GetMealDishes([FromQuery]GetMealDishesListRequestDto request)
        {
            var dishesBiz = new MealDishesBiz();

            return Success(await dishesBiz.GetMealDishes(request));
        }

        /// <summary>
        /// 创建菜品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> CreateMealDishes([FromBody]AddMealDishesRequestDto request)
        {
            var verifyResult = VerifyDishes(request);
            if (verifyResult.Code != ErrorCode.Success) { return verifyResult; };

            var dishesBiz = new MealDishesBiz();

            var model = request.ToModel<MealDishesModel>();
            model.DishesOnsale = 1;
            model.DishesGuid = Guid.NewGuid().ToString("N");
            model.CreatedBy = UserID;
            model.LastUpdatedBy = UserID;
            model.OrgGuid = "";

            var exsitName = await dishesBiz.ExistDishesName(request.HospitalGuid, request.DishesName.Trim());

            if (exsitName)
            {
                return Failed(ErrorCode.Empty, $"菜品“{request.DishesName}”已存在");
            }

            var result = await dishesBiz.CreateDish(model);

            return !string.IsNullOrEmpty(result) ? Success()
                : Failed(ErrorCode.DataBaseError, "菜品创建失败！");
        }

        /// <summary>
        /// 修改菜品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateMealDishes([FromBody]AddMealDishesRequestDto request)
        {
            if (string.IsNullOrEmpty(request.DishesGuid))
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            var verifyResult = VerifyDishes(request);
            if (verifyResult.Code != ErrorCode.Success) { return verifyResult; };

            var dishesBiz = new MealDishesBiz();

            var model = await dishesBiz.GetMealDishesModelById(request.HospitalGuid, request.DishesGuid);

            if (model is null)
            {
                return Failed(ErrorCode.Empty, "菜品不存在");
            }

            var existDishes = await dishesBiz.ExistDishesName(request.HospitalGuid, request.DishesName.Trim(), request.DishesGuid);

            if (existDishes)
            {
                return Failed(ErrorCode.Empty, $"菜品“{request.DishesName}已存在”");
            }

            model.DishesImg = request.DishesImg;
            model.DishesDescription = request.DishesDescription;
            model.DishesExternalPrice = request.DishesExternalPrice;
            model.DishesInternalPrice = request.DishesInternalPrice;
            model.DishesName = request.DishesName;
            model.LastUpdatedBy = UserID;
            model.LastUpdatedDate = DateTime.Now;

            var result = await dishesBiz.UpdateDish(model);

            return result > 0 ? Success() : Failed(ErrorCode.DataBaseError, "菜品更新失败！");
        }


        /// <summary>
        /// 菜品上下架
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="dishesGuid"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateMealDishesStatus(string hospitalGuid,
            string dishesGuid)
        {
            if (string.IsNullOrEmpty(hospitalGuid) || string.IsNullOrEmpty(dishesGuid))
            {
                return Failed(ErrorCode.Empty, "参数不正确");
            }

            var dishesBiz = new MealDishesBiz();

            var model = await dishesBiz.GetMealDishesModelById(hospitalGuid, dishesGuid);

            if (model is null)
            {
                return Failed(ErrorCode.Empty, "菜品不存在");
            }

            model.DishesOnsale = model.DishesOnsale == 0 ? (sbyte)1 : (sbyte)0;

            var result = await dishesBiz.UpdateDish(model);

            return result > 0 ? Success() : Failed(ErrorCode.DataBaseError, "菜品上下架失败！");
        }

        /// <summary>
        /// 校验菜品参数
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        ResponseDto VerifyDishes(AddMealDishesRequestDto requestDto)
        {
            if (requestDto.DishesInternalPrice == 0)
            {
                return Failed(ErrorCode.Empty, "内部价需要填写");
            }

            if (requestDto.DishesExternalPrice == 0)
            {
                return Failed(ErrorCode.Empty, "外部价需要填写");
            }

            if (!string.IsNullOrEmpty(requestDto.DishesDescription))
            {
                if (requestDto.DishesDescription.Length > 100)
                {
                    return Failed(ErrorCode.Empty, "菜品介绍超过100个字符串最大长度限制");
                }
            }

            return Success();
        }

        #endregion

        #region 会员流水
        /// <summary>
        /// 获取指定医院用户钱包账户明细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMealAccountDetailListResponseDto>))]
        public async Task<IActionResult> GetMealAccountDetails([FromQuery]GetMealAccountDetailListRequestDto request)
        {
            if (request.StartTime.HasValue && request.EndTime.HasValue)
            {
                if (request.StartTime > request.EndTime)
                {
                    return Failed(ErrorCode.Empty, "开始时间需小于结束时间");
                }
            }

            var accountDetailBiz = new MealAccountDetailBiz();

            var details = await accountDetailBiz.GetDetailsByHospitalGuid(request);

            return Success(details);
        }
        #endregion

        #region 账户管理
        /// <summary>
        /// 获取指定医院用户钱包账户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetMealAccountListResponseDto<MealAccountItem>>))]
        public async Task<IActionResult> GetMealAccounts([FromQuery] GetMealAccountListRequestDto request)
        {
            var accountBiz = new MealAccountBiz();
            var accounts = await accountBiz.GetMealAccounts(request);
            return Success(accounts);
        }

        /// <summary>
        /// 更新账户状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateMealAccountStatus([FromBody] ModifyMealAccountRequestDto request)
        {
            var result = false;

            var accountBiz = new MealAccountBiz();

            //若账户不存在，则创建个人充值账户
            var accountTypes = await accountBiz.GetAccountTypes(request.HospitalGuid, request.UserGuid);

            if (accountTypes is null || accountTypes.Count() <= 0)
            {
                var models = new List<MealAccountModel>()
                {
                    new MealAccountModel()
                    {
                        AccountBalance = 0,
                        AccountGuid = Guid.NewGuid().ToString("N"),
                        AccountType = MealAccountTypeEnum.Recharge.ToString(),
                        CreatedBy =  UserID,
                        HospitalGuid = request.HospitalGuid,
                        UserGuid = request.UserGuid,
                        LastUpdatedBy =  UserID,
                        OrgGuid = "",
                        UserType =  MealUserTypeEnum.External.ToString(),
                        Enable = false
                    }
                };

                result = await accountBiz.CreateAccount(models) ? true : false;
            }
            else
            {
                result = await accountBiz.UpdateAccountStatus(request);
            }

            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新状态失败！");
        }

        /// <summary>
        /// 更新用户账户身份
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> UpdateMealAccountIdentity([FromBody] ModifyMealAccountRequestDto request)
        {
            if (!request.UserType.Equals(MealUserTypeEnum.Internal) &&
                !request.UserType.Equals(MealUserTypeEnum.External))
            {
                return Failed(ErrorCode.Empty, "用户身份参数不正确！");
            }

            var accountBiz = new MealAccountBiz();

            var accountTypes = (await accountBiz.GetAccountTypes(request.HospitalGuid, request.UserGuid))
                .ToList();

            var addModels = new List<MealAccountModel>();

            var accountDetail = (MealAccountDetailModel)null;

            // 若搜索手机号码，账户不存在，则创建账户
            if (accountTypes is null || accountTypes.Count() <= 0)
            {
                // 若用户类型为内部人员，则创建个人充值账户和赠款账户
                if (request.UserType == MealUserTypeEnum.Internal)
                {
                    addModels.Add(new MealAccountModel()
                    {
                        AccountBalance = 0,
                        AccountGuid = Guid.NewGuid().ToString("N"),
                        AccountType = MealAccountTypeEnum.Recharge.ToString(),
                        CreatedBy = UserID,
                        HospitalGuid = request.HospitalGuid,
                        UserGuid = request.UserGuid,
                        LastUpdatedBy = UserID,
                        OrgGuid = "",
                        UserType = MealUserTypeEnum.Internal.ToString(),
                        Enable = true
                    });

                    addModels.Add(new MealAccountModel()
                    {
                        AccountBalance = 0,
                        AccountGuid = Guid.NewGuid().ToString("N"),
                        AccountType = MealAccountTypeEnum.Grant.ToString(),
                        CreatedBy = UserID,
                        HospitalGuid = request.HospitalGuid,
                        UserGuid = request.UserGuid,
                        LastUpdatedBy = UserID,
                        OrgGuid = "",
                        UserType = MealUserTypeEnum.Internal.ToString(),
                        Enable = true
                    });
                }
                else
                {
                    // 若用户类型为外部人员，则只创建个人充值账户
                    addModels.Add(new MealAccountModel()
                    {
                        AccountBalance = 0,
                        AccountGuid = Guid.NewGuid().ToString("N"),
                        AccountType = MealAccountTypeEnum.Recharge.ToString(),
                        CreatedBy = UserID,
                        HospitalGuid = request.HospitalGuid,
                        UserGuid = request.UserGuid,
                        LastUpdatedBy = UserID,
                        OrgGuid = "",
                        UserType = MealUserTypeEnum.External.ToString(),
                        Enable = true
                    });
                }
            }
            else
            {
                // 如果修改为内部人员，若赠款账户不存在，则创建
                if (request.UserType == MealUserTypeEnum.Internal)
                {
                    var grantAccount = accountTypes.FirstOrDefault(d => d.AccountType == MealAccountTypeEnum.Grant.ToString());

                    if (grantAccount is null)
                    {
                        addModels.Add(new MealAccountModel()
                        {
                            AccountBalance = 0,
                            AccountGuid = Guid.NewGuid().ToString("N"),
                            AccountType = MealAccountTypeEnum.Grant.ToString(),
                            CreatedBy = UserID,
                            HospitalGuid = request.HospitalGuid,
                            UserGuid = request.UserGuid,
                            LastUpdatedBy = UserID,
                            OrgGuid = "",
                            UserType = MealUserTypeEnum.Internal.ToString(),
                            Enable = true
                        });
                    }

                    accountTypes.All(d =>
                    {
                        d.UserType = MealUserTypeEnum.Internal.ToString();

                        // 只有当用户身份有外部账户切换为内部账户时，才更新账户为启用
                        if (d.UserType == MealUserTypeEnum.External.ToString())
                        {
                            d.Enable = true;
                        }
                        return true;
                    });
                }
                else
                {
                    // 如果修改为外部人员，若用户为内部人员且赠款账户存在，则清零,用户类型修改为外部人员
                    var updateModel = accountTypes.FirstOrDefault(d => d.AccountType == MealAccountTypeEnum.Grant.ToString());

                    if (updateModel != null && updateModel.AccountBalance > 0)
                    {
                        accountDetail = new MealAccountDetailModel()
                        {
                            AccountDetailBeforeFee = updateModel.AccountBalance,
                            AccountDetailFee = updateModel.AccountBalance,
                            AccountDetailAfterFee = 0,
                            AccountGuid = updateModel.AccountGuid,
                            AccountDetailType = MealAccountDetailTypeEnum.Deduction.ToString(),
                            AccountDetailGuid = Guid.NewGuid().ToString("N"),
                            AccountDetailDescription = "清零",
                            AccountDetailIncomeType = 1,
                            CreatedBy = UserID,
                            LastUpdatedBy = UserID,
                            OrgGuid = ""
                        };

                    }

                    accountTypes.All(d =>
                    {
                        // 只有当用户身份有内部账户切换为外部账户时，才更新账户为启用
                        if (d.UserType == MealUserTypeEnum.Internal.ToString())
                        {
                            d.Enable = true;
                        }
                        d.UserType = MealUserTypeEnum.External.ToString();
                        if (d.AccountType == MealAccountTypeEnum.Grant.ToString())
                        {
                            d.AccountBalance = 0;
                        }
                        return true;
                    });
                }
            }

            var result = await accountBiz.UpdateAccountIdentity(addModels, accountTypes, accountDetail);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "更新身份失败！");
        }
        #endregion

        #region 充值管理

        /// <summary>
        /// 个人充值账户充值或扣减
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> RechargeAccountRechargeOrDeduction([FromBody]
        AddRechargeAccountRequestDto request)
        {
            if (!request.Type.Equals(MealAccountDetailTypeEnum.Deduction) &&
                !request.Type.Equals(MealAccountDetailTypeEnum.Recharge))
            {
                return Failed(ErrorCode.Empty, "个人账户充值或扣减参数类型不正确！");
            }

            if (request.Fee <= 0)
            {
                return Failed(ErrorCode.Empty, "金额需大于0！");
            }

            var accountBiz = new MealAccountBiz();

            var addModels = (List<MealAccountModel>)null;

            var updateModels = (List<MealAccountModel>)null;

            var accountDetailModels = (List<MealAccountDetailModel>)null;

            var accountTypes = await accountBiz.GetAccountTypes(request.HospitalGuid, request.UserGuid);

            // 个人账户不存在，则无法进行扣减
            if ((accountTypes is null || accountTypes.Count() <= 0) &&
                request.Type == MealAccountDetailTypeEnum.Deduction)
            {
                return Failed(ErrorCode.Empty, "账户不存在，请先充值然后扣减！");
            }

            var rechargeAccount = accountTypes.FirstOrDefault(d => d.AccountType == MealAccountTypeEnum.Recharge.ToString());

            // 查询是否已存在赠款账户即属于内部员工，那么创建的个人充值账户用户类型应属于内部人员
            var userType = accountTypes.Any(d => d.UserType == MealUserTypeEnum.Internal.ToString());

            // 若个人账户不存在，则创建个人充值账户
            if (rechargeAccount is null)
            {
                // 创建个人充值账户
                rechargeAccount = new MealAccountModel()
                {
                    AccountGuid = Guid.NewGuid().ToString("N"),
                    AccountType = MealAccountTypeEnum.Recharge.ToString(),
                    CreatedBy = UserID,
                    HospitalGuid = request.HospitalGuid,
                    UserGuid = request.UserGuid,
                    LastUpdatedBy = UserID,
                    OrgGuid = "",
                    UserType = userType ? MealUserTypeEnum.Internal.ToString() : MealUserTypeEnum.External.ToString(),
                    Enable = true
                };

                addModels = new List<MealAccountModel>
                {
                    rechargeAccount
                };
            }
            else
            {
                if (!rechargeAccount.Enable)
                {
                    return Failed(ErrorCode.Empty, "个人账户已被“禁用”，无法进行此项操作！");
                }

                // 更新账户余额
                updateModels = new List<MealAccountModel>()
                {
                    rechargeAccount
                };
            }

            // 添加账户充值或扣减明细
            var accountDetail = new MealAccountDetailModel()
            {
                AccountDetailFee = request.Fee,
                AccountDetailType = request.Type.ToString(),
                AccountDetailGuid = Guid.NewGuid().ToString("N"),
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = ""
            };

            accountDetail.AccountDetailBeforeFee = rechargeAccount.AccountBalance;
            accountDetail.AccountGuid = rechargeAccount.AccountGuid;

            if (request.Type == MealAccountDetailTypeEnum.Deduction)
            {
                if (rechargeAccount.AccountBalance < request.Fee)
                {
                    return Failed(ErrorCode.Empty, "账户余额需大于扣减金额！");
                }

                rechargeAccount.AccountBalance -= request.Fee;

                accountDetail.AccountDetailIncomeType = 1;
                accountDetail.AccountDetailDescription = "个人扣减";
            }
            else
            {
                rechargeAccount.AccountBalance += request.Fee;

                accountDetail.AccountDetailIncomeType = 0;
                accountDetail.AccountDetailDescription = "个人充值";
            }

            accountDetail.AccountDetailAfterFee = rechargeAccount.AccountBalance;

            accountDetailModels = new List<MealAccountDetailModel>() { accountDetail };

            var result = await accountBiz.AccountRechargeOrDeduction(addModels, updateModels, accountDetailModels);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "操作失败！");
        }

        /// <summary>
        /// 指定医院所有赠款账户充值或扣减
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> AllGrantAccountRechargeOrDeduction([FromBody]
        AddAllGrantAccountRequestDto request)
        {
            if (!request.Type.Equals(MealAccountDetailTypeEnum.Deduction) &&
             !request.Type.Equals(MealAccountDetailTypeEnum.Recharge))
            {
                return Failed(ErrorCode.Empty, "赠款账户充值或扣减参数类型不正确！");
            }

            if (request.Fee <= 0)
            {
                return Failed(ErrorCode.Empty, "赠款或扣减金额需大于0！");
            }

            var accountBiz = new MealAccountBiz();

            var granAccounts = await accountBiz.GetGrantAccounts(request.HospitalGuid);

            if (granAccounts.Count() <= 0)
            {
                return Failed(ErrorCode.Empty, "该医院下暂无赠款账户！");
            }

            var updateModels = new List<MealAccountModel>();

            var accountDetailModels = new List<MealAccountDetailModel>();

            foreach (var grantAccount in granAccounts)
            {
                var accountDetail = new MealAccountDetailModel()
                {
                    AccountDetailFee = request.Fee,
                    AccountDetailType = request.Type.ToString(),
                    AccountDetailGuid = Guid.NewGuid().ToString("N"),
                    AccountDetailBeforeFee = grantAccount.AccountBalance,
                    AccountGuid = grantAccount.AccountGuid,
                    CreatedBy = UserID,
                    LastUpdatedBy = UserID,
                    OrgGuid = ""
                };

                if (request.Type == MealAccountDetailTypeEnum.Deduction)
                {
                    // 当赠款账户余额为0时，不记录明细
                    if (grantAccount.AccountBalance <= 0)
                    {
                        continue;
                    }

                    // 赠款账户扣减，若账户余额小于扣减金额则账户重置为0
                    if (grantAccount.AccountBalance < request.Fee)
                    {
                        grantAccount.AccountBalance = 0;
                    }
                    else
                    {
                        grantAccount.AccountBalance -= request.Fee;
                    }

                    accountDetail.AccountDetailIncomeType = 1;
                    accountDetail.AccountDetailDescription = "赠款扣减";
                }
                else
                {
                    grantAccount.AccountBalance += request.Fee;

                    accountDetail.AccountDetailIncomeType = 0;
                    accountDetail.AccountDetailDescription = "赠款充值";
                }

                accountDetail.AccountDetailAfterFee = grantAccount.AccountBalance;
                accountDetailModels.Add(accountDetail);

                grantAccount.LastUpdatedBy = UserID;

                updateModels.Add(grantAccount);
            }

            var result = await accountBiz.AccountRechargeOrDeduction(null, updateModels, accountDetailModels);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "操作失败！");
        }

        /// <summary>
        /// 指定医院指定赠款账户充值或扣减
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> GrantAccountRechargeOrDeduction([FromBody]
        AddGrantAccountRequestDto request)
        {
            if (!request.Type.Equals(MealAccountDetailTypeEnum.Deduction) &&
              !request.Type.Equals(MealAccountDetailTypeEnum.Recharge))
            {
                return Failed(ErrorCode.Empty, "赠款账户充值或扣减参数类型不正确！");
            }

            if (string.IsNullOrEmpty(request.UserGuid))
            {
                return Failed(ErrorCode.Empty, "请选择需要充值或扣减的用户！");
            }

            if (request.Fee <= 0)
            {
                return Failed(ErrorCode.Empty, "金额需大于0！");
            }

            var accountBiz = new MealAccountBiz();

            var accountTypes = await accountBiz.GetAccountTypes(request.HospitalGuid, request.UserGuid);

            if (accountTypes is null || accountTypes.Count() <= 0)
            {
                return Failed(ErrorCode.Empty, "赠款账户只能应用于内部员工，请更换用户身份！");
            }

            var grantAccount = accountTypes.FirstOrDefault(d => d.AccountType == MealAccountTypeEnum.Grant.ToString());

            if (grantAccount is null)
            {
                return Failed(ErrorCode.Empty, "赠款账户只能应用于内部员工，请更换用户身份！");
            }

            if (!grantAccount.Enable)
            {
                return Failed(ErrorCode.Empty, "用户赠款账户已被“禁用”，无法进行此项操作！");
            }

            var accountDetail = new MealAccountDetailModel()
            {
                AccountDetailFee = request.Fee,
                AccountDetailType = request.Type.ToString(),
                AccountDetailGuid = Guid.NewGuid().ToString("N"),
                AccountDetailBeforeFee = grantAccount.AccountBalance,
                AccountGuid = grantAccount.AccountGuid,
                CreatedBy = UserID,
                LastUpdatedBy = UserID,
                OrgGuid = ""
            };

            if (request.Type == MealAccountDetailTypeEnum.Deduction)
            {
                // 当赠款账户余额为0时，不记录明细
                if (grantAccount.AccountBalance <= 0)
                {
                    return Success();
                }

                if (grantAccount.AccountBalance < request.Fee)
                {
                    return Failed(ErrorCode.Empty, "账户余额需大于扣减金额！");
                }

                grantAccount.AccountBalance -= request.Fee;

                accountDetail.AccountDetailIncomeType = 1;
                accountDetail.AccountDetailDescription = "赠款扣减";
            }
            else
            {
                grantAccount.AccountBalance += request.Fee;

                accountDetail.AccountDetailIncomeType = 0;
                accountDetail.AccountDetailDescription = "赠款充值";
            }

            accountDetail.AccountDetailAfterFee = grantAccount.AccountBalance;

            var accountDetailModels = new List<MealAccountDetailModel>
            {
                accountDetail
            };

            grantAccount.LastUpdatedBy = UserID;

            var updateModels = new List<MealAccountModel>
            {
                grantAccount
            };

            var result = await accountBiz.AccountRechargeOrDeduction(null, updateModels, accountDetailModels);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "操作失败！");
        }
        #endregion

        #region 食堂操作员管理

        /// <summary>
        /// 获取食堂操作员列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<IEnumerable<GetMealOperatorListResponseDto>>))]
        public async Task<IActionResult> GetMealOperators([FromQuery] GetMealOperatorListRequestDto request)
        {
            var operatorBiz = new MealOperatorBiz();

            return Success(await operatorBiz.GetMealOperators(request));
        }

        /// <summary>
        /// 删除操作员
        /// </summary>
        /// <param name="hospitalGuid"></param>
        /// <param name="operatorGuid"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> DeleteMealOperator(string hospitalGuid, string operatorGuid)
        {
            if (string.IsNullOrEmpty(hospitalGuid) || string.IsNullOrEmpty(operatorGuid))
            {
                return Failed(ErrorCode.Empty, "参数不正确！");
            }

            var operatorBiz = new MealOperatorBiz();

            var result = await operatorBiz.Delete(hospitalGuid, operatorGuid);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "删除失败！");
        }

        /// <summary>
        /// 重置操作员密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> ResetMealOperatorPassword([FromBody] ModifyMealOperatorPasswordRequestDto request)
        {
            var operatorBiz = new MealOperatorBiz();

            var exist = await operatorBiz.ExistOperator(request.HospitalGuid, request.OperatorGuid);
            if (!exist)
            {
                return Failed(ErrorCode.Empty, "操作员不存在！");
            }

            var model = request.ToModel<MealOperatorModel>();
            model.LastUpdatedBy = UserID;

            var result = await operatorBiz.UpdatePassword(model);

            return result ? Success() : Failed(ErrorCode.DataBaseError, "密码更新失败！");
        }

        /// <summary>
        /// 创建食堂操作员账户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<bool>))]
        public async Task<IActionResult> CreateMealOperator([FromBody]AddMealOperatorRequestDto request)
        {
            var operatorBiz = new MealOperatorBiz();

            if (await operatorBiz.ExistUserName(request.UserName))
            {
                return Failed(ErrorCode.Empty, $"手机号“{request.UserName}”已存在！");
            }

            var model = request.ToModel<MealOperatorModel>();
            model.OperatorGuid = Guid.NewGuid().ToString("N");
            model.CreatedBy = UserID;
            model.LastUpdatedBy = UserID;
            model.OrgGuid = "";

            var result = await operatorBiz.CreateMealOperator(model);

            return !string.IsNullOrEmpty(result) ? Success() : Failed(ErrorCode.DataBaseError, "创建操作员失败！");
        }
        #endregion
    }
}
