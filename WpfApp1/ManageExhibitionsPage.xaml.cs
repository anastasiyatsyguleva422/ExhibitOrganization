using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class ManageExhibitionsPage : Page
    {
        public ManageExhibitionsPage()
        {
            InitializeComponent();
            LoadExhibitions();
        }

        private void LoadExhibitions()
        {
            using (var db = new ModernArtEntities())
            {
                var exhibitions = db.Выставки.Include("Место_проведения").ToList();
                ExhibitionsDataGrid.ItemsSource = exhibitions;
            }
        }

        private void EditExhibition_Click(object sender, RoutedEventArgs e)
        {
            var selected = ExhibitionsDataGrid.SelectedItem as Выставки;
            if (selected != null)
            {
                NavigationService?.Navigate(new EditExhibitionPage(selected.ID_Выставки));
            }
            else
            {
                MessageBox.Show("Выберите выставку для редактирования.");
            }


        }
        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new OrganizerPage());
        }
    }
}
