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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using SDE.MediaManager.SqlClient;

namespace SDE.MediaManager
{
    /// <summary>
    /// Abstract base class for an entity used by the Media Manager.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class EntityBase
    {
        /// <summary>
        /// Cache of the properties of different types.
        /// </summary>
        private static Dictionary<Type, Dictionary<string, PropertyInfo>> _columnDictionaries 
            = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
        
        /// <summary>
        /// The <see cref="Type"/>s in the current assembly which are derived from <see cref="EntityBase"/>.
        /// </summary>
        private Collection<Type> _entityTypes;
        
        /// <summary>
        /// The tables in the database which correspond to types derived from <see cref="EntityBase"/>.
        /// </summary>
        private Collection<Table> _entityTables;
        
        /// <summary>
        /// The names of the tables in the database which correspond to types derived from <see cref="EntityBase"/>.
        /// </summary>
        private Collection<string> _entityTableNames;
        
        /// <summary>
        /// Gets the runtime type of the current instance.
        /// TODO: is TypeName required? Not referenced anywhere.
        /// </summary>
        [Category(Category.Identities)]
        public string TypeName
        {
            get 
            {
                return this.GetType().FullName;
            }
        }
        
        #region EntityTypes property
        /// <summary>
        /// Gets the <see cref="Type"/>s in the current assembly which are derived from <see cref="EntityBase"/>.
        /// </summary>
        public Collection<Type> EntityTypes
        {
            get
            {
                if (_entityTypes == null)
                {
                    _entityTypes = new Collection<Type>();
                    Type entityBase = typeof(EntityBase);
                    Assembly currentAssembly = Assembly.GetAssembly(entityBase);
                    IEnumerable<Type> exportedTypes = currentAssembly.ExportedTypes;
                    foreach (Type t in exportedTypes)
                    {
                        if (entityBase.IsAssignableFrom(t))
                        {
                            if (t == entityBase)
                            {
                                // EntityBase isn't an entity itself, it's a base type 
                                // implementing common functionality, so don't add it
                                continue;
                            }
                            
                            if (t == typeof(Table))
                            {
                                // Table doesn't have a table, it's just a SP which lists the tables
                                continue;
                            }
                            
                            // then T is derived from EntityBase, but isn't EntityBase itself
                            // so add it to the collection.
                            _entityTypes.Add(t);
                        }
                    }
                }
                
                return _entityTypes;
            }
        }
        #endregion
        
        #region EntityTables property
        /// <summary>
        /// Gets the tables in the database which correspond to types derived from <see cref="EntityBase"/>.
        /// </summary>
        public Collection<Table> EntityTables
        {
            get
            {
                if (_entityTables == null)
                {
                    using (EnquireTablesSP sp = new EnquireTablesSP(typeof(Table)))
                    {
                        _entityTables = FromDataTable<Table>(sp.ExecuteAndReturnDataTable());
                    }
                }
                
                return _entityTables;
            }
        }
        #endregion
        
        #region EntityTableNames property
        /// <summary>
        /// Gets the names of the tables in the database which correspond to types derived from <see cref="EntityBase"/>.
        /// </summary>
        public Collection<string> EntityTableNames
        {
            get
            {
                if (_entityTableNames == null)
                {
                    Collection<Table> tables = EntityTables;
                    _entityTableNames = new Collection<string>();
                    foreach (Table table in tables)
                    {
                        _entityTableNames.Add(table.Name);
                    }
                }
                
                return _entityTableNames;
            }
        }
        #endregion
        
        #region public static GetColumns method
        /// <summary>
        /// Gets a dictionary of the properties of the supplied type which 
        /// are included in the parameters or result set of stored procedures 
        /// for the supplied type.
        /// The dictionary is built only once for each type and is cached.
        /// </summary>
        /// <param name="type">The type for which to return property names and types.</param>
        /// <returns>Names and <see cref="PropertyInfo"/>s of the properties.</returns>
        public static Dictionary<string, PropertyInfo> GetColumns(Type type)
        {
            if (_columnDictionaries.ContainsKey(type) == false)
            {
                // No column dictionary for the supplied type, so build one
                Dictionary<string, PropertyInfo> columns = new Dictionary<string, PropertyInfo>();
                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
                PropertyInfo[] properties = type.GetProperties(bindingFlags);
                foreach (PropertyInfo property in properties) 
                {
                    // TODO: why do we still need to test for property being writeable?
                    if (property.CanWrite == false)
                    {
                        // property without a setter, ignore and move on to the next property
                        continue;
                    }
                    
                    // DONE: test for Category.PropertiesOfThisClass and Category.Identities instead
                    CategoryAttribute att = (CategoryAttribute)property.GetCustomAttribute(typeof(CategoryAttribute));
                    if (att == null)
                    {
                        // the property isn't decorated with a CategoryAttribute so move on to the next property
                        continue;
                    }
                    
                    if (att.Category == Category.Identities || att.Category == Category.PropertiesOfThisClass)
                    {
                        string propertyName = property.Name;
                        columns.Add(propertyName, property);
                    }
                }
                
                _columnDictionaries.Add(type, columns);
            }
            
            return _columnDictionaries[type];
        }
        #endregion
        
        /// <summary>
        /// When overridden in a derived class, 
        /// inserts the current instance into the database as a new record.
        /// </summary>
        public abstract void UpdateDataStore();
        
        // TODO: is there anything to prevent FromDataTable and FromDataRow from moving into MediaManager.SqlClient project?
        #region protected static FromDataTable method
        /// <summary>
        /// Creates a list of instances of T from the supplied <see cref="DataTable"/>.
        /// </summary>
        /// <param name="table">A <see cref="DataTable"/> as returned from the database.</param>
        /// <returns>A list of T instances.</returns>
        /// <typeparam name="T">The <see cref="Type"/> of the rows in the table.</typeparam>
        protected static Collection<T> FromDataTable<T>(DataTable table) where T : EntityBase, new()
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            
            Collection<T> list = new Collection<T>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(FromDataRow<T>(row));
            }
            
