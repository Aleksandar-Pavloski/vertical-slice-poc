using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VerticalSlice.POC.Services.Features.Users
{
    public class GetAllUsers
    {
        public class Request : BaseRequest<Response>
        {
        }

        public class Response : BaseResponse
        {
            public IEnumerable<Item> Items { get; set; }

            public class Item
            {
                public Guid Id { get; set; }
                public string Username { get; set; }
                public string Email { get; set; }
            }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                return new Response
                {
                    Items = GetUser()
                };
            }

            private IEnumerable<Response.Item> GetUser()
            {
                return _ef.Users.Select(x => new Response.Item
                {
                    Email = x.Email,
                    Id = x.Id,
                    Username = x.Username
                });
            }
        }
    }
}
