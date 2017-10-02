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
using System.Linq;

namespace SDE.MediaManager
{
    /// <summary>
    /// TODO: remove FileAttribute now we're using EF?
    /// Bridging entity which links <see cref="Attribute"/>s to <see cref="MediaFile"/>s.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711", Justification = "Different meaning to System.Attribute")]
    public class FileAttribute
    {
        #region instance properties
        /// <summary>
        /// Gets or sets the primary key of the record in the database.
        /// </summary>
        [Category(Category.Identities)]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or sets the identity of the <see cref="MediaFile"/> which has the <see cref="Attribute"/>. 
        /// </summary>
        [Category(Category.Identities)]
        public int FileId { get; set; }
        
        /// <summary>
        /// Gets or sets the identity of the <see cref="Attribute"/> of the <see cref="MediaFile"/>.
        /// </summary>
        [Category(Category.Identities)]
        public int AttributeId { get; set; }
        
        /// <summary>
        /// Gets or sets the identity of the <see cref="PossibleValue"/> of the <see cref="FileAttribute"/>.
        /// </summary>
        [Category(Category.Identities)]
        public int ValueId { get; set; }
        #endregion
        
        // TODO: EF implementation of instance properties from other tables
    }
}