            return list;
        }
        #endregion
        
        #region private static FromDataRow<T> method
        /// <summary>
        /// Creates am instance of T from the supplied <see cref="DataRow"/>.
        /// </summary>
        /// <param name="row">A <see cref="DataRow"/> representing an instance of T.</param>
        /// <returns>The instance of T represented by the <see cref="DataRow"/>.</returns>
        /// <typeparam name="T">The <see cref="Type"/> represented by the <see cref="DataRow"/>.</typeparam>
        private static T FromDataRow<T>(DataRow row) where T : EntityBase, new()
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }
            
            // TODO: Use GetColumns instead
            Dictionary<string, PropertyInfo> columns = GetColumns(typeof(T));
            T instance = new T();
            
            foreach (PropertyInfo property in columns.Values) 
            {
                #region get the value from the column with the same name as the property
                string propertyName = property.Name;
                object propertyValue;
                Type propertyType = property.PropertyType;
                try
                {
                    propertyValue = row[propertyName];
                }
                catch (ArgumentException argEx)
                {
                // TODO: could thismessage be more explanatory?
                    string error = "The DataRow from the table '" + row.Table.TableName 
                        + "' does not contain a column called '" + propertyName + "'.";
                    
                    // FIXME: The DataRow from the table 'Attribute' does not contain a column called 'FileId'. ---> System.ArgumentException: Column 'FileId' does not belong to table Attribute.
                    throw new InvalidOperationException(error, argEx);
                }
                #endregion
                
                #region set the value of the property
                try
                {
                    property.SetValue(instance, propertyValue);
                }
                catch (ArgumentException argEx)
                {
                    ThrowExceptionWhenSettingValue<T>(argEx, propertyName, propertyType, propertyValue);
                }
                catch (TargetException ex)
                {
                    ThrowExceptionWhenSettingValue<T>(ex, propertyName, propertyType, propertyValue);
                }
                #endregion
            }
            
            return instance;
        }
        #endregion
        
        #region private static ThrowExceptionWhenSettingValue method
        /// <summary>
        /// Wraps the supplied exception in an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="ex">The exception to wrap.</param>
        /// <param name="propertyName">Name of the property being set when the exception occurred.</param>
        /// <param name="propertyType">Type of the property being set when the exception occurred.</param>
        /// <param name="propertyValue">The value the property was being set to.</param>
        /// <typeparam name="T">The type declaring the property being set.</typeparam>
        private static void ThrowExceptionWhenSettingValue<T>(Exception ex, string propertyName, Type propertyType, object propertyValue)
        {
            string error = "Error whilst setting property '" + propertyName 
                + "', type '" + propertyType.Name 
                + "' declared on class '" + typeof(T).Name 
                + "', value '" + propertyValue.ToString() + "'";
            throw new InvalidOperationException(error, ex);
        }
        #endregion
    }
}
