using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace WpfApp1
{
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();

            CmbRole.SelectionChanged += CmbRole_SelectionChanged;
        }

        private void CmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRole = (CmbRole.SelectedItem as ComboBoxItem)?.Content.ToString().Trim();

            if (selectedRole == "Посетитель")
            {
                AgeTextBlock.Visibility = Visibility.Visible;
                AgeTextBox.Visibility = Visibility.Visible;
                BenefitsTextBlock.Visibility = Visibility.Visible;
                BenefitsTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                AgeTextBlock.Visibility = Visibility.Collapsed;
                AgeTextBox.Visibility = Visibility.Collapsed;
                BenefitsTextBlock.Visibility = Visibility.Collapsed;
                BenefitsTextBox.Visibility = Visibility.Collapsed;
            }
        }


        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxFIO.Text) ||
                string.IsNullOrEmpty(TextBoxLogin.Text) ||
                string.IsNullOrEmpty(PasswordBox.Password) ||
                string.IsNullOrEmpty(TextBoxPhone.Text) ||
                (RadioMale.IsChecked == false && RadioFemale.IsChecked == false) ||
                CmbRole.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string selectedRole = (CmbRole.SelectedItem as ComboBoxItem)?.Content.ToString().Trim();
            Dictionary<string, string> roleMapping = new Dictionary<string, string>
    {
        { "Администратор", "Admin" },
        { "Посетитель", "Visitor" },
        { "Деятель", "Artist" },
        { "Организатор", "Organizer" }
    };

            if (roleMapping.ContainsKey(selectedRole))
            {
                selectedRole = roleMapping[selectedRole];
            }
            else
            {
                MessageBox.Show("Выбрана некорректная роль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new ModernArtEntities())
                {
                    if (db.User.Any(u => u.Login.Trim() == TextBoxLogin.Text.Trim()))
                    {
                        MessageBox.Show("Этот логин уже занят!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    string selectedGender = RadioMale.IsChecked == true ? "Мужской" : "Женский";
                    int? age = selectedRole == "Visitor" && !string.IsNullOrEmpty(AgeTextBox.Text) ? Convert.ToInt32(AgeTextBox.Text) : (int?)null;
                    string benefits = selectedRole == "Visitor" ? BenefitsTextBox.Text.Trim() : null;

                    User newUser = new User
                    {
                        FIO = TextBoxFIO.Text.Trim(),
                        Login = TextBoxLogin.Text.Trim(),
                        Password = PasswordBox.Password.Trim(),
                        Gender = selectedGender,
                        Role = selectedRole,
                        PhoneNumber = TextBoxPhone.Text.Trim(),
                    };

                    db.User.Add(newUser);
                    db.SaveChanges();

                     if (selectedRole == "Visitor")
                    {
                        var newVisitor = new Посетители
                        {
                            ФИО = newUser.FIO,   
                            Льготы = benefits,  
                            Пол = selectedGender, 
                            Возраст = age 
                        };

                        db.Посетители.Add(newVisitor);
                        db.SaveChanges();

                        newUser.ID_Посетители = newVisitor.ID_Посетители;
                        db.SaveChanges();
                    }

                    MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new AuthPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            TextBoxFIO.Clear();
            TextBoxLogin.Clear();
            PasswordBox.Clear();
            TextBoxPhone.Clear();
            RadioMale.IsChecked = false;
            RadioFemale.IsChecked = false;
            CmbRole.SelectedIndex = -1;
            NavigationService.Navigate(new AuthPage());
        }
    }

}
