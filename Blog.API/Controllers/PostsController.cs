using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Entities;
using Blog.Core.Interfaces;
using Blog.Infrastructure.Database;
using Blog.Infrastructure.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PostsController(
            IPostRepository postRepository, 
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await _postRepository.GetAllPosts();
            var postResources = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResource>>(posts);
            return Ok(postResources);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var post = new Post
            {
                Author = "Admin",
                Body = "Test",
                Title = "Test A",
                LastModified = DateTime.Now
            };

            await _postRepository.AddPost(post);

            await _unitOfWork.SaveAsync();

            return Ok();
        }
    }
}