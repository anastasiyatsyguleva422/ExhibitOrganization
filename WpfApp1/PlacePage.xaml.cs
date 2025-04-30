using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1
{
    public partial class PlacePage : Page
    {
        public PlacePage()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string city = CityTextBox.Text.Trim();
            string type = TypeTextBox.Text.Trim();
            string name = NameTextBox.Text.Trim();
            string contact = TextBoxPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(type) ||
                string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(contact))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new ModernArtEntities())
                {
                    var venue = new Место_проведения
                    {
                        Город = city,
                        Тип_места = type,
                        Название = name,
                        Контакты = contact
                    };

                    db.Место_проведения.Add(venue);
                    db.SaveChanges();
                }

                MessageBox.Show("Место проведения успешно добавлено!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.Navigate(new OrganizerPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new OrganizerPage());
        }

        private void TextBoxPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "\\d");
        }

        private void TextBoxPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxPhone.TextChanged -= TextBoxPhone_TextChanged;
            string phone = new string(TextBoxPhone.Text.Where(char.IsDigit).ToArray());

            if (!phone.StartsWith("8"))
            {
                phone = "8" + phone;
            }

            if (phone.Length > 11)
                phone = phone.Substring(0, 11);

            string formattedPhone = "8";
            if (phone.Length > 1) formattedPhone += $" ({phone.Substring(1, Math.Min(3, phone.Length - 1))}";
            if (phone.Length > 4) formattedPhone += $") {phone.Substring(4, Math.Min(3, phone.Length - 4))}";
            if (phone.Length > 7) formattedPhone += $"-{phone.Substring(7, Math.Min(2, phone.Length - 7))}";
            if (phone.Length > 9) formattedPhone += $"-{phone.Substring(9, Math.Min(2, phone.Length - 9))}";

            TextBoxPhone.Text = formattedPhone;
            TextBoxPhone.CaretIndex = TextBoxPhone.Text.Length;
            TextBoxPhone.TextChanged += TextBoxPhone_TextChanged;
        }

        private void CityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}