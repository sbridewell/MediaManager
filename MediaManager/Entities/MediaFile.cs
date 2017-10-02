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
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
// RESTORE: using SDE.Console.Runners;
using SDE.Utility;

namespace SDE.MediaManager
{
    /// <summary>
    /// Represents a media file, e.g. a video or audio file.
    /// </summary>
    [Table("MediaFile")]
    public class MediaFile
    {
        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFile"/> class.
        /// </summary>
        public MediaFile()
        {
            Metadata = new Collection<FileMetadata>();
            Attributes = new Collection<Attribute>();
        }
        #endregion
        
        #region instance properties
        /// <summary>
        /// Gets the primary key of the file's record in the database.
        /// </summary>
        [Category(Category.Identities)]
        [SuppressMessage("Microsoft.Performance", "CA1811", Justification = "This property is set by Entity Framework")]
        public int Id { get; private set; }
        
        /// <summary>
        /// Gets or sets the name of the file, without the path.
        /// </summary>
        [Category(Category.PropertiesOfThisClass)]
        public string FileName { get; set; }
        
        /// <summary>
        /// Gets or sets the path to the folder which contains the file.
        /// </summary>
        [Category(Category.PropertiesOfThisClass)]
        public string Folder { get; set; }
        
        /// <summary>
        /// Gets the fully qualified filename of the media file.
        /// </summary>
        public string FullFileName
        {
            get { return Path.Combine(Folder, FileName); }
        }
        
        /// <summary>
        /// Gets or sets the size of the file in bytes.
        /// </summary>
        [Category(Category.PropertiesOfThisClass)]
        public long SizeInBytes { get; set; }
        
        /// <summary>
        /// Gets or sets the date and time the file was created.
        /// </summary>
        [Category(Category.PropertiesOfThisClass)]
        public DateTime CreatedTimestamp { get; set; }
        
        /// <summary>
        /// Gets or sets the date and time the file was modified.
        /// </summary>
        [Category(Category.PropertiesOfThisClass)]
        public DateTime ModifiedTimestamp { get; set; }
        #endregion
        
        #region derived properties
        // RESTORE: ? DisplayMember
//        /// <summary>
//        /// Gets a member to be used as the display name for the instance.
//        /// </summary>
//        [Category(Category.Hidden)]
//        public string DisplayMember
//        {
//            get
//            {
//                StringBuilder sb = new StringBuilder();
//                
//                string album = GetMetadata("Album");
//                if (!string.IsNullOrEmpty(album))
//                {
//                    sb.Append(album + " ::: ");
//                }
//                
//                string trackNumber = GetMetadata("TrackNumber");
//                if (!string.IsNullOrEmpty(trackNumber))
//                {
//                    sb.Append("[" + trackNumber + "] ::: ");
//                }
//                
//                string artist = GetMetadata("Artist");
//                if (!string.IsNullOrEmpty(artist))
//                {
//                    sb.Append(artist + " ::: ");
//                }
//                
//                string title = GetMetadata("Title");
//                if (!string.IsNullOrEmpty(title))
//                {
//                    sb.Append(title);
//                }
//                
//                if (sb.Length == 0)
//                {
//                    sb.Append(FileName + " (" + Folder + ")");
//                }
//                
//                return sb.ToString();
//            }
//        }
        #endregion
        
        /// <summary>
        /// Gets this <see cref="MediaFile"/>'s <see cref="FileMetadata"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811", Justification = "This property is set by Entity Framework")]
        public virtual ICollection<FileMetadata> Metadata { get; private set; }
        
        /// <summary>
        /// Gets this <see cref="MediaFile"/>'s <see cref="Attribute"/>s.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811", Justification = "This property is set by Entity Framework")]
        public virtual ICollection<Attribute> Attributes { get; private set; }
        
        #region AreSame method
        /// <summary>
        /// Determines whether the supplied instance is the same as the current instance.
        /// </summary>
        /// <param name="other">The instance to compare the current instance with.</param>
        /// <returns>True if the instances are the same, otherwise false.</returns>
        public bool AreSame(MediaFile other)
        {
            Guard.NotNull("other", other);
            bool areSame = true;
            
            if (TimestampsAreSimilar(CreatedTimestamp, other.CreatedTimestamp) == false)
            {
                areSame = false;
            }
            
            if (TimestampsAreSimilar(ModifiedTimestamp, other.ModifiedTimestamp) == false)
            {
                areSame = false;
            }
            
            if (SizeInBytes != other.SizeInBytes)
            {
                areSame = false;
            }
            
            // TODO: compare metadata
            return areSame;
        }
        #endregion
        
