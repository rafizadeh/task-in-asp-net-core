using Application.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class BaseController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.GetService<IMediator>();
    }
}