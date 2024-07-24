using System.Linq;
using System.Windows;
using Szakdolgozat.Models;
using Szakdolgozat.ViewModels;

public static class WindowHelper
{
    //A megnyitott ablakok listájából megkeresi, hogy az adott view megvan-e nyitva "Add" módban
    //It searches in a collection of opened views for a view that is opened with "Add" mode
    public static bool IsDolgozoAddWindowOpen<T>(out T window) where T : Window
    {
        window = Application.Current.Windows.OfType<T>().FirstOrDefault();

        foreach (var a in Application.Current.Windows.OfType<T>())
        {
            if(a.DataContext is DolgozokModifyOrAddViewModel viewModel)
            {
                if(viewModel.EditMode == EditMode.Add)
                {
                    window = a;
                    return true;
                }
            }
        }
        return false;
    }

    public static bool IsMaganSzemelyAddWindowOpen<T>(out T window) where T : Window
    {
        window = Application.Current.Windows.OfType<T>().FirstOrDefault();

        foreach (var a in Application.Current.Windows.OfType<T>())
        {
            if (a.DataContext is MaganSzemelyekModifyOrAddViewModel viewModel)
            {
                if (viewModel.EditMode == EditMode.Add)
                {
                    window = a;
                    return true;
                }
            }
        }
        return false;
    }

    public static bool IsGazdalkodoSzervezetAddWindowOpen<T>(out T window) where T : Window
    {
        window = Application.Current.Windows.OfType<T>().FirstOrDefault();

        foreach (var a in Application.Current.Windows.OfType<T>())
        {
            if (a.DataContext is GazdalkodoSzervezetModifyOrAddViewModel viewModel)
            {
                if (viewModel.EditMode == EditMode.Add)
                {
                    window = a;
                    return true;
                }
            }
        }
        return false;
    }
}