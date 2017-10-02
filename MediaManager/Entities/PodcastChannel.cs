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
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SDE.MediaManager
{
    /// <summary>
    /// A channel which publishes podcasts.
    /// This class is initially populated from the content of a RSS XML file which contains
    /// the attributes of the podcast channel and the available episodes.
    /// </summary>
    [Table("PodcastChannel")]
    public class PodcastChannel
    {
        /// <summary>
        /// Path to the local folder where this podcast channel's files are kept.
        /// </summary>
        private string _fileSystemLocation;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastChannel"/> class.
        /// </summary>
        public PodcastChannel()
        {
            Episodes = new Collection<PodcastEpisode>();
            ParseErrors = new Collection<string>();
            LastBuildDate = null;
            PublishedDate = null;
        }
        
        /// <summary>
        /// Gets the name of the subfolder within the Podcasts folder where backups of podcast information are stored.
        /// </summary>
        public static string BackupFolder 
        { 
            get { return "Backups"; } 
        }
        
        #region instance properties
        /// <summary>
        /// Gets the unique identifier for this instance.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811", Justification = "This property is set by Entity Framework")]
        [XmlIgnore]
        public int Id { get; private set; }

        /// <summary>
        /// Gets or sets the title of the podcast.
        /// </summary>
        [XmlAttribute]
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets a hyperlink associated with the channel.
        /// This may be a web page or it may be the URL of the RSS feed.
        /// </summary>
        [XmlAttribute]
        public string Link { get; set; }
        
        /// <summary>
        /// Gets or sets the URL of the channel's RSS feed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1056", Justification = "System.Uri not supported by Entity Framework")]
        [XmlAttribute]
        public string RssUrl { get; set; }
        
        /// <summary>
        /// Gets or sets a description of this podcast.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets the language of this podcast.
        /// </summary>
        [XmlAttribute]
        public string Language { get; set; }
        
        /// <summary>
        /// Gets or sets the copyright statement for this podcast.
        /// </summary>
        [XmlAttribute]
        public string Copyright { get; set; }
        
        /// <summary>
        /// Gets or sets the date and time that the RSS feed was last updated.
        /// </summary>
        public DateTime? LastBuildDate { get; set; }
        
        /// <summary>
        /// Gets or sets the date and time that the RSS feed was published.
        /// </summary>
        public DateTime? PublishedDate { get; set; }
        
        /// <summary>
        /// Gets or sets the categories of this podcast.
        /// </summary>
        [XmlAttribute]
        public string Categories { get; set; }
        
        /// <summary>
        /// Gets or sets the author of this podcast.
        /// </summary>
        [XmlAttribute]
        public string Author { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the owner of this podcast.
        /// </summary>
        [XmlAttribute]
        public string OwnerName { get; set; }
        
        /// <summary>
        /// Gets or sets the email address of the owner of this podcast.
        /// </summary>
        [XmlAttribute]
        public string OwnerEmail { get; set; }
        
        /// <summary>
        /// Gets or sets the image for this channel.
        /// </summary>
        public CachedImage Image { get; set; }
        
        /// <summary>
        /// Gets or sets the title of the image for this channel.
        /// </summary>
        [XmlAttribute]
        public string ImageTitle { get; set; }
        
        /// <summary>
        /// Gets or sets a link for the image for this channel.
        /// </summary>
        [XmlAttribute]
        public string ImageLink { get; set; }
        
        /// <summary>
        /// Gets or sets the keywords for this channel.
        /// </summary>
        [XmlAttribute]
        public string Keywords { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether // TODO: what does this property mean?.
        /// </summary>
        [XmlAttribute]
        public bool ITunesExplicit { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the tool used to generate the RSS feed.
        /// </summary>
        [XmlAttribute]
        public string Generator { get; set; }
        
        /// <summary>
        /// Gets or sets the location of more information about the channel.
        /// </summary>
        [XmlAttribute]
        public string Documents { get; set; }
        
        /// <summary>
        /// Gets or sets the channel's managing editor.
        /// </summary>
        [XmlAttribute]
        public string ManagingEditor { get; set; }
        
        /// <summary>
        /// Gets or sets the channel's subtitle.
        /// </summary>
        [XmlAttribute]
        public string Subtitle { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the user is subscribed to this podcast.
        /// </summary>
        [XmlAttribute]
        public bool Subscribed { get; set; }
        
        /// <summary>
        /// Gets the episodes of this podcast.
        /// </summary>
        /// <remarks>
        /// This property needs to be virtual in order to support lazy loading of the episodes.
        /// </remarks>
        public virtual Collection<PodcastEpisode> Episodes { get; private set; }
        
        #region properties not mapped to the database
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

        /// <summary>
        /// Gets or sets the original XML of the RSS feed.
        /// </summary>
        [NotMapped]
        [XmlIgnore]
        public string PodcastXml { get; set; }
        
        /// <summary>
        /// Gets or sets a neatly indented copy of the RSS data.
        /// </summary>
        [NotMapped]
        [XmlIgnore]
        public string PrettyRssData { get; set; }
        
        #region FileSystemLocation property
        /// <summary>
        /// Gets the path to the local folder where files for this podcast channel are kept.
        /// </summary>
        /// <remarks>
        /// This returns a folder unique to the current channel, within the podcast folder.
        /// The subfolder name is generated from the URL of the channel's RSS feed, replacing
        /// any characters which are not valid in a file system path with an underscore.
        /// This should make each channel's FileSystemLocation unique, although it's possible
        /// for two channels to end up with the same FileSystemLocation, for example if their
        /// RSS feed URLs are example.com?podcast.xml and example.com/podcast.xml.
        /// </remarks>
        [NotMapped]
        [XmlIgnore]
        public string FileSystemLocation
        {
            get
            {
                if (string.IsNullOrEmpty(RssUrl))
                {
                    // Either the channel hasn't been initialised yet,
                    // or its RssUrl property isn't set, in which case we can't download
                    // anything from it anyway.
                    return null;
                }
                
                if (_fileSystemLocation == null)
                {
                    Settings settings = Settings.GetInstance;
                    string podcastIdentifier = WebHelper.GetLocalFileNameFromUrl(RssUrl);
                    _fileSystemLocation = Path.Combine(settings.RootFolder, settings.PodcastSubfolder, podcastIdentifier);
                    if (!Directory.Exists(_fileSystemLocation))
                    {
                        Directory.CreateDirectory(_fileSystemLocation);
                    }
                }
                
                return _fileSystemLocation;
            }
        }
        #endregion
        
        #endregion
        
        #endregion
        
        #region public static FromPodcastUrl method
        /// <summary>
        /// Creates a <see cref="PodcastChannel"/> instance from the RSS data at the supplied URL.
        /// </summary>
        /// <param name="url">URL of the channel's RSS feed.</param>
        /// <param name="showUnparsedContent">
        /// True to record any content from the RSS feed which isn't processed by the parser.
        /// This is useful for developers to identify where feeds contain content which the parser
        /// isn't looking for, and therefore information which the user can't see about the channel.
        /// </param>
        /// <returns>A new <see cref="PodcastChannel"/> instance.</returns>
        [SuppressMessage("Microsoft.Design", "CA1054", Justification = "System.Uri not supported by Entity Framework")]
        public static async Task<PodcastChannel> FromPodcastUrl(string url, bool showUnparsedContent)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream s = response.GetResponseStream();
            var reader = new StreamReader(s);
            string xml = await reader.ReadToEndAsync();
            reader.Close();
            
            var parser = new PodcastChannelParser(xml);
            parser.RemoveParsedContent = showUnparsedContent;
            PodcastChannel channel = await Task.Run(() => parser.Parse());
            channel.PodcastXml = xml;
            return channel;
        }
        #endregion
        
        #region public async UpdateChannel method
        /// <summary>
        /// Updates the channel with any new episodes which have been published.
        /// </summary>
        /// <returns>Nothing - it's a <see cref="Task"/>.</returns>
        public async Task UpdateChannel()
        {
            PodcastChannel latestChannel = await FromPodcastUrl(RssUrl, false);
            var episodeGuids = new Collection<string>();
            
            // Get the guids of the episodes we already know about
            foreach (PodcastEpisode episode in Episodes)
            {
                episodeGuids.Add(episode.Guid);
            }
            
            // Add any published episodes that we don't already know about
            foreach (PodcastEpisode episode in latestChannel.Episodes)
            {
                if (!episodeGuids.Contains(episode.Guid))
                {
                    Episodes.Add(episode);
                }
            }
            
            // FIXME: user needs to switch to another channel and back to see the new episodes
            // TODO: consider updating other channel metadata if different to local copy
        }
        #endregion
        
        #region public SaveAsXml method
        /// <summary>
        /// Saves everything we know about this PodcastChannel and its episodes to a XML file.
        /// Useful if you need to rebuild the database and don't want to lose information about older episodes which
        /// are no longer in the podcast channel's RSS feed.
        /// </summary>
        public void SaveAsXml()
        {
            Settings s = Settings.GetInstance;
            string backupFolder = Path.Combine(s.RootFolder, s.PodcastSubfolder, BackupFolder);
            if (!Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
            }
            
            string path = Path.Combine(backupFolder, WebHelper.GetLocalFileNameFromUrl(RssUrl));
            var childTypes = new List<Type>();
            if (Episodes.Count > 0)
            {
                childTypes.Add(Episodes[0].GetType());
            }
            
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                // Parameters passed to the XmlSerializer constructor:
                //  1) this.GetType() - the type being serialized. This needs to be the runtime type of PodcastChannel because Entity Framework generates a subclass which is what is actually being serialized.
                //  2) null - XmlAttributeOverrides
                //  3) childTypes.ToArray() - array of runtime types of otherwise unexpected objects contained within the class being serialized. In this case, Entity Framework also generates a subclass of PodcastEpisode, and it's that which is serialized.
                //  4) new XmlRootAttribute("PodcastChannel") - controls the name of the root element of the serialized file, so that it's not set to the name of Entity Framework's derived type.
                //  5) null - default namespace.
                
                // FIXED: Duration property of episodes is serialized as an empty element
                var serializer = new XmlSerializer(this.GetType(), null, childTypes.ToArray(), new XmlRootAttribute("PodcastChannel"), null);
                serializer.Serialize(writer, this);
            }
        }
        #endregion
        
        #region override ToString methods
        /// <summary>
        /// Gets a string representation of the current instance.
        /// </summary>
        /// <returns>The title of the podcast channel.</returns>
        public override string ToString()
        {
            return Title;
        }
        #endregion
    }
}
