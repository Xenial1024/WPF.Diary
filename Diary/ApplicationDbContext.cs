using Diary.Models.Configurations;
using Diary.Models.Domains;
using System.Data.Entity;

namespace Diary
{
    public class ApplicationDbContext : DbContext
    {
        private static readonly string _connectionString = Settings.Default.AreSystemCredentialsUsed? 
            $"Server={Settings.Default.ServerAddress}\\{Settings.Default.ServerName};Database={Settings.Default.DatabaseName};Trusted_Connection=True;"
            :
            $"Server={Settings.Default.ServerAddress}\\{Settings.Default.ServerName};Database={Settings.Default.DatabaseName};User Id={Settings.Default.Username};Password={Settings.Default.Password};";

        public ApplicationDbContext() : base(_connectionString)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new StudentConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new RatingConfiguration());
        }
    }
}