using Blog.Core.Entities;
using Blog.Core.Interfaces;
using Blog.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly MyContext _myContext;
        public PostRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        public async Task AddPost(Post post)
        {
           await _myContext.AddAsync(post);
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _myContext.Posts.ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _myContext.Posts.FindAsync(id);
        }
    }
}
