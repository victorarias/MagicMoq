using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MagicMoqController.Tests.WebFake.Controllers
{
    public class SampleController : Controller
    {
        private readonly ISomethingRepository _somethingRepository;
        private readonly ISomethingService _somethingService;

        public SampleController(ISomethingRepository somethingRepository, ISomethingService somethingService)
        {
            _somethingRepository = somethingRepository;
            _somethingService = somethingService;
        }

        public ActionResult SomeActionWhichSetCookies()
        {
            _somethingService.DoSomething();

            Response.Cookies.Add(new HttpCookie("CookieName", "CookieValue"));
            return View("SomeActionWhichSetCookies");
        }

        public ActionResult SomeActionWhichSetSessionValue()
        {
            Session["MagicMoqRocks"] = "Totally!";
            return View("SomeActionWhichSetSessionValue");
        }

        public bool HasCookie(string cookieName)
        {
            return Request.Cookies.AllKeys.Contains(cookieName);
        }

        public string ReadStringSession(string sessionKey)
        {
            return (string)Session[sessionKey];
        }

        public string ReadFormValue(string inputname)
        {
            return Request.Form[inputname];
        }
    }

    public interface ISomethingService
    {
        void DoSomething();
    }

    public interface ISomethingRepository
    {
        object GetSomething();
    }
}