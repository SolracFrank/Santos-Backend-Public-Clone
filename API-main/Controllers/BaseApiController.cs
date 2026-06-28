namespace EsApi.Controllers;

using Domain.Exceptions;
using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
[Authorize]
[EnableRateLimiting("DefaultPolicy")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected BaseApiController(IMediator mediator)
    {
        Mediator = mediator;
    }

    protected IMediator Mediator { get; }

    protected async Task<IActionResult> SendAsync<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var response = await Mediator.Send(request, cancellationToken);

        if (TryMapResultResponse(response, out var resultAction))
        {
            return resultAction;
        }

        return response switch
        {
            null => NotFound(ToProblemDetails(
                StatusCodes.Status404NotFound,
                "Resource not found.",
                "The requested resource was not found.")),
            Result<TResponse> result => FromResult(result),
            _ => Ok(response)
        };
    }

    protected async Task<IActionResult> SendResultAsync<TResponse>(
        IRequest<Result<TResponse>> request,
        CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return FromResult(result);
    }

    protected async Task<IActionResult> SendNoContentResultAsync<TResponse>(
        IRequest<Result<TResponse>> request,
        CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return ResultNoContent(result);
    }

    protected async Task<IActionResult> SendNoContentAsync(
        IRequest request,
        CancellationToken cancellationToken = default)
    {
        await Mediator.Send(request, cancellationToken);
        return NoContent();
    }

    protected async Task<IActionResult> SendNoContentAsync(
        IRequest<Unit> request,
        CancellationToken cancellationToken = default)
    {
        await Mediator.Send(request, cancellationToken);
        return NoContent();
    }

    protected async Task<IActionResult> SendOkAsync(
        IRequest<Unit> request,
        CancellationToken cancellationToken = default)
    {
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    protected async Task<IActionResult> SendOkAsync(
        IRequest<Result<Unit>> request,
        CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return result.Match(
            Succ: _ => Ok(),
            Fail: MapFailure);
    }

    protected async Task<IActionResult> SendOkAsync(
        IRequest request,
        CancellationToken cancellationToken = default)
    {
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    protected IActionResult FromResult<T>(Result<T> result)
    {
        return result.Match(
            Succ: value => Ok(value),
            Fail: MapFailure);
    }
    protected IActionResult ResultNoContent<T>(Result<T> result)
    {
        return result.Match(
            Succ: _ => NoContent(),
            Fail: MapFailure);
    }

    private IActionResult MapFailure(Exception ex)
    {
        return ex switch
        {
            ApiException apiException => StatusCode(apiException.StatusCode, ToProblemDetails(
                apiException.StatusCode,
                apiException.Title,
                apiException.Message,
                apiException.ErrorCode)),

            FluentValidation.ValidationException validationException => BadRequest(ToProblemDetails(
                StatusCodes.Status400BadRequest,
                "Validation failed.",
                validationException.Message)),

            KeyNotFoundException notFoundException => NotFound(ToProblemDetails(
                StatusCodes.Status404NotFound,
                "Resource not found.",
                notFoundException.Message)),

            InvalidOperationException conflictException => Conflict(ToProblemDetails(
                StatusCodes.Status409Conflict,
                "Conflict.",
                conflictException.Message)),

            _ => StatusCode(StatusCodes.Status500InternalServerError, ToProblemDetails(
                StatusCodes.Status500InternalServerError,
                "Internal server error.",
                "An unexpected error occurred."))
        };
    }

    private bool TryMapResultResponse<TResponse>(TResponse response, out IActionResult actionResult)
    {
        actionResult = default!;

        if (response is null)
        {
            return false;
        }

        var responseType = response.GetType();
        if (!responseType.IsGenericType || responseType.GetGenericTypeDefinition() != typeof(Result<>))
        {
            return false;
        }

        var valueType = responseType.GetGenericArguments()[0];
        var method = GetType()
            .GetMethod(nameof(FromResult), BindingFlags.Instance | BindingFlags.NonPublic)!
            .MakeGenericMethod(valueType);

        actionResult = (IActionResult)method.Invoke(this, [response])!;
        return true;
    }

    private static ProblemDetails ToProblemDetails(int statusCode, string title, string detail, string? errorCode = null)
    {
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Status = statusCode,
            Detail = detail
        };

        if (!string.IsNullOrWhiteSpace(errorCode))
        {
            problemDetails.Extensions["errorCode"] = errorCode;
        }

        return problemDetails;
    }
}
