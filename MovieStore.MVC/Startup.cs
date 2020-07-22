using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MovieStore.Infrastructure.Data;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Infrastructure.Repositories;
using MovieStore.Core.ServiceInterfaces;
using MovieStore.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MovieStore.MVC
{
    public class Startup
    {
        //we are going to add new things in the startup files and these are the minimum code
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<MovieStoreDbContext>(options=>options.UseSqlServer(Configuration.GetConnectionString("MovieStoreDbConnection")));
            //DI in ASP.NET Core has 3 types of Lifetimes
            //Scoped, Singleton, Transient
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                            options =>
                            {
                                options.Cookie.Name = "MovieStoreAuthCookie";
                                options.ExpireTimeSpan = TimeSpan.FromHours(2);
                                options.LoginPath = "/Account/Login";
                            }

                );
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieService, MovieService>();//you can call MovieServieTest
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICryptoService, CryptoService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
