using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class ArtistsListPage : Page
    {
        public ArtistsListPage()
        {
            InitializeComponent();
            LoadArtists();
        }

        private void LoadArtists()
        {
            using (var db = new ModernArtEntities())
            {
                var artists = db.Деятели
                    .Select(d => new
                    {
                        d.Имя,
                        d.Биография,
                        d.Контакты
                    })
                    .ToList();

                ArtistsDataGrid.ItemsSource = artists;
            }
        }

        private void ArtistsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedArtist = ArtistsDataGrid.SelectedItem as dynamic;  

            if (selectedArtist != null)
            {
                string artistInfo = $"ФИО: {selectedArtist.Имя}\nБиография: {selectedArtist.Биография}\nКонтакты: {selectedArtist.Контакты}";

                MessageBox.Show(artistInfo, "Информация о деятеле");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
