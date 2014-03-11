using System.Web;
using NUnit.Framework;
using FluentAssertions;
using MagicMoqController.Tests.WebFake.Controllers;

namespace MagicMoqTests
{
    [TestFixture]
    public class MoqerControllerTests
    {
        private SampleController _controller;

        [SetUp]
        public void SetUp()
        {
            var moqer = new MagicMoq.MoqerController();
            _controller = moqer.Resolve<SampleController>();
        }

        [Test]
        public void ShouldCreateACookieAndItsValueShouldBeCookieValue()
        {
            _controller.SomeActionWhichSetCookies();
            _controller.Response.Cookies["CookieName"].Value.Should().Be("CookieValue");
        }

        [Test]
        public void ShouldGetCookieNameFromRequest()
        {
            _controller.Request.Cookies.Add(new HttpCookie("CookieName", "CookieValue"));
            _controller.HasCookie("CookieName").Should().Be(true);
        }

        [Test]
        public void ShouldSetSomethingToSession()
        {
            _controller.SomeActionWhichSetSessionValue();
            _controller.Session["MagicMoqRocks"].Should().Be("Totally!");
        }

        [Test]
        public void ShouldReadValueFromSession()
        {
            _controller.Session["MagicMoqRocks"] = "Totally!";
            _controller.ReadStringSession("MagicMoqRocks").Should().Be("Totally!");
        }

        [Test]
        public void ShouldReadValuesFromFormCollection()
        {
            _controller.Request.Form.Add("inputName", "value");
            _controller.ReadFormValue("inputName").Should().Be("value");
        }
    }
}
