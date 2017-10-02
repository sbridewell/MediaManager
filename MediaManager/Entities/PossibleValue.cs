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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SDE.MediaManager
{
    /// <summary>
    /// A possible value of an <see cref="Attribute"/>.
    /// </summary>
    [Table("PossibleValue")]
    public class PossibleValue
    {
        #region instance properties
        /// <summary>
        /// Gets or sets the primary key of the record in the database.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="Attribute"/> which can have this value.
        /// TODO: should this be AttributeId? Not used anywhere.
        /// </summary>
        public Attribute Attribute { get; set; }
        
        /// <summary>
        /// Gets or sets the value of the <see cref="Attribute"/>.
        /// </summary>
        public string Value { get; set; }
        #endregion
        
        // TODO: EF implementation of FileAttribute property?
        #region override ToString method
        /// <summary>
        /// Gets a string representation of the current instance.
        /// </summary>
        /// <returns>A string representation of the current instance.</returns>
        public override string ToString()
        {
            return Value;
        }
        #endregion
    }
}
