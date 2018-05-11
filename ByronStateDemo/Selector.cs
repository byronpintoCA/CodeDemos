using System.Windows;
using System.Windows.Controls;

namespace ByronStateDemo
{


        public class Selector : DataTemplateSelector
        {
            public override DataTemplate SelectTemplate(object item, DependencyObject container)
            {
                if (item is MainDrinkChoice)
                    return (container as FrameworkElement).FindResource("templateDrinkChoices") as DataTemplate;
                else if (item is FlavorChoice)
                    return (container as FrameworkElement).FindResource("templateFlavor") as DataTemplate;
                else if (item is SalesTaxChoice)
                {
                    return (container as FrameworkElement).FindResource("templateFinal") as DataTemplate;
                }


                return null;
            }
        }
}