using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MediaManager.Mvc.UnitTests
{
    /// <summary>
    /// If you are unit testing a controller which attempts to access properties
    /// of the HTTP request or the context in which the request is being made,
    /// set the controller's ControllerContext property to a new instance of
    /// <see cref="FakeControllerContext"/> to prevent exceptions.
    /// </summary>
    public class FakeControllerContext : ControllerContext
    {
        HttpContextBase _context = new FakeHttpContext();

        /// <summary>
        /// Gets and sets a fake HTTP context.
        /// </summary>
        public override HttpContextBase HttpContext
        {
            get { return _context; }
            set { _context = value; }
        }
    }

    /// <summary>
    /// Fake HTTP context.
    /// Used by the <see cref="FakeControllerContext"/> class.
    /// </summary>
    internal class FakeHttpContext : HttpContextBase
    {
        HttpRequestBase _request = new FakeHttpRequest();

        public override HttpRequestBase Request
        {
            get { return _request; }
        }
    }

    /// <summary>
    /// Fake HTTP request.
    /// Used by the <see cref="FakeHttpContext"/> class.
    /// </summary>
    internal class FakeHttpRequest : HttpRequestBase
    {
        public override string this[string key]
        {
            get { return null; }
        }

        public override NameValueCollection Headers
        {
            get { return new NameValueCollection(); }
        }
    }
}
