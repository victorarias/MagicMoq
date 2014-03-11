using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace MagicMoq
{
    public class MoqerController : Moqer
    {
        private Mock<HttpContextBase> _context;
        private Mock<HttpRequestBase> _request;
        private Mock<HttpResponseBase> _response;

        public new T Resolve<T>() where T : Controller
        {
            var controller = MockControllerDependencies(base.Resolve<T>());
            return controller;
        }

        public void AuthenticateRequest()
        {
            _request.SetupGet(c => c.IsAuthenticated).Returns(true);
        }

        private T MockControllerDependencies<T>(T controller) where T : Controller
        {
            _context = new Mock<HttpContextBase>();
            _request = new Mock<HttpRequestBase>();
            _response = new Mock<HttpResponseBase>(); 
            
            _request.SetupAllProperties();
            _response.SetupAllProperties();

            _context.Setup(ctx => ctx.Request).Returns(_request.Object);
            _context.Setup(ctx => ctx.Response).Returns(_response.Object);

            MockSession(_context);
            MockCookies(_request, _response);
            MockFormCollection(_request);

            var controllerContext = new ControllerContext(_context.Object, new RouteData(), controller);
            controller.ControllerContext = controllerContext;
            return controller;
        }

        private void MockFormCollection(Mock<HttpRequestBase> request)
        {
            var formCollection = new FormCollection();
            request.Setup(r => r.Form).Returns(formCollection);
        }

        private void MockCookies(Mock<HttpRequestBase> request, Mock<HttpResponseBase> response)
        {
            var cookies = new HttpCookieCollection();
            response.Setup(r => r.Cookies).Returns(cookies);
            request.Setup(r => r.Cookies).Returns(cookies);
        }

        private void MockSession(Mock<HttpContextBase> context)
        {
            var session = new HttpSessionStateFake();
            context.Setup(ctx => ctx.Session).Returns(session);
        }

        private class HttpSessionStateFake : HttpSessionStateBase {

            readonly Dictionary<string, object> _sessionValues = new Dictionary<string, object>();

            public override object this[string name]
            {
                get { return _sessionValues[name]; }
                set { if (_sessionValues.ContainsKey(name)) _sessionValues[name] = value; else this.Add(name, value); }
            }

            public override void Add(string name, object value)
            {
                _sessionValues[name] = value;
            }

            public override int Count
            {
                get { return _sessionValues.Count; }
            }
        }
    }
}
