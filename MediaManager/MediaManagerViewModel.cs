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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

using System.Threading.Tasks;
// REMOVE: using SDE.MediaManager.SqlClient;

namespace SDE.MediaManager
{
    /// <summary>
    /// View model for the Media Manager application.
    /// TODO: need to remove the MediaManagerViewModel class
    /// </summary>
    [Obsolete("View models should be in the MediaManager.ViewModels project")]
    public class MediaManagerViewModelOld : INotifyPropertyChanged
    {
        #region declarations
        /// <summary>
        /// Connection to the database where the managed media data is stored.
        /// </summary>
        private static SqlConnection _connection;
        #endregion
        
        /// <summary>
        /// The event that is raised when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        // TODO: these properties probably belong in the DB context class
        #region properties
//        /// <summary>
//        /// Gets all the <see cref="DataType"/> instances from the database.
//        /// </summary>
//        public static Collection<DataType> AllDataTypes
//        {
//            get
//            {
//                ThrowExceptionIfNotInitialised();
//                return DataType.All;
//            }
//        }
//        
//        /// <summary>
//        /// Gets all the <see cref="Attribute"/> instances from the database.
//        /// </summary>
//        public static Collection<Attribute> AllAttributes
//        {
//            get
//            {
//                ThrowExceptionIfNotInitialised();
//                return Attribute.All;
//            }
//        }
//        
//        /// <summary>
//        /// Gets all the <see cref="FileAttribute"/> instances from the database.
//        /// </summary>
//        public static Collection<FileAttribute> AllFileAttributes
//        {
//            get
//            {
//                ThrowExceptionIfNotInitialised();
//                return FileAttribute.All;
//            }
//        }
//        
//        /// <summary>
//        /// Gets all the <see cref="MediaFile"/> instances from the database.
//        /// </summary>
//        public static Collection<MediaFile> AllMediaFiles
//        {
//            get
//            {
//                ThrowExceptionIfNotInitialised();
//                return MediaFile.All;
//            }
//        }
//        
//        /// <summary>
//        /// Gets all the <see cref="PossibleValue"/> instances from the database.
//        /// </summary>
//        public static Collection<PossibleValue> AllPossibleValues
//        {
//            get
//            {
//                ThrowExceptionIfNotInitialised();
//                return PossibleValue.All;
//            }
//        }
        
        /// <summary>
        /// Gets the file system file which is currently being examined.
        /// Used during population of the database.
        /// </summary>
        public static string ExaminingFile 
        {
            get
            {
                return MediaFile.ExaminingFile;
            }
        }
        #endregion
        
        #region Initialise method
        /// <summary>
        /// Initialises the view model.
        /// Properties will throw an exception if called when this method has not been called.
        /// </summary>
        /// <param name="databaseServer">Server and instance name where the media database is hosted.</param>
        /// <param name="databaseName">Name of the media database.</param>
        public static void Initialise(string databaseServer, string databaseName)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = databaseServer;
            builder.InitialCatalog = databaseName;
            builder.IntegratedSecurity = true;
            _connection = new SqlConnection(builder.ConnectionString);
            _connection.Open();
        }
        #endregion
        
