using System;
using System.Collections.Generic;
using System.Text;
using Blog.Core.Entities;
using Blog.Infrastructure.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Database
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) 
            : base(options) 
        {
        }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new PostConfiguration());
        }
    }
}
