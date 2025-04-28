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
    /// Логика взаимодействия для VisitorPage.xaml
    /// </summary>
    public partial class VisitorPage : Page
    {
        public VisitorPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RegisterToExhibition_Click(object sender, RoutedEventArgs e)
        {
           
                NavigationService.Navigate(new ListPage());
            }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());

        }

        private void ViewExhibitions_Click(object sender, RoutedEventArgs e)
        {

            NavigationService?.Navigate(new ExhibitionListPage());
        }

        private void BuyTicket_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new BuyTicketPage());

        }

        private void BuyExhibit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new BuyExhibitPage());

        }

        private void WriteReview_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new WriteReviewPage());

        }
    }
    }
