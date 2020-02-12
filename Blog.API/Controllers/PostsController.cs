using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.API.Helpers;
using Blog.Core.Entities;
using Blog.Core.Interfaces;
using Blog.Infrastructure.Database;
using Blog.Infrastructure.Extensions;
using Blog.Infrastructure.Resources;
using Blog.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IPropertyMappingContainer _propertyMappingContainer;
        public PostsController(
            IPostRepository postRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ITypeHelperService typeHelperService,
            IPropertyMappingContainer propertyMappingContainer)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _typeHelperService = typeHelperService;
            _propertyMappingContainer = propertyMappingContainer;
    }

        [HttpGet(Name = "GetPosts")]
        public async Task<IActionResult> Get([FromQuery]PostParameters postParameters)
        {
            if (!_propertyMappingContainer.ValidateMappingExistsFor<PostResource, Post>(postParameters.OrderBy))
            {
                return BadRequest("Can't find fields for sorting");
            }
            if (!_typeHelperService.TypeHasProperties<PostResource>(postParameters.Fields))
            {
                return BadRequest("Fields not exist");
            }
            var postList = await _postRepository.GetAllPostsAsync(postParameters);
            var postResources = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResource>>(postList);
            var shapedPostResources = postResources.ToDynamicIEnumberable(postParameters.Fields);
            var previousPageLink = postList.HasPrevious ?
                CreatePostUri(postParameters, PaginationResourceUriType.PreviousPage) : null;
            var nextPageLink = postList.HasNext ?
                CreatePostUri(postParameters, PaginationResourceUriType.NextPage) : null;
            var meta = new
            {
                postList.PageSize,
                postList.PageIndex,
                postList.TotalItemsCount,
                postList.PageCount,
                previousPageLink,
                nextPageLink
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
            return Ok(shapedPostResources);
        }

        [HttpGet("{id}",Name = "GetPost")]
        public async Task<IActionResult> Get(int id, string fields = null)
        {
            if (!_typeHelperService.TypeHasProperties<PostResource>(fields))
            {
                return BadRequest("Fields not exist");
            }
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            var postResource = _mapper.Map<Post, PostResource>(post);
            var shapedPostResource = postResource.ToDynamic(fields);
            return Ok(shapedPostResource);
        }

        [HttpPost(Name="CreatePost")]
        public async Task<IActionResult> Post([FromBody] PostAddResource postAddResource)
        {
            if (postAddResource == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }

            var newPost = _mapper.Map<PostAddResource, Post>(postAddResource);

            newPost.Author = "admin";
            newPost.LastModified = DateTime.Now;

            await _postRepository.AddPost(newPost);

            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception("Save Failed!");
            }

            var resultResource = _mapper.Map<Post, PostResource>(newPost);

            return CreatedAtRoute("GetPost", new { id = newPost.Id }, resultResource);
        }

        [HttpPut("{id}", Name = "UpdatePost")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] PostUpdateResource postUpdate)
        {
            if (postUpdate == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            post.LastModified = DateTime.Now;
            _mapper.Map(postUpdate, post);
            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Updating post {id} failed when saving");
            }
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeletePost")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return BadRequest();
            }
            _postRepository.Delete(post);
            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Deleting post {id} failed when saving.");
            }
            return NoContent();
        }

        [HttpPatch("{id}", Name = "PartiallyUpdatePost")]
        public async Task<IActionResult> PartiallyUpdatePost(int id, 
            [FromBody] JsonPatchDocument<PostUpdateResource> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            var postToPatch = _mapper.Map<PostUpdateResource>(post);
            patchDoc.ApplyTo(postToPatch, ModelState);
            TryValidateModel(postToPatch);
            if (!ModelState.IsValid)
            {
                return new MyUnprocessableEntityObjectResult(ModelState);
            }
            _mapper.Map(postToPatch, post);
            post.LastModified = DateTime.Now;
            _postRepository.Update(post);
            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Updating post {id} failed when saving");
            }
            return NoContent();
        }

        private string CreatePostUri(PostParameters postParameters, PaginationResourceUriType paginationResourceUriType)
        {
            switch (paginationResourceUriType)
            {
                case PaginationResourceUriType.PreviousPage:
                    var previousParameters = new
                    {
                        pageIndex = postParameters.PageIndex - 1,
                        pageSize = postParameters.PageSize,
                        orderBy = postParameters.OrderBy,
                        fields = postParameters.Fields
                    };
                    return Url.Link("GetPosts", previousParameters);
                case PaginationResourceUriType.NextPage:
                    var nextParameters = new
                    {
                        pageIndex = postParameters.PageIndex + 1,
                        pageSize = postParameters.PageSize,
                        orderBy = postParameters.OrderBy,
                        fields = postParameters.Fields
                    };
                    return Url.Link("GetPosts", nextParameters);
                default:
                    var currentParameters = new
                    {
                        pageIndex = postParameters.PageIndex,
                        pageSize = postParameters.PageSize,
                        orderBy = postParameters.OrderBy,
                        fields = postParameters.Fields
                    };
                    return Url.Link("GetPosts", currentParameters);
            }
        }
    }
}