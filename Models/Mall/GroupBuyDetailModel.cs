using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GD.Common.Base;

namespace GD.Models.Mall
{
    /// <inheritdoc />
    /// <summary>
    /// 团购子信息
    /// </summary>
    public class GroupBuyDetailModel : BaseModel
    {
        ///<summary>
        ///团购子信息主键
        ///</summary>
        [Column("detail_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "主键")]
        public string DetailGuid { get; set; }

        ///<summary>
        ///团购Guid
        ///</summary>
        [Column("groupbuy_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "团购Guid")]
        public string GroupBuyGuid { get; set; }
        ///<summary>
        ///用户Guid
        ///</summary>
        [Column("user_guid"), Required(ErrorMessage = "{0}必填"), Display(Name = "用户Guid")]
        public string UserGuid { get; set; }
        ///<summary>
        ///购买数
        ///</summary>
        [Column("num"), Required(ErrorMessage = "{0}必填"), Display(Name = "购买数")]
        public string Num { get; set; }
        
    }
}
