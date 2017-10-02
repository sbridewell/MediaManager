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
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace SDE.MediaManager
{
    /// <summary>
    /// Manages local caching of images from the web.
    /// </summary>
    public static class WebHelper
    {
        #region static GetLocalFileNameFromUrl method
        /// <summary>
        /// Creates a safe local filename from a URL by replacing any invalid characters
        /// in the URL with an underscore.
        /// </summary>
        /// <param name="url">The URL to create the filename from.</param>
        /// <returns>A filename created from the URL.</returns>
        [SuppressMessage("Microsoft.Design", "CA1054", Justification = "System.Uri not supported by Entity Framework")]
        [SuppressMessage("Microsoft.Design", "CA1055", Justification = "System.Uri not supported by Entity Framework")]
        public static string GetLocalFileNameFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }
            
            var invalidChars = Path.GetInvalidFileNameChars();
            string localName = url;
            foreach (char invalid in invalidChars)
            {
                localName = localName.Replace(invalid, '_');
            }
            
            return localName;
        }
        #endregion
    }
}
