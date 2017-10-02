namespace SDE.MediaManager.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using SDE.Utility;

    /// <summary>
    /// This class is created by running the command
    /// <code>enable-migrations -EnableAutomaticMigration:$true</code>
    /// in the package management console.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812", Justification = "This class is instantiated by Entity Framework")]
    internal sealed class Configuration : DbMigrationsConfiguration<SDE.MediaManager.MediaManagerContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        /// <summary>
        /// Seeds the database with static data after a migration.
        /// </summary>
        /// <param name="context">Entity Framework context for the media manager database.</param>
        protected override void Seed(SDE.MediaManager.MediaManagerContext context)
        {
            // This method will be called after migrating to the latest version.
            // You can use the DbSet<T>.AddOrUpdate() helper extension method 
            // to avoid creating duplicate seed data. E.g.
            //
            ////    context.People.AddOrUpdate(
            ////      p => p.FullName,
            ////      new Person { FullName = "Andrew Peters" },
            ////      new Person { FullName = "Brice Lambson" },
            ////      new Person { FullName = "Rowan Miller" }
            ////    );
            Guard.NotNull("context", context);

            // populate the DataType table
            context.DataTypes.AddOrUpdate(
                dt => dt.Name,
                new DataType { Name = "System.Int32" },
                new DataType { Name = "System.String" });
            
            // populate the FileType table
            context.FileTypes.AddOrUpdate(
                ft => ft.Extension,
                new FileType { Extension = "mp3", Name = "MPEG layer 3 audio", Description = "MP3" },
                new FileType { Extension = "wma", Name = "WMA", Description = "WMA" },
                new FileType { Extension = "m4a", Name = "M4A", Description = "M4A" },
                new FileType { Extension = "mpg", Name = "MPG", Description = "MPG" },
                new FileType { Extension = "mpeg", Name = "MPEG", Description = "MPEG" },
                new FileType { Extension = "avi", Name = "AVI", Description = "AVI" },
                new FileType { Extension = "mov", Name = "MOV", Description = "Quicktime" },
                new FileType { Extension = "flv", Name = "FLV", Description = "Flash video" },
                new FileType { Extension = "mp4", Name = "MP4", Description = "MP4" });
            
            context.Settings.AddOrUpdate(
                s => s.RootFolder, 
                new Settings { RootFolder = @"C:\Users\Simon\Music", PodcastSubfolder = "Podcasts" });
        }
    }
}
