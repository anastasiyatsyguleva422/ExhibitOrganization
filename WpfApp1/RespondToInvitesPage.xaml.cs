using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class RespondToInvitesPage : Page
    {
        private ModernArtEntities db = new ModernArtEntities();

        public RespondToInvitesPage()
        {
            InitializeComponent();
            LoadInvites();
        }

        private void LoadInvites()
        {
            var currentId = App.CurrentUser?.ID_Деятели;

            if (currentId != null)
            {
                var invites = db.Приглашения
                    .Where(p => p.ID_Деятели == currentId)
                    .ToList();

                InvitesListView.ItemsSource = invites;
            }
        }

        private void AcceptInvite_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var invite = button?.DataContext as Приглашения;

            if (invite != null)
            {
                invite.Статус = "Принято";

                var alreadyRegistered = db.Выставки_Деятели.Any(x =>
                    x.ID_Выставки == invite.ID_Выставки &&
                    x.ID_Деятели == invite.ID_Деятели);

                if (!alreadyRegistered)
                {
                    var registration = new Выставки_Деятели
                    {
                        ID_Выставки = invite.ID_Выставки,
                        ID_Деятели = invite.ID_Деятели,
                        ДатаРегистрации = DateTime.Now
                    };

                    db.Выставки_Деятели.Add(registration);
                }

                db.SaveChanges();
                LoadInvites();
            }
        }

        private void RejectInvite_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var invite = button?.DataContext as Приглашения;

            if (invite == null)
            {
                MessageBox.Show("Приглашение не найдено (null)");
                return;
            }

            invite.Статус = "Отклонено";

            db.Приглашения.Remove(invite);
            db.SaveChanges();

            LoadInvites();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}

