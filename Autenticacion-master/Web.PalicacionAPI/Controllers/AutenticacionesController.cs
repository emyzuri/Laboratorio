using Core.Aplicacion.Funciones.Comandos.Cliente;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Web.PalicacionAPI.Controllers
{
    public class AutenticacionesController : BaseApiController
    {
        /// <summary>
        /// Instacia de servicio mediador
        /// </summary>
        readonly IMediator mediador;

        public AutenticacionesController(IMediator mediador)
        {
            this.mediador = mediador;   
        }
        //protected IMediator Mediador => mediador ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpPost("~/connect/token"), Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Autenticar()
        {
            OpenIddictRequest peticion = HttpContext.GetOpenIddictServerRequest() ?? throw new InvalidOperationException("Error en la configuración endpoint identidad");
            if (peticion.IsPasswordGrantType())
            {
                ValidarClienteCom usuario = new(peticion.Password, peticion.Username);
                var cliente = await mediador.Send(usuario);
                ClaimsIdentity claimsIdentity = new(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                claimsIdentity.AddClaim(OpenIddictConstants.Claims.Subject, OpenIddictConstants.Destinations.AccessToken);
                ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
                claimsPrincipal.SetScopes(peticion.GetScopes());
                claimsIdentity.SetDestinations(Direccion);
                return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            else
            {
                throw new InvalidOperationException("Autenticación no especificada");
            }
        }

        /// <summary>
        /// Dirección de la respuesta
        /// </summary>
        /// <param name="claim">Claims de la petición</param>
        /// <returns>Acceso</returns>
        private static IEnumerable<string> Direccion(Claim claim)
        {
            yield return Destinations.AccessToken;
        }
    }


}
