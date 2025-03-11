using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenIddict.Server;
using OpenIdDictPractice.Configure;
using System.Security.Cryptography;

namespace OpenIdDictPractice
{
    /// <summary>
    /// Responsible for configuring the necessary elements of the program
    /// </summary>
    public class Startup
    {
        public IConfiguration Configuration { get; }
        /// <summary>
        /// default constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Service configuration.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDatabase");
                options.UseOpenIddict();
            });


            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                           .UseDbContext<ApplicationDbContext>();
                })
                .AddServer(options =>
                {
                    options.SetAuthorizationEndpointUris("/connect/authorize")
                   .SetTokenEndpointUris("/connect/token")
                   .DisableAccessTokenEncryption()
                   .AcceptAnonymousClients()
                   .AddEphemeralEncryptionKey()
                   .AllowPasswordFlow()
                   .AllowRefreshTokenFlow()
                   .UseAspNetCore()
                   .EnableTokenEndpointPassthrough();

                    // Configurar tiempo de vida del token
                    options.SetAccessTokenLifetime(TimeSpan.FromMinutes(15));

                    // Agregar una clave simétrica para firmar tokens
                    options.AddSigningKey(new SymmetricSecurityKey(Convert.FromBase64String("YmFzZTY0X2tleV9leGFtcGxlXzEyMzQ1Njc4OTAxMjM0NTY3ODkwMTIzNA==")));

                    // Agregar un certificado de desarrollo para cumplir con el requisito de la clave asimétrica
                    options.AddDevelopmentSigningCertificate();
                });
            services.AddControllersWithViews();
        }

        /// <summary>
        /// Application configuration.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/></param>
        /// <param name="env"><see cref="IWebHostEnvironment"/></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty; // Para que Swagger UI esté en la raíz
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
