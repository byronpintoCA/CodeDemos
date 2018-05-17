using ByronSouthParkDemo.Common;
using ByronSouthParkDemo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ByronSouthParkDemo.ViewModel
{
    public class MainScreenViewModel : ViewModelBase
    {
        public enum ViewModeKind { ViewOne, ViewTwo, ViewThree };

        private ViewModeKind _viewMode;


        public MainScreenViewModel()
        {
            ViewMode = ViewModeKind.ViewOne;

            // Just for demo purposes . a little wow..
            //Task.Run(() => { Thread.Sleep(4000); ViewMode = ViewModeKind.ViewOne; });
        }

        public ViewModeKind ViewMode
        {
            get { return _viewMode; }
            set
            {
                _viewMode = value;
                OnPropertyChanged();

                var timeService = SouthParkViewModelFactory.GetInstance().DaTimeService;

                if (MainScreenContent != null)
                {
                    MainScreenContent.UnSubscribe(timeService);
                    MainScreenContent = new MainScreenContentViewModel(value, MainScreenContent.SelectedCharacter, timeService);
                }
                else
                {

                    MainScreenContent = new MainScreenContentViewModel(value, null, timeService);
                }
            }
        }

        private MainScreenContentViewModel _changes;
        public MainScreenContentViewModel MainScreenContent
        {
            get { return _changes; }
            set { _changes = value; OnPropertyChanged(); }
        }

        public List<String> ViewModeList
        {
            get
            {
                List<String> temp = new List<string>();

                foreach (var name in Enum.GetNames(typeof(ViewModeKind)))
                {
                    temp.Add(name);
                }

                return temp;
            }
            private set { }
        }
    }

}
