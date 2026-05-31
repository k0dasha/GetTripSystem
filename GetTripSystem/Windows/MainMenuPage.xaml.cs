using GetTripSystem.Interfaces;
using GetTripSystem.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GetTripSystem.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        private readonly ICreateOperation _createOps;
        private readonly IManagement _manage;
        private readonly IRegistration _reg;
        private User _user;
        public MainMenuPage(IServiceProvider serviceProvider, User user)
        {
            _manage = serviceProvider.GetRequiredService<IManagement>();
            _reg = serviceProvider.GetRequiredService<IRegistration>();
            _createOps = serviceProvider.GetRequiredService<ICreateOperation>();
            _user = user;
            InitializeComponent();
        }

        private void Button_Plan_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PlannedTripsPage());
        }

        private void Button_Manage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManageTripsPage());
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
