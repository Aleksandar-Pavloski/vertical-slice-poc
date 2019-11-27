using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VerticalSlice.POC.Core;
using VerticalSlice.POC.DataAccess.Entities;

namespace VerticalSlice.POC.Services.Features.Users
{
    public class GetUserById
    {
        public class Request : BaseRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Validator : BaseValidator<Request>
        {
            public override IValidator Setup()
            {
                RuleFor(x => x.Id).NotEmpty();

                return this;
            }
        }

        public class Response : BaseResponse
        {
            public Guid Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await GetUser(request.Id);

                if (user == null)
                {
                    throw new CoreException("User cannot be found");
                }

                return new Response
                {
                    Email = user.Email,
                    Id = user.Id,
                    Username = user.Email
                };
            }

            private async Task<User> GetUser(Guid id)
            {
                return await _ef.Users.FirstOrDefaultAsync(x => x.Id == id);
            }
        }
    }
}
