using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace LoadUIFromDll
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var dict = GetUserControlList();
            DisplayDictionary(dict);
            DataContext = new MyVM();
        }

        private void DisplayDictionary(Dictionary<string, UserControl> dict)
        {
            Grid grid = MyGridHost; // new Grid();
            var height = dict.Count / 100d;
            int ctr = 0;
            foreach (var item in dict)
            {
                var rowDef = new RowDefinition();
                rowDef.Height = new GridLength(height, GridUnitType.Star);
                grid.RowDefinitions.Add(rowDef);
                grid.Children.Add(item.Value);
                Grid.SetRow(item.Value, ctr);
                Grid.SetColumn(item.Value, ctr++);
            }
            //this.Content = grid;
        }

        private Dictionary<string ,UserControl> GetUserControlList()
        {
            Dictionary<string, UserControl> ucDictionary = new Dictionary<string, UserControl>();
            string assemblyPath = $"{AppDomain.CurrentDomain.BaseDirectory}Dll_HasUserControl.dll";

            var kv = LoadContentFromDll(assemblyPath, "ByronUserControl");
            ucDictionary.Add( kv.Key,kv.Value);


            //Change it later to a relative reference
            //assemblyPath = @"C:\Dev\Svxr\CCFInspection\X200\OperatorInterfaceComponents\SVXR.wpfUI\bin\x64\Debug\SVXR.wpfUI.dll";
            //kv = LoadContentFromDll(assemblyPath, "attachProcess");
            //ucDictionary.Add(kv.Key, kv.Value);

            return ucDictionary;
        }

        private KeyValuePair<String, UserControl> LoadContentFromDll(String assemblyPath, String userControlName)
         {
            var assembly = Assembly.LoadFile(assemblyPath);
            userControlName = userControlName.Trim().ToLower();

            foreach (var t in assembly.GetTypes())
            {
               // var i = t.GetInterface("test.ILib");
                if (t.Name.Trim().ToLower() == userControlName)
                {
                    var uc = Activator.CreateInstance(t) as UserControl;
                    
                    return new KeyValuePair<string, UserControl>(userControlName, uc);
                }
            }

            return new KeyValuePair<string, UserControl>(userControlName,null);
        }

    }
}
