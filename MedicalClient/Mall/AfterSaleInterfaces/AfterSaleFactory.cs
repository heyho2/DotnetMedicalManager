using GD.Dtos.CommonEnum;

namespace GD.Mall.AfterSaleInterfaces
{
    public class AfterSaleFactory
    {
        public static IAfterSaleService GetSaleService(AfterSaleServiceTypeEnum type)
        {
            IAfterSaleService afterSaleService = null;

            switch (type)
            {
                case AfterSaleServiceTypeEnum.Exchange:
                    afterSaleService = new ExchangeService();
                    break;
                case AfterSaleServiceTypeEnum.RefundWhithReturn:
                    afterSaleService = new RefundWhithReturnService();
                    break;
                case AfterSaleServiceTypeEnum.RefundWhitoutReturn:
                    afterSaleService = new RefundWhitoutReturnService();
                    break;
            }

            return afterSaleService;
        }
    }
}
