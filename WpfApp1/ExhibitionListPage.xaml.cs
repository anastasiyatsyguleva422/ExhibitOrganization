using System.Windows.Controls;
using System.Windows;
using System.Linq;

namespace WpfApp1
{
    public partial class ExhibitionListPage : Page
    {
        public ExhibitionListPage()
        {
            InitializeComponent();
            LoadExhibitions();
        }

        private void LoadExhibitions()
        {
            using (var db = new ModernArtEntities())
            {
                var exhibitions = db.Выставки
                    .Select(v => new
                    {
                        Название = v.Название,
                        МестоПроведения = v.Место_проведения.Город + ", " + v.Место_проведения.Название, 
                        ДатаНачала = v.Дата_начала,
                        ДатаОкончания = v.Дата_окончания
                    })
                    .ToList();

                ExhibitionsDataGrid.ItemsSource = exhibitions;
            }
        }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser?.Role == "Visitor")
            {
                NavigationService?.Navigate(new VisitorPage());  
            }
            else if (App.CurrentUser?.Role == "Artist" || App.CurrentUser?.Role == "Organizer")
            {
                NavigationService?.Navigate(new DealtelPage());  
            }
            else
            {
                NavigationService?.Navigate(new AuthPage());  
            }
        }

    }
}

