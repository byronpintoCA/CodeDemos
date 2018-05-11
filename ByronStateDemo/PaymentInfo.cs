using GalaSoft.MvvmLight;

namespace ByronStateDemo
{

    public class PaymentInfo : ViewModelBase
    {
        private decimal _totalCost;

        public decimal TotalCost
        {

            get
            {
                return _totalCost;
            }
            set
            {
                _totalCost = value;
                RaisePropertyChanged(nameof(this.TotalCost));
            }

        }
    }
}