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
    /// <summary>
    /// Логика взаимодействия для InviteParticipantsPage.xaml
    /// </summary>
    public partial class InviteParticipantsPage : Page
    {
        public InviteParticipantsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var db = new AppDbContext())
            {
                ExhibitionComboBox.ItemsSource = db.Exhibitions.ToList();
                DeityComboBox.ItemsSource = db.Deities.ToList();
            }
        }

        private void InviteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedExhibition = ExhibitionComboBox.SelectedItem as Выставки;
            var selectedDeity = DeityComboBox.SelectedItem as Деятели;

            if (selectedExhibition != null && selectedDeity != null)
            {
                using (var db = new AppDbContext())
                {
                    var invitation = new Приглашения
                    {
                        ID_Выставки = selectedExhibition.ID_Выставки,
                        ID_Деятели = selectedDeity.ID_Деятели,
                        Статус = "Ожидает",
                        Дата_приглашения = DateTime.Now
                    };

                    db.Invitations.Add(invitation);
                    db.SaveChanges();
                }

                MessageBox.Show("Приглашение успешно отправлено!");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите и выставку, и деятеля.");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}