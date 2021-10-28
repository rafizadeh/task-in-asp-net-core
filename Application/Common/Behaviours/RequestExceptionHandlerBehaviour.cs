using Application.Common.Interfaces;
using MediatR.Pipeline;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class RequestExceptionHandlerBehaviour<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException> where TException : Exception
    {
        private readonly IApplicationDbContext _context;
        public RequestExceptionHandlerBehaviour(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
        {
            if (!_context.IsCurrentTransactionNull())
            {
                await _context.RoleBackAsync();
            }
            state.SetHandled(state.Response);
        }
    }
}