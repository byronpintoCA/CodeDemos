namespace ByronStateDemo
{

    public class SalesTaxChoice : OrderState
    {
        public SalesTaxChoice(PaymentInfo payment, string selection) : base(payment, selection)
        {
        }

    }
}