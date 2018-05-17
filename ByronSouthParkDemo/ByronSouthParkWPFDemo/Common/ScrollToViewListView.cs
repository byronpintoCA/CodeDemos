using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ByronSouthParkDemo.Common
{
    public class ScrollToViewListView : ListView
    {

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            var it = this.SelectedItem;

            if (it != null)
            {
                ScrollIntoView(it);
            }

        }
    }
}
