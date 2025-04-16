namespace ManagerProjects.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ManagerProjects.Classes.ProjectsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;  // Включаем авто-миграции
            AutomaticMigrationDataLossAllowed = false; // Запрещаем потерю данных
            ContextKey = "ManagerProjects.Classes.ProjectsContext";
        }

        protected override void Seed(ManagerProjects.Classes.ProjectsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
