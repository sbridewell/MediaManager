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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace SDE.MediaManager
{
    /// <summary>
    /// Represents the file type of a <see cref="MediaFile"/> as defined by its filename extension.
    /// </summary>
    [Table("FileType")]
    public class FileType
    {
        #region instance properties
        /// <summary>
        /// Gets or sets the filename extension of the file type, without the preceding full stop. 
        /// </summary>
        [Key]
        public string Extension { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the file type.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets a description of the file type.
        /// </summary>
        public string Description { get; set; }
        #endregion
    }
}
