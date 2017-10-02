using System;
using MediaManager.MvcViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MediaManager.Mvc.UnitTests
{
    /// <summary>
    /// Fake implementation of <see cref="IMediaManagerDataSource"/>
    /// for use in unit tests to avoid having to access the real database.
    /// </summary>
    internal class FakeMediaManagerDb : IMediaManagerDataSource
    {
        public Dictionary<Type, object> Sets = new Dictionary<Type, object>();

        public List<object> Added = new List<object>();
        public List<object> Updated = new List<object>();
        public List<object> Removed = new List<object>();
        public bool Saved = false;

        public IQueryable<T> Query<T>() where T : class
        {
            return Sets[typeof(T)] as IQueryable<T>;
        }

        public void AddSet<T>(IQueryable<T> objects)
        {
            Sets.Add(typeof(T), objects);
        }

        public void Add<T>(T entity) where T : class
        {
            Added.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            Updated.Add(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            Removed.Add(entity);
        }

        public void SaveChanges()
        {
            Saved = true;
        }
    }
}