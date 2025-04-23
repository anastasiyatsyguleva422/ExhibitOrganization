using System.Data.Entity.Migrations;

namespace WpfApp1.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<WpfApp1.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WpfApp1.AppDbContext context)
        {
            // данные по умолчанию, если нужны
        }
    }
}
