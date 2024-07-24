using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Szakdolgozat.Models;

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

        public delegate ObservableCollection<object> DataRequestEventHandler();
        public static event DataRequestEventHandler DataRequest;
        public static ObservableCollection<object> NotifyDataRequest()
        {
           return DataRequest?.Invoke();
        }

        //Visszaadja az újonnan hozzáadott dolgozo-t annak a view-nak, ami feliratkozott az eseményre
        //It gives back the new dolgozo that was added to the view that subscribed to the event
        public delegate void NewDolgozoAddedEventHandler(Dolgozo dolgozo);
        public static event NewDolgozoAddedEventHandler NewDolgozoAdded;
        public static void NotifyNewDolgozoAdded(Dolgozo dolgozo)
        {
            NewDolgozoAdded?.Invoke(dolgozo);
        }

        //Visszaadja a módosított dolgozo-t annak a view-nak, ami feliratkozott az eseményre
        //It gives back the modified dolgozo to the view that subscribed to the event
        public delegate void DolgozoModifiedEventHandler(Dolgozo dolgozo);
        public static event DolgozoModifiedEventHandler DolgozoModified;
        public static void NotifyModifiedDolgozo(Dolgozo dolgozo)
        {
            DolgozoModified?.Invoke(dolgozo);
        }

        //Visszaadja az újonnan hozzáadott gazdalkodo szervezet-et annak a view-nak, ami feliratkozott az eseményre
        //It gives back the new gazdalkodo szervezet that was added to the view that subscribed to the event
        public delegate void NewGazdalkodoSzervezetAddedEventHandler(GazdalkodoSzervezet gazdalkodoSzervezet);
        public static event NewGazdalkodoSzervezetAddedEventHandler NewGazdalkodoSzervezetAdded;
        public static void NotifyNewGazdalkodoSzervezetAdded(GazdalkodoSzervezet gazdalkodoSzervezet)
        {
            NewGazdalkodoSzervezetAdded?.Invoke(gazdalkodoSzervezet);
        }

        //Visszaadja a módosított gazdalkodo szervezet-et annak a view-nak, ami feliratkozott az eseményre
        //It gives back the modified gazdalkodo szervezet to the view that subscribed to the event
        public delegate void GazdalkodoSzervezetModifiedEventHandler(GazdalkodoSzervezet gazdalkodoSzervezet);
        public static event GazdalkodoSzervezetModifiedEventHandler GazdalkodoSzervezetModified;
        public static void NotifyModifiedGazdalkodoSzervezet(GazdalkodoSzervezet gazdalkodoSzervezet)
        {
            GazdalkodoSzervezetModified?.Invoke(gazdalkodoSzervezet);
        }

        //public delegate void CheckBoxChangedEventHandler(string dataGridName);
        //public static event CheckBoxChangedEventHandler CheckBoxChanged;
        //public static void NotifyCheckBoxChanged(string dataGridName)
        //{
        //    CheckBoxChanged?.Invoke(dataGridName);
        //}


        public delegate TabControl GetTabControlEventHandler();
        public static event GetTabControlEventHandler GetTabControl;
        public static TabControl NotifyGetTabControl()
        {
            return GetTabControl?.Invoke();
        }
    }
}
