using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GD.Common.Base;

namespace GD.Models.Utility
{
    /// <summary>
    /// 证书表模型
    /// </summary>
    [Table("t_utility_certificate")]
    public class CertificateModel : BaseModel
    {
        ///<summary>
        ///证书GUID
        ///</summary>
        [Column("certificate_guid"), Key, Required(ErrorMessage = "{0}必填"), Display(Name = "证书GUID")]
        public string CertificateGuid
        {
            get;
            set;
        }

        ///<summary>
        ///证书图片GUID
        ///</summary>
        [Column("picture_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "证书图片GUID")]
        public string PictureGuid
        {
            get;
            set;
        }

        ///<summary>
        ///证书持有人GUID
        ///</summary>
        [Column("owner_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "证书持有人GUID")]
        public string OwnerGuid
        {
            get;
            set;
        }

        ///<summary>
        ///配置项GUID
        ///</summary>
        [Column("dic_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "配置项GUID")]
        public string DicGuid
        {
            get;
            set;
        }
    }
}
