using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class UsersListPage : Page
    {
        public UsersListPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            using (var db = new ModernArtEntities())
            {
                var users = db.User
                    .Select(u => new
                    {
                        u.Login,
                        u.FIO,
                        u.Role,
                        u.PhoneNumber
                    })
                    .ToList();

                UsersDataGrid.ItemsSource = users;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
