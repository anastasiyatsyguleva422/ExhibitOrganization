using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class DealtelPage : Page
    {
        public DealtelPage()
        {
            InitializeComponent();
        }

        private void RegisterToExhibition_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegisterToExhibition());
        }

        private void AddExhibit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddExhibitPage());
        }

        private void ViewExhibitions_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ExhibitionListPage());
        }
        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AuthPage());
        }
        private void MyInvites_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RespondToInvitesPage());
        }


    }
}