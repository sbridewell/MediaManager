using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaManager.Mvc.Controllers;
using MediaManager.MvcViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MediaManager.Mvc.UnitTests
{
    /// <summary>
    /// Test fixture for the <see cref="MediaFileController"/> class.
    /// </summary>
    [TestClass]
    public class MediaFileControllerTest
    {
        /// <summary>
        /// Example test method for the Index method.
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            var db = new FakeMediaManagerDb();
            db.AddSet(TestData.MediaFiles);
            db.AddSet(TestData.Metadata);
            var controller = new MediaFileController(db);
            controller.ControllerContext = new FakeControllerContext();

            ViewResult result = (ViewResult)controller.Index(new ListMediaFilesViewModel());
            ListMediaFilesViewModel model = result.Model as ListMediaFilesViewModel;

            Assert.AreEqual(10, model.MediaFiles.Count);
        }
    }
}
