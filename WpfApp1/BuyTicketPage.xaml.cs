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
    public partial class BuyTicketPage : Page
    {
        public BuyTicketPage()
        {
            InitializeComponent();
            LoadExhibitions();
        }

        private void LoadExhibitions()
        {
            using (var db = new ModernArtEntities())
            {
                var registeredExhibitions = db.Выставки_Посетители
                    .Where(x => x.ID_Посетители == App.CurrentUser.ID_Посетители)
                    .Select(x => x.Выставки)
                    .ToList();

                ExhibitionComboBox.ItemsSource = registeredExhibitions;
                ExhibitionComboBox.DisplayMemberPath = "Название";
                ExhibitionComboBox.SelectedValuePath = "ID_Выставки";
            }
        }


        private void ExhibitionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedExhibition = ExhibitionComboBox.SelectedItem as Выставки;
            if (selectedExhibition != null)
            {
                PriceTextBlock.Text = $"Стоимость: {selectedExhibition.Стоимость:C2}";
            }
        }

       private void BuyTicketButton_Click(object sender, RoutedEventArgs e)
{
    var selectedExhibition = ExhibitionComboBox.SelectedItem as Выставки;

    if (selectedExhibition == null)
    {
        MessageBox.Show("Пожалуйста, выберите выставку.");
        return;
    }

    if (App.CurrentUser?.ID_Посетители == null)
    {
        MessageBox.Show("Вы не зарегистрированы как посетитель.");
        return;
    }

    using (var db = new ModernArtEntities())
    {
        var ticketPurchase = new ПокупкиБилетов
        {
            ID_Выставки = selectedExhibition.ID_Выставки,
            ID_Посетители = App.CurrentUser.ID_Посетители.Value,
            Стоимость = selectedExhibition.Стоимость,
            ДатаПокупки = DateTime.Now
        };

        db.ПокупкиБилетов.Add(ticketPurchase);
        db.SaveChanges();

        var exhibitionToRemove = db.Выставки_Посетители
            .FirstOrDefault(x => x.ID_Посетители == App.CurrentUser.ID_Посетители && x.ID_Выставки == selectedExhibition.ID_Выставки);

        if (exhibitionToRemove != null)
        {
            db.Выставки_Посетители.Remove(exhibitionToRemove);
            db.SaveChanges();
        }

        var writeReviewPage = new WriteReviewPage();
        writeReviewPage.LoadExhibitions();  

        LoadExhibitions();
    }

    MessageBox.Show("Билет успешно куплен!");

    NavigationService?.Navigate(new VisitorPage());
}



        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new VisitorPage());  
        }
    }
}
