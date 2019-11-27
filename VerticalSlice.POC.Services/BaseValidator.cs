using FluentValidation;
using MediatR;
using System.Runtime.CompilerServices;

namespace VerticalSlice.POC.Services
{
    public abstract class BaseValidator<TRequest> : AbstractValidator<TRequest>
        where TRequest : IRequest<BaseResponse>
    {
        public abstract IValidator Setup();
    }
}
