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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SDE.MediaManager
{
    /// <summary>
    /// A single episode from a <see cref="PodcastChannel"/>.
    /// </summary>
    [Table("PodcastEpisode")]
    public class PodcastEpisode
    {
        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastEpisode"/> class.
        /// </summary>
        public PodcastEpisode()
        {
            ParseErrors = new Collection<string>();
            PublishDate = null;
        }
        #endregion
        
        #region Entity Framework properties
        /// <summary>
        /// Gets the unique identifier within the database for this episode.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811", Justification = "This property is set by Entity Framework")]
        [XmlIgnore]
        public int Id { get; private set; }
        
        /// <summary>
        /// Gets or sets the title of this episode.
        /// </summary>
        [XmlAttribute]
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets the URL of a web page with more information about this episode.
        /// </summary>
        [XmlAttribute]
        public string Link { get; set; }
        
        /// <summary>
        /// Gets or sets the description of this episode.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets the author of this episode.
        /// </summary>
        [XmlAttribute]
        public string Author { get; set; }
        
        /// <summary>
        /// Gets or sets the URL of media file for this episode.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1056", Justification = "System.Uri not supported by Entity Framework")]
        [XmlAttribute]
        public string EnclosureUrl { get; set; }
        
        /// <summary>
        /// Gets or sets the size in bytes of the media file for this episode.
        /// </summary>
        [XmlAttribute]
        public long EnclosureSize { get; set; }
        
        /// <summary>
        /// Gets or sets the MIME content type of the media file for this episode.
        /// </summary>
        [XmlAttribute]
        public string EnclosureContentType { get; set; }
        
        /// <summary>
        /// Gets or sets a unique identifier for this episode within its <see cref="PodcastChannel"/>.
        /// </summary>
        [XmlAttribute]
        public string Guid { get; set; }
        
        /// <summary>
        /// Gets or sets the date and time that this episode was published.
        /// </summary>
        public DateTime? PublishDate { get; set; }
        
        /// <summary>
        /// Gets or sets the subtitle of this episode.
        /// </summary>
        public string Subtitle { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether // TODO: what is the itunes:explicit property?.
        /// </summary>
        [XmlAttribute]
        public bool ITunesExplicit { get; set; }
        
        /// <summary>
        /// Gets or sets the length of the episode.
        /// </summary>
        /// <remarks>
        /// This property isn't serialized, instead the DurationString property is serialized in its place.
        /// </remarks>
        [XmlIgnore]
        public TimeSpan Duration { get; set; }
        
        /// <summary>
        /// XmlSerializer does not support <see cref="TimeSpan"/>, so we serialize this property instead of the Duration property.
        /// </summary>
        [Browsable(false)]
        [XmlElement(DataType="duration", ElementName="Duration")]
        [NotMapped]
        public string DurationString
        {
            get 
            { 
                return XmlConvert.ToString(Duration); 
            }
            set 
            { 
                Duration = string.IsNullOrEmpty(value) ? TimeSpan.Zero : XmlConvert.ToTimeSpan(value); 
            }
        }
        
        /// <summary>
        /// Gets or sets the image for this episode.
        /// </summary>
        public CachedImage Image { get; set; }
        
        /// <summary>
        /// Gets or sets the primary key of the channel to which this episode belongs.
        /// Required for lazy loading of the episodes of a channel.
        /// </summary>
        [ForeignKey("PodcastChannel")]
        [XmlIgnore]
        public int ChannelId { get; set; }
        
        /// <summary>
        /// Gets or sets the channel to which this episode belongs.
        /// Required for lazy loading of the episodes of a channel.
        /// </summary>
        [XmlIgnore]
        public PodcastChannel PodcastChannel { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this episode has been downloaded.
        /// </summary>
        [XmlAttribute]
        public bool Downloaded { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the user has chosen to ignore (not download) this episode.
        /// </summary>
        [XmlAttribute]
        public bool Ignored { get; set; }
        #endregion
        
        #region non Entity Framework properties
        /// <summary>
        /// Gets any errors encountered when parsing this channel's RSS feed.
        /// </summary>
        [NotMapped]
        [XmlIgnore]
        public Collection<string> ParseErrors { get; private set; }

        /// <summary>
        /// Gets or sets any content from the RSS file which hasn't been consumed by the parser.
        /// </summary>
        [NotMapped]
        [XmlIgnore]
        public string UnparsedRssData { get; set; }
        #endregion
        
        #region override ToString method
        /// <summary>
        /// Gets a string representation of the current instance.
        /// </summary>
        /// <returns>The episode's title.</returns>
        public override string ToString()
        {
            return Title + " : " + Subtitle;
        }
        #endregion
    }
}
