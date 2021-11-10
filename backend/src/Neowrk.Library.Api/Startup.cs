using System;
using System.Security.Cryptography;
using Autofac;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Neowrk.Library.Api.Security;
using Neowrk.Library.Infra.IoC;
using Neowrk.Library.Service.Validators;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Neowrk.Library.Api
{
    public class Startup
    {
        public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
        {
            public void Configure(string name, JwtBearerOptions options)
            {
                RSA rsa = RSA.Create();
                rsa.ImportRSAPublicKey(Convert.FromBase64String(""), out _);
            }

            public void Configure(JwtBearerOptions options)
            {
                throw new System.NotImplementedException();
            }
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddTransient<IDbConnection>((sp) => new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers()
                .AddFluentValidation(p =>
                {
                    p.RegisterValidatorsFromAssemblyContaining<BorrowBookDtoValidator>();
                    p.ValidatorOptions.LanguageManager.Culture = new CultureInfo("pt-BR");
                });

            services.AddCors(setup =>
            {
                setup.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:8080")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetIsOriginAllowedToAllowWildcardSubdomains();
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(o =>
                {
                    o.SecurityTokenValidators.Clear();
                    o.SecurityTokenValidators.Add(new GoogleTokenValidator());
                });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting()
                .UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
