using Catalog.CrossCutting.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using FluentValidation;
using Catalog.Domain.Exceptions;

namespace Catalog.CrossCutting.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                var statusCode = ex switch
                {
                    GlobalException apiEx => (int)apiEx.StatusCode,
                    ValidationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                object response = ex switch
                {
                    GlobalException apiEx => new
                    {
                        status = (int)apiEx.StatusCode,
                        message = apiEx.Message,
                        details = apiEx.Details
                    },
                    ValidationException validationEx => new
                    {
                        status = StatusCodes.Status400BadRequest,
                        message = "Validation failed",
                        errors = validationEx.Errors
                            .GroupBy(e => e.PropertyName)
                            .Select(g => new
                            {
                                field = g.Key,
                                messages = g.Select(x => x.ErrorMessage).ToList()
                            })
                    },
                    DomainException domainEx => new
                    {
                        statusCode = StatusCodes.Status400BadRequest,
                        message = domainEx.Message
                    },
                    _ => new
                    {
                        status = StatusCodes.Status500InternalServerError,
                        message = "An unexpected error occurred"
                    }
                };

                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}