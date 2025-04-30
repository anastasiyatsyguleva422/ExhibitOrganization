using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace WpfApp1
{
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }
        private void RegisterPageButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegPage());
        }
        private void LoginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "Введите логин")
            {
                LoginTextBox.Text = "";
                LoginTextBox.Foreground = Brushes.Black;
            }
        }
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.Unicode.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (var t in bytes)
                {
                    builder.Append(t.ToString("X2"));
                }
                return builder.ToString();
            }
        }

        private void LoginTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                LoginTextBox.Text = "Введите логин";
                LoginTextBox.Foreground = Brushes.Gray;
            }
        }
        private void LoginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox == null || PasswordBox == null)
            {
                MessageBox.Show("Ошибка: не все поля ввода инициализированы!");
                return;
            }

            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(login) || login == "Введите логин" || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            using (var db = new ModernArtEntities())
            {
                string hashedPassword = ComputeSha256Hash(password);
                var user = db.User.FirstOrDefault(u => u.Login.Trim() == login && u.Password.Trim().ToLower() == hashedPassword.ToLower());

                if (user != null)
                {
                    App.CurrentUser = user;

                    if (user.Role == "artist" && user.ID_Деятели == null)
                    {
                        var newDeatel = new Деятели
                        {
                            Имя = user.FIO,
                            Биография = "Пока нет описания",
                            Контакты = user.PhoneNumber
                        };

                        db.Деятели.Add(newDeatel);
                        db.SaveChanges();

                        user.ID_Деятели = newDeatel.ID_Деятели;
                        db.SaveChanges();

                        App.CurrentUser.ID_Деятели = newDeatel.ID_Деятели;
                    }

                    MessageBox.Show($"Добро пожаловать, {user.Role} {user.FIO}!");

                    if (user.Role?.Trim().ToLowerInvariant() == "artist" && user.ID_Деятели == null)
                    {
                        using (var dbCreate = new ModernArtEntities())
                        {
                            var newDeatel = new Деятели
                            {
                                Имя = user.FIO,
                                Биография = "Пока нет описания",
                                Контакты = user.PhoneNumber
                            };

                            dbCreate.Деятели.Add(newDeatel);
                            dbCreate.SaveChanges();

                            var currentUserFromDb = dbCreate.User.FirstOrDefault(u => u.ID == user.ID);
                            if (currentUserFromDb != null)
                            {
                                currentUserFromDb.ID_Деятели = newDeatel.ID_Деятели;
                                dbCreate.SaveChanges();

                                App.CurrentUser = currentUserFromDb;
                            }
                        }
                    }
                    else
                    {
                        App.CurrentUser = user; 
                    }

                    try
                    {
                        switch (user.Role?.Trim().ToLowerInvariant())
                        {
                            case "admin":
                                NavigationService?.Navigate(new AdminPage());
                                break;
                            case "organizer":
                                NavigationService?.Navigate(new OrganizerPage());
                                break;
                            case "artist":
                                NavigationService?.Navigate(new DealtelPage());
                                break;
                            case "visitor":
                                NavigationService?.Navigate(new VisitorPage());
                                break;
                            default:
                                MessageBox.Show("Неизвестная роль пользователя!");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                        Debug.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Неверные логин или пароль!");
                }
            }
        }

    }
}
