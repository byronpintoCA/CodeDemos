using ByronSouthParkDemo.Model;
using ByronSouthParkDemo.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace ByronSouthParkDemo.View
{

    /// <summary>
    /// Interaction logic for AddEditCharacter.xaml
    /// </summary>
    public partial class AddEditCharacter : Window
    {

        public AddEditCharacter(AddEditCharacterViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void OnCloseCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
