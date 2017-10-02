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

namespace SDE.Utility
{
    /// <summary>
    /// Static method containing common validation methods.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Validates that the supplied parameter is not null.
        /// </summary>
        /// <param name="parameterName">
        /// Name of the parameter to validate.
        /// This will be included in the thrown exception if the parameter value is null.
        /// </param>
        /// <param name="parameterValue">Value of the parameter to test.</param>
        /// <exception cref="ArgumentNullException">The supplied value is null.</exception>
        public static void NotNull(string parameterName, [ValidatedNotNull] object parameterValue)
        {
            if (parameterValue == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
