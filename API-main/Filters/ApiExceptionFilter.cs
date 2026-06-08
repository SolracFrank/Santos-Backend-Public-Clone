using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidationException = FluentValidation.ValidationException;

namespace EsApi.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ApiException apiException)
        {
            context.Result = new ObjectResult(ToProblemDetails(apiException))
            {
                StatusCode = apiException.StatusCode
            };

            context.ExceptionHandled = true;
            return;
        }

        if (context.Exception is FluentValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(e => e.ErrorMessage).ToArray());

            context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors)
            {
                Title = "Validation failed.",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validation errors occurred."
            });

            context.ExceptionHandled = true;
            return;
        }

        if (context.Exception is KeyNotFoundException notFoundException)
        {
            context.Result = new NotFoundObjectResult(new ProblemDetails
            {
                Title = "Resource not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = notFoundException.Message
            });

            context.ExceptionHandled = true;
            return;
        }

        if (context.Exception is UnauthorizedAccessException unauthorizedAccessException)
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Title = "Unauthorized.",
                Status = StatusCodes.Status403Forbidden,
                Detail = unauthorizedAccessException.Message
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };

            context.ExceptionHandled = true;
            return;
        }

        if (context.Exception is InvalidOperationException invalidOperationException)
        {
            context.Result = new ConflictObjectResult(new ProblemDetails
            {
                Title = "Conflict.",
                Status = StatusCodes.Status409Conflict,
                Detail = invalidOperationException.Message
            });

            context.ExceptionHandled = true;
        }
    }

    private static ProblemDetails ToProblemDetails(ApiException exception)
    {
        var problemDetails = new ProblemDetails
        {
            Title = exception.Title,
            Status = exception.StatusCode,
            Detail = exception.Message
        };

        if (!string.IsNullOrWhiteSpace(exception.ErrorCode))
        {
            problemDetails.Extensions["errorCode"] = exception.ErrorCode;
        }

        return problemDetails;
    }
}
