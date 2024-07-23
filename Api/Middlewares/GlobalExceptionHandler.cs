using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementSystem.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await TryHandleAsync(httpContext, ex, httpContext.RequestAborted);
            }
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails();
            problemDetails.Instance = httpContext.Request.Path;

            if (exception is FluentValidation.ValidationException fluentException)
            {
                problemDetails.Title = "one or more validation errors occurred.";
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                var validationErrors = new Dictionary<string, string[]>();
                foreach (var error in fluentException.Errors)
                {
                    if (!validationErrors.ContainsKey(error.PropertyName))
                    {
                        validationErrors[error.PropertyName] = new[] { error.ErrorMessage };
                    }
                    else
                    {
                        var errors = validationErrors[error.PropertyName].ToList();
                        errors.Add(error.ErrorMessage);
                        validationErrors[error.PropertyName] = errors.ToArray();
                    }
                }
                problemDetails.Extensions.Add("errors", validationErrors);
            }

            else if(exception is BaseException e)
            {
                httpContext.Response.StatusCode = (int)e.StatusCode;
                problemDetails.Title = e.Message;
            }

            else
            {
                problemDetails.Title = exception.Message;
            }

            _logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);

            problemDetails.Status = httpContext.Response.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
            return true;
        }
    }
}
