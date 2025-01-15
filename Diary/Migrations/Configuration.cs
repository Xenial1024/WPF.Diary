using System.Data.Entity.Migrations;

namespace Diary.Migrations
{
    sealed class Configuration : DbMigrationsConfiguration<Diary.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Diary.ApplicationDbContext";
        }

        protected override void Seed(Diary.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