        // RESTORE:
        #region public ExamineForMetadataUsingVlc method
        ///// <summary>
        ///// Uses the VLC player to extract metadata from the supplied <see cref="MediaFile"/>
        ///// and add it to the file's Metadata property.
        ///// </summary>
        //public void ExamineForMetadataUsingVlc()
        //{
        //    using (VlcConsoleRunner runner = new VlcConsoleRunner(FullFileName))
        //    {
        //        runner.Run();
        //        foreach (string name in runner.Metadata.Keys)
        //        {
        //            Metadata.Add(new FileMetadata() { Name = name, Value = runner.Metadata[name] });
        //        }
        //    }
        //}
        #endregion
        
        #region public ExamineForMetadata method
        /// <summary>
        /// Uses the Sharp TagLib library to read the metadata items from the media file
        /// and store them in the Metadata collection.
        /// </summary>
        public void ExamineForMetadata()
        {
            using (TagLib.File file = TagLib.File.Create(FullFileName))
            {
                if (file == null)
                {
                    return;
                }
                
                TagLib.Tag tag = file.Tag;
                if (tag != null)
                {
                    BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
                    PropertyInfo[] properties = typeof(TagLib.Tag).GetProperties(flags);
                    foreach (PropertyInfo property in properties) 
                    {
                        if (property.Name == "TagTypes")
                        {
                            continue;
                        }
                        
                        object propertyValue = property.GetValue(tag);
                        if (propertyValue != null)
                        {
                            string stringValue = propertyValue as string;
                            if (propertyValue.GetType().Name == "String")
                            {
                                if (string.IsNullOrEmpty(stringValue))
                                {
                                    continue;
                                }
                                else
                                {
                                    Metadata.Add(new FileMetadata { Name = property.Name, Value = stringValue });
                                    continue;
                                }
                            }
                            
                            string[] stringArrayValue = propertyValue as string[];
                            if (stringArrayValue != null)
                            {
                                if (stringArrayValue.Length == 0)
                                {
                                    continue;
                                }
                                
                                StringBuilder sb = new StringBuilder();
                                foreach (string element in stringArrayValue)
                                {
                                    sb.AppendLine(element);
                                }
                                
                                continue;
                            }
                            
                            TagLib.Picture[] pictureArrayValue = propertyValue as TagLib.Picture[];
                            if (pictureArrayValue != null)
                            {
                                if (pictureArrayValue.Length == 0)
                                {
                                    continue;
                                }
                                
                                Metadata.Add(new FileMetadata { Name = property.Name, Value = "Picture[] not yet supported" });
                                continue;
                            }
                            
                            string toStringValue = propertyValue.ToString();
                            
                            uint uintValue;
                            bool isUint = uint.TryParse(toStringValue, out uintValue);
                            bool boolValue;
                            bool isBool = bool.TryParse(toStringValue, out boolValue);
                            
                            if (isUint || isBool)
                            {
                                Metadata.Add(new FileMetadata { Name = property.Name, Value = propertyValue.ToString() });
                            }
                            else
                            {
                                Metadata.Add(new FileMetadata { Name = property.Name, Value = "Unknown property type: " + propertyValue.GetType().Name });
                            }
                        }
                    }
                }
            }
        }
        #endregion
        
        #region private static TimestampsAreSimilar method
        /// <summary>
        /// Determines whether the supplied timestamps are the same, without considering
        /// factors such as whether either of them are local daylight savings time.
        /// This works by simply comparing the year, month, day, hour, minute and second
        /// components of each DateTime.
        /// </summary>
        /// <param name="first">The first DateTime.</param>
        /// <param name="second">The second DateTime.</param>
        /// <returns>True if the instances are the same, otherwise false.</returns>
        private static bool TimestampsAreSimilar(DateTime first, DateTime second)
        {
            bool similar = first.Year == second.Year
                && first.Month == second.Month
                && first.Day == second.Day
                && first.Hour == second.Hour
                && first.Minute == second.Minute
                && first.Second == second.Second;
            return similar;
        }
        #endregion
        
        #region private instance methods
        // RESTORE: ? GetMetadata method
//        /// <summary>
//        /// Gets the metadata item with the supplied name from the current <see cref="MediaFile"/>
//        /// </summary>
//        /// <param name="name">Name of the metadata item to get.</param>
//        /// <returns>The value of the supplied metadata if present, otherwise string.Empty.</returns>
//        private string GetMetadata(string name)
//        {
//            IEnumerable<FileMetadata> datas = Metadata.Where(x => x.Name == name);
//            if (datas.Any())
//            {
//                // We've found a Metadata with the supplied name, and there can be only one per name so return it
//                return datas.ElementAt(0).Value;
//            }
//            else
//            {
//                // This file doesn't have the supplied metadata
//                return string.Empty;
//            }
//        }
        #endregion
    }
}
