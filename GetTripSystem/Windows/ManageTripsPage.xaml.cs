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
    /// Логика взаимодействия для ManageTripsWindow.xaml
    /// </summary>
    public partial class ManageTripsPage : Page
    {
        private readonly ICreateOperation _createOps;
        private readonly IManagement _manage;
        private readonly User _user;
        private List<Trip> UserTripsList;
        public ManageTripsPage(ICreateOperation createOperation, IManagement management, User user)
        {
            InitializeComponent();
            _createOps = createOperation;
            _manage = management;
            _user = user;
            LoadUserTrips();
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private async void Button_WatchMembs_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Trip trip = button.Tag as Trip;

            KickMembersWindow kickWindow = new KickMembersWindow(_manage, trip);
            
            kickWindow.ShowDialog();

            await LoadUserTrips();
        }

        private async void OnKickWindowClosed(object sender, EventArgs e)
        {
            await LoadUserTrips();
        }

        private async void Button_AddTrip_Click(object sender, RoutedEventArgs e)
        {
            CreateTripWindow createTripWindow = new CreateTripWindow(_createOps, _user);
            createTripWindow.ShowDialog();

            await LoadUserTrips();
        }
        private async Task LoadUserTrips()
        {

            UserTripsList = await _manage.GetUserTrips(_user.Id);
            UserTripsListView.ItemsSource = UserTripsList;
        }
    }
}
