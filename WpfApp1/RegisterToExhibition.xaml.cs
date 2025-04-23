using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace WpfApp1
{
    public partial class RegisterToExhibition : Page
    {
        public RegisterToExhibition()
        {
            InitializeComponent();
            LoadExhibitions();
        }

        private void LoadExhibitions()
        {
            using (var db = new ModernArtEntities())
            {
                var exhibitions = db.Выставки.ToList();

                MessageBox.Show($"Загружено выставок: {exhibitions.Count}"); 

                ExhibitionComboBox.ItemsSource = exhibitions;
                ExhibitionComboBox.DisplayMemberPath = "Название";         
                ExhibitionComboBox.SelectedValuePath = "ID_Выставки";    
            }
        }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new DealtelPage());
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedExhibition = ExhibitionComboBox.SelectedItem as Выставки;
            if (selectedExhibition == null)
            {
                MessageBox.Show("Пожалуйста, выберите выставку.");
                return;
            }

            if (App.CurrentUser?.ID_Деятели == null)
            {
                MessageBox.Show("Вы не зарегистрированы как деятель.");
                return;
            }

            using (var db = new ModernArtEntities())
            {
                var exists = db.Выставки_Деятели
                    .FirstOrDefault(x => x.ID_Деятели == App.CurrentUser.ID_Деятели &&
                                         x.ID_Выставки == selectedExhibition.ID_Выставки);

                if (exists != null)
                {
                    MessageBox.Show("Вы уже зарегистрированы на эту выставку.");
                    return;
                }

                var registration = new Выставки_Деятели
                {
                    ID_Выставки = selectedExhibition.ID_Выставки,
                    ID_Деятели = App.CurrentUser.ID_Деятели.Value
                };

                db.Выставки_Деятели.Add(registration);
                db.SaveChanges();
            }

            MessageBox.Show("Вы успешно зарегистрировались на выставку!");
            NavigationService?.Navigate(new DealtelPage());


        }

    }
}