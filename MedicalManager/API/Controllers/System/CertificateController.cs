using GD.Common;
using GD.Dtos.Certificate;
using GD.Manager.Utility;
using GD.Models.Manager;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GD.API.Controllers.System
{
    /// <summary>
    /// 证书控制器
    /// </summary>
    public class CertificateController : SystemBaseController
    {
        /// <summary>
        /// 获取证书列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost, Produces(typeof(ResponseDto<GetCertificateListItemDto>))]
        public async Task<IActionResult> GetCertificateListAsync([FromBody]GetCertificateListRequestDto request)
        {
            string type = null;
            switch (request.Type)
            {
                case GetCertificateListRequestDto.CertificateType.Doctors:
                    type = DictionaryType.DoctorDicConfig;
                    break;
                case GetCertificateListRequestDto.CertificateType.Merchant:
                    type = DictionaryType.MerchantDicConfig;
                    break;
            }
            var response = await new CertificateBiz().GetCertificateListAsync(request.OwnerGuid, type);
            return Success(response);
        }
    }
}
