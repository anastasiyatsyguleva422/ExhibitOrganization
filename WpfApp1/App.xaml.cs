using System.Windows;
using WpfApp1; 

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
