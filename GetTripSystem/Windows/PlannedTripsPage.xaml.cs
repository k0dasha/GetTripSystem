using GetTripSystem.Entities;
using GetTripSystem.Interfaces;
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
    /// Логика взаимодействия для PlannedTripsWindow.xaml
    /// </summary>
    public partial class PlannedTripsPage : Page
    {
        private List<Trip> RegsTripsList;
        private readonly IManagement _manage;
        private User _user;
        public PlannedTripsPage(IManagement management, User user)
        {
            _manage = management;
            _user = user;
            InitializeComponent();
            LoadRegs();
        }

        private async void Button_CancelTrip_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = RegsListView.SelectedItem;

            if (selectedItem is Trip trip)
            {
                int tripId = trip.Id;
                int userId = _user.Id;
                await _manage.CancelRegistration(userId, tripId);
                await LoadRegs();
            }
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private async Task LoadRegs()
        {
            RegsTripsList = await _manage.GetUserRegistrations(_user.Id);
            RegsListView.ItemsSource = RegsTripsList;
        }
    }
}
