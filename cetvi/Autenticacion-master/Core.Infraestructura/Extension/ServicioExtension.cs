using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Core.Infraestructura.Extension
{
    public static class ServicioExtension
    {
        public static IServiceCollection AgregarServicioInfraestructura(this IServiceCollection services, IConfiguration configuration)
        {
            //GenerarCer();
            //GenerarSign();
            services.AddDbContext<DbContext>(options =>
            {
                options.UseInMemoryDatabase(nameof(DbContext));
                options.UseOpenIddict();
            });

            string sitio = configuration["Sitio"];
            
            services.AddOpenIddict().AddCore(options =>
            {
                options.UseEntityFrameworkCore().UseDbContext<DbContext>();
            })
                .AddClient(options =>
            {
                options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();
            })
            .AddServer(options =>
            {
                options.AllowPasswordFlow();
                options.SetIssuer(new Uri(configuration["Issuer"]));
                options.SetTokenEndpointUris(sitio + "/connect/token");
                options.SetCryptographyEndpointUris(sitio + "/.well.known/jwks");
                options.SetConfigurationEndpointUris(sitio + "/.well-known/openid-configuration");
                options.AddEncryptionKey(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["EncryptionKey"])));
                //options.AddEncryptionCertificate(LoadCertificate("encryption-certificate"));
                //options.AddSigningCertificate(LoadCertificate("signing-certificate"));
                options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();
                options.RegisterScopes("api");
                options.SetAccessTokenLifetime(TimeSpan.FromSeconds(30));
                options.UseAspNetCore().EnableTokenEndpointPassthrough();
                options.UseAspNetCore().DisableTransportSecurityRequirement();
            });

            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddHealthChecks();
            services.AddControllers();

            return services;
        }

        private static X509Certificate2 LoadCertificate(string thumbprint)
        {
            var bytes = File.ReadAllBytes($"\\ApiComun\\Core.Autenticacio\\Web.PalicacionAPI\\{thumbprint}.cer");
            return new X509Certificate2(bytes);
        }
        private  static void GenerarCer()
        {
            using var algorithm = RSA.Create(keySizeInBits: 2048);

            var subject = new X500DistinguishedName("CN=Fabrikam Server Encryption Certificate");
            var request = new CertificateRequest(subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyEncipherment, critical: true));

            var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));

            File.WriteAllBytes("server-encryption-certificate.pfx", certificate.Export(X509ContentType.Pfx, string.Empty));
        }

        private static void GenerarSign()
        {
            using var algorithm = RSA.Create(keySizeInBits: 2048);

            var subject = new X500DistinguishedName("CN=Fabrikam Server Signing Certificate");
            var request = new CertificateRequest(subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, critical: true));

            var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));

            File.WriteAllBytes("server-signing-certificate.pfx", certificate.Export(X509ContentType.Pfx, string.Empty));
        }
    }
}
