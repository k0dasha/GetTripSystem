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
        private List<Trip> TripList; 
        public MainMenuPage(IServiceProvider serviceProvider, User user)
        {
            _manage = serviceProvider.GetRequiredService<IManagement>();
            _reg = serviceProvider.GetRequiredService<IRegistration>();
            _createOps = serviceProvider.GetRequiredService<ICreateOperation>();
            _user = user;
            LoadTrips();
            InitializeComponent();
        }

        private void Button_Plan_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PlannedTripsPage(_manage, _user));
        }

        private void Button_Manage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManageTripsPage(_createOps, _manage, _user));
        }

        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = comboBox_selectSorting.SelectedItem as ComboBoxItem;
            var selection = item?.Content.ToString();

            switch (selection)
            {
                case "Дата":
                    TripList = await _reg.ToSort(0);
                    break;
                case "Локация":
                    TripList = await _reg.ToSort(1);
                    break;
                
            }
            TripListView.ItemsSource = TripList;
        }
        private async void LoadTrips()
        {
            TripList = await _reg.GetAllTrips();
            TripListView.ItemsSource = TripList;
        }

        private void TripListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewTripWindow viewTripWindow = new ViewTripWindow();
            viewTripWindow.ShowDialog();
        }
    }
}
