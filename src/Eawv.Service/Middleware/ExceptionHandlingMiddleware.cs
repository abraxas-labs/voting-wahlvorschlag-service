// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;

namespace Eawv.Service.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private ProblemDetails
        SimpleProblemDetails(HttpStatusCode status, string title = null, string message = null) =>
        new ProblemDetails { Status = (int)status, Title = title, Detail = message };

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        ProblemDetails problem;

        switch (exception)
        {
            case IHttpStatusCodeException httpStatusCodeException:
                problem = SimpleProblemDetails(
                    httpStatusCodeException.GetHttpStatusCode(),
                    httpStatusCodeException.GetType().Name,
                    exception.Message);
                break;
            case DbUpdateException dbException:
                problem = GetProblemForDBException(dbException);
                break;
            default:
                problem = SimpleProblemDetails(HttpStatusCode.InternalServerError);
                break;
        }

        if (problem.Status == (int)HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception caused internal server error.");
        }
        else
        {
            _logger.LogWarning(exception, "Unhandled exception mapped to http status code.");
        }

        var result = JsonConvert.SerializeObject(problem);
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = problem.Status ?? (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync(result);
    }

    private ProblemDetails GetProblemForDBException(DbUpdateException exception)
    {
        switch (exception)
        {
            case DbUpdateConcurrencyException _:
                return SimpleProblemDetails(
                    HttpStatusCode.NotFound,
                    nameof(EntityNotFoundException),
                    $"Entity with id path {string.Join(", ", GetEntityIdsForDBException(exception))} not found");
        }

        switch ((exception.InnerException as PostgresException)?.SqlState
        )
        {
            // see https://www.postgresql.org/docs/current/static/errcodes-appendix.html
            case "23503": // foreign key violation
                return SimpleProblemDetails(HttpStatusCode.BadRequest, "Invalid reference");
            case "23502": // not null violation
                return SimpleProblemDetails(HttpStatusCode.BadRequest, "Required value is null");
            case "23505": // duplicate key
                return SimpleProblemDetails(HttpStatusCode.BadRequest, "Duplicate");
            case string code:
                return SimpleProblemDetails(HttpStatusCode.InternalServerError, code);
            default:
                return SimpleProblemDetails(HttpStatusCode.InternalServerError);
        }
    }

    private IEnumerable<T> GetEntitiesForDBException<T>(DbUpdateException ex)
    {
        return ex.Entries.Select(e => e.Entity)
            .OfType<T>();
    }

    private IEnumerable<Guid> GetEntityIdsForDBException(DbUpdateException ex)
    {
        return GetEntitiesForDBException<BaseEntity>(ex)
            .Select(e => e.Id);
    }
}
