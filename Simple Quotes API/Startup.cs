using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Simple_Quotes_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json.Serialization;
using Simple_Quotes_API.Middleware;

namespace Simple_Quotes_API
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
            string connectionString = Configuration.GetConnectionString("QuotesCS");

            services.AddDbContext<QuotesDbContext>(options => 
                options.UseSqlServer(connectionString));

            services.AddMvc()
                    .AddNewtonsoftJson(o => {
                        o.SerializerSettings.ReferenceLoopHandling =
                            Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                        o.SerializerSettings.ContractResolver = 
                            new CamelCasePropertyNamesContractResolver();
                    });

            /*services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Simple_Quotes_API", Version = "v1" });
            });*/

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    //open, not safe
                    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IAuthorRepo, AuthorRepo>();
            services.AddScoped<IQuoteRepo, QuoteRepo>();

            services.Configure<ApiBehaviorOptions>(apiBehaviorOptions => 
                apiBehaviorOptions.InvalidModelStateResponseFactory = actionContext => {
                    return new BadRequestObjectResult(new
                    {
                        Messages = actionContext.ModelState.Values.SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage)
                    });
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<LogHandler>();
            app.UseMiddleware<ExceptionHandler>();

            /*if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple_Quotes_API v1"));
            }*/

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
