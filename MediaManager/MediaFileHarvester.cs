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
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Vlc.DotNet.Core;

namespace SDE.MediaManager
{
    /// <summary>
    /// Helper class containing functionality to scan the filesystem for media files.
    /// </summary>
    public static class MediaFileHarvester
    {
        /// <summary>
        /// Used to obtain metadata from the files when populating the database.
        /// TODO: get VLC path from config file.
        /// </summary>
        private static VlcMediaPlayer _player = new VlcMediaPlayer(new DirectoryInfo(@"C:\Program Files (x86)\VideoLAN\VLC"));
        
        /// <summary>
        /// Gets the file system file which is currently being examined.
        /// Used during population of the database.
        /// </summary>
        public static string ExaminingFile { get; private set; }
        
        /// <summary>
        /// Recursively scans the supplied folder and subfolders and returns a list of media files found.
        /// </summary>
        /// <param name="mediaPath">Path to the folder to scan.</param>
        /// <returns>A list of <see cref="MediaFile"/> instances found in the supplied folder.</returns>
        public static async Task<Collection<MediaFile>> ScanFileSystem(string mediaPath)
        {
            using (MediaManagerContext context = new MediaManagerContext())
            {
                DbSet<FileType> fileTypes = context.FileTypes;
                List<MediaFile> mediaFiles = await GetFilesRecursive(mediaPath, fileTypes);
                return new Collection<MediaFile>(mediaFiles);
            }
        }
        
        #region private static GetFilesRecursive method
        /// <summary>
        /// Gets a list of <see cref="MediaFile"/> instances representing all files in 
        /// the supplied location and its subfolders with extensions matching the
        /// supplied list of <see cref="FileType"/>s.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="fileTypes">File types to return.</param>
        /// <returns>A list of <see cref="MediaFile"/>s representing the media files found.</returns>
        private static async Task<List<MediaFile>> GetFilesRecursive(string path, DbSet<FileType> fileTypes)
        {
            List<MediaFile> returnValue = new List<MediaFile>();
            foreach (FileType fileType in fileTypes)
            {
                string[] files = Directory.GetFiles(path, "*." + fileType.Extension, SearchOption.AllDirectories);
                foreach (string file in files) 
                {
                    ExaminingFile = file;
                    FileInfo info = new FileInfo(file);
                    MediaFile instance = new MediaFile
                    {
                        FileName = Path.GetFileName(file),
                        Folder = Path.GetDirectoryName(file),
                        SizeInBytes = info.Length,
                        CreatedTimestamp = info.CreationTime,
                        ModifiedTimestamp = info.LastWriteTime,
                    };
                    
                    await Task.Run(
                        () =>
                        {
                            ExamineForMetadata(instance);
                        });
                    
                    returnValue.Add(instance);
                }
            }
            
            return returnValue;
        }
        #endregion
        // REMOVE: examineForMetadata and dependent methods (now in MediaFile)
        #region private static ExamineForMetadata method
        /// <summary>
        /// Uses the VLC player to extract metadata from the supplied <see cref="MediaFile"/>
        /// and add it to the file's Metadata property.
        /// </summary>
        /// <param name="instance">The <see cref="MediaFile"/> for which to extract metadata.</param>
        private static void ExamineForMetadata(MediaFile instance)
        {
            _player.SetMedia(new FileInfo(instance.Folder + Path.DirectorySeparatorChar + instance.FileName), new string[] { });
            VlcMedia media = _player.GetMedia();
            
            // Parse the media - if we don't do this then the metadata isn't available
            media.Parse();
            AddMetadata(instance, "Length", new TimeSpan(_player.Length));

            // FIXED: all properties blank except Mrl and Title
            // FIXED: Title property contains filename, not actual title                    
            AddMetadata(instance, "Album", media.Album);
            AddMetadata(instance, "Artist", media.Artist);
            AddMetadata(instance, "ArtworkURL", media.ArtworkURL);
            AddMetadata(instance, "Copyright", media.Copyright);
            AddMetadata(instance, "Date", media.Date);
            AddMetadata(instance, "Description", media.Description);
            AddMetadata(instance, "Duration", media.Duration);
            AddMetadata(instance, "EncodedBy", media.EncodedBy);
            AddMetadata(instance, "Genre", media.Genre);
            AddMetadata(instance, "Language", media.Language);
            AddMetadata(instance, "Mrl", media.Mrl);
            AddMetadata(instance, "NowPlaying", media.NowPlaying);
            AddMetadata(instance, "Publisher", media.Publisher);
            AddMetadata(instance, "Rating", media.Rating);
            AddMetadata(instance, "Setting", media.Setting);
            AddMetadata(instance, "Title", media.Title);
            AddMetadata(instance, "TrackID", media.TrackID);
            AddMetadata(instance, "TrackNumber", media.TrackNumber);
            AddMetadata(instance, "URL", media.URL);
            foreach (var trackInfo in media.TracksInformations)
            {
                AddMetadata(instance, "TracksInformations.Audio.Channels", trackInfo.Audio.Channels);
                AddMetadata(instance, "TracksInformations.Audio.Rate", trackInfo.Audio.Rate);
                AddMetadata(instance, "TracksInformations.CodecFourCC", trackInfo.CodecFourcc);
                AddMetadata(instance, "TracksInformations.CodecName", trackInfo.CodecName);
                AddMetadata(instance, "TracksInformations.Level", trackInfo.Level);
                AddMetadata(instance, "TracksInformations.Profile", trackInfo.Profile);
                AddMetadata(instance, "TracksInformations.Video.Height", trackInfo.Video.Height);
                AddMetadata(instance, "TracksInformations.Video.Width", trackInfo.Video.Width);
            }
        }
        #endregion
        
