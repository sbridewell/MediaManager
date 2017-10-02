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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SDE.MediaManager
{
    /// <summary>
    /// An attribute of a <see cref="MediaFile"/> which is set from file metadata.
    /// </summary>
    [Table("FileMetadata")]
    public class FileMetadata
    {
        #region instance properties
        /// <summary>
        /// Gets the primary key of the <see cref="FileMetadata"/> in the database.
        /// </summary>
        [Category(Category.Identities)]
        [SuppressMessage("Microsoft.Performance", "CA1811", Justification = "This property is set by Entity Framework")]
        public int Id { get; private set; }
        
        /// <summary>
        /// Gets or sets the name of the metadata field.
        /// </summary>
        [Category(Category.PropertiesOfThisClass)]
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the value of the metadata field.
        /// </summary>
        [Category(Category.PropertiesOfThisClass)]
        public string Value { get; set; }
        #endregion
        
        /// <summary>
        /// Gets or sets the <see cref="MediaFile"/> to which this metadata belongs.
        /// </summary>
        public MediaFile MediaFile { get; set; }
        
        // TODO: EF implementation of properties from other tables
    }
}
