using System.Collections.Generic;

namespace GD.Dtos.Merchant.Merchant
{
    /// <summary>
    /// 获取服务人员详细信息
    /// </summary>
    public class GetTherapistDetailInfoResponseDto
    {
        /// <summary>
        /// 服务人员姓名
        /// </summary>
        public string TherapistName { get; set; }
        /// <summary>
        /// 服务人员手机号
        /// </summary>
        public string TherapistPhone { get; set; }
        /// <summary>
        ///服务人员头像附件guid
        /// </summary>
        public string PortraitGuid { get; set; }
        /// <summary>
        /// 服务人员头像附件Url
        /// </summary>
        public string PortraitUrl { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        public string JobTitle { get; set; }
        /// <summary>
        /// 个人介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 擅长标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 所属分类
        /// </summary>
        public List<TherapistClassify> Classifies { get; set; }
        /// <summary>
        /// 所关联服务项目
        /// </summary>
        public List<ClassifyProject> Projects { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TherapistClassify
    {
        /// <summary>
        /// 分类guid
        /// </summary>
        public string ClassifyGuid { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string ClassifyName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ClassifyProject
    {
        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectGuid { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
    }
}
