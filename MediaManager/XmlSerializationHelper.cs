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
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace SDE.MediaManager
{
    /// <summary>
    /// Helper class for serializing and de-serializing objects to and from XML.
    /// </summary>
    /// <typeparam name="TType">The <see cref="System.Type"/> of the object to serialize or deserialize.</typeparam>
    public class XmlSerializationHelper<TType>
    {
        #region FromXml
        /// <summary>
        /// Deserializes an XML string to an instance of <see paramref="TType"/>.
        /// </summary>
        /// <param name="xml">
        /// An XML string representing an instance of the supplied System.Type.
        /// </param>
        /// <returns>
        /// The <see paramref="TType"/> instance represented by the supplied XML string.
        /// </returns>
        /// <exception cref="InvalidDataException">
        /// The XML string could not be deserialized to the given type. See the
        /// InnerException for more information.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The supplied System.Type is null.
        /// </exception>
        public TType FromXml(string xml)
        {
            try
            {
                var xs = new XmlSerializer(typeof(TType));
                using (TextReader tr = new StringReader(xml))
                {
                    return (TType)xs.Deserialize(tr);
                }
            }
            catch (InvalidOperationException ex)
            {
                #region handle the exception
                string message = "Unable to "
                    + "deserialize an XML string to an instance of the type "
                    + typeof(TType).ToString()
                    + ". "
                    + Environment.NewLine
                    + Environment.NewLine
                    + "This could mean the XML string is corrupt, or it could mean "
                    + "you are trying to deserialize an XML string which is a "
                    + "representation of a completely different System.Type."
                    + Environment.NewLine
                    + Environment.NewLine
                    + "The XML string is: "
                    + xml;
                throw new InvalidDataException(message, ex);
                #endregion
            }
        }
        #endregion
    }
}
