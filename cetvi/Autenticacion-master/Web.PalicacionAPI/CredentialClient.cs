using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Web.PalicacionAPI
{
    public class CredentialClient : IHostedService
    {
        readonly IConfiguration configuration;

        readonly IServiceProvider serviceProvider;

        public CredentialClient(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            this.configuration = configuration;
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope serviceScope = serviceProvider.CreateScope();
            DbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<DbContext>();
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
            IOpenIddictApplicationManager manager = serviceScope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            if(await manager.FindByClientIdAsync(configuration["ClientId"], cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = configuration["ClientId"],
                    ClientSecret = configuration["ClientSecret"],
                    Permissions = { 
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.Prefixes.Scope + "api"
                    }
                }, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
