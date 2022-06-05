using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SGPB.Web.Data;
using SGPB.Web.Data.Entities;
using SGPB.Web.Helpers;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Azure;
using Azure.Storage.Queues;
using Azure.Storage.Blobs;
using Azure.Core.Extensions;
using System;

namespace SGPB.Web
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
                        services.AddIdentity<User, IdentityRole>(cfg =>
                        {
                                cfg.User.RequireUniqueEmail = true;
                                cfg.Password.RequireDigit = false;
                                cfg.Password.RequiredUniqueChars = 0;
                                cfg.Password.RequireLowercase = false;
                                cfg.Password.RequireNonAlphanumeric = false;
                                cfg.Password.RequireUppercase = false;
                        }).AddEntityFrameworkStores<ApplicationDbContext>();

                        services.AddAuthentication()
                            .AddCookie()
                            .AddJwtBearer(cfg =>
                            {
                                    cfg.TokenValidationParameters = new TokenValidationParameters
                                    {
                                            ValidIssuer = Configuration["Tokens:Issuer"],
                                            ValidAudience = Configuration["Tokens:Audience"],
                                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                                    };
                            });

                        services.ConfigureApplicationCookie(options =>
                        {
                                options.LoginPath = "/Account/NotAuthorized";
                                options.AccessDeniedPath = "/Account/NotAuthorized";
                        });

                        //Ignorar las referencias circulares
                        services.AddControllers().AddJsonOptions(x =>
                                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

                        services.AddDbContext<ApplicationDbContext>(cfg =>
                        {
                                cfg.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                        });
                        services.AddScoped<IBlobHelper, BlobHelper>();
                        services.AddScoped<IConverterHelper, ConverterHelper>();
                        services.AddScoped<ICombosHelper, CombosHelper>();
                        services.AddTransient<SeedDb>();
                        services.AddScoped<IUserHelper, UserHelper>();
                        services.AddControllersWithViews();
                        services.AddAzureClients(builder =>
                        {
                                builder.AddBlobServiceClient(Configuration["Blob:ConnectionString:blob"], preferMsi: true);
                                builder.AddQueueServiceClient(Configuration["Blob:ConnectionString:queue"], preferMsi: true);
                        });
                }

                // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
                public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
                {
                        if (env.IsDevelopment())
                        {
                                //app.UseStatusCodePagesWithReExecute("/error/{0}");
                                app.UseDeveloperExceptionPage();
                        }
                        else
                        {
                                //app.UseExceptionHandler("/Home/Error");
                                app.UseStatusCodePagesWithReExecute("/error/{0}");
                                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                                app.UseHsts();
                        }
                        app.UseHttpsRedirection();
                        app.UseStaticFiles();
                        app.UseAuthentication();
                        app.UseRouting();
                        app.UseStatusCodePagesWithReExecute("/error/{0}");
                        app.UseAuthorization();

                        app.UseEndpoints(endpoints =>
                        {
                                endpoints.MapControllerRoute(
                                name: "default",
                                pattern: "{controller=Home}/{action=Index}/{id?}");
                        });
                }
        }
        internal static class StartupExtensions
        {
                public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
                {
                        if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
                        {
                                return builder.AddBlobServiceClient(serviceUri);
                        }
                        else
                        {
                                return builder.AddBlobServiceClient(serviceUriOrConnectionString);
                        }
                }
                public static IAzureClientBuilder<QueueServiceClient, QueueClientOptions> AddQueueServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
                {
                        if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
                        {
                                return builder.AddQueueServiceClient(serviceUri);
                        }
                        else
                        {
                                return builder.AddQueueServiceClient(serviceUriOrConnectionString);
                        }
                }
        }
}
