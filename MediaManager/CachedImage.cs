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
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Xml.Serialization;

namespace SDE.MediaManager
{
    /// <summary>
    /// An image from the web which can be cached locally.
    /// </summary>
    [ComplexType]
    public class CachedImage
    {
        /// <summary>
        /// Paths to images to be deleted the next time the application starts.
        /// </summary>
        private static Collection<string> _imagesToDelete = new Collection<string>();
        
        /// <summary>
        /// The folder in which locally cached images are saved.
        /// </summary>
        private static string _localImageFolder;
        
        #region properties
        /// <summary>
        /// Gets the path to the folder where locally cached images are stored.
        /// </summary>
        [NotMapped]
        public static string LocalImageFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_localImageFolder))
                {
                    Settings s = Settings.GetInstance;
                    _localImageFolder = Path.Combine(s.RootFolder, s.PodcastSubfolder, "Images");
                }
                
                return _localImageFolder;
            }
        }

        /// <summary>
        /// Gets the path to the image to use for uninitialized image properties.
        /// </summary>
        [NotMapped]
        public static string DefaultImage
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), "NoImage.png"); }
        }
        
        /// <summary>
        /// Gets the path to a file containing a list of filenames of locally cached images to be deleted
        /// next time the application is executed.
        /// </summary>
        public static string ImagesToDeleteFile
        {
            get
            {
                return Path.Combine(LocalImageFolder, "ImagesToDelete.txt");
            }
        }
        
        /// <summary>
        /// Gets or sets the image's URL on the web.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1056", Justification = "System.Uri not supported by Entity Framework")]
        [XmlAttribute]
        public string RemoteUrl { get; set; }
        
        /// <summary>
        /// Gets the path to the locally cached copy of the image.
        /// </summary>
        [NotMapped]
        public string LocalPath
        {
            get
            {
                return GetCachedImagePath(LocalImageFolder, RemoteUrl);
            }
        }
        #endregion
        
        #region public methods
        
        #region Delete
        /// <summary>
        /// Deletes the locally cached copy of the image.
        /// </summary>
        /// <remarks>
        /// The image is not deleted immediately because it might still be in use by the UI.
        /// Instead, the paths of all images to delete are written to a file, to be deleted
        /// the next time the application is started.
        /// The intention is that the main view model will read this file and delete the images
        /// in its constructor.
        /// Based on an idea by Josh Smith: <a href="https://joshsmithonwpf.wordpress.com/2008/02/21/deleting-an-image-file-displayed-by-an-image-element/"/>.
        /// </remarks>
        public void Delete()
        {
            if (!_imagesToDelete.Contains(LocalPath))
            {
                _imagesToDelete.Add(LocalPath);
                StringBuilder sb = new StringBuilder();
                foreach (string i in _imagesToDelete)
                {
                    sb.AppendLine(i);
                }
                
                File.WriteAllText(ImagesToDeleteFile, sb.ToString());
            }
        }
        #endregion
        
        #endregion
        
        #region private methods
        
        #region static GetCachedImagePath method
        /// <summary>
        /// Gets the location in the local cache of the image at the supplied URL.
        /// If the image is not already in the cache it is downloaded.
        /// </summary>
        /// <param name="cacheFolder">The folder in which to hold cached images.</param>
        /// <param name="url">Remote URL of the image.</param>
        /// <returns>Path to the location of the image in the local cache.</returns>
        [SuppressMessage("Microsoft.Design", "CA1054", Justification = "System.Uri not supported by Entity Framework")]
        private static string GetCachedImagePath(string cacheFolder, string url)
        {
            return GetCachedImagePath(cacheFolder, url, true);
        }
        
        /// <summary>
        /// Gets the location in the local cache of the image at the supplied URL.
        /// </summary>
        /// <param name="cacheFolder">The folder in which to hold cached images.</param>
        /// <param name="url">Remote URL of the image.</param>
        /// <param name="downloadIfNotCached">True to download the image if it's not in the cache.</param>
        /// <returns>Path to the location of the image in the local cache.</returns>
        [SuppressMessageAttribute("Microsoft.Design", "CA1054", Justification = "System.Uri not supported by Entity Framework")]
        private static string GetCachedImagePath(string cacheFolder, string url, bool downloadIfNotCached)
        {
            if (string.IsNullOrEmpty(cacheFolder))
            {
                throw new ArgumentNullException("cacheFolder");
            }
            
            // Prevent runtime errors about ImageSourceConverter not being able to convert an empty string
            if (string.IsNullOrEmpty(url))
            {
                return DefaultImage;
            }
            
            if (!Directory.Exists(cacheFolder))
            {
                Directory.CreateDirectory(cacheFolder);
            }
            
            string localFilename = WebHelper.GetLocalFileNameFromUrl(url);
            string cachedImageLocation = Path.Combine(cacheFolder, localFilename);
            if (downloadIfNotCached)
            {
                if (!File.Exists(cachedImageLocation))
                {
                    using (var client = new WebClient())
                    {
                        byte[] imageData = client.DownloadData(url);
                        using (var memoryStream = new MemoryStream(imageData))
                        {
                            using (var image = Image.FromStream(memoryStream))
                            {
                                switch (Path.GetExtension(cachedImageLocation).ToUpperInvariant())
                                {
                                    case ".JPG":
                                    case ".JPEG":
                                        image.Save(cachedImageLocation, ImageFormat.Jpeg);
                                        break;
                                        
                                    case ".GIF":
                                        image.Save(cachedImageLocation, ImageFormat.Gif);
                                        break;
                                        
                                    case ".PNG":
                                        image.Save(cachedImageLocation, ImageFormat.Png);
                                        break;
                                        
                                    default:
                                        cachedImageLocation += ".jpg";
                                        image.Save(cachedImageLocation, ImageFormat.Jpeg);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            
            return cachedImageLocation;
        }
        #endregion

        #endregion
    }
}
