using Application.Common.Base.Models;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Application.Common.Base
{
    public class BaseRequest<TResponse> : IRequest<CQRSResponse<TResponse>>
    {
        Dictionary<string, string> _errors;
        public Dictionary<string, string> Errors
        {
            get
            {
                return _errors ??= new Dictionary<string, string>();
            }
            set
            {
                if (value != null) _errors = value;
            }
        }

        protected IApplicationDbContext Context { get; set; }
        protected HttpContext HttpContext { get; set; }
    }
}
