using GD.Common.Helper;
using GD.Models.CommonEnum;
using GD.Utility;
using System;

namespace GD.Consumer
{
    /// <summary>
    /// 消费者行为业务处理类
    /// </summary>
    public class ConsumerActionBiz
    {
        /// <summary>
        /// 收藏/取消收藏医生
        /// </summary>
        /// <param name="userId">用户GUID</param>
        /// <param name="doctorGuid">医生GUID</param>
        /// <param name="collectionState">收藏状态</param>
        /// <param name="first">是否是第一次评价</param>
        /// <returns></returns>
        public void CollectDoctorToGetScore(string userId, string doctorGuid, CollectionStateEnum collectionState, bool first)
        {
            try
            {
                //收藏医生添加积分,取消医生减少积分
                if (CollectionStateEnum.Establish == collectionState)
                {
                    new ScoreRulesBiz().AddScoreByRules(doctorGuid, Common.EnumDefine.ActionEnum.AddFan, Common.EnumDefine.UserType.Doctor);
                }
                else
                {
                    new ScoreRulesBiz().AddScoreByRules(doctorGuid, Common.EnumDefine.ActionEnum.DeleteFan, Common.EnumDefine.UserType.Doctor);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"action:{nameof(CollectDoctorToGetScore)}  userId:{userId}  doctorGuid:{doctorGuid}  collectionState:{collectionState.GetDescription()}  message:{ex.Message}");
            }
        }

        

        
    }
}