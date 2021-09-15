using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoadUIFromDll
{
    public class MyVM
    {
        public ICommand ChangeSomething { get; set; }
        public ObservableCollection<Person> MyFriends { get; set; }
        public MyVM()
        {
            MyFriends = new ObservableCollection<Person>();
            MyFriends.Add(new Person("Houdong Gu"));
            MyFriends.Add(new Person("Rabin Mahadevan"));

            ChangeSomething = new ByronCommand((a) => { MyFriends[1].FullName = "Rabin is the Best. He has the courage of a lion"; });
        }
    }

    public class Person : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        public Person(string v)
        {
            FullName = v;
        }


        string _fullName;
        public String FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                NotifyPropertyChanged();
            }
        }
    }

    public class ByronCommand : ICommand
    {
        private Action<object> _callback;

        public ByronCommand(Action<object> callback)
        {
            _callback = callback;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _callback(parameter);
        }
    }
}
