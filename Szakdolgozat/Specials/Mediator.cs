using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
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

        //Visszaadja az újonnan hozzáadott magan szemely-t annak a view-nak, ami feliratkozott az eseményre
        //It gives back the new magan szemely that was added to the view that subscribed to the event
        public delegate void NewMaganSzemelyAddedEventHandler(MaganSzemely maganSzemely);
        public static event NewMaganSzemelyAddedEventHandler NewMaganSzemelyAdded;
        public static void NotifyNewMaganSzemelyAdded(MaganSzemely maganSzemely)
        {
            NewMaganSzemelyAdded?.Invoke(maganSzemely);
        }

        //Visszaadja a módosított magan szemely-t annak a view-nak, ami feliratkozott az eseményre
        //It gives back the modified magan szemely to the view that subscribed to the event
        public delegate void MaganSzemelyModifiedEventHandler(MaganSzemely maganSzemely);
        public static event MaganSzemelyModifiedEventHandler MaganSzemelyModified;
        public static void NotifyModifiedMaganSzemely(MaganSzemely maganSzemely)
        {
            MaganSzemelyModified?.Invoke(maganSzemely);
        }

        //Visszaadja az újonnan hozzáadott bevetelKiadas-t annak a view-nak, ami feliratkozott az eseményre
        //It gives back the new bevetelKiadas that was added to the view that subscribed to the event
        public delegate void NewBevetelKiadasAddedEventHandler(BevetelKiadas bevetelKiadas);
        public static event NewBevetelKiadasAddedEventHandler NewBevetelKiadasAdded;
        public static void NotifyNewBevetelKiadasAdded(BevetelKiadas bevetelKiadas)
        {
            NewBevetelKiadasAdded?.Invoke(bevetelKiadas);
        }

        //Visszaadja a módosított bevetelKiadas-t annak a view-nak, ami feliratkozott az eseményre
        //It gives back the modified bevetelKiadas to the view that subscribed to the event
        public delegate void BevetelKiadasModifiedEventHandler(BevetelKiadas bevetelKiadas);
        public static event BevetelKiadasModifiedEventHandler BevetelKiadasModified;
        public static void NotifyModifiedBevetelKiadas(BevetelKiadas bevetelKiadas)
        {
            BevetelKiadasModified?.Invoke(bevetelKiadas);
        }

        //Visszaadja az újonnan hozzáadott kotelezettsegKoveteles-t annak a view-nak, ami feliratkozott az eseményre
        //It gives back the new kotelezettsegKoveteles that was added to the view that subscribed to the event
        public delegate void NewKotelezettsegKovetelesAddedEventHandler(KotelezettsegKoveteles kotelezettsegKoveteles);
        public static event NewKotelezettsegKovetelesAddedEventHandler NewKotelezettsegKovetelesAdded;
        public static void NotifyNewKotelezettsegKovetelesAdded(KotelezettsegKoveteles kotelezettsegKoveteles)
        {
            NewKotelezettsegKovetelesAdded?.Invoke(kotelezettsegKoveteles);
        }

        //Visszaadja a módosított kotelezettsegKoveteles-t annak a view-nak, ami feliratkozott az eseményre
        //It gives back the modified kotelezettsegKoveteles to the view that subscribed to the event
        public delegate void KotelezettsegKovetelesModifiedEventHandler(KotelezettsegKoveteles kotelezettsegKoveteles);
        public static event KotelezettsegKovetelesModifiedEventHandler KotelezettsegKovetelesModified;
        public static void NotifyModifiedKotelezettsegKoveteles(KotelezettsegKoveteles kotelezettsegKoveteles)
        {
            KotelezettsegKovetelesModified?.Invoke(kotelezettsegKoveteles);
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

        public delegate void SetSeriesVisibilityEventHandler(string seriesType);
        public static event SetSeriesVisibilityEventHandler SetSeriesVisibility;
        public static void NotifySetSeriesVisibility(string seriesType)
        {
            SetSeriesVisibility?.Invoke(seriesType);
        }

        public delegate void HideOrShowLineSeriesEventHandler(string name, bool isSelected);
        public static event HideOrShowLineSeriesEventHandler HideOrShowLineSeries;
        public static void NotifyHideOrShowLineSeries(string name, bool isSelected)
        {
            HideOrShowLineSeries?.Invoke(name, isSelected);
        }

        public delegate void SetLineSeriesNewColorEventHandler(string name, SolidColorBrush color);
        public static event SetLineSeriesNewColorEventHandler SetLineSeriesNewColor;
        public static void NotifySetLineSeriesNewColor(string name, SolidColorBrush color)
        {
            SetLineSeriesNewColor?.Invoke(name, color);
        }
    }
}
