using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookApi
{
    public class Startup
    {
        private readonly IConfiguration _Config;
        public Startup(IConfiguration config)
        {
            _Config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connectionString = _Config["connectionStrings:bookDbConnectionString"];
            services.AddDbContext<BookDbContext>(c => c.UseSqlServer(connectionString));
        
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IReviewerRepository, ReviewerRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, BookDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }




            //context.SeedDataContext();

            app.UseMvc();

            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync("Hello World!");
            // });
        }
    }
}
