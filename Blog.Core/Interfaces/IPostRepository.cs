using Blog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Interfaces
{
    public interface IPostRepository
    {
        Task<PaginatedList<Post>> GetAllPostsAsync(PostParameters postParameters);
        Task<Post> GetPostByIdAsync(int id);
        Task AddPost(Post post);
        void Delete(Post post);
        void Update(Post post);
    }
}
