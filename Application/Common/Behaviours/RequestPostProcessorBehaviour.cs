using Application.Common.Interfaces;
using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class RequestPostProcessorBehaviour<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {
        private readonly IApplicationDbContext _context;
        public RequestPostProcessorBehaviour(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            await _context.CommitAsync();
        }
    }
}