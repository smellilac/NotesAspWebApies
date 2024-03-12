using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Notes.WebApi.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public abstract class BaseController : Controller
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    //protected IMediator Mediator
    //{
    //    get
    //    {
    //        if (_mediator == null)
    //        {
    //            _mediator = HttpContext.RequestServices.GetService<IMediator>();
    //        }

    //        return _mediator;
    //    }
    //}
    internal Guid UserId => !User.Identity.IsAuthenticated
        ? Guid.Empty
        : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
}
