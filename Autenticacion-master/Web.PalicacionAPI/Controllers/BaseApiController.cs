using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Web.PalicacionAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator mediador;
        protected IMediator Mediador => mediador ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
