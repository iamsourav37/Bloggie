using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitTag()
        {
            var name = Request.Form["name"];
            var displayName = Request.Form["displayName"];
            return View("Add");
        }
    }
}
