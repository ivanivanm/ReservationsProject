using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ReservationsDbContext:DbContext
    {
        public ReservationsDbContext():base()
        {
            
        }
        public ReservationsDbContext(DbContextOptions<ReservationsDbContext> options):base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = DESKTOP-SMAPSCS\\SQLEXPRESS; Database = ReservationsDb; Trusted_Connection = True;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>().Property(r => r.Id).UseIdentityColumn(0);
            modelBuilder.Entity<Client>().Property(c => c.Id).UseIdentityColumn(0);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
    }
}
