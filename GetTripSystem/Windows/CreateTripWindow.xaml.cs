using GetTripSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Packaging;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GetTripSystem.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateTripWindow.xaml
    /// </summary>
    public partial class CreateTripWindow : Window
    {
        private readonly ICreateOperation _createOps;
        private User _user;
        public CreateTripWindow(ICreateOperation createOperation, User user)
        {
            InitializeComponent();
            _createOps = createOperation;
            _user = user;
        }

        private async Task Button_CreateTrip_Click(object sender, RoutedEventArgs e)
        {
            string tripName = textBoxName.Text;

            string dateStr = MyDatePicker.Text;
            DateTime date = ConvertToDate(dateStr);

            string location = textLocation.Text;

            string membsAmountStr = textMembsAmount.Text;
            int membsAmount = GetFormat(membsAmountStr);

            string contact = textContact.Text;
            string desc = textBox_Description.Text;

            if(membsAmount == -1)
                MessageBox.Show("Введите числовое значение для поля 'Количество участников'", "Ошибка записи",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
            if (!string.IsNullOrWhiteSpace(tripName))
                {
                    _createOps.RegisterTrip(tripName, location, 0, membsAmount, _user.Id, desc, date, contact);
                    this.Close();
                }
        }
        private int GetFormat(string number)
        {
            
            if (int.TryParse(number, out int result))
                return result;

            return -1;
            
        }
        private DateTime ConvertToDate(string date)
        {
            var result = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            return result;

        }
    }
}
