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
            var title = TitleTextBox.Text;
            var description = DescriptionTextBox.Text;
            var startDate = StartDatePicker.SelectedDate;
            var endDate = EndDatePicker.SelectedDate;
            var cost = decimal.TryParse(CostTextBox.Text, out decimal exhibitionCost) ? exhibitionCost : 0;
            var venueId = (int?)VenueComboBox.SelectedValue;

            if (string.IsNullOrWhiteSpace(title) || startDate == null || endDate == null || venueId == null)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
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
                    Стоимость = cost,
                    ID_Место_проведения = venueId
                };

                db.Выставки.Add(newExhibition);
                db.SaveChanges();
                MessageBox.Show("Выставка успешно создана!");

                NavigationService?.Navigate(new OrganizerPage());
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new OrganizerPage());
        }
    }
}
