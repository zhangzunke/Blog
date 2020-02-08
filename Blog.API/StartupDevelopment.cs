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
using Blog.Infrastructure.Services;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

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

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.ApiName = "restapi";
                });

            services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                // options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter())
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .AddFluentValidation();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddTransient<IValidator<PostAddResource>, PostAddOrUpdateResourceValidator<PostAddResource>>();
            services.AddTransient<IValidator<PostUpdateResource>, PostAddOrUpdateResourceValidator<PostUpdateResource>>();
            services.AddTransient<ITypeHelperService, TypeHelperService>();
            var propertyMappingContainer = new PropertyMappingContainer();
            propertyMappingContainer.Register<PostPropertyMapping>();
            services.AddSingleton<IPropertyMappingContainer>(propertyMappingContainer);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
            });

            services.Configure<MvcOptions>(options => 
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //app.UseDeveloperExceptionPage();
            app.UseAuthentication();
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
