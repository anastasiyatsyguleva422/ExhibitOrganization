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
                        v.Название,
                        v.Место_проведения,
                        v.Дата_начала,
                        v.Дата_окончания
                    })
                    .ToList();

                ExhibitionsDataGrid.ItemsSource = exhibitions;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new DealtelPage());
        }
    }
}
