using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Blog.Core.Entities;

namespace Blog.Infrastructure.Database
{
    public class MyContextSeed
    {
        public static async Task SeedAsync(MyContext myContext,
            ILoggerFactory loggerFactory, int retry = 0)
        {
            int retryForAvailability = retry;
            try
            {
                if (!myContext.Posts.Any())
                {
                    myContext.Posts.AddRange(
                        new List<Post> 
                        {
                          new Post 
                          {
                            Title = "Post Title",
                            Body = "Post Body 1",
                            Author = "Mike",
                            LastModified = DateTime.Now
                          },
                          new Post
                          {
                            Title = "Post Title 2",
                            Body = "Post Body 2",
                            Author = "Jack",
                            LastModified = DateTime.Now
                          },
                          new Post
                          {
                            Title = "Post Title 3",
                            Body = "Post Body 3",
                            Author = "Dave",
                            LastModified = DateTime.Now
                          },
                          new Post
                          {
                            Title = "Post Title 4",
                            Body = "Post Body 4",
                            Author = "Lily",
                            LastModified = DateTime.Now
                          },
                          new Post
                          {
                            Title = "Post Title 5",
                            Body = "Post Body 5",
                            Author = "Lily",
                            LastModified = DateTime.Now
                          },
                          new Post
                          {
                            Title = "Post Title 6",
                            Body = "Post Body 6",
                            Author = "Tony",
                            LastModified = DateTime.Now
                          }
                        });
                    await myContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if(retryForAvailability < 10) 
                {
                    retryForAvailability++;
                    var logger = loggerFactory.CreateLogger<MyContextSeed>();
                    logger.LogError(ex.Message);
                    await SeedAsync(myContext, loggerFactory, retryForAvailability);
                }
            }
        }
    }
}
