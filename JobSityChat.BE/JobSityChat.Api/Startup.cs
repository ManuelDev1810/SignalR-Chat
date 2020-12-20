using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobSityChat.Api.Hubs;
using JobSityChat.Api.MBQueues;
using JobSityChat.Core.Handlers.Interfaces;
using JobSityChat.Core.MBQueues;
using JobSityChat.Infrastructure.Services.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            //SignalR
            services.AddSignalR();

            //Cors
            services.AddCors(options =>
            {
                options.AddPolicy("JobSityUI",
                   builder => builder.WithOrigins("https://localhost:5001")
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

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

            //Dependy Injections
            services.AddScoped<ICommandHandler, CommandHandler>();
            services.AddSingleton<IStockQueueProducer, StockQueueProducer>();
            services.AddHostedService<StockQueueConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
            app.UseCors("JobSityUI");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JobSityChat.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/jobsity_chathub");
            });
        }
    }
}
