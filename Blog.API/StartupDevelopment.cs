using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Blog.Infrastructure.Database;
using Blog.Core.Interfaces;
using Blog.Infrastructure.Repositories;
using Blog.API.Extensions;
using Microsoft.Extensions.Logging;
using AutoMapper;
using FluentValidation;
using Blog.Infrastructure.Resources;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Blog.API
{
    public class StartupDevelopment
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlite("Data Source=Blog.db");
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5001;
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddTransient<IValidator<PostResource>, PostResourceValidator>();

            services.AddMvc(options => 
            {
                options.ReturnHttpNotAcceptable = true;
                // options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter())
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //app.UseDeveloperExceptionPage();
            app.UseMyExceptionHandler(loggerFactory);
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
