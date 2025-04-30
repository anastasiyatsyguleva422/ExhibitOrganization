using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для OrganizerPage.xaml
    /// </summary>
    public partial class OrganizerPage : Page
    {
        public OrganizerPage()
        {
            InitializeComponent();
        }
        private void ManageExhibitions_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ManageExhibitionsPage());
        }
        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AuthPage());
        }
        private void InviteParticipant_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InviteParticipantsPage());
        }
        private void CreateExhibitionButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CreateExhibitionPage());
        }
        private void SelectExhibits_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SelectExhibitsPage());
        }

        private void Place_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new PlacePage());
        }
    }
}
