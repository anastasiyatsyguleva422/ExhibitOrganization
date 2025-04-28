using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class CreateExhibitionPage : Page
    {
        public CreateExhibitionPage()
        {
            InitializeComponent();
            LoadVenues();
        }

        private void LoadVenues()
        {
            using (var db = new ModernArtEntities())
            {
                var venues = db.Место_проведения.ToList();
                VenueComboBox.ItemsSource = venues;
                VenueComboBox.DisplayMemberPath = "Название";
                VenueComboBox.SelectedValuePath = "ID_Место_проведения";
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var title = TitleTextBox.Text.Trim();
            var description = DescriptionTextBox.Text.Trim();
            var startDate = StartDatePicker.SelectedDate;
            var endDate = EndDatePicker.SelectedDate;
            var costText = CostTextBox.Text.Trim();
            var venueId = (int?)VenueComboBox.SelectedValue;

            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(description) ||
                startDate == null ||
                endDate == null ||
                venueId == null ||
                string.IsNullOrWhiteSpace(costText))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(costText, out decimal exhibitionCost) || exhibitionCost <= 0)
            {
                MessageBox.Show("Введите корректную положительную стоимость участия!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (startDate <= DateTime.Today)
            {
                MessageBox.Show("Дата начала выставки должна быть позже сегодняшнего дня!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (endDate <= startDate)
            {
                MessageBox.Show("Дата окончания выставки должна быть позже даты начала!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new ModernArtEntities())
            {
                var newExhibition = new Выставки
                {
                    Название = title,
                    Описание_тематики = description,
                    Дата_начала = startDate.Value,
                    Дата_окончания = endDate.Value,
                    Стоимость = exhibitionCost,
                    ID_Место_проведения = venueId.Value
                };

                db.Выставки.Add(newExhibition);
                db.SaveChanges();

                MessageBox.Show("Выставка успешно создана!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.Navigate(new OrganizerPage());
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new OrganizerPage());
        }
    }
}
