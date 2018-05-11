namespace ByronStateDemo
{


    public class FlavorChoice : OrderState
    {

        private bool _IsStrawberryChecked;
        private bool _IsOrangeChecked;
        private bool _IsVanillaChecked;
        private bool _IsCherryChecked;

        public bool IsStrawberryChecked
        {
            get { return _IsStrawberryChecked; }
            set
            {
                _IsStrawberryChecked = value;
                if (_IsStrawberryChecked == true)
                {
                    SelectionTotal += 0.25M;

                }
                else
                {
                    SelectionTotal -= 0.25M;
                    if (SelectionTotal < 0)
                    {
                        SelectionTotal = 0M;
                    }
                }

            }
        }

        public bool IsOrangeChecked
        {
            get { return _IsOrangeChecked; }
            set
            {
                _IsOrangeChecked = value;
                if (_IsOrangeChecked == true)
                {
                    SelectionTotal += 0.25M;

                }
                else
                {
                    SelectionTotal -= 0.25M;
                    if (SelectionTotal < 0)
                    {
                        SelectionTotal = 0M;
                    }
                }

            }
        }

        public bool IsVanillaChecked
        {
            get { return _IsVanillaChecked; }
            set
            {
                _IsVanillaChecked = value;
                if (_IsVanillaChecked == true)
                {
                    SelectionTotal += 0.25M;

                }
                else
                {
                    SelectionTotal -= 0.25M;
                    if (SelectionTotal < 0)
                    {
                        SelectionTotal = 0M;
                    }
                }

            }
        }

        public bool IsCherryChecked
        {
            get { return _IsCherryChecked; }
            set
            {
                _IsCherryChecked = value;
                if (_IsCherryChecked == true)
                {
                    SelectionTotal += 0.25M;

                }
                else
                {
                    SelectionTotal -= 0.25M;
                    if (SelectionTotal < 0)
                    {
                        SelectionTotal = 0M;
                    }
                }

            }
        }

        public FlavorChoice(PaymentInfo payment, string selection) : base(payment, selection)
        {
        }

    }
}