        #region EmptyAndRepopulateDatabase method
        /// <summary>
        /// Empties all the tables in the database and repopulates them.
        /// </summary>
        /// <param name="mediaPath">Path to the folder containing the media files.</param>
        /// <returns>Nothing - it's a <see cref="Task"/></returns>
        public static async Task EmptyAndRepopulateDatabase(string mediaPath)
        {
            // empty the tables
            // TODO: EF implementation to empty all the tables?
//            DeleteTruncateAllTablesSP sp = new DeleteTruncateAllTablesSP();
//            try
//            {
//                sp.Command.ExecuteNonQuery();
//            }
//            finally
//            {
//                sp.Dispose();
//            }
            
            // TODO: move this to a seeding method
//            // populate the DataType table
//            new DataType { Name = "System.Int32" }.UpdateDataStore();
//            new DataType { Name = "System.String" }.UpdateDataStore();
//            
//            // populate the FileType table
//            new FileType { Extension = "mp3", Name = "MPEG layer 3 audio", Description = "MP3" }.UpdateDataStore();
//            new FileType { Extension = "wma", Name = "WMA", Description = "WMA" }.UpdateDataStore();
//            new FileType { Extension = "m4a", Name = "M4A", Description = "M4A" }.UpdateDataStore();
//            new FileType { Extension = "mpg", Name = "MPG", Description = "MPG" }.UpdateDataStore();
//            new FileType { Extension = "mpeg", Name = "MPEG", Description = "MPEG" }.UpdateDataStore();
//            new FileType { Extension = "avi", Name = "AVI", Description = "AVI" }.UpdateDataStore();
//            new FileType { Extension = "mov", Name = "MOV", Description = "Quicktime" }.UpdateDataStore();
//            new FileType { Extension = "flv", Name = "FLV", Description = "Flash video" }.UpdateDataStore();
//            new FileType { Extension = "mp4", Name = "MP4", Description = "MP4" }.UpdateDataStore();
//            
//            // populate the Attribute table
//            new Attribute { Name = "Dummy attribute name 1", DataTypeId = AllDataTypes[0].Id }.UpdateDataStore();
//            new Attribute { Name = "Dummy attribute name 2", DataTypeId = AllDataTypes[0].Id }.UpdateDataStore();
//            
//            // populate the PossibleValue table
//            int attribute1Id = AllAttributes[0].Id;
//            new PossibleValue { AttributeId = attribute1Id, Value = "Dummy attribute 1 value 1" }.UpdateDataStore();
//            new PossibleValue { AttributeId = attribute1Id, Value = "Dummy attribute 1 value 2" }.UpdateDataStore();
//            int attribute2Id = AllAttributes[1].Id;
//            new PossibleValue { AttributeId = attribute2Id, Value = "Dummy attribute 2 value 1" }.UpdateDataStore();
//            new PossibleValue { AttributeId = attribute2Id, Value = "Dummy attribute 2 value 2" }.UpdateDataStore();
            
            // TODO: new way of getting media files from the filesystem into the database 
//            // get the file types from the database
//            Collection<MediaFile> mediaFiles = await MediaFile.ScanFileSystem(mediaPath);
//            
//            // get the media files from the filesystem
//            foreach (MediaFile mediaFile in mediaFiles)
//            {
//                mediaFile.UpdateDataStore();
//            }
            
            // populate the FileAttribute table
            // REMOVE:
//            int fileId = AllMediaFiles[0].Id;
//            int value1Id = AllPossibleValues[0].Id;
//            int value2Id = AllPossibleValues[1].Id;
//            int value3Id = AllPossibleValues[2].Id;
//            int value4Id = AllPossibleValues[3].Id;
//            new FileAttribute { FileId = fileId, AttributeId = attribute1Id, ValueId = value1Id }.UpdateDataStore();
//            new FileAttribute { FileId = fileId, AttributeId = attribute1Id, ValueId = value2Id }.UpdateDataStore();
//            new FileAttribute { FileId = fileId, AttributeId = attribute2Id, ValueId = value3Id }.UpdateDataStore();
//            new FileAttribute { FileId = fileId, AttributeId = attribute2Id, ValueId = value4Id }.UpdateDataStore();
        }
        #endregion
        
        #region private methods
        
        #region private static ThrowExceptionIfNotInitialised method
        /// <summary>
        /// Throws an exception if the Initialise method has not yet been called.
        /// </summary>
        /// <exception cref="InvalidOperationException">The Initialize method has not been called yet.</exception>
        private static void ThrowExceptionIfNotInitialised()
        {
            if (_connection == null)
            {
                const string Error = "The Initialise method of the media manager view model class has not been called";
                throw new InvalidOperationException(Error);
            }
        }
        #endregion
        
        #region private void NotifyPropertyChanged method
        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property which has changed.</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #endregion
    }
}
