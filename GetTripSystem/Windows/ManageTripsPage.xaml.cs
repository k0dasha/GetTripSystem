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
        public ManageTripsPage()
        {
            InitializeComponent();
        }

        private void Button_EditTrip_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_DeleteTrip_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_AddTrip_Click(object sender, RoutedEventArgs e)
        {
            CreateTripWindow createTripWindow = new CreateTripWindow();
            createTripWindow.Show();
        }
    }
}
