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
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        private readonly ICreateOperation _createOps;
        private readonly IManagement _manage;
        private readonly IRegistration _reg;
        private readonly IServiceProvider _serviceProvider;
        public AuthorizationPage(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _manage = serviceProvider.GetRequiredService<IManagement>();
            _reg = serviceProvider.GetRequiredService<IRegistration>();
            _createOps = serviceProvider.GetRequiredService<ICreateOperation>();
            _serviceProvider = serviceProvider;
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage(_serviceProvider));
        }

        private async void Button_Enter_Click(object sender, RoutedEventArgs e)
        {
            string username = textBox_Login.Text;
            string passwd = textBox_Passwd.Text;

            try
            {
                if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(passwd))
                {
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
                    MessageBox.Show("Заполните все поля", "Ошибка входа",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch (InvalidOperationException)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка входа",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                });
            }
            catch (ArgumentException)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show("Указанного пользователя не существует", "Ошибка входа",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                });
            }
        }

    }
    
}
