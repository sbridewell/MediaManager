using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MediaManager.MvcViewModels;
using PagedList;
using SDE.MediaManager;

namespace MediaManager.Mvc.Controllers
{
    /// <summary>
    /// Controller for the /MediaFile URL.
    /// </summary>
    public class MediaFileController : Controller
    {
        /// <summary>
        /// Data source holding the media file data.
        /// </summary>
        private IMediaManagerDataSource _dataSource;

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFileController"/> class.
        /// </summary>
        /// <param name="dataSource">
        /// Data source holding the media file data.
        /// MVC will use dependency injection to instantiate this parameter.
        /// As this project uses StructureMap for dependency injection, set the
        /// concrete runtime type to use in place of IMediaManagerDataSource in the
        /// DependencyResolution\IoC.cs file.
        /// </param>
        public MediaFileController(IMediaManagerDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        #endregion

        #region Index action method
        /// <summary>
        /// Action method for the /Index URL.
        /// Lists media files and allows the user to search or filter them.
        /// </summary>
        /// <param name="model">
        /// View model for this controller.
        /// If this method is called from a postback, the model will contain the bound data
        /// from the page, otherwise MVC will instantiate it using the default constructor.
        /// </param>
        /// <param name="searchTerm">Text to search for in filenames or metadata.</param>
        /// <returns>A view which lists media files.</returns>
        /// <param name="pageNumber">Page number within the paged list to view.</param>
        public ActionResult Index(
            ListMediaFilesViewModel model, 
            string searchTerm = null, 
            int pageNumber = 1)
        {
            // Index method allows user to filter files based on metadata?
            if (model.IsInitialized)
            {
                SetModelMediaFiles(model, searchTerm, pageNumber);
            }
            else
            {
                model = new ListMediaFilesViewModel();
                model.SearchTerm = searchTerm;
                SetModelMediaFiles(model, searchTerm, pageNumber);

                // TODO: settings page and table to store which metadata items to display 
                // on index page and to use in searches
                // Get a distinct list of the names of metadata that the media files have
                model.MetadataNames = _dataSource.Query<FileMetadata>()
                                                    .Select(m => m.Name)
                                                    .Distinct()
                                                    .OrderBy(m => m);

                model.ShowMetadata = new Dictionary<string, bool>();
                foreach (var name in model.MetadataNames)
                {
                    model.ShowMetadata.Add(name, false);
                }

                // Attempt to show a sensible selection of metadata items by default.
                // Remember we don't know at design time what metadata names will be
                // available, so this is just a guess at what the user will want to see.
                TryShowMetadata(model, "Album");
                TryShowMetadata(model, "FirstArtist");
                TryShowMetadata(model, "Title");
                TryShowMetadata(model, "FirstGenre");

                model.IsInitialized = true;
            }

            // This doesn't do anything right now because the form isn't using Ajax
            if (Request.IsAjaxRequest())
            {
                // It's an asynchronous request so send only the partial view containing
                // the parts of the page which will have changed
                return PartialView("_MediaFiles", model);
            }

            // It's a full page request so send the entire view
            return View(model);
        }
        #endregion

        #region Details action method
        /// <summary>
        /// Action method for viewing the details of a single media file.
        /// </summary>
        /// <param name="id">The Id of the file to view.</param>
        /// <returns>A view showing the details of the file.</returns>
        public ActionResult Details(int id)
        {
            // TODO: Now we're copying media files to a cache folder within the application, we need a way of housekeeping them
            var model = _dataSource.Query<MediaFile>().Single(f => f.Id == id);
            var appFolder = Request.PhysicalApplicationPath;
            var localCacheFileName = System.IO.Path.GetFullPath(appFolder + @"Content\Media\" + model.FileName);
            ViewBag.CacheFileName = "/Content/Media/" + model.FileName;
            if (!System.IO.File.Exists(localCacheFileName))
            {
                System.IO.File.Copy(model.FullFileName, localCacheFileName);
            }

            return View(model);
        }
        #endregion

        #region private SetModelMediaFiles method
        /// <summary>
        /// Sets the media files held in the model to only those which meet the search
        /// criteria.
        /// </summary>
        /// <param name="model">View model used by this controller.</param>
        /// <param name="searchTerm">Text to search for.</param>
        /// <param name="pageNumber">Page number within the paged list to view.</param>
        private void SetModelMediaFiles(ListMediaFilesViewModel model, string searchTerm, int pageNumber)
        {
            model.SearchTerm = searchTerm;
            // REMOVE: model.PageNumber = pageNumber;
            model.MediaFiles = _dataSource.Query<MediaFile>()
                                          .Where(
                f => searchTerm == null
                || f.FileName.Contains(searchTerm)
                // This returns everything! || f.Metadata.Select(m => m.Value.Contains(searchTerm)).Any()
                ).OrderBy(f => f.FileName)
                .ToPagedList(pageNumber, 10);
        }
        #endregion

        #region private TryShowMetadata method
        /// <summary>
        /// If the dictionary of metadata names contains the supplied name, sets the
        /// flag to display that metadata item.
        /// </summary>
        /// <param name="model">View model used by this controller.</param>
        /// <param name="name">Name of the metadata item.</param>
        private void TryShowMetadata(ListMediaFilesViewModel model, string name)
        {
            if (model.ShowMetadata.ContainsKey(name))
            {
                model.ShowMetadata[name] = true;
            }
        }
        #endregion
    }
}