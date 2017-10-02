using System;
using System.IO;
using System.Collections.ObjectModel;
using SDE.MediaManager;

namespace PopulateDatabase
{
    /// <summary>
    /// A quick and dirty way of populating the MediaManager database with any media
    /// files on your file system.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point.
        /// </summary>
        /// <param name="args">
        /// Command line arguments.
        /// This program expects one argument, the full path to the folder containing
        /// media files. The program will recursively scan this folder for any files
        /// with one of the filename extensions in the FileType table.
        /// </param>
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                string message = @"Usage: C:\users\Simon\Music"
                    + Environment.NewLine
                    + @"where C:\users\Simon\Music is the path containing the media files"
                    + Environment.NewLine
                    + "Database connection string is set in the app.config file.";
                Console.WriteLine(message);
            }

            string mediaPath = args[0];

            var context = new MediaManager.MvcViewModels.MediaManagerContext();
            var extensions = new Collection<string>();
            foreach (var fileType in context.FileTypes)
            {
                extensions.Add(fileType.Extension);
            }

            ScanFolder(mediaPath, context, extensions);

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static void ScanFolder(string path, MediaManager.MvcViewModels.MediaManagerContext context, Collection<string> extensions)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                string extension = Path.GetExtension(file).Replace(".", string.Empty);
                if (extensions.Contains(extension))
                {
                    Console.WriteLine("Adding " + file);
                    var fi = new FileInfo(file);
                    var mf = new MediaFile
                    {
                        FileName = fi.Name,
                        Folder = fi.DirectoryName,
                        SizeInBytes = fi.Length,
                        CreatedTimestamp = fi.CreationTime,
                        ModifiedTimestamp = fi.LastWriteTime
                    };

                    mf.ExamineForMetadata();
                    context.MediaFiles.Add(mf);
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Skipping " + file);
                }
            }

            foreach (var folder in Directory.GetDirectories(path))
            {
                ScanFolder(folder, context, extensions);
            }
        }
    }
}
