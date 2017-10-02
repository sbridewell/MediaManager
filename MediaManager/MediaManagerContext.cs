#region Copyright (C) Simon Bridewell
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 3
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

// You can read the full text of the GNU General Public License at:
// http://www.gnu.org/licenses/gpl.html

// See also the Wikipedia entry on the GNU GPL at:
// http://en.wikipedia.org/wiki/GNU_General_Public_License
#endregion

using System;
using System.Data.Entity;

namespace SDE.MediaManager
{
    /// <summary>
    /// Provides the database context for the media manager application.
    /// </summary>
    /// <remarks>
    /// Making this a public class derived from <see cref="DbContext"/> means that all
    /// projects which reference the assembly containing this class, and all projects
    /// which reference those projects, also need to reference Entity Framework.
    /// </remarks>
    public class MediaManagerContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaManagerContext"/> class.
        /// </summary>
        public MediaManagerContext() : base("MediaManager") // TODO: a better way of setting the database name, e.g. connection string and config file
        {
             // RESTORE: Database.SetInitializer(new MigrateDatabaseToLatestVersion<MediaManagerContext, Migrations.Configuration>());

             // REMOVE: 
             Database.SetInitializer(new CreateDatabaseIfNotExists<MediaManagerContext>());
        }
        
        /// <summary>
        /// Gets or sets the application settings.
        /// We expect this table to contain only one record.
        /// </summary>
        public DbSet<Settings> Settings { get; set; }
        
        /// <summary>
        /// Gets or sets the media files known to the application.
        /// </summary>
        public DbSet<MediaFile> MediaFiles { get; set; }
        
        /// <summary>
        /// Gets or sets the names of the expected subfolders of the root folder.
        /// </summary>
        public DbSet<ExpectedFolder> ExpectedFolders { get; set; }
        
        /// <summary>
        /// Gets or sets the file types known to the application.
        /// </summary>
        public DbSet<FileType> FileTypes { get; set; }
        
        /// <summary>
        /// Gets or sets the attributes which can be assigned to media files.
        /// </summary>
        public DbSet<Attribute> Attributes { get; set; }
        
        /// <summary>
        /// Gets or sets the values that attributes can be set to.
        /// </summary>
        public DbSet<PossibleValue> PossibleValues { get; set; }
        
        /// <summary>
        /// Gets or sets the data types that attributes can have.
        /// </summary>
        public DbSet<DataType> DataTypes { get; set; }
        
        /// <summary>
        /// Gets or sets the metadata associated with media files.
        /// </summary>
        public DbSet<FileMetadata> FileMetadata { get; set; }
        
        /// <summary>
        /// Gets or sets the podcast channels known to this media manager.
        /// </summary>
        public DbSet<PodcastChannel> PodcastChannels { get; set; }
        
        /// <summary>
        /// Gets or sets the podcast episodes known to this media manager.
        /// </summary>
        public DbSet<PodcastEpisode> PodcastEpisodes { get; set; }
    }
}
