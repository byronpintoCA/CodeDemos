using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ByronStateDemo
{
    public class TheVM : ViewModelBase
    {
        private OrderState _currentState;

        public OrderState CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState = value;
                RaisePropertyChanged(nameof(this.CurrentState));

            }
        }

        public TheVM()
        {

            CurrentState = Factory.GetInitialState();
            CurrentState.Activate();

            MoveNextCommand = new RelayCommand(MoveNext);

            MoveBackCommand = new RelayCommand(MoveBack);

            MoveFinishCommand = new RelayCommand(MoveFinish);
        }

        private void MoveFinish()
        {
            if (CurrentState.IsFinishEnabled)
            {
                CurrentState = CurrentState.MoveFinish();
            }
        }

        private void MoveBack()
        {
            if (CurrentState.IsBackEnabled)
            {
                CurrentState = CurrentState.MoveBack();
            }
        }

        private void MoveNext()
        {
            if (CurrentState.IsNextEnabled)
            {
                CurrentState = CurrentState.MoveNext();
            }

            //CurrentState = new SalesTaxChoice();
        }

        public RelayCommand MoveNextCommand { get; private set; }
        public RelayCommand MoveBackCommand { get; private set; }
        public RelayCommand MoveFinishCommand { get; private set; }

    }
    }
