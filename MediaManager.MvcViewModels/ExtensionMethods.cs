using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MediaManager.MvcViewModels
{
    /// <summary>
    /// Static class containing extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Turns a collection of strings into a collection suitable for binding to a
        /// drop-down list.
        /// </summary>
        /// <param name="metadataNames">The names to convert.</param>
        /// <param name="selectedName">The name to be initially selected in the list.</param>
        /// <returns>A collection suitable for binding to a drop-down list.</returns>
        public static IEnumerable<SelectListItem> ToSelectListItems(
            this IEnumerable<string> metadataNames,
            string selectedName)
        {
            return metadataNames.OrderBy(name => name)
                                .Select(name =>
                                new SelectListItem
                                {
                                    Selected = (name == selectedName),
                                    Text = name,
                                    Value = name
                                });
        }
    }
}
