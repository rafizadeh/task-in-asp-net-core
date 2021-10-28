using Application.Common.Base.Models;
using MediatR;

namespace Application.Common.Base
{
    public interface IBaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, CQRSResponse<TResponse>> where TRequest : BaseRequest<TResponse> { }
}