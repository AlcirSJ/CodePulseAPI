using CodePulseAPI.Data;
using CodePulseAPI.Models.Domain;
using CodePulseAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CodePulseAPI.Repositories.Implementation;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly ApplicationDbContext _dbContext;   
    public BlogPostRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<BlogPost> CreateAsync(BlogPost blogPost)
    {
        await _dbContext.BlogPosts.AddAsync(blogPost);
        await _dbContext.SaveChangesAsync();
        return blogPost;
    }

    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        return await _dbContext.BlogPosts.Include("Categories")
                                         .ToListAsync();
    }

    public async Task<BlogPost> GetByIdAsync(Guid id)
    {
        return await _dbContext.BlogPosts.Include("Categories")
                                         .FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
    {
      var existingBlogPost=  await _dbContext.BlogPosts.Include(x => x.Categories)
                                              .FirstOrDefaultAsync(x => x.Id.Equals(blogPost.Id));

       if(existingBlogPost is null)
        {
            return null;
        }

        //update blogPost
        _dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

        // Update Categories
        existingBlogPost.Categories = blogPost.Categories;

        await _dbContext.SaveChangesAsync();

        return blogPost;
        
    }
}
