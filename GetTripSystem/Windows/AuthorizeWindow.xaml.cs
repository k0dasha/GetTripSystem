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
using System.Windows.Shapes;

namespace GetTripSystem.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthorizeWindow.xaml
    /// </summary>
    public partial class AuthorizeWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        public AuthorizeWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            MainFrame.Content = new AuthorizationPage(serviceProvider);
        }
    }
}
