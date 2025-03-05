using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenIddict.Server;
using OpenIdDictPractice.Configure;
using System.Security.Cryptography;

namespace OpenIdDictPractice
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

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

                    // Agregar una clave simétrica para firmar tokens
                    options.AddSigningKey(new SymmetricSecurityKey(Convert.FromBase64String("YmFzZTY0X2tleV9leGFtcGxlXzEyMzQ1Njc4OTAxMjM0NTY3ODkwMTIzNA==")));

                    // Agregar un certificado de desarrollo para cumplir con el requisito de la clave asimétrica
                    options.AddDevelopmentSigningCertificate();
                });
            services.AddControllersWithViews();
        }

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
