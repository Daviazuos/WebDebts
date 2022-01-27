using MicroServices.WebDebts.Application.Models.AuthModels;
using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MicroServices.WebDebts.Api.Controllers
{
    [Route("User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserApplicationService _authApplicationService;

        public UserController(IUserApplicationService authApplicationService)
        {
            _authApplicationService = authApplicationService;
        }

        [HttpPost, Route("Create")]
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> CreateUserAsync([FromBody] UserAppModel userAppModel)
        {
            await _authApplicationService.CreateUser(userAppModel);

            return new OkResult();
        }

        [HttpPost, Route("Login")]
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginAppModelReponse>> Login([FromBody] LoginAppModelRequest loginAppModel)
        {
            LoginAppModelReponse login = await _authApplicationService.Login(loginAppModel);

            return new OkObjectResult(login);
        }
    }
}
