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
using System.ComponentModel.DataAnnotations.Schema;

namespace SDE.MediaManager
{
    /// <summary>
    /// A setting of the application.
    /// </summary>
    [Table("Settings")]
    public class Settings
    {
        // TODO: one record per setting rather than one column per setting?
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
        }
        
        /// <summary>
        /// Gets the single instance of the <see cref="Settings"/> class.
        /// </summary>
        /// <returns>The application settings.</returns>
        public static Settings GetInstance
        {
            get
            {
                using (var context = new MediaManagerContext())
                {
                    return context.Settings.Find(1);
                }
            }
        }
        
        /// <summary>
        /// Gets the primary key of this table in the database.
        /// </summary>
        public int Id { get; private set; }
        
        /// <summary>
        /// Gets or sets the root folder controlling the portion of the filesystem that this 
        /// media manager manages.
        /// </summary>
        public string RootFolder { get; set; }
        
        /// <summary>
        /// Gets or sets the subfolder of the root folder which contains podcasts.
        /// </summary>
        public string PodcastSubfolder { get; set; }
    }
}
