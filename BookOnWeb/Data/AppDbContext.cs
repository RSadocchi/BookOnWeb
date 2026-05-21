using BookOnWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BookOnWeb.Data
{
    public class AppDbContext : DbContext
    {
        public string ConnectionString { get; set; } = null!;

        private void _loadConfiguration()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                    .Build();
                ConnectionString = configuration.GetConnectionString("Application") ?? throw new ArgumentNullException(nameof(ConnectionString), "Connection string 'Default' is not configured.");
            }
        }

        public AppDbContext() { _loadConfiguration(); }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            _loadConfiguration();
        }

        public DbSet<Autore> Autori { get; set; }
        public DbSet<Libro> Libri { get; set; }
        public DbSet<Prestito> Prestiti { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
                optionsBuilder
                    .UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Autore)
                .WithMany(a => a.Libri)
                .HasForeignKey(l => l.AutoreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prestito>()
                .HasOne(l => l.Libro)
                .WithMany(p => p.Prestiti)
                .HasForeignKey(p => p.LibroId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
