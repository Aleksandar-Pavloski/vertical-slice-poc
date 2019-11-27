using MediatR;

namespace VerticalSlice.POC.Services
{
    public abstract class BaseRequest<TResponse> : IRequest<TResponse>
        where TResponse : BaseResponse
    {
    }
}
