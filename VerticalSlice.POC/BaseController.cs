using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VerticalSlice.POC.Services;

namespace VerticalSlice.POC
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController : Controller
    {
        private IMediator _mediator;

        [StructureMap.Attributes.SetterProperty]
        public IMediator Mediator
        {
            set
            {
                if (_mediator != null)
                    throw new InvalidOperationException("Cannot change Mediator property! It should be dependency resolved!");
                else
                    _mediator = value;
            }
        }

        protected async Task<TResponse> Handle<TResponse>(BaseRequest<TResponse> request)
            where TResponse : BaseResponse
        {
            return await _mediator.Send(request);
        }
    }
}
