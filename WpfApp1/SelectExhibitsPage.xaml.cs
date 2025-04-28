using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class SelectExhibitsPage : Page
    {
        private ModernArtEntities db = new ModernArtEntities();

        public SelectExhibitsPage()
        {
            InitializeComponent();
            LoadExhibits();
        }


        private void LoadExhibits()
        {
            var exhibits = db.Экспонат
                .Select(e => new
                {
                    e.ID_Экспонат,
                    e.Название,
                    e.Тип_экспоната,
                    e.Год_создания,
                    НазваниеВыставки = e.ID_Выставки != null ? e.Выставки.Название : "Не выбрана"
                })
                .ToList();

            ExhibitsListView.ItemsSource = exhibits;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = ExhibitsListView.SelectedItems.Cast<dynamic>().ToList();

            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы один экспонат.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var chooseWindow = new ChooseExhibitionWindow();
            if (chooseWindow.ShowDialog() == true)
            {
                int? selectedExhibitionId = chooseWindow.SelectedExhibitionId;

                if (selectedExhibitionId == null)
                {
                    MessageBox.Show("Выставка не выбрана!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                foreach (var selectedItem in selectedItems)
                {
                    var exhibit = db.Экспонат.Find(selectedItem.ID_Экспонат);
                    if (exhibit != null)
                    {
                        exhibit.ID_Выставки = selectedExhibitionId.Value;
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Экспонаты успешно добавлены на выставку!");
                LoadExhibits();
            }
        }



        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = ExhibitsListView.SelectedItems.Cast<dynamic>().ToList();

            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы один экспонат.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (var selectedItem in selectedItems)
            {
                var exhibit = db.Экспонат.Find(selectedItem.ID_Экспонат);
                if (exhibit != null)
                {
                    exhibit.ID_Выставки = null;
                }
            }

            db.SaveChanges();
            MessageBox.Show("Экспонаты успешно убраны с выставки!");
            LoadExhibits();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = ExhibitsListView.SelectedItems.Cast<dynamic>().ToList();

            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы один экспонат.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (var selectedItem in selectedItems)
            {
                var exhibit = db.Экспонат.Find(selectedItem.ID_Экспонат);
                if (exhibit != null)
                {
                    db.Экспонат.Remove(exhibit);
                }
            }

            db.SaveChanges();
            MessageBox.Show("Экспонаты удалены!");
            LoadExhibits();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
