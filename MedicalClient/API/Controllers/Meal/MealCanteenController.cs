using GD.Common;
using GD.Common.Helper;
using GD.Dtos.Meal.MealCanteen;
using GD.Meal;
using GD.Models.Meal;
using GD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.API.Controllers.Meal
{
    /// <summary>
    /// 食堂端
    /// </summary>
    public class MealCanteenController : MealBaseController
    {
        /// <summary>
        /// 获取已预订菜品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetBookMealListAsyncResponseDto>))]
        public async Task<IActionResult> GetBookMealListAsync(GetBookMealListAsyncRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.HospitalGuid))
            {
                var userModel = await new MealOperatorBiz().GetModelAsync(UserID);
                if (userModel == null)
                {
                    return Failed(ErrorCode.DataBaseError, "无法获取医院所在信息！");
                }
                request.HospitalGuid = userModel.HospitalGuid;
            }
            //做分组等
            var responseList = await new MealMenuBiz().GetBookMealListAsync(request);
            var responseGroup = responseList.GroupBy(a => a.CategoryName);
            List<GetBookMealListAsyncReturnResponseDto> returnResponse = new List<GetBookMealListAsyncReturnResponseDto>();
            foreach (var item in responseGroup)
            {
                var firstOrDefault = item.FirstOrDefault();
                string[] hoursArr = firstOrDefault.CategoryScheduleTime.Trim().Split(":");
                int housrs = Convert.ToInt32(hoursArr[0]);
                int minute = Convert.ToInt32(hoursArr[1]);
                var expirationDateTime = firstOrDefault.MealDate.Value.AddDays(-firstOrDefault.CategoryAdvanceDay).AddHours(housrs).AddMinutes(minute);
                bool isBigger = expirationDateTime > DateTime.Now;
                var resp = new GetBookMealListAsyncReturnResponseDto
                {
                    CategoryGuid = firstOrDefault.CategoryGuid,
                    CategoryName = firstOrDefault.CategoryName,
                    CategoryAdvanceDay = firstOrDefault.CategoryAdvanceDay,
                    CategoryScheduleTime = firstOrDefault.CategoryScheduleTime,
                    HosName = firstOrDefault.HosName,
                    MenuDate = firstOrDefault.MealDate,
                    IsExpirationBook = isBigger
                };
                var respDetailList = new List<GetBookMealListAsyncReturnResponseDto.BookedDishesInfo>();
                foreach (var detail in item)
                {
                    var respDetail = new GetBookMealListAsyncReturnResponseDto.BookedDishesInfo
                    {
                        DishesGuid = detail.DishesGuid,
                        DishesName = detail.DishesName,
                        BookedTotal = detail.BookedTotal
                    };
                    respDetailList.Add(respDetail);
                }
                resp.BookedDishesInfoList = respDetailList;
                returnResponse.Add(resp);
            }

            return Success(returnResponse);
        }

        /// <summary>
        /// 获取分类（如早中晚餐）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetCategoryListAsyncResponseDto>))]
        public async Task<IActionResult> GetCategoryListAsync(GetCategoryListAsyncRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.HospitalGuid))
            {
                var userModel = await new MealOperatorBiz().GetModelAsync(UserID);
                if (userModel == null)
                {
                    return Failed(ErrorCode.DataBaseError, "无法获取医院所在信息！");
                }
                request.HospitalGuid = userModel.HospitalGuid;
            }
            var response = await new MealCategoryBiz().GetCategoryListAsync(request);
            return Success(response);
        }
        /// <summary>
        /// 获取已填日期
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetUseFullDateListAsyncResponseDto>))]
        public async Task<IActionResult> GetUseFullDateListAsync(GetUseFullDateListAsyncRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.HospitalGuid))
            {
                var userModel = await new MealOperatorBiz().GetModelAsync(UserID);
                if (userModel == null)
                {
                    return Failed(ErrorCode.DataBaseError, "无法获取医院所在信息！");
                }
                request.HospitalGuid = userModel.HospitalGuid;
            }
            var response = await new MealCategoryBiz().GetUseFullDateListAsync(request);
            return Success(response);
        }

        /// <summary>
        /// 扫码验证
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<ScanToGetDisheAsyncResponseDto>))]
        public async Task<IActionResult> ScanToGetDisheAsync(ScanToGetDisheAsyncRequestDto request)
        {
            var orderModel = await new MealOrderBiz().GetModelAsync(request.OrderGuid);
            if (orderModel == null)
            {
                return Failed(ErrorCode.DataBaseError, "查询不到订单信息，请检查！");
            }
            if (!orderModel.OrderStatus.ToString().ToLower().Equals(MealOrderStatusEnum.Paided.ToString().ToLower()) || orderModel.Enable == false)
            {
                return Failed(ErrorCode.DataBaseError, "订单状态不可用，请检查！");
            }
            var userModel = await new MealOperatorBiz().GetModelAsync(UserID);
            if (!orderModel.HospitalGuid.Equals(userModel.HospitalGuid))
            {
                return Failed(ErrorCode.DataBaseError, "该订单无法在此医院就餐，请检查！");
            }
            var orderDetailModelList = await new MealOrderDetailBiz().GetModelsByOrderGuidAsync(orderModel.OrderGuid);
            if (orderDetailModelList == null || !orderDetailModelList.Any())
            {
                return Failed(ErrorCode.DataBaseError, "订单详情信息无数据，请检查！");
            }
            if (DateTime.Now < orderModel.MealStartTime)
            {
                return Failed(ErrorCode.UserData, "还未到就餐时间！");
            }

            if (orderModel.MealEndTime < DateTime.Now)
            {
                return Failed(ErrorCode.UserData, "已过就餐时间！");
            }
            //消费
            orderModel.OrderStatus = MealOrderStatusEnum.Completed.ToString();
            var IsRightConsume = await new MealOrderBiz().UpdateAsync(orderModel);
            if (!IsRightConsume)
            {
                return Failed(ErrorCode.DataBaseError, "验证失败，请联系管理员！");
            }
            ScanToGetDisheAsyncResponseDto response = new ScanToGetDisheAsyncResponseDto
            {
                OrderGuid = orderModel.OrderGuid,
                TotalPrice = orderModel.TotalPrice
            };
            List<ScanToGetDisheAsyncResponseDto.MealOrderDetailInfo> detailList = new List<ScanToGetDisheAsyncResponseDto.MealOrderDetailInfo>();
            foreach (var item in orderDetailModelList)
            {
                var detailModel = new ScanToGetDisheAsyncResponseDto.MealOrderDetailInfo()
                {
                    DishesGuid = item.DishesGuid,
                    DishesName = item.DishesName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };
                detailList.Add(detailModel);
            }
            response.DishesDetail = detailList;
            return Success(response);
        }

        /// <summary>
        /// 菜品维护-查询
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<GetDisheMaintenanceAsyncResponseDto>))]
        public async Task<IActionResult> GetDisheMaintenanceAsync(GetDisheMaintenanceAsyncRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.HospitalGuid))
            {
                var userModel = await new MealOperatorBiz().GetModelAsync(UserID);
                if (userModel == null)
                {
                    return Failed(ErrorCode.DataBaseError, "无法获取医院所在信息！");
                }
                request.HospitalGuid = userModel.HospitalGuid;
            }
            var response = await new MealMenuBiz().GetDisheMaintenanceAsync(request);
            return Success(response);
        }

        /// <summary>
        /// 菜品维护-查询已选
        /// </summary>
        /// <returns></returns>
        [HttpGet, Produces(typeof(ResponseDto<List<GetCheckedDisheMaintenanceAsyncResponseDto>>))]
        public async Task<IActionResult> GetCheckedDisheMaintenanceAsync(GetCheckedDisheMaintenanceAsyncRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.HospitalGuid))
            {
                var userModel = await new MealOperatorBiz().GetModelAsync(UserID);
                if (userModel == null)
                {
                    return Failed(ErrorCode.DataBaseError, "无法获取医院所在信息！");
                }
                request.HospitalGuid = userModel.HospitalGuid;
            }
            GetDisheMaintenanceAsyncRequestDto newDto = new GetDisheMaintenanceAsyncRequestDto
            {
                CategoryGuid = request.CategoryGuid,
                Date = request.Date,
                HospitalGuid = request.HospitalGuid
            };
            var checkedMealMenuModelList = await new MealMenuBiz().GetDisheMaintenanceAsync(newDto);
            var allDisheModel = await new MealDishesBiz().GetModelsByHospitalGuidAsync(request.HospitalGuid);
            //var valList = checkedMealMenuModelList.Where(a => a.DishesGuid.Any(n => allDisheModel.Any(t => t.DishesGuid.Contains(n, StringComparison.InvariantCultureIgnoreCase))));
            List<GetCheckedDisheMaintenanceAsyncResponseDto> reponse = new List<GetCheckedDisheMaintenanceAsyncResponseDto>();
            foreach (var model in allDisheModel)
            {
                var newModel = new GetCheckedDisheMaintenanceAsyncResponseDto
                {
                    DishesGuid = model.DishesGuid,
                    DishesName = model.DishesName,
                    IsChecked = false
                };
                foreach (var item in checkedMealMenuModelList)
                {
                    if (model.DishesGuid.Equals(item.DishesGuid))
                    {
                        newModel.IsChecked = true;
                    }
                }
                reponse.Add(newModel);
            }
            return Success(reponse);
        }

        /// <summary>
        /// 菜品维护-添加
        /// </summary>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public async Task<IActionResult> AddDisheMaintenanceAsync([FromBody]AddDisheMaintenanceAsyncRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.HospitalGuid))
            {
                var userModel = await new MealOperatorBiz().GetModelAsync(UserID);
                if (userModel == null)
                {
                    return Failed(ErrorCode.DataBaseError, "无法获取医院所在信息！");
                }
                request.HospitalGuid = userModel.HospitalGuid;
            }
            List<MealMenuModel> newMealMenuModelList = new List<MealMenuModel>();
            if (request.DishesGuidArr == null || !request.DishesGuidArr.Any() || string.IsNullOrWhiteSpace(request.DishesGuidArr[0]))
            {
                //return Failed(ErrorCode.Empty, "菜品项为空，请选择菜项！");
            }
            else
            {
                var dishesModelList = await new MealDishesBiz().GetModelsByIdArrAndHospitalGuidAsync(request.DishesGuidArr, request.HospitalGuid);
                if (dishesModelList == null || !dishesModelList.Any())
                {
                    return Failed(ErrorCode.Empty, "传入菜品项Guid有误，请检查！");
                }
                foreach (var item in dishesModelList)
                {
                    var model = new MealMenuModel()
                    {
                        MenuGuid = Guid.NewGuid().ToString("N"),
                        HospitalGuid = request.HospitalGuid,
                        CategoryGuid = request.CategoryGuid,
                        DishesGuid = item.DishesGuid,
                        MenuDate = request.Date.Value,
                        Enable = true,
                        CreatedBy = UserID,
                        LastUpdatedBy = UserID,
                        CreationDate = DateTime.Now,
                        LastUpdatedDate = DateTime.Now,
                        OrgGuid = "G"
                    };
                    newMealMenuModelList.Add(model);
                }
            }
            var oldMealMenuModeList = await new MealMenuBiz().GetModelListByCondition(request.CategoryGuid, request.HospitalGuid, request.Date);
            var response = await new MealMenuBiz().AddDisheMaintenanceAsync(newMealMenuModelList, oldMealMenuModeList);
            if (!response)
            {
                return Failed(ErrorCode.Empty, "添加菜品失败，请联系管理员！");
            }
            return Success();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto))]
        public IActionResult MealOperatorUpdatePassword(string password)
        {
            // 前端传输的密码为MD5加密后的结果
            if (string.IsNullOrEmpty(password) || password.Length != 32)
            {
                return Failed(ErrorCode.FormatError, "密码为空或者无效");
            }
            var mealOperatorBiz = new MealOperatorBiz();
            var userModel = mealOperatorBiz.GetModelAsync(UserID).Result;
            if (userModel == null)
            {
                return Failed(ErrorCode.Empty, "用户不存在或者已经注销");
            }
            userModel.Password = CryptoHelper.AddSalt(UserID, password);
            if (string.IsNullOrEmpty(userModel.Password))
            {
                return Failed(ErrorCode.SystemException, "密码加盐失败");
            }
            return mealOperatorBiz.UpdateAsync(userModel).Result ? Success() : Failed(ErrorCode.DataBaseError, "密码更新失败");
        }

        /// <summary>
        /// 食堂端登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Produces(typeof(ResponseDto<MealCanteenLoginResponseDto>)), AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] MealCanteenLoginRequestDto request)
        {
            var modelList = await new MealOperatorBiz().GetModelListByCondition(request.UserName);
            var model = modelList.FirstOrDefault(m => string.Equals(m.Password, CryptoHelper.AddSalt(m.OperatorGuid, request.Password), StringComparison.OrdinalIgnoreCase));
            if (model is null)
            {
                return Failed(ErrorCode.InvalidIdPassword, "账号或密码不正确");
            }
            var token = CreateToken(model.OperatorGuid, Common.EnumDefine.UserType.Unknown, 30);
            return Success(new MealCanteenLoginResponseDto
            {
                OperatorGuid = model.OperatorGuid,
                HospitalGuid = model.HospitalGuid,
                UserName = model.UserName,
                Token = token
            });
        }

    }
}
