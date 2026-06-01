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
    /// Логика взаимодействия для KickMembersWindow.xaml
    /// </summary>
    public partial class KickMembersWindow : Window
    {
        private readonly IManagement _manage;
        private List<User> users;
        private readonly Trip _trip;

        public KickMembersWindow(IManagement management, Trip trip)
        {
            InitializeComponent();
            _manage = management;
            _trip = trip;
            Loaded += async (s, e) => await LoadData();
        }

        private async Task LoadData()
        {
            await GetUsersNames();
            LoadCards();
        }

        private async Task GetUsersNames()
        {
            users = await _manage.GetMembersOfTrip(_trip.Id);
        }

        private void LoadCards()
        {
            CardsPanel.Children.Clear();

            foreach (var user in users)
            {
                var card = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 201)),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(8),
                    Margin = new Thickness(0, 0, 0, 10),
                    Padding = new Thickness(20, 5, 10, 5),
                    Height = 50,
                    Width = 300
                };

                var content = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center
                };

                content.Children.Add(new TextBlock
                {
                    Text = user.Name,
                    Foreground = Brushes.Black,
                    FontSize = 14,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 200
                });

                content.Children.Add(new System.Windows.Controls.Control
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Width = double.NaN
                });

                var btn = new Button
                {
                    Tag = user,
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(30,0,0,0),
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("/Fonts/Kick.png", UriKind.Relative)),
                        Width = 25,
                        Height = 20
                    }
                };

                btn.Click += Remove_Click;
                content.Children.Add(btn);

                card.Child = content;
                CardsPanel.Children.Add(card);
            }
        }

        private async void Remove_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            User user = button.Tag as User;

            if (user == null) return;

            await _manage.KickMember(user.Id, _trip.Id);
            await GetUsersNames();
            LoadCards();
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
