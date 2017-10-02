using SDE.MediaManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Mvc.UnitTests
{
    /// <summary>
    /// Provides sets of test data for unit tests.
    /// </summary>
    public static class TestData
    {
        /// <summary>
        /// Gets a set of 100 media files generated at runtime.
        /// </summary>
        public static IQueryable<MediaFile> MediaFiles
        {
            get
            {
                var mediaFiles = new List<MediaFile>();
                for (int i = 0; i < 100; i++)
                {
                    var mediaFile = new MediaFile
                    {
                        FileName = "File" + i,
                        Folder = "Folder" + i,
                        SizeInBytes = i * 1000,
                        CreatedTimestamp = DateTime.Now,
                        ModifiedTimestamp = DateTime.Now.AddMinutes(i),
                    };

                    mediaFiles.Add(mediaFile);
                }

                return mediaFiles.AsQueryable();
            }
        }

        /// <summary>
        /// Gets a collection containing a single <see cref="FileMetadata"/> instance,
        /// just to stop test fixtures from throwing exceptions when the classes under
        /// test attempt to access metadata.
        /// </summary>
        public static IQueryable<FileMetadata> Metadata
        {
            get
            {
                var metadatas = new List<FileMetadata>();
                metadatas.Add(new FileMetadata { Name = "metadata1", Value = "value1" });
                return metadatas.AsQueryable();
            }
        }
    }
}
