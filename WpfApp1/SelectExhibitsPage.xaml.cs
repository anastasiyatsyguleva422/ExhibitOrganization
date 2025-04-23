using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class SelectExhibitsPage : Page
    {
        private ModernArtEntities db = new ModernArtEntities();
        private int selectedExhibitionId;

        public SelectExhibitsPage(int exhibitionId)
        {
            InitializeComponent();
            selectedExhibitionId = exhibitionId; 
            LoadExhibits();
        }

        private void LoadExhibits()
        {
            var exhibits = db.Экспонат.ToList(); 
            ExhibitsListView.ItemsSource = exhibits;
        }


        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedExhibits = ExhibitsListView.SelectedItems.Cast<Экспонат>().ToList();

            if (selectedExhibits.Any())
            {
                foreach (var exhibit in selectedExhibits)
                {
                    exhibit.ID_Выставки = selectedExhibitionId;  
                }

                db.SaveChanges();
                MessageBox.Show("Экспонаты успешно отобраны для выставки!");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите хотя бы один экспонат.");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
