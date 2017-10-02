using PagedList;
using SDE.MediaManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MediaManager.MvcViewModels
{
    /// <summary>
    /// View model for listing media files.
    /// </summary>
    public class ListMediaFilesViewModel
    {
        /// <summary>
        /// Gets or sets a value which indicates whether this view model has been initialised.
        /// Use this in controller action methods to determine whether this instance was posted
        /// from a view (true) or instantiated by dependency injection (false).
        /// Don't forget to set it to true after initialising the properties.
        /// </summary>
        public bool IsInitialized { get; set; }

        /// <summary>
        /// Gets or sets the media files in scope for this view model.
        /// </summary>
        public IPagedList<MediaFile> MediaFiles { get; set; }

        /// <summary>
        /// Gets or sets a distinct list of the names of metadata assigned to the media files.
        /// </summary>
        public IEnumerable<string> MetadataNames { get; set; }

        /// <summary>
        /// Gets or sets whether each of the metadata items should be shown in a list
        /// of media files.
        /// The dictionary key is the name of the metadata item (Album etc).
        /// </summary>
        public Dictionary<string, bool> ShowMetadata { get; set; }

        /// <summary>
        /// Gets or sets a string to search for in filenames and/or metadata values.
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets a collection of <see cref="SelectListItem"/> instances which can
        /// be used in a MVC view using syntax similar to
        /// <code>
        /// @Html.DropDownListFor(m => m.MetadataNames, Model.MetadataNameItems)
        /// </code>
        /// </summary>
        public IEnumerable<SelectListItem> MetadataNameItems
        {
            get
            {
                return MetadataNames.ToSelectListItems(MetadataNames.FirstOrDefault());
            }
        }

        /// <summary>
        /// Gets the possible values of the named metadata item.
        /// </summary>
        /// <param name="metadataName">Name of the metadata item, e.g. "Album".</param>
        /// <returns>The possible values of the named metadata item.</returns>
        /// <remarks>
        /// For example, if you use the string "Album" as the metadata name, this method
        /// will return the names of all the albums that your media files are in.
        /// </remarks>
        public IQueryable<string> MetadataValues(string metadataName)
        {
            throw new NotImplementedException();
            // REMOVE: return _dataSource.Metadata.Where(m => m.Name == metadataName).Select(m => m.Value);
        }

        /// <summary>
        /// Gets or sets the name of the currently selected metadata item.
        /// </summary>
        public string SelectedMetadataName { get; set; }
    }
}
