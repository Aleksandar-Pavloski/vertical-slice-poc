using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VerticalSlice.POC.Services.Features.Users;

namespace VerticalSlice.POC.Controllers
{
    [Route("api/v1/[controller]")]
    public class UsersController : BaseController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<Login.Response> Login([FromBody] Login.Request request)
            => await Handle(request);

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<Register.Response> Register([FromBody] Register.Request request)
            => await Handle(request);

        [HttpGet]
        [Route("get-all-users")]
        public async Task<GetAllUsers.Response> GetAllUsers()
            => await Handle(new GetAllUsers.Request());

        [HttpGet]
        [Route("get-user-by-id")]
        public async Task<GetUserById.Response> GetUserById(GetUserById.Request request)
            => await Handle(request);
    }
}
