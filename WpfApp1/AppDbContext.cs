using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WpfApp1
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("ModernArtEntities") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Выставки> Exhibitions { get; set; }
        public DbSet<Выставки_Деятели> ExhibitionParticipants { get; set; }
        public DbSet<Отзывы> Reviews { get; set; }
        public DbSet<Посетители> Visitors { get; set; }
        public DbSet<Посетители_Отзывы> VisitorReviews { get; set; }
        public DbSet<Продажи> Sales { get; set; }
        public DbSet<Экспонат> Exhibits { get; set; }
        public DbSet<Место_проведения> PlacesOfExhibition { get; set; }
        public DbSet<Деятели> Deities { get; set; }
        public DbSet<Приглашения> Invitations { get; set; }




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Деятели>()
                .HasKey(d => d.ID_Деятели);

            modelBuilder.Entity<Выставки_Деятели>()
                .HasKey(x => new { x.ID_Выставки, x.ID_Деятели });

            base.OnModelCreating(modelBuilder);
        }
    }
}
