using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GD.Models.CommonEnum
{
    /// <summary>
    /// 积分来源类型：Consumption 消费；Distribution 分销提成
    /// </summary>
    public enum ScoreSourceTypeEnum
    {
        /// <summary>
        /// 消费
        /// </summary>
        [Description("消费")]
        Consumption,

        /// <summary>
        /// 分销提成
        /// </summary>
        [Description("分销提成")]
        Distribution
    }
    /// <summary>
    /// 用户行为动作枚举
    /// </summary>
    public enum BehaviorTypeEnum
    {
        /// <summary>
        /// 刷卡
        /// </summary>
        [Description("页面")]
        Page,
        /// <summary>
        /// 刷卡
        /// </summary>
        [Description("产品")]
        Product,
        /// <summary>
        /// 文章
        /// </summary>
        [Description("文章")]
        Article,
        /// <summary>
        /// 动作、活动
        /// </summary>
        [Description("动作")]
        Movement

    }
    /// <summary>
    /// 双美-分类枚举表
    /// </summary>
    public enum ClassifyEnum
    {
        /// <summary>
        /// 明星产品
        /// </summary>
        [Description("明星产品")]
        StartProduct,
        /// <summary>
        /// 明星医生
        /// </summary>
        [Description("明星医生")]
        StartDoctor,
        /// <summary>
        /// 超值优惠
        /// </summary>
        [Description("超值优惠")]
        SuperValu,
        /// <summary>
        /// 拼团聚划算
        /// </summary>
        [Description("拼团聚划算")]
        GroupBuy

    }
    /// <summary>
    /// AboutType
    /// </summary>
    public enum TopicAboutTypeEnum
    {
        /// <summary>
        /// 产品
        /// </summary>
        [Description("产品")]
        Product,
        /// <summary>
        /// 医生
        /// </summary>
        [Description("医生")]
        Doctor,
        /// <summary>
        /// 医院
        /// </summary>
        [Description("医院")]
        Hospital

    }
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatusEnum
    {

        /// <summary>
        /// 全部 0
        /// </summary>
        [Description("全部")]
        All,
        /// <summary>
        /// 待付款 1
        /// </summary>
        [Description("待付款")]
        Obligation,

        /// <summary>
        /// 待发货 2
        /// </summary>
        [Description("待发货")]
        Shipped,

        /// <summary>
        /// 待收货 3
        /// </summary>
        [Description("待收货")]
        Received,

        /// <summary>
        /// 已完成 4
        /// </summary>
        [Description("已完成")]
        Completed,

        /// <summary>
        /// 已取消 5
        /// </summary>
        [Description("已取消")]
        Canceled,

        /// <summary>
        /// 已退款 6
        /// </summary>
        [Description("交易关闭")]
        Closed
    }

    /// <summary>
    /// 支付方式
    /// </summary>
    public enum PayTypeEnum
    {
        /// <summary>
        /// 刷卡
        /// </summary>
        [Description("刷卡")]
        Card,
        /// <summary>
        /// 现金
        /// </summary>
        [Description("现金")]
        Cash,
        /// <summary>
        /// 苹果支付
        /// </summary>
        [Description("苹果支付")]
        Apple,
        /// <summary>
        /// 积分
        /// </summary>
        [Description("积分")]
        Score,
        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        Wechat,
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        Alipay,

        /// <summary>
        /// 线下支付
        /// </summary>
        [Description("线下支付")]
        OffLinePay
    }




}
