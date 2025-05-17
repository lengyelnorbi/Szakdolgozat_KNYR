using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Szakdolgozat.ViewModels;
using Szakdolgozat.Views;

namespace Szakdolgozat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            var loginView = new LoginView();
            loginView.Show(); 
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {
                    try
                    {
                        int userID = 0;
                        string role = "";
                        if (loginView.DataContext is LoginViewModel viewModel)
                        {
                            userID = viewModel.UserID;
                            role = viewModel.UserRole;
                        }
                        var mainView = new MainView(userID, role);
                        mainView.Show();
                        loginView.Close();
                    }
                    catch { }
                   
                }
            };
        }
    }
}
