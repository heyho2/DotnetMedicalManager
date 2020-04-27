using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Doctor.Doctor
{
    /// <summary>
    /// 获取患者详情信息响应Dto
    /// </summary>
    public class GetPatientInfoResponseDto
    {
        /// <summary>
        /// 患者guid
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 患者昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 备注名
        /// </summary>
        public string AliasName { get; set; }

        /// <summary>
        /// 患者头像
        /// </summary>
        public string PortraitUrl { get; set; }

        /// <summary>
        /// 患者性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 患者特征列表
        /// </summary>
        public List<CharacterInfo> CharacterInfos { get; set; }

        /// <summary>
        /// 患者特征
        /// </summary>
        public class CharacterInfo
        {
            /// <summary>
            /// 特征名称
            /// </summary>
            public string CharacterName { get; set; }

            /// <summary>
            /// 特征值
            /// </summary>
            public string CharacterValue { get; set; }

            /// <summary>
            /// 特征类型
            /// </summary>
            public string ValueType { get; set; }
        }
    }
}
