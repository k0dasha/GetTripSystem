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
            MyDatePicker.SelectedDate = DateTime.Now;
        }

        private async void Button_CreateTrip_Click(object sender, RoutedEventArgs e)
        {
            string tripName = textBoxName.Text;

            DateTime date = MyDatePicker.SelectedDate.Value;
            DateTime utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            

            string location = CheckFill(textLocation.Text, "'Локация'");
            if (location == null)
                return;

            string contact = CheckFill(textContact.Text, "'Контакт с организатором'");
            if (contact == null)
                return;

            string desc = textBox_Description.Text;

            int membsAmount = GetFormat(textMembsAmount.Text);
            if (membsAmount == -1)
            {
                MessageBox.Show("Введите числовое значение для поля 'Количество участников'", "Ошибка записи",
                                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            await _createOps.RegisterTrip(tripName, location, 0, membsAmount, _user.Id, desc, utcDate, contact);
            this.Close();

        }
        private int GetFormat(string number)
        {
            
            if (int.TryParse(number, out int result))
                return result;

            return -1;
            
        }
        private string CheckFill(string field, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                MessageBox.Show($"Введите значение для поля {fieldName}", "Обязательное поле", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return null;
            }
            return field;
        }

        private void textMembsAmount_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                return;
            }

            if ((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                return;
            }
            e.Handled = true;
        }
        private void dateTimePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
    
}
