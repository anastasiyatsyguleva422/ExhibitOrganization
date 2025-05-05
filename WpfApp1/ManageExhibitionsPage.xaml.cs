using System.Data.Entity;
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
            this.KeepAlive = false; 
            LoadExhibitions();

        }

        public void LoadExhibitions()
        {
            using (var db = new ModernArtEntities())
            {
                var exhibitions = db.Выставки
                    .Include(x => x.Место_проведения)
                    .AsNoTracking() 
                    .ToList();

                ExhibitionsDataGrid.ItemsSource = null;
                ExhibitionsDataGrid.Items.Clear();
                ExhibitionsDataGrid.ItemsSource = exhibitions;
                ExhibitionsDataGrid.Items.Refresh();
            }
        }


        private void EditExhibition_Click(object sender, RoutedEventArgs e)
        {
            var selected = ExhibitionsDataGrid.SelectedItem as Выставки;
            if (selected != null)
            {
                NavigationService?.Navigate(new EditExhibitionPage(selected.ID_Выставки, this));
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
