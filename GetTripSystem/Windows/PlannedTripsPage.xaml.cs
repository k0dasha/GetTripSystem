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
        public PlannedTripsPage()
        {
            InitializeComponent();
        }

        private void Button_CancelTrip_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
