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

namespace SDE.MediaManager
{
    /// <summary>
    /// TODO: (later) need to implement the Attribute class some time
    /// An attribute of a <see cref="MediaFile"/>.
    /// </summary>
    [Table("Attribute")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711", Justification = "Different meaning to System.Attribute")]
    public class Attribute
    {
        #region instance properties
        /// <summary>
        /// Gets the primary key of the attribute's record in the database.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811", Justification = "This property is set by Entity Framework")]
        public int Id { get; private set; }
        
        /// <summary>
        /// Gets or sets the name of the attribute.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="DataType"/> of the <see cref="Attribute"/>.
        /// TODO: does Attribute need a DataType property? Doesn't seem to be used.
        /// </summary>
        public DataType DataType { get; set; }
        #endregion
    }
}
