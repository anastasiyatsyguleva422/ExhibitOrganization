using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void ViewArtistsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ArtistsListPage());
        }

        private void ViewUsers_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new UsersListPage());
        }

        private void ViewExhibitions_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ExhibitionListPage());
        }

        private void ViewReviews_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ReviewsListPage());
        }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AuthPage());
        }
    }
}