using GetTripSystem.Interfaces;
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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private readonly ICreateOperation _createOps;
        private readonly IManagement _manage;
        private readonly IServiceProvider _serviceProvider;
        public RegistrationPage(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _createOps = serviceProvider.GetRequiredService<ICreateOperation>();
            _manage = serviceProvider.GetRequiredService<IManagement>();
            _serviceProvider = serviceProvider;
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string username = textBox_Login.Text;
            string passwd = textBox_Passwd.Text;

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(passwd))
            {
                _createOps.RegisterUser(username, passwd);
                var user = await _manage.GetUser(username, passwd);

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    MainWindow mainWindow = new MainWindow(_serviceProvider, user);
                    mainWindow.Show();

                    var window = Window.GetWindow(this);
                    window.Close();
                });
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Ошибка регистрации",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
