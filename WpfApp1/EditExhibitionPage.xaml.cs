using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class EditExhibitionPage : Page
    {
        private int exhibitionId;
        private Выставки currentExhibition;
        private ManageExhibitionsPage _parentPage;

        public EditExhibitionPage(int id, ManageExhibitionsPage parentPage)
        {
            InitializeComponent();
            exhibitionId = id;
            _parentPage = parentPage; 
            LoadExhibition();
        }



        private void LoadExhibition()
        {
            using (var db = new ModernArtEntities())
            {
                currentExhibition = db.Выставки.Find(exhibitionId);
                if (currentExhibition == null)
                {
                    MessageBox.Show("Выставка не найдена!");
                    NavigationService?.GoBack();
                    return;
                }

                TitleTextBox.Text = currentExhibition.Название;
                StartDatePicker.SelectedDate = currentExhibition.Дата_начала;
                EndDatePicker.SelectedDate = currentExhibition.Дата_окончания;
                CostTextBox.Text = currentExhibition.Стоимость?.ToString();
                DescriptionTextBox.Text = currentExhibition.Описание_тематики;

                var venues = db.Место_проведения.ToList();
                VenueComboBox.ItemsSource = venues;
                VenueComboBox.SelectedValue = currentExhibition.ID_Место_проведения;
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
                !StartDatePicker.SelectedDate.HasValue ||
                !EndDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Заполните обязательные поля (Название, Даты).");
                return;
            }

            using (var db = new ModernArtEntities())
            {
                var exhibition = db.Выставки.Find(exhibitionId);
                if (exhibition != null)
                {
                    exhibition.Название = TitleTextBox.Text.Trim();
                    exhibition.Дата_начала = StartDatePicker.SelectedDate.Value;
                    exhibition.Дата_окончания = EndDatePicker.SelectedDate.Value;
                    exhibition.Описание_тематики = DescriptionTextBox.Text.Trim();

                    if (decimal.TryParse(CostTextBox.Text, out decimal cost))
                        exhibition.Стоимость = cost;
                    else
                        exhibition.Стоимость = null;

                    if (VenueComboBox.SelectedValue != null)
                        exhibition.ID_Место_проведения = (int)VenueComboBox.SelectedValue;

                    db.SaveChanges();
                }
            }

            MessageBox.Show("Выставка успешно обновлена!");

            NavigationService?.Navigate(new ManageExhibitionsPage());
        }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ManageExhibitionsPage());
        }
    }
}
