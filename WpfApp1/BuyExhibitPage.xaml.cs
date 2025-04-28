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
    /// Логика взаимодействия для BuyExhibitPage.xaml
    /// </summary>
    public partial class BuyExhibitPage : Page
    {
        public BuyExhibitPage()
        {
            InitializeComponent();
            LoadExhibits();
        }

        private void LoadExhibits()
        {
            using (var db = new ModernArtEntities())
            {
                var availableExhibits = db.Экспонат
                    .Where(e => e.Цена != null) 
                    .Select(e => new { e.ID_Экспонат, e.Название, e.Цена })
                    .ToList();

                ExhibitComboBox.ItemsSource = availableExhibits;
                ExhibitComboBox.DisplayMemberPath = "Название";
                ExhibitComboBox.SelectedValuePath = "ID_Экспонат";
            }
        }

        private void ExhibitComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedExhibit = ExhibitComboBox.SelectedItem as dynamic;
            if (selectedExhibit != null)
            {
                PriceTextBlock.Text = $"Стоимость: {selectedExhibit.Цена} руб.";
            }
        }


        private void BuyExhibitButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedExhibit = ExhibitComboBox.SelectedItem as dynamic;

            if (selectedExhibit == null)
            {
                MessageBox.Show("Пожалуйста, выберите экспонат для покупки.");
                return;
            }

            if (selectedExhibit.Цена <= 0)
            {
                MessageBox.Show("Этот экспонат не продается или цена не установлена.");
                return;
            }

            try
            {
                using (var db = new ModernArtEntities())
                {
                    var sale = new Продажи
                    {
                        ID_Экспонат = selectedExhibit.ID_Экспонат,
                        Стоимость = selectedExhibit.Цена  
                    };

                    db.Продажи.Add(sale);  
                    db.SaveChanges();      
                }

                MessageBox.Show("Экспонат успешно куплен!");
                NavigationService?.Navigate(new VisitorPage());  
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при покупке: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new VisitorPage());  
        }
    }
}