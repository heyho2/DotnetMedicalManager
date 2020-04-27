using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using GD.Common.Base;


namespace GD.Dtos.Utility.Utility
{
    ///<summary>
    ///用户信息更新
    ///</summary>
    public class AddUserInfoRequestDto : BaseDto
    {

        #region 注释
        /////<summary>
        /////身高
        /////</summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "身高")]
        //public KeyValue Height { get; set; }

        /////<summary>
        /////体重
        /////</summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "体重")]
        //public KeyValue Weight
        //{
        //    get;
        //    set;
        //}
        /////<summary>
        /////婚姻
        /////</summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "婚姻")]
        //public KeyValue Marital
        //{
        //    get;
        //    set;
        //}

        /////<summary>
        /////过敏
        /////</summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "过敏")]
        //public KeyValue Allergy
        //{
        //    get;
        //    set;
        //}

        /////<summary>
        /////三高
        /////</summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "三高")]
        //public KeyValue Hypertension
        //{
        //    get;
        //    set;
        //}

        /////<summary>
        /////家族病
        /////</summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "家族病")]
        //public KeyValue FamilyHistory
        //{
        //    get;
        //    set;
        //}

        /////<summary>
        /////饮酒
        /////</summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "饮酒")]
        //public KeyValue Drink
        //{
        //    get;
        //    set;
        //}

        /////<summary>
        /////吸烟
        /////</summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "吸烟")]
        //public KeyValue Smoke
        //{
        //    get;
        //    set;
        //}

        /////<summary>
        /////规律饮食
        /////</summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "规律饮食")]
        //public KeyValue RegularDiet
        //{
        //    get;
        //    set;
        //}

        /////<summary>
        /////规律睡眠
        /////</summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "规律睡眠")]
        //public KeyValue RegularSleep
        //{
        //    get;
        //    set;
        //}

        /////<summary>
        /////规律大小便
        /////</summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "规律大小便")]
        //public KeyValue RegularDefecate
        //{
        //    get;
        //    set;
        //}

        /////<summary>
        /////长期服药
        /////</summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "长期服药")]
        //public KeyValue LongTermUseMedicine
        //{
        //    get;
        //    set;
        //}

        /////<summary>
        /////其他备注
        /////</summary>
        //[Required(ErrorMessage = "{0}必填"), Display(Name = "其他备注")]
        //public KeyValue OtherRemark
        //{
        //    get;
        //    set;
        //}

        //public class KeyValue
        //{
        //    public string  Guid { get; set; }

        //    public string  Value { get; set; }
        //}
        #endregion
           
        /// <summary>
        /// 用户信息列表
        /// </summary>
        public List<UserInfo> UserInfoDtoList { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public class UserInfo
        {
            /// <summary>
            /// 值的名称
            /// </summary>
            [Display(Name = "值的名称")]
            public string PropName { get; set; }
            /// <summary>
            /// 字典表对应的Guid
            /// </summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "字典表对应的Guid")]
            public string Guid { get; set; }
            /// <summary>
            /// 特征的值
            /// </summary>
            [Required(ErrorMessage = "{0}必填"), Display(Name = "特征的值")]
            public string Value { get; set; }
        }





    }
}
