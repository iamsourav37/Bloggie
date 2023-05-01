using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.Repositories;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {

        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitTag(AddTagRequest addTagRequest)
        {
            // Mapping AddTagRequest to Tag domain model
            var tag = new Tag { Name = addTagRequest.Name, DisplayName = addTagRequest.DisplayName };

            await tagRepository.AddTagAsync(tag);


            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {

            // Read the Tags from DB using DBContext
            var allTags = await this.tagRepository.GetAllTagsAsync();

            return View(allTags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid tagId)
        {

            var tag = await this.tagRepository.GetTagAsync(tagId);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest { DisplayName = tag.DisplayName, Id = tag.Id, Name = tag.Name };
                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag { Id = editTagRequest.Id, Name = editTagRequest.Name, DisplayName = editTagRequest.DisplayName };

            var updatedTag = await this.tagRepository.UpdateTagAsync(tag);
            if (updatedTag != null)
            {
                // Show Success notification
                return RedirectToAction("List");
            }

            //Todo: Show error notification
            return View(null);
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(Guid tagId)
        {
            var deletedTag = await this.tagRepository.DeleteTagAsync(tagId);
            if (deletedTag != null)
            {
                // Show success notification
                return RedirectToAction("List");
            }
            return RedirectToAction("List");
        }
    }
}
