using INS364.DigitalNews.Context;
using INS364.DigitalNews.Data;
using INS364.DigitalNews.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace INS364.DigitalNews
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<NewsDbContext>(options =>
            {
                string connectionString = Configuration.GetConnectionString("DigitalNewsConnection");
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Auth/Index";
                options.ReturnUrlParameter = "returnUrl";
            });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

             
             this.ConfigureDatabase(app);

             app.UseHttpsRedirection();
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

        private void ConfigureDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<NewsDbContext>();
                bool createdFirstRun = context.Database.EnsureCreated();

                
                if (createdFirstRun)
                {
                    context.NewsTags.AddRange(new NewsTagModel[]
                    {
                         new NewsTagModel() { NewsTagDesc = "Econom�a" },
                         new NewsTagModel() { NewsTagDesc = "Tecnolog�a" },
                         new NewsTagModel() { NewsTagDesc = "Salud" },
                         new NewsTagModel() { NewsTagDesc = "Politica" },
                         new NewsTagModel() { NewsTagDesc = "Internacional" },
                         new NewsTagModel() { NewsTagDesc = "Negocios" },
                         new NewsTagModel() { NewsTagDesc = "Arte y m�sica" }
                    });

                    context.NewsImpacts.AddRange(new NewsImpactModel[]
                    {
                        new NewsImpactModel() { NewsImpactDesc = "Nulo" },
                        new NewsImpactModel() { NewsImpactDesc = "Bajo" },
                        new NewsImpactModel() { NewsImpactDesc = "Medio" },
                        new NewsImpactModel() { NewsImpactDesc = "Alto" }
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}