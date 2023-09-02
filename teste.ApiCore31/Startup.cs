using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using teste.ApiCore31.Constatns;
using teste.ApiCore31.Helpers;
using teste.ApiCore31.Interfaces;

namespace teste.ApiCore31
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
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {   
                c.EnableAnnotations();
                c.SwaggerDoc(Parameters.ApiCurrentVersion, new OpenApiInfo { 
                    Title = Parameters.ApiName, 
                    Version = Parameters.ApiCurrentVersion,
                    Description = Parameters.ApiDescription,
                    Contact = new OpenApiContact{
                        Email = "m.soares.viterbo@avanade.com",
                        Name = "Marcio Viterbo"
                    } 
                });
                // c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // c.IncludeXmlComments(xmlPath);
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: Parameters.AllowedSpecificOrigins,
                    policy =>
                    {
                        policy.WithOrigins(Parameters.Origins);
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            
            app.UseSwaggerUI(c => {
                var virtualDirectory = Configuration.GetSection("VirtualDirectory").Value;
                c.SwaggerEndpoint(
                        env.IsDevelopment() 
                            ? $"/swagger/{Parameters.ApiCurrentVersion}/swagger.json" 
                            : $"{virtualDirectory}/swagger/{Parameters.ApiCurrentVersion}/swagger.json",
                        $"{Parameters.ApiName} {Parameters.ApiCurrentVersion}"
                );
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(Parameters.AllowedSpecificOrigins);

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}