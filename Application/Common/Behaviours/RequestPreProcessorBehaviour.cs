using Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class RequestPreProcessorBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly HttpContext _httpContext;
        private readonly IApplicationDbContext _dbContext;
        public RequestPreProcessorBehaviour(IHttpContextAccessor httpContextAccessor, IApplicationDbContext dbContext)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _dbContext = dbContext;
        }
        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {

            PropertyInfo dbContext = typeof(TRequest).GetProperty("Context", BindingFlags.NonPublic | BindingFlags.Instance);

            if (dbContext == null)
                throw new NullReferenceException();

            dbContext.SetValue(request, _dbContext);

            //HttpContext setting
            var httpcontext = typeof(TRequest).GetProperty("HttpContext", BindingFlags.NonPublic | BindingFlags.Instance);

            if (httpcontext == null)
                throw new NullReferenceException();

            httpcontext.SetValue(request, _httpContext);

            //Start Transaction
            await _dbContext.BeginTransactionAsync();
        }
    }
}
