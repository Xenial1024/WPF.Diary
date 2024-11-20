namespace Diary
{
    using Diary.Models.Configurations;
    using Diary.Models.Domains;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Data.Entity.Infrastructure; 
    using System.Data.Entity.Migrations; 


    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("name=ApplicationDbContext")
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public static string CreateConnectionString()
        {
            return $"Server={Settings.Default.ServerAddress}\\{Settings.Default.ServerName};Database={Settings.Default.DatabaseName};User Id={Settings.Default.Username};Password={Settings.Default.Password};";
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new StudentConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new RatingConfiguration());
        }
    }
}