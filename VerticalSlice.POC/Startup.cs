using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using System;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Features;
using VerticalSlice.POC.DataAccess;
using VerticalSlice.POC.Infrastructure.Mediator;
using VerticalSlice.POC.Infrastructure.MiddlewareFilters;
using VerticalSlice.POC.Infrastructure.Services;

namespace VerticalSlice.POC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("VerticalSlicePOC"))
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "VerticalSlice POC API", Version = "v1" });
                c.CustomSchemaIds((t) => t.FullName);
                c.DescribeAllEnumsAsStrings();
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                c.AddSecurityRequirement(security);
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services
                .AddMvc()
                .AddFeatureFolders()
                .AddControllersAsServices()
                .AddJsonOptions(opts =>
                {
                    // Force Camel Case to JSON
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                }); ;
            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<VerticalSliceDbContext>((options) => options.UseSqlServer(Configuration.GetConnectionString("VerticalSlicePOCDbConnection")));

            var container = new Container(c =>
            {
                c.AddRegistry(new MediatorPipelineRegistry());
                c.AddRegistry(new ServicesRegistry());
                c.For<IConfiguration>().Use(Configuration);
                c.For<IHttpContextAccessor>().Use<HttpContextAccessor>();
                c.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.EnableValidator();
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "VerticalSlice POC API");
            });
            app.UseAuthentication();

            app.UseMiddleware<ErrorHandlingFilter>();
            app.UseMvc();
        }
    }
}
