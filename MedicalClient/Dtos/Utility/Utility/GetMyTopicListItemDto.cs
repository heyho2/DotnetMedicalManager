using GD.Common.Base;
using System;

namespace GD.Dtos.Utility.Utility
{
    /// <summary>
    /// 批量新增Topic记录 请求 
    /// </summary>
    public class GetMyTopicListItemDto : BaseDto
    {
        ///<summary>
        ///主题GUID
        ///</summary>
        public string TopicGuid
        {
            get;
            set;
        }

        ///<summary>
        ///发起者GUID
        ///</summary>
        public string SponsorGuid
        {
            get;
            set;
        }

        ///<summary>
        ///接收者GUID
        ///</summary>
        public string ReceiverGuid
        {
            get;
            set;
        }

        ///<summary>
        ///话题关于GUID(如来自商品或直接问的医生）
        ///</summary>
        public string AboutGuid
        {
            get;
            set;
        }

        ///<summary>
        ///aboutGuid对应表的类型
        ///</summary>
        public string EnumTb
        {
            get;
            set;
        }

        ///<summary>
        ///开始时间
        ///</summary>
        public DateTime BeginDate
        {
            get;
            set;
        }

        ///<summary>
        ///结束时间
        ///</summary>
        public DateTime EndDate
        {
            get;
            set;
        }
    }
}
