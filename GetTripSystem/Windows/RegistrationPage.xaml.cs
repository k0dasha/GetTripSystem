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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private readonly ICreateOperation _createOps;
        public RegistrationPage(ICreateOperation createOperation)
        {
            InitializeComponent();
            _createOps = createOperation;
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string username = textBox_Login.Text;
            string passwd = textBox_Passwd.Text;

            _createOps.RegisterUser(username, passwd);
            
        }
    }
}
