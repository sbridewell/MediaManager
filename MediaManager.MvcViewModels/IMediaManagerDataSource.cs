using SDE.MediaManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.MvcViewModels
{
    /// <summary>
    /// Interface for the data store used by the MediaManager application.
    /// </summary>
    /// <remarks>
    /// Coding against this interface in your controllers rather than using the
    /// actual concrete type (e.g. an Entity Framework DbContext) has two advantages:
    /// <list type="ordered">
    /// <item>
    /// Unit testing is easier because the actual data store can be replaced with test
    /// doubles such as fakes or stubs, removing the need to maintain a database in a
    /// consistent state.
    /// </item>
    /// <item>
    /// It makes it easier to replace one data provider with another, e.g. changing from
    /// SQL Server to some other database infrastructure.
    /// </item>
    /// </list>
    /// </remarks>
    public interface IMediaManagerDataSource
    {
        /// <summary>
        /// Gets the instances of <typeparamref name="T"/> from the data store.
        /// You can append method calls from the System.Linq namespace to further
        /// filter, order, group or perform other operations on the collection.
        /// </summary>
        /// <typeparam name="T"><see cref="System.Type"/> of the entities to get.</typeparam>
        /// <returns>The instances of T from the data store.</returns>
        IQueryable<T> Query<T>() where T : class;

        /// <summary>
        /// Adds the supplied instance of <typeparamref name="T"/> to the data store.
        /// </summary>
        /// <typeparam name="T"><see cref="System.Type"/> of the instance to add.</typeparam>
        /// <param name="entity">The instance to add.</param>
        void Add<T>(T entity) where T : class;

        /// <summary>
        /// Updates the properties of the supplied instance of <typeparamref name="T"/> 
        /// in the data store.
        /// </summary>
        /// <typeparam name="T"><see cref="System.Type"/> of the instance to update.</typeparam>
        /// <param name="entity">The instance to update.</param>
        void Update<T>(T entity) where T : class;

        /// <summary>
        /// Removes the supplied instance of <typeparamref name="T"/> from the data store.
        /// </summary>
        /// <typeparam name="T"><see cref="System.Type"/> of the instance to remove.</typeparam>
        /// <param name="entity">The instance to remove.</param>
        void Remove<T>(T entity) where T : class;

        /// <summary>
        /// Commits to the data store any changes made using the Add, Update or Remove methods.
        /// </summary>
        void SaveChanges();
    }
}
