using FluentValidation;
using MediatR;

namespace VerticalSlice.POC.Services
{
    public abstract class BaseValidator<TRequest> : AbstractValidator<TRequest>
        where TRequest : IRequest<BaseResponse>
    {
        public abstract IValidator Setup();
    }
}
