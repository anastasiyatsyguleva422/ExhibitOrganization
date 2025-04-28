using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class ReviewsListPage : Page
    {
        public ReviewsListPage()
        {
            InitializeComponent();
            LoadReviews();
        }

        private void LoadReviews()
        {
            using (var db = new ModernArtEntities())
            {
                var reviews = db.Отзывы
                    .Join(db.Посетители_Отзывы,
                          r => r.ID_Отзывы,
                          pr => pr.ID_Отзывы,
                          (r, pr) => new
                          {
                              r.Текст,
                              Выставка = db.Выставки
                                    .Where(e => e.ID_Выставки == r.ID_Выставки)
                                    .Select(e => e.Название)
                                    .FirstOrDefault() ?? "Не указана",
                              Дата = pr.Дата
                          })
                    .ToList();

                ReviewsDataGrid.ItemsSource = reviews;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}