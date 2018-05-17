using ByronSouthParkDemo.Common;

namespace ByronSouthParkDemo.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = SouthParkViewModelFactory.GetInstance().MainScreen;
        }
    }
}