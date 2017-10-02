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

namespace SDE.MediaManager
{
    /// <summary>
    /// Constants to use in conjunction with the CategoryAttribute to control how
    /// properties are grouped in a PropertyGroup.
    /// </summary>
    public static class Category
    {
        /// <summary>
        /// Used to decorate properties which are either the identity of the current class 
        /// (primary key) or the identity of another class (foreign key).
        /// </summary>
        public const string Identities = "Identities";
        
        /// <summary>
        /// Used to decorate properties which are attributes of the current class but
        /// are not identities.
        /// </summary>
        public const string PropertiesOfThisClass = "Properties of this class";
        
        /// <summary>
        /// Used to decorate properties which are instances of other classes.
        /// </summary>
        public const string PropertiesFromOtherTables = "Properties from other tables";
        
        /// <summary>
        /// Used to decorate properties whose type implements the ICommand interface.
        /// </summary>
        public const string Commands = "Commands";
        
        /// <summary>
        /// Used to decorate properties which are hidden TODO: is this used anywhere? .
        /// </summary>
        public const string Hidden = "Hidden";
        
        /// <summary>
        /// Used to decorate properties of view models which do not raise PropertyChanged events.
        /// </summary>
        public const string Properties = "Properties";
        
        /// <summary>
        /// Used to decorate properties of view models which raise PropertyChanged events.
        /// </summary>
        public const string PropertiesWhichRaisePropertyChangedEvents = "Properties which raise PropertyChanged events";
    }
}
