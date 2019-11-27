using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using VerticalSlice.POC.Core;
using VerticalSlice.POC.DataAccess.Entities;

namespace VerticalSlice.POC.Services.Features.Users
{
    public class Register
    {
        public class Request : BaseRequest<Response>
        {
            public string Username { get; set; }
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
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
                RuleFor(x => x.Username).NotEmpty();

                return this;
            }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (await CheckIfExist(request))
                {
                    throw new CoreException("Username already exists");
                }

                var userId = Guid.NewGuid();
                AddUser(userId, request);

                return new Response
                {
                    UserId = userId,
                    Username = request.Email
                };
            }

            private async Task<bool> CheckIfExist(Request request)
            {
                return await _ef.Users.Where(x => x.Email == request.Email).AnyAsync();
            }

            private void AddUser(Guid userId, Request request)
            {
                Add(new User
                {
                    Email = request.Email,
                    Password = request.Password,
                    Id = userId,
                    Username = request.Username
                });
            }
        }
    }
}
