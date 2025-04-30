using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class AddExhibitPage : Page
    {
        public AddExhibitPage()
        {
            InitializeComponent();
            LoadExhibitions();
            LoadYears();
            SaleComboBox.SelectionChanged += SaleComboBox_SelectionChanged;
        }

        private void LoadExhibitions()
        {
            using (var db = new ModernArtEntities())
            {
                var exhibitions = db.Выставки.ToList();
                ExhibitionComboBox.ItemsSource = exhibitions;
            }
        }

        private void LoadYears()
        {
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear; year >= 1900; year--)
            {
                YearComboBox.Items.Add(year);
            }
        }

        private void SaleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SaleComboBox.SelectedItem != null)
            {
                string selectedOption = (SaleComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                if (selectedOption == "Да")
                {
                    PriceLabel.Visibility = Visibility.Visible;
                    PriceTextBox.Visibility = Visibility.Visible;
                }
                else
                {
                    PriceLabel.Visibility = Visibility.Collapsed;
                    PriceTextBox.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void AddExhibitButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser?.ID_Деятели == null)
            {
                MessageBox.Show("Вы не зарегистрированы как деятель.");
                return;
            }

            string title = TitleTextBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();
            string type = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            int? year = YearComboBox.SelectedItem as int?;
            var selectedExhibition = ExhibitionComboBox.SelectedItem as Выставки;
            string saleOption = (SaleComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            decimal? price = null;

            if (saleOption == "Да" && !string.IsNullOrEmpty(PriceTextBox.Text))
            {
                decimal parsedPrice;
                if (Decimal.TryParse(PriceTextBox.Text.Trim(), out parsedPrice))
                {
                    if (parsedPrice < 0)
                    {
                        MessageBox.Show("Цена не может быть отрицательной.");
                        return;
                    }
                    price = parsedPrice;
                }
                else
                {
                    MessageBox.Show("Введите корректную цену.");
                    return;
                }
            }


            if (string.IsNullOrWhiteSpace(title) || type == null || year == null || selectedExhibition == null)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }

            using (var db = new ModernArtEntities())
            {
                var exhibit = new Экспонат
                {
                    Название = title,
                    Описание = description,
                    Тип_экспоната = type,
                    Год_создания = year,
                    ID_Выставки = selectedExhibition.ID_Выставки,
                    ID_Деятели = App.CurrentUser.ID_Деятели.Value,
                    Цена = price
                };

                db.Экспонат.Add(exhibit);
                db.SaveChanges();

                MessageBox.Show($"Экспонат успешно добавлен! (ID: {exhibit.ID_Экспонат})");
                NavigationService?.Navigate(new DealtelPage());
            }
        }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new DealtelPage());
        }
    }
}
