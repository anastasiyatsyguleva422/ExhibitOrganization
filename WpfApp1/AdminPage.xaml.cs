using System.Windows;
using System.Windows.Controls;
using ClosedXML.Excel;
using System.IO;
using System.Linq;

namespace WpfApp1
{
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }
        private void ExportAttendanceAndRevenue_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ModernArtEntities())
            {
                var data = db.Выставки_Посетители
                    .Where(x => x.Выставки != null && x.Посетители != null)
                    .Select(x => new
                    {
                        Посетитель = x.Посетители.ФИО,
                        Выставка = x.Выставки.Название,
                        Стоимость = x.Выставки.Стоимость
                    }).ToList();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Посетители");
                    worksheet.Cell(1, 1).Value = "Посетитель";
                    worksheet.Cell(1, 2).Value = "Выставка";
                    worksheet.Cell(1, 3).Value = "Стоимость билета";

                    for (int i = 0; i < data.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = data[i].Посетитель;
                        worksheet.Cell(i + 2, 2).Value = data[i].Выставка;
                        worksheet.Cell(i + 2, 3).Value = data[i].Стоимость;
                    }

                    var path = "Посетители_и_билеты.xlsx";
                    workbook.SaveAs(path);
                    MessageBox.Show($"Отчёт сохранён: {Path.GetFullPath(path)}");
                }
            }
        }
        private void ExportCurrentExhibitions_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ModernArtEntities())
            {
                var data = db.Выставки
                    .Select(x => new
                    {
                        Название = x.Название,
                        ДатаНачала = x.Дата_начала,
                        ДатаОкончания = x.Дата_окончания,
                        Место = x.Место_проведения.Название
                    }).ToList();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Выставки");
                    worksheet.Cell(1, 1).Value = "Название";
                    worksheet.Cell(1, 2).Value = "Дата начала";
                    worksheet.Cell(1, 3).Value = "Дата окончания";
                    worksheet.Cell(1, 4).Value = "Место проведения";

                    for (int i = 0; i < data.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = data[i].Название;
                        worksheet.Cell(i + 2, 2).Value = data[i].ДатаНачала.ToShortDateString();
                        worksheet.Cell(i + 2, 3).Value = data[i].ДатаОкончания.ToShortDateString();
                        worksheet.Cell(i + 2, 4).Value = data[i].Место;
                    }

                    var path = "Список_выставок.xlsx";
                    workbook.SaveAs(path);
                    MessageBox.Show($"Отчёт сохранён: {Path.GetFullPath(path)}");
                }
            }
        }
        private void ExportExhibitSales_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ModernArtEntities())
            {
                var data = db.Продажи
                    .Where(x => x.Экспонат != null && x.Экспонат.Деятели != null)
                    .Select(x => new
                    {
                        Работа = x.Экспонат.Название,
                        Деятель = x.Экспонат.Деятели.Имя,
                        Цена = x.Стоимость
                    }).ToList();

                using (var workbook = new ClosedXML.Excel.XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Продажи");
                    worksheet.Cell(1, 1).Value = "Название работы";
                    worksheet.Cell(1, 2).Value = "Деятель";
                    worksheet.Cell(1, 3).Value = "Цена";

                    for (int i = 0; i < data.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = data[i].Работа;
                        worksheet.Cell(i + 2, 2).Value = data[i].Деятель;
                        worksheet.Cell(i + 2, 3).Value = data[i].Цена;
                    }

                    var path = "Продажи_экспонатов.xlsx";
                    workbook.SaveAs(path);
                    MessageBox.Show($"Отчёт сохранён: {System.IO.Path.GetFullPath(path)}");
                }
            }
        }


        private void ViewArtistsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ArtistsListPage());
        }

        private void ViewUsers_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new UsersListPage());
        }

        private void ViewExhibitions_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ExhibitionListPage());
        }

        private void ViewReviews_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ReviewsListPage());
        }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AuthPage());
        }

        
    }
}