using GalaSoft.MvvmLight;
using System;

namespace ByronStateDemo
{
    public class OrderState : ViewModelBase
    {
        public OrderState Back;
        public OrderState Finish;
        public OrderState Next;

        private PaymentInfo _pay;
        private String _selectionText;
        private decimal _SelectionTotal;
        private bool _alreadyAdded;

        public PaymentInfo Payment
        {
            get
            {
                return _pay;
            }
            set
            {
                _pay = value;
                RaisePropertyChanged(nameof(this.Payment));

            }

        }

       

        public String SelectionText
        {
            get
            {
                return _selectionText;
            }
            set
            {
                _selectionText = value;
                RaisePropertyChanged(nameof(this.IsBackEnabled));

            }
        }

        public decimal SelectionTotal
        {
            get
            {
                return _SelectionTotal;
            }
            set
            {
                _SelectionTotal = value;
                RaisePropertyChanged(nameof(this.SelectionTotal));

            }
        }

        public bool IsBackEnabled
        {
            get
            {
                return (Back != null);
            }
            set
            {

                RaisePropertyChanged(nameof(this.IsBackEnabled));

            }
        }
        public bool IsNextEnabled
        {

            get
            {
                return (Next != null);
            }
            set
            {

                RaisePropertyChanged(nameof(this.IsNextEnabled));

            }
        }

        public bool IsFinishEnabled
        {
            get
            {
                return (Finish != null);
            }
            set
            {

                RaisePropertyChanged(nameof(this.IsFinishEnabled));

            }
        }


        public OrderState(PaymentInfo payment, String selection)
        {
            _pay = payment;
            _selectionText = selection;
        }

        public virtual void AddToPayment()
        {
            if (_alreadyAdded==false)
            {
                _pay.TotalCost += SelectionTotal;
                _alreadyAdded = true;

            }
           
        }

        public virtual void ReduceFromPayment()
        {
            if (_alreadyAdded == true)
            {
                _pay.TotalCost -= SelectionTotal;
                _alreadyAdded = false;
            }
        }
        public virtual void Activate()
        {
            ReduceFromPayment();
        }

        public OrderState MoveBack()
        {
            if (Back != null)
            {
                ReduceFromPayment();
                Back.Activate();

                return Back;
            }

            return null;
        }


        public OrderState MoveNext()
        {
            if (Next != null)
            {
                AddToPayment();
                Next.Activate();
                
                return Next;
            }

            return null;
        }


        public OrderState MoveFinish()
        {
            if (Finish != null)
            {
                AddToPayment();
                Finish.Activate();
                return Finish;
            }

            return null;
        }

        
    }
}
