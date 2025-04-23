using System.Windows;
using WpfApp1; // ← скорее всего, это нужное пространство имён (если нет — см. ниже)

namespace WpfApp1
{
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var context = new AppDbContext()) { }
        }
    }
}
