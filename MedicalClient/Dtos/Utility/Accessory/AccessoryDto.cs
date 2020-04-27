using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Utility.Accessory
{
    /// <summary>
    /// 附件Dto
    /// </summary>
    public class AccessoryDto:BaseDto
    {
        ///<summary>
        ///附件GUID
        ///</summary>
        public string AccessoryGuid
        {
            get;
            set;
        }

        ///<summary>
        ///来源GUID
        ///</summary>
        public string OwnerGuid
        {
            get;
            set;
        }

        ///<summary>
        ///基础路径
        ///</summary>
        public string BasePath
        {
            get;
            set;
        }

        ///<summary>
        ///相对路径
        ///</summary>
        public string RelativePath
        {
            get;
            set;
        }

        ///<summary>
        ///文件后缀名
        ///</summary>
        public string Extension
        {
            get;
            set;
        }

        /// <summary>
        /// 完整地址
        /// </summary>
        public string FullPath { get; set; }
    }
}
