namespace ByronStateDemo
{


    public class MainDrinkChoice : OrderState
    {
      
        public MainDrinkChoice(PaymentInfo payment, string selection) : base(payment, selection)
        {
        }

        private bool _isCokeChecked;
        private bool _IsPepseChecked;
        private bool _IsMountainDewChecked;
        private bool _IsGatoradeChecked;

        public bool IsCokeChecked
        {
            get { return _isCokeChecked; }
            set
            {
                _isCokeChecked = value;
                if (_isCokeChecked == true)
                {
                    SelectionTotal = 0.75M;
                   
                }
               
            }
        }

        public bool IsPepseChecked
        {
            get { return _IsPepseChecked; }
            set
            {
                _IsPepseChecked = value;
                if (_IsPepseChecked == true)
                {
                    SelectionTotal = 1;

                }
               
            }
        }

        public bool IsMountainDewChecked
        {
            get { return _IsMountainDewChecked; }
            set
            {
                _IsMountainDewChecked = value;
                if (_IsMountainDewChecked == true)
                {
                    SelectionTotal = 1.25M;

                }
               
            }
        }

        public bool IsGatoradeChecked
        {
            get { return _IsGatoradeChecked; }
            set
            {
                _IsGatoradeChecked = value;
                if (_IsGatoradeChecked == true)
                {
                    SelectionTotal = 1.50M;

                }
                
            }
        }

        public override void Activate()
        {
            base.Activate();
            Payment.TotalCost = 0;
        }

    }
}