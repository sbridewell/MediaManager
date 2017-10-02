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
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace SDE.MediaManager
{
    /// <summary>
    /// Factory class to create an instance of <see cref="PodcastChannel"/> from its RSS feed.
    /// </summary>
    public class PodcastChannelParser : XmlParser
    {
        #region declarations
        /// <summary>
        /// The channel being parsed.
        /// </summary>
        private PodcastChannel _channel;
        
        /// <summary>
        /// Temporary collection to hold keywords which have been found in the RSS feed.
        /// </summary>
        private Collection<string> _keywords;
        #endregion
        
        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PodcastChannelParser"/> class.
        /// </summary>
        /// <param name="rssData">The RSS XML document to parse.</param>
        public PodcastChannelParser(string rssData) : base(rssData)
        {
            _channel = new PodcastChannel();

            using (var stringWriter = new StringWriter(new StringBuilder(), CultureInfo.InvariantCulture))
            {
                var xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
                XmlDocument xDoc = new XmlDocument();
                using (var r = new StringReader(rssData))
                {
                    xDoc.Load(r);
                    xDoc.Save(xmlTextWriter);
                    _channel.PrettyRssData = stringWriter.ToString();
                }
            }
        }
        #endregion
        
        #region public Parse method
        /// <summary>
        /// Parses the RSS feed.
        /// </summary>
        /// <returns>The <see cref="PodcastChannel"/> instance represented by the RSS data.</returns>
        public PodcastChannel Parse()
        {
            XmlNode channelElement = ((XmlNode)NodeToParse).SelectSingleNode("rss/channel");
            if (channelElement == null)
            {
                ParseErrors.Add("No rss/channel node found");
                SaveParseErrors(_channel);
                return _channel;
            }
            
            GetTitle(channelElement);
            GetLink(channelElement);
            GetRssUrl(channelElement);
            GetDescription(channelElement);
            GetLanguage(channelElement);
            GetCopyright(channelElement);
            GetLastBuildDate(channelElement);
            GetPublishDate(channelElement);
            GetAuthor(channelElement);
            GetImage(channelElement);
            GetCategories(channelElement);
            GetKeywords(channelElement);
            GetDocuments(channelElement);
            GetGenerator(channelElement);
            GetManagingEditor(channelElement);
            GetSubtitle(channelElement);
            _channel.ITunesExplicit = GetBooleanFromChildElement("itunes:explicit", channelElement);
            
            XmlNode ownerElement = channelElement.SelectSingleNode("itunes:owner", NamespaceManager);
            if (ownerElement != null)
            {
                _channel.OwnerEmail = GetStringFromChildElement("itunes:email", ownerElement);
                _channel.OwnerName = GetStringFromChildElement("itunes:name", ownerElement);
                if (RemoveParsedContent && ownerElement.ChildNodes.Count == 0)
                {
                    channelElement.RemoveChild(ownerElement);
                    SaveUnparsedContent();
                }
            }
            
            if (RemoveParsedContent && channelElement.ChildNodes.Count == 0)
            {
                channelElement.ParentNode.RemoveChild(channelElement);
                SaveUnparsedContent();
            }
            
            XmlNodeList items = channelElement.SelectNodes("item");
            foreach (XmlNode item in items) 
            {
                PodcastEpisodeParser p = new PodcastEpisodeParser(item, NamespaceManager);
                p.RemoveParsedContent = RemoveParsedContent;
                PodcastEpisode episode = p.Parse();
                if (episode.Image.LocalPath == CachedImage.DefaultImage)
                {
                    episode.Image = _channel.Image;
                }
                
                _channel.Episodes.Add(episode);
                if (RemoveParsedContent)
                {
                    channelElement.RemoveChild(item);
                    SaveUnparsedContent();
                }
            }
            
            SaveParseErrors(_channel);
            
            if (RemoveParsedContent)
            {
                UnparsedContent.Seek(0, SeekOrigin.Begin);
                _channel.UnparsedRssData = new StreamReader(UnparsedContent).ReadToEnd();
            }
            
            return _channel;
        }
        #endregion
        
        #region private methods
        
        #region static AddCategory method
        /// <summary>
        /// Adds the supplied category to the supplied collection, if it is not
        /// null or empty and isn't already in the collection.
        /// </summary>
        /// <param name="category">The category to add.</param>
        /// <param name="categories">The collection.</param>
        private static void AddCategory(string category, Collection<string> categories)
        {
            if (!string.IsNullOrEmpty(category))
            {
                if (!categories.Contains(category))
                {
                    categories.Add(category);
                }
            }
        }
        #endregion
        
        #region GetTitle
        /// <summary>
        /// Gets the channel's title.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetTitle(XmlNode channelElement)
        {
            _channel.Title = GetStringFromChildElement("title", channelElement);
            if (string.IsNullOrEmpty(_channel.Title))
            {
                ParseErrors.Add("No title found");
            }
        }
        #endregion
        
        #region GetLink
        /// <summary>
        /// Gets the channel's link.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetLink(XmlNode channelElement)
        {
            _channel.Link = GetStringFromChildElement("link", channelElement);
            if (string.IsNullOrEmpty(_channel.Link))
            {
                ParseErrors.Add("No link found");
            }
        }
        #endregion
        
        #region GetRssUrl
        /// <summary>
        /// Gets the channel's RSS URL.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetRssUrl(XmlNode channelElement)
        {
            _channel.RssUrl = GetStringFromChildElementAttribute("atom:link", "href", channelElement);
            if (string.IsNullOrEmpty(_channel.Link))
            {
                ParseErrors.Add("No RSS URL found");
            }
            
            if (RemoveParsedContent)
            {
                // We don't use the values of the rel or type attributes but if they're the only remaining
                // attributes them remove the atom:link element because we've used all its useful content.
                XmlNode atomLink = channelElement.SelectSingleNode("atom:link", NamespaceManager);
                XmlAttributeCollection atts = atomLink.Attributes;
                if (atts.Count == 2)
                {
                    if (atts[0].Name == "rel" || atts[1].Name == "rel")
                    {
                        if (atts[0].Name == "type" || atts[1].Name == "type")
                        {
                            channelElement.RemoveChild(atomLink);
                            SaveUnparsedContent();
                        }
                    }
                }
            }
        }
        #endregion
        
        #region GetDescription
        /// <summary>
        /// Gets the channel's description.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetDescription(XmlNode channelElement)
        {
            string description = GetStringFromChildElement("description", channelElement);
            string iTunesSummary = GetStringFromChildElement("itunes:summary", channelElement);
            _channel.Description = description.Length > iTunesSummary.Length ? description : iTunesSummary;
            if (string.IsNullOrEmpty(description) && string.IsNullOrEmpty(iTunesSummary))
            {
                ParseErrors.Add("No description found");
            }
        }
        #endregion
        
        #region GetLanguage
        /// <summary>
        /// Gets the channel's language.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetLanguage(XmlNode channelElement)
        {
            _channel.Language = GetStringFromChildElement("language", channelElement);
            if (string.IsNullOrEmpty(_channel.Language))
            {
                ParseErrors.Add("No language found");
            }
        }
        #endregion
        
        #region GetCopyright
        /// <summary>
        /// Gets the channel's copyright statement.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetCopyright(XmlNode channelElement)
        {
            _channel.Copyright = GetStringFromChildElement("copyright", channelElement);
            if (string.IsNullOrEmpty(_channel.Copyright))
            {
                ParseErrors.Add("No copyright found");
            }
        }
        #endregion
        
        #region GetLastBuildDate
        /// <summary>
        /// Gets the date the RSS feed was last built.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetLastBuildDate(XmlNode channelElement)
        {
            _channel.LastBuildDate = GetDateTimeFromChildElement("lastBuildDate", channelElement);
            if (_channel.LastBuildDate == DateTime.MinValue)
            {
                ParseErrors.Add("No last build date found");
            }
        }
        #endregion
        
        #region GetPublishDate
        /// <summary>
        /// Gets the date the channel was last published.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetPublishDate(XmlNode channelElement)
        {
            // Not a mandatory field so no need to log an error if it's not found
            _channel.PublishedDate = GetDateTimeFromChildElement("pubDate", channelElement);
        }
        #endregion
        
        #region GetCategories
        /// <summary>
        /// Gets the channel's categories.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetCategories(XmlNode channelElement)
        {
            Collection<string> categories = new Collection<string>();
            string category;
            
            // First look for a single element like <category>Music</category>
            category = GetStringFromChildElement("category", channelElement);
            AddCategory(category, categories);
            
            // Then look for elements like <itunes:category text="Music"><itunes:category text="Dance" /></itunes:category>
            // Assume no limit to the depth of nesting.
            XmlNodeList categoryNodes = channelElement.SelectNodes("itunes:category", NamespaceManager);
            foreach (XmlNode categoryNode in categoryNodes)
            {
                category = ParseNestedCategory(string.Empty, categoryNode);
                AddCategory(category, categories);
            }
            
            StringBuilder sb = new StringBuilder();
            foreach (string cat in categories)
            {
                sb.Append(cat + ", ");
            }
            
            sb.Remove(sb.Length - 2, 2);
            _channel.Categories = sb.ToString();
        }
        #endregion
        
        #region ParseNestedCategory
        /// <summary>
        /// Recursively parses nested itunes:category elements and returns the category
        /// name and all sub-categories as a single string.
        /// </summary>
        /// <param name="containingCategories">String representation of the parent categories.</param>
        /// <param name="categoryNode">The element to parse.</param>
        /// <returns>String representation of parent categories plus this category.</returns>
        private string ParseNestedCategory(string containingCategories, XmlNode categoryNode)
        {
            if (categoryNode == null)
            {
                return containingCategories;
            }
            
            XmlAttribute categoryAttribute = categoryNode.Attributes["text"];
            if (categoryAttribute == null)
            {
                return containingCategories;
            }
            
            categoryNode.Attributes.RemoveNamedItem("text");
            SaveUnparsedContent();
            
            string category = categoryAttribute.Value;
            if (string.IsNullOrEmpty(category))
            {
                return containingCategories;
            }
            
            string newCategories;
            XmlNode childElement = categoryNode.SelectSingleNode("itunes:category", NamespaceManager);
            if (string.IsNullOrEmpty(containingCategories))
            {
                newCategories = ParseNestedCategory(category, childElement);
            }
            else
            {
                containingCategories += " -> " + category;
                newCategories = ParseNestedCategory(containingCategories, childElement);
            }
            
            if (categoryNode.ChildNodes.Count == 0)
            {
                categoryNode.ParentNode.RemoveChild(categoryNode);
            }
            
            return newCategories;
        }
        #endregion
        
        #region GetAuthor
        /// <summary>
        /// Gets the channel's author.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetAuthor(XmlNode channelElement)
        {
            _channel.Author = GetStringFromChildElement("itunes:author", channelElement);
            if (string.IsNullOrEmpty(_channel.Author))
            {
                ParseErrors.Add("No author found");
            }
        }
        #endregion
        
        #region GetImage
        /// <summary>
        /// Gets the channel's image.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetImage(XmlNode channelElement)
        {
            string iTunesImageUrl = GetStringFromChildElementAttribute("itunes:image", "href", channelElement);
            XmlNode imageElement = channelElement.SelectSingleNode("image");
            string imageUrl = string.Empty;
            _channel.Image = new CachedImage();
            if (imageElement != null)
            {
                imageUrl = GetStringFromChildElement("url", imageElement);
            }
            
            if (string.IsNullOrEmpty(iTunesImageUrl) && string.IsNullOrEmpty(imageUrl))
            {
                ParseErrors.Add("No image found");
            }
            
            if (string.IsNullOrEmpty(imageUrl))
            {
                _channel.Image.RemoteUrl = iTunesImageUrl;
                _channel.ImageTitle = GetStringFromChildElement("itunes:link", channelElement);
            }
            else
            {
                _channel.Image.RemoteUrl = imageUrl;
                _channel.ImageTitle = GetStringFromChildElement("title", imageElement);
                _channel.ImageLink = GetStringFromChildElement("link", imageElement);
                if (RemoveParsedContent)
                {
                    if (imageElement.ChildNodes.Count == 0)
                    {
                        channelElement.RemoveChild(imageElement);
                        SaveUnparsedContent();
                    }
                }
            }
        }
        #endregion
        
        #region GetKeywords
        /// <summary>
        /// Gets the channel's keywords.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetKeywords(XmlNode channelElement)
        {
            string keywords = GetStringFromChildElement("keywords", channelElement);
            string iTunesKeywords = GetStringFromChildElement("itunes:keywords", channelElement);
            string iTunesKeyword = GetStringFromChildElement("itunes:keyword", channelElement);
            
            if (string.IsNullOrEmpty(keywords) && string.IsNullOrEmpty(iTunesKeywords) && string.IsNullOrEmpty(iTunesKeyword))
            {
                ParseErrors.Add("No keywords found");
                return;
            }
            
            // Merge and deduplicate each variable in turn
            _keywords = new Collection<string>();
            MergeAndDeduplicateKeywords(keywords);
            MergeAndDeduplicateKeywords(iTunesKeywords);
            MergeAndDeduplicateKeywords(iTunesKeyword);
            
            var sb = new StringBuilder();
            foreach (string keyword in _keywords) 
            {
                sb.Append(keyword + ", ");
            }
            
            // remove final comma
            sb.Remove(sb.Length - 2, 2);
            
            _channel.Keywords = sb.ToString();
        }
        #endregion
        
        #region MergeAndDeduplicateKeywords
        /// <summary>
        /// Adds the supplied list of keywords to the _keywords collection, where
        /// they are not already in the collection.
        /// </summary>
        /// <param name="keywords">Comma-separated list of keywords to add.</param>
        private void MergeAndDeduplicateKeywords(string keywords)
        {
            string[] keywordArray = keywords.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in keywordArray) 
            {
                string trimmedWord = word.Trim();
                if (!_keywords.Contains(trimmedWord))
                {
                    _keywords.Add(trimmedWord);
                }
            }
        }
        #endregion
        
        #region GetDocuments
        /// <summary>
        /// Gets the channel's document link.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetDocuments(XmlNode channelElement)
        {
            string documents = GetStringFromChildElement("docs", channelElement);
            if (!string.IsNullOrEmpty(documents))
            {
                _channel.Documents = documents;
            }
        }
        #endregion
        
        #region GetManagingEditor
        /// <summary>
        /// Gets the channel's managing editor.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetManagingEditor(XmlNode channelElement)
        {
            string managingEditor = GetStringFromChildElement("managingEditor", channelElement);
            if (!string.IsNullOrEmpty(managingEditor))
            {
                _channel.ManagingEditor = managingEditor;
            }
        }
        #endregion
        
        #region GetGenerator
        /// <summary>
        /// Gets the channel's generator.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetGenerator(XmlNode channelElement)
        {
            string generator = GetStringFromChildElement("generator", channelElement);
            if (!string.IsNullOrEmpty(generator))
            {
                _channel.Generator = generator;
            }
        }
        #endregion
        
        #region GetSubtitle
        /// <summary>
        /// Gets the channel's subtitle.
        /// </summary>
        /// <param name="channelElement">The channel element from the RSS feed.</param>
        private void GetSubtitle(XmlNode channelElement)
        {
            string subtitle = GetStringFromChildElement("itunes:subtitle", channelElement);
            if (!string.IsNullOrEmpty(subtitle))
            {
                _channel.Subtitle = subtitle;
            }
        }
        #endregion
        
        #region SaveParseErrors
        /// <summary>
        /// Copies any parse errors to the channel.
        /// </summary>
        /// <param name="channel">The channel being created by this parser.</param>
        private void SaveParseErrors(PodcastChannel channel)
        {
            foreach (string error in ParseErrors)
            {
                channel.ParseErrors.Add(error);
            }
        }
        #endregion
        
        #endregion
    }
}
