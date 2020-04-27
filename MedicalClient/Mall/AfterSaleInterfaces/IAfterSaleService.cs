using GD.Dtos.Mall.Mall;
using System.Threading.Tasks;

namespace GD.Mall.AfterSaleInterfaces
{
    public interface IAfterSaleService
    {
        Task ProcessAfterSaleServiceContextAsync(ProcessAfterSaleServiceContext context);
    }
}
