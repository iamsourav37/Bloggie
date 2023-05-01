using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Models.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<Tag> AddTagAsync(Tag tag)
        {
            await this.bloggieDbContext.Tags.AddAsync(tag);
            await this.bloggieDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteTagAsync(Guid id)
        {
            var tagToDelete = await this.GetTagAsync(id);

            if (tagToDelete != null)
            {
                this.bloggieDbContext.Tags.Remove(tagToDelete);
                await bloggieDbContext.SaveChangesAsync();
                return tagToDelete;
            }
            return null;

        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await this.bloggieDbContext.Tags.ToListAsync();
        }

        public async Task<Tag?> GetTagAsync(Guid id)
        {
            return await this.bloggieDbContext.Tags.FindAsync(id);
        }

        public async Task<Tag?> UpdateTagAsync(Tag tag)
        {
            var existingTag = await this.GetTagAsync(tag.Id);
            if (existingTag != null)
            {
                existingTag.DisplayName = tag.DisplayName;
                existingTag.Name = tag.Name;
                await this.bloggieDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }
    }
}
