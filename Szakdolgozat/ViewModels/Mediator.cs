using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.ViewModels
{
    public static class Mediator
    {
        public delegate void SelectedRowsChangedOnChildViewEventHandler(ObservableCollection<object> SelectedRows);
        public static event SelectedRowsChangedOnChildViewEventHandler SelectedRowsChangedOnChildView;

        public static void NotifySelectedRowsChangedOnChildView(ObservableCollection<object> SelectedRows)
        {
            SelectedRowsChangedOnChildView?.Invoke(SelectedRows);
        }
    }
}
