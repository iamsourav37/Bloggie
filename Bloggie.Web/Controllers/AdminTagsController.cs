using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {

        private readonly BloggieDbContext bloggieDbContext;

        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitTag(AddTagRequest addTagRequest)
        {
            // Mapping AddTagRequest to Tag domain model
            var tag = new Tag { Name = addTagRequest.Name, DisplayName = addTagRequest.DisplayName };


            this.bloggieDbContext.Tags.Add(tag);
            this.bloggieDbContext.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult List()
        {

            // Read the Tags from DB using DBContext
            var allTags = this.bloggieDbContext.Tags.ToList();


            return View(allTags);
        }

        [HttpGet]
        public IActionResult Edit(Guid tagId)
        {

            var tag = this.bloggieDbContext.Tags.Find(tagId);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest { DisplayName = tag.DisplayName, Id = tag.Id, Name = tag.Name };
                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag { Id = editTagRequest.Id, Name = editTagRequest.Name, DisplayName = editTagRequest.DisplayName };

            var existingTag = bloggieDbContext.Tags.Find(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                // Save the changes
                bloggieDbContext.SaveChanges();

                //Todo: Show success notification
                return RedirectToAction("List");
            }

            //Todo: Show error notification
            return View(null);
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Delete(Guid tagId)
        {
            var tagToDelete = bloggieDbContext.Tags.Find(tagId);

            if (tagToDelete != null)
            {
                bloggieDbContext.Tags.Remove(tagToDelete);
                bloggieDbContext.SaveChanges();

                // Show Success notification
            }
            return RedirectToAction("List");
        }
    }
}
