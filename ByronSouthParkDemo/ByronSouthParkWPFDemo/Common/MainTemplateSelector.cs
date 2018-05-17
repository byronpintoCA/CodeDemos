using ByronSouthParkDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ByronSouthParkDemo.Common
{
    class MainTemplateSelector : DataTemplateSelector
    {
        const string TemplateNamePostFix = ".DataTemplate";
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return default(DataTemplate);

            bool customProcessing = SpecialProcessing(item, out string specialTemplate);

            String TemplateName = customProcessing == true ? specialTemplate : DefaultTemplateSelection(item);

            try
            {
                var template = (container as FrameworkElement).FindResource(TemplateName) as DataTemplate;

                return template;
            }
            catch (Exception ex)
            {
                // Could Not Find Template 
                if (Debugger.IsAttached == true)
                {
                    MessageBox.Show($"Could not find template .. Error : {ex.Message}");
                    throw; // Error while debugging . Don't hide 
                }
                else
                {
                    MessageBox.Show("Critical Error . Please contact application support");
                    // Log error 
                    return null;
                }

            }
        }

        private static string DefaultTemplateSelection(object item)
        {
            return $"{item.ToString()}{TemplateNamePostFix}";
        }

        private bool SpecialProcessing(object item, out string specialTemplate)
        {
            bool special = false;
            specialTemplate = "";

            if (item is MainScreenContentViewModel)
            {
                special = true;
                var daViewMode = (MainScreenContentViewModel)item;

                specialTemplate = $"ByronSouthParkDemo.ViewModel.MainScreenViewModel.DataTemplate.{daViewMode.DaView}";
            }

            return special;
        }
    }
}
