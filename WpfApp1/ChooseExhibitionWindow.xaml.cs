using System.Linq;
using System.Windows;

namespace WpfApp1
{
    public partial class ChooseExhibitionWindow : Window
    {
        public int? SelectedExhibitionId { get; private set; }

        public ChooseExhibitionWindow()
        {
            InitializeComponent();
            LoadExhibitions();
        }

        private void LoadExhibitions()
        {
            using (var db = new ModernArtEntities())
            {
                var exhibitions = db.Выставки.ToList();
                ExhibitionsComboBox.ItemsSource = exhibitions;
                ExhibitionsComboBox.DisplayMemberPath = "Название";
                ExhibitionsComboBox.SelectedValuePath = "ID_Выставки";
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedExhibitionId = (int?)ExhibitionsComboBox.SelectedValue;
            if (SelectedExhibitionId == null)
            {
                MessageBox.Show("Пожалуйста, выберите выставку!");
                return;
            }
            this.DialogResult = true;
        }
    }
}
