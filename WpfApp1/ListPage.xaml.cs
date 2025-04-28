using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class ListPage : Page
    {
        public ListPage()
        {
            InitializeComponent();
            LoadExhibitions();  
        }

        private void LoadExhibitions()
        {
            using (var db = new ModernArtEntities())
            {
                var exhibitions = db.Выставки.ToList();
                ExhibitionComboBox.ItemsSource = exhibitions;
                ExhibitionComboBox.DisplayMemberPath = "Название"; 
                ExhibitionComboBox.SelectedValuePath = "ID_Выставки"; 
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new VisitorPage()); 
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedExhibition = ExhibitionComboBox.SelectedItem as Выставки;
            if (selectedExhibition == null)
            {
                MessageBox.Show("Пожалуйста, выберите выставку.");
                return;
            }

            if (App.CurrentUser?.Role != "Visitor")
            {
                MessageBox.Show("Вы не зарегистрированы как посетитель.");
                return;
            }

            using (var db = new ModernArtEntities())
            {
                var exists = db.Выставки_Посетители
                    .FirstOrDefault(x => x.ID_Посетители == App.CurrentUser.ID_Посетители &&
                                         x.ID_Выставки == selectedExhibition.ID_Выставки);

                if (exists != null)
                {
                    MessageBox.Show("Вы уже зарегистрированы на эту выставку.");
                    return;
                }

                var registration = new Выставки_Посетители
                {
                    ID_Выставки = selectedExhibition.ID_Выставки,
                    ID_Посетители = App.CurrentUser.ID_Посетители.Value,
                    ДатаРегистрации = DateTime.Now
                };

                db.Выставки_Посетители.Add(registration);  
                db.SaveChanges();
            }

            MessageBox.Show("Вы успешно зарегистрировались на выставку!");
            NavigationService?.Navigate(new VisitorPage());  
        }

        
    }
}
