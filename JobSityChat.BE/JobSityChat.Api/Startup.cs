using System.Linq;
using System.Threading.Tasks;
using JobSityChat.Api.Hubs;
using JobSityChat.Api.MBQueues;
using JobSityChat.Core.Handlers.Interfaces;
using JobSityChat.Core.MBQueues;
using JobSityChat.Core.Repository.Interfaces;
using JobSityChat.Infrastructure.Persistent;
using JobSityChat.Infrastructure.Services.Handlers;
using JobSityChat.Infrastructure.Services.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace JobSityChat.Api
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
            //Jwt Authentication for SignalR
            services.AddAuthentication(options =>
            {
                // Identity made Cookie authentication the default.
                // We change it to JWT Bearer Auth
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                //Hooking the OnMessageReceived event to allow JWT handler the access token from query string
                //We restrict the access to only calls from our hub
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is from our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.Value.Contains(Configuration["SignalR:hub"])))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            //SignalR
            services.AddSignalR();

            //Controllers
            services.AddControllers();

            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JobSityChat.Api", Version = "v1" });
            });

            //ResponseCompression
            services.AddResponseCompression(opts => {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/oncet-stream" });
            });

            //Preparing the connection string
            services.AddDbContext<JobsityChatDbContext>(opt => opt.UseSqlite(Configuration["ConnectionStrings:JobSityChat"],
                x => x.MigrationsAssembly("JobSityChat.Infrastructure.Migrations")));
            
            //Dependy Injections
            services.AddScoped<ICommandHandler, CommandHandler>();
            services.AddScoped<IUserMessageRepository, UserMessageRepository>();
            services.AddSingleton<IStockQueueProducer, StockQueueProducer>();
            services.AddHostedService<StockQueueConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JobSityChat.Api v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>(Configuration["SignalR:hub"]);
            });
        }
    }
}
