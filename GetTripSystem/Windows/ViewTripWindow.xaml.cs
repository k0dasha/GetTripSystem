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
    /// Логика взаимодействия для ViewTripWindow.xaml
    /// </summary>
    public partial class ViewTripWindow : Window
    {
        private readonly IRegistration _reg;
        private readonly Trip _trip;
        private User _user;
        public ViewTripWindow(IRegistration registration, Trip trip, User user)
        {
            InitializeComponent();
            _reg = registration;
            _trip = trip;
            _user = user;
            FillTextBlocks();
        }

        private async void Button_AddMember_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _reg.AddMember(_user.Id, _trip.Id);
                this.Close();
            }
            catch (InvalidOperationException)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show("Места закончились", "Ошибка записи",
                                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    this.Close();
                });
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Повторная запись невозможна", "Ошибка записи",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
            }
        }
        private void FillTextBlocks()
        {
            textLocation.Text = _trip.Location;

            string formattedDate = _trip.Date.ToString("dd.MM.yyyy");
            textDate.Text = formattedDate;

            string curAmount = _trip.CurMembs_amount.ToString();
            string maxAmount = _trip.MaxMembs_amount.ToString();
            string range = $"{curAmount} / {maxAmount}";
            textMembsAmount.Text = range;

            textContact.Text = _trip.CreatorContact;
            textBox_Description.Text = _trip.Description;
            
        }
    }
}