        #region private static void AddMetadata methods
        /// <summary>
        /// Adds the supplied metadata to the supplied <see cref="MediaFile"/>.
        /// </summary>
        /// <param name="file">The file to add metadata to.</param>
        /// <param name="metadataName">Name of the metadata.</param>
        /// <param name="metadataValue">Value of the metadata.</param>
        private static void AddMetadata(MediaFile file, string metadataName, DateTime metadataValue)
        {
            if (metadataValue.Year != 1)
            {
                AddMetadata(file, metadataName, metadataValue.ToString());
            }
        }
        
        /// <summary>
        /// Adds the supplied metadata to the supplied <see cref="MediaFile"/>.
        /// </summary>
        /// <param name="file">The file to add metadata to.</param>
        /// <param name="metadataName">Name of the metadata.</param>
        /// <param name="metadataValue">Value of the metadata.</param>
        private static void AddMetadata(MediaFile file, string metadataName, TimeSpan metadataValue)
        {
            if (metadataValue.Ticks > 0)
            {
                AddMetadata(file, metadataName, metadataValue.ToString());
            }
        }
        
        /// <summary>
        /// Adds the supplied metadata to the supplied <see cref="MediaFile"/>.
        /// </summary>
        /// <param name="file">The file to add metadata to.</param>
        /// <param name="metadataName">Name of the metadata.</param>
        /// <param name="metadataValue">Value of the metadata.</param>
        private static void AddMetadata(MediaFile file, string metadataName, uint metadataValue)
        {
            AddMetadata(file, metadataName, metadataValue.ToString(CultureInfo.InvariantCulture));
        }
        
        /// <summary>
        /// Adds the supplied metadata to the supplied <see cref="MediaFile"/>.
        /// </summary>
        /// <param name="file">The file to add metadata to.</param>
        /// <param name="metadataName">Name of the metadata.</param>
        /// <param name="metadataValue">Value of the metadata.</param>
        private static void AddMetadata(MediaFile file, string metadataName, int metadataValue)
        {
            AddMetadata(file, metadataName, metadataValue.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Adds the supplied metadata to the supplied <see cref="MediaFile"/>.
        /// </summary>
        /// <param name="file">The file to add metadata to.</param>
        /// <param name="metadataName">Name of the metadata.</param>
        /// <param name="metadataValue">Value of the metadata.</param>
        private static void AddMetadata(MediaFile file, string metadataName, string metadataValue)
        {
            if (!string.IsNullOrWhiteSpace(metadataValue))
            {
                FileMetadata metadata = new FileMetadata { Name = metadataName, Value = metadataValue };
                file.Metadata.Add(metadata);
            }
        }
        #endregion
    }
}
