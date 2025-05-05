using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class WriteReviewPage : Page
    {
        public WriteReviewPage()
        {
            InitializeComponent();  
            LoadExhibitions();  
        }

        public void LoadExhibitions()
        {
            using (var db = new ModernArtEntities())
            {
                if (App.CurrentUser?.ID_Посетители == null)
                {
                    MessageBox.Show("Ошибка: пользователь не является посетителем.");
                    return;
                }

                var purchasedExhibitions = db.ПокупкиБилетов
                    .Where(p => p.ID_Посетители == App.CurrentUser.ID_Посетители)
                    .Select(p => p.Выставки)
                    .ToList();

                ExhibitionComboBox.ItemsSource = purchasedExhibitions;
                ExhibitionComboBox.DisplayMemberPath = "Название";
                ExhibitionComboBox.SelectedValuePath = "ID_Выставки";
            }
        }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new VisitorPage());  
        }

        private void SubmitReviewButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedExhibition = ExhibitionComboBox.SelectedItem as Выставки;
            var reviewText = ReviewTextBox.Text.Trim();

            if (selectedExhibition == null || string.IsNullOrWhiteSpace(reviewText))
            {
                MessageBox.Show("Пожалуйста, выберите выставку и введите отзыв.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (App.CurrentUser?.ID_Посетители == null)
            {
                MessageBox.Show("Вы не зарегистрированы как посетитель.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new ModernArtEntities())
            {
                var review = new Отзывы
                {
                    Текст = reviewText,
                    ID_Выставки = selectedExhibition.ID_Выставки
                };

                db.Отзывы.Add(review);
                db.SaveChanges();

                var visitorReview = new Посетители_Отзывы
                {
                    ID_Посетители = App.CurrentUser.ID_Посетители.Value,
                    ID_Отзывы = review.ID_Отзывы,
                    Дата = DateTime.Now
                };

                db.Посетители_Отзывы.Add(visitorReview);
                db.SaveChanges();
            }

            MessageBox.Show("Отзыв успешно оставлен!");
            NavigationService?.Navigate(new VisitorPage());  
        }
    }
}
