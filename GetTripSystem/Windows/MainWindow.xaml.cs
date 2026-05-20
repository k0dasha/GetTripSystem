using GetTripSystem.Interfaces;
using GetTripSystem.Windows;
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

namespace GetTripSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IRegistration _reg;
        private readonly IManagement _manage;

        public MainWindow(IRegistration registration, IManagement management)
        {
            InitializeComponent();
            MainFrame.Content = new MainMenuPage();
            _reg = registration;
            _manage = management;
        }
    }
}