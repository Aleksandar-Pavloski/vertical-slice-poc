using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VerticalSlice.POC.Core;
using VerticalSlice.POC.DataAccess.Entities;

namespace VerticalSlice.POC.Services.Features.Users
{
    public class Login
    {
        public class Request : BaseRequest<Response>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Response : AuthenticationResponse
        {
        }

        public class Validator : BaseValidator<Request>
        {
            public override IValidator Setup()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("EmailAddressRequired");
                RuleFor(x => x.Password).NotEmpty().MinimumLength(8);

                return this;
            }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await GetUser(request);

                if (user == null)
                {
                    throw new CoreException("Invalid Credentials");
                }

                return new Response
                {
                    UserId = user.Id,
                    Username = user.Email
                };
            }

            private async Task<User> GetUser(Request request)
            {
                return await _ef.Users.FirstOrDefaultAsync(x => x.Email == request.Email
                                                     && x.Password == request.Password);
            }
        }
    }
}
