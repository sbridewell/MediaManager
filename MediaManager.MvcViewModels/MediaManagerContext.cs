using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDE.MediaManager;

namespace MediaManager.MvcViewModels
{
    /// <summary>
    /// Concrete implementation of the <see cref="IMediaManagerDataSource"/>
    /// interface for the MediaManager application.
    /// This implementation uses Entity Framework.
    /// </summary>
    /// <remarks>
    /// Code against the <see cref="IMediaManagerDataSource"/> interface rather than
    /// against this concrete class, to make unit testing and changing data stores
    /// easier.
    /// See the remarks on the <see cref="IMediaManagerDataSource"/> interface for more
    /// information about the benefits of doing this.
    /// </remarks>
    public sealed class MediaManagerContext : DbContext, IMediaManagerDataSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaManagerContext"/> class
        /// using the "DefaultConnection" connection string from the application config file.
        /// </summary>
        public MediaManagerContext() : base("DefaultConnection")
        {
        }

        /// <summary>
        /// Gets or sets the instances of <see cref="MediaFile"/> from the database.
        /// </summary>
        public DbSet<MediaFile> MediaFiles { get; set; }

        /// <summary>
        /// Gets or sets the instances of <see cref="FileMetadata"/> from the database.
        /// </summary>
        public DbSet<FileMetadata> Metadata { get; set; }

        /// <summary>
        /// Gets or sets the instances of <see cref="FileType"/> from the database.
        /// </summary>
        public DbSet<FileType> FileTypes { get; set; }

        /// <summary>
        /// Gets the instances of <typeparamref name="T"/> from the data store.
        /// You can append method calls from the System.Linq namespace to further
        /// filter, order, group or perform other operations on the collection.
        /// </summary>
        /// <typeparam name="T"><see cref="System.Type"/> of the entities to get.</typeparam>
        /// <returns>The instances of T from the data store.</returns>
        IQueryable<T> IMediaManagerDataSource.Query<T>()
        {
            return Set<T>();
        }

        /// <summary>
        /// Adds the supplied instance of <typeparamref name="T"/> to the data store.
        /// </summary>
        /// <typeparam name="T"><see cref="System.Type"/> of the instance to add.</typeparam>
        /// <param name="entity">The instance to add.</param>
        void IMediaManagerDataSource.Add<T>(T entity)
        {
            Set<T>().Add(entity);
        }

        /// <summary>
        /// Updates the properties of the supplied instance of <typeparamref name="T"/> 
        /// in the data store.
        /// </summary>
        /// <typeparam name="T"><see cref="System.Type"/> of the instance to update.</typeparam>
        /// <param name="entity">The instance to update.</param>
        void IMediaManagerDataSource.Update<T>(T entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Removes the supplied instance of <typeparamref name="T"/> from the data store.
        /// </summary>
        /// <typeparam name="T"><see cref="System.Type"/> of the instance to remove.</typeparam>
        /// <param name="entity">The instance to remove.</param>
        void IMediaManagerDataSource.Remove<T>(T entity)
        {
            Set<T>().Remove(entity);
        }

        /// <summary>
        /// Commits to the data store any changes made using the Add, Update or Remove methods.
        /// </summary>
        void IMediaManagerDataSource.SaveChanges()
        {
            SaveChanges();
        }

        // REMOVE:
        //IQueryable<MediaFile> IMediaManagerDataSource.MediaFiles
        //{
        //    get { return MediaFiles; }
        //}

        //IQueryable<FileMetadata> IMediaManagerDataSource.Metadata
        //{
        //    get { return MetaData; }
        //}
    }
}
