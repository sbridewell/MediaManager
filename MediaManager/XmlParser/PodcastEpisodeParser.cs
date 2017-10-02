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
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace SDE.MediaManager
{
    /// <summary>
    /// Factory class to create an instance of <see cref="PodcastEpisode"/> from an 
    /// &lt;tem&gt; element of a <see cref="PodcastChannel"/> RSS feed.
    /// </summary>
    public class PodcastEpisodeParser : XmlParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastEpisodeParser"/> class.
        /// </summary>
        /// <param name="rssData">The &lt;item&gt; node from the <see cref="PodcastChannel"/>'s RSS feed.</param>
        /// <param name="manager">Used to resolve namespaces.</param>
        public PodcastEpisodeParser(IXPathNavigable rssData, XmlNamespaceManager manager) : base(rssData, manager)
        {
        }
        
        /// <summary>
        /// Parses the supplied RSS data.
        /// </summary>
        /// <returns>A <see cref="PodcastEpisode"/> instance represented by the RSS data.</returns>
        public PodcastEpisode Parse()
        {
            PodcastEpisode episode = new PodcastEpisode();
            
            episode.Title = GetStringFromChildElement("title", NodeToParse);
            episode.Link = GetStringFromChildElement("link", NodeToParse);
            episode.Description = GetStringFromChildElement("description", NodeToParse);
            GetAuthor(episode);
            episode.EnclosureUrl = GetStringFromChildElementAttribute("enclosure", "url", NodeToParse);
            episode.EnclosureSize = GetLongFromChildElementAttribute("enclosure", "length", NodeToParse);
            episode.EnclosureContentType = GetStringFromChildElementAttribute("enclosure", "type", NodeToParse);
            episode.Guid = GetStringFromChildElement("guid", NodeToParse);
            episode.PublishDate = GetDateTimeFromChildElement("pubDate", NodeToParse);
            episode.Subtitle = GetStringFromChildElement("itunes:subtitle", NodeToParse);
            episode.ITunesExplicit = GetBooleanFromChildElement("itunes:explicit", NodeToParse);
            episode.Duration = GetTimeSpanFromChildElement("itunes:duration", NodeToParse);
            GetImage(episode);
            SaveParseErrors(episode);
            
            if (RemoveParsedContent)
            {
                UnparsedContent.Seek(0, System.IO.SeekOrigin.Begin);
                episode.UnparsedRssData = new StreamReader(UnparsedContent).ReadToEnd();
            }
            
            return episode;
        }
        
        /// <summary>
        /// Gets the author.
        /// </summary>
        /// <param name="episode">The episode being parsed.</param>
        private void GetAuthor(PodcastEpisode episode)
        {
            string author = GetStringFromChildElement("author", NodeToParse);
            string iTunesAuthor = GetStringFromChildElement("itunes:author", NodeToParse);
            
            if (!string.IsNullOrEmpty(author))
            {
                episode.Author = author;
            }
            else if (!string.IsNullOrEmpty(iTunesAuthor))
            {
                episode.Author = iTunesAuthor;
            }
            else
            {
                episode.Author = string.Empty;
                ParseErrors.Add("No author found");
            }
        }
        
        /// <summary>
        /// Gets the image for the episode.
        /// </summary>
        /// <param name="episode">The episode being parsed.</param>
        private void GetImage(PodcastEpisode episode)
        {
            string imageUrl = GetStringFromChildElementAttribute("itunes:image", "href", NodeToParse);
            episode.Image = new CachedImage();
            if (string.IsNullOrEmpty(imageUrl))
            {
                ParseErrors.Add("No image found");
            }
            else
            {
                episode.Image.RemoteUrl = imageUrl;
            }
        }
        
        /// <summary>
        /// Copies any parse errors to the channel.
        /// </summary>
        /// <param name="episode">The episode being created by this parser.</param>
        private void SaveParseErrors(PodcastEpisode episode)
        {
            foreach (string error in ParseErrors)
            {
                episode.ParseErrors.Add(error);
            }
        }
    }
}
