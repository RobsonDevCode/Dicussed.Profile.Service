using System.ComponentModel.DataAnnotations;
using System.Net;
using KeyNotFoundException = System.Collections.Generic.KeyNotFoundException;

namespace Discussed.Profile.Api.Handlers.GraphQL;

public class GraphQLErrorFilter : IErrorFilter
{
    private readonly ILogger<GraphQLErrorFilter> _logger;

    public GraphQLErrorFilter(ILogger<GraphQLErrorFilter> logger)
    {
        _logger = logger;
    }

    public IError OnError(IError error)
    {
        _logger.LogError("Error occured: {error}, at path: {path}", error.Message, error.Path);

        var statusCode = HttpStatusCode.InternalServerError;
        var message = "An unhandled error has occurred please try again later!";

        if (error.Exception != null)
        {
            switch (error.Exception)
            {
                case ArgumentException or ArgumentNullException or ValidationException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = error.Message;
                    break;

                case OperationCanceledException or TaskCanceledException:
                    statusCode = HttpStatusCode.RequestTimeout;
                    message = error.Message;

                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "User not authorized to perform this operation!";
                    break;

                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = error.Message;
                    break;
            }
        }
        //Hot chocolate based error codes
        else
        {
            switch (error.Code)
            {
                case "INVALID_ARGUMENT":
                case "VALIDATION_FAILED":
                    statusCode = HttpStatusCode.BadRequest;
                    message = error.Message;
                    break;

                case "REQUEST_TIMEOUT":
                    statusCode = HttpStatusCode.RequestTimeout;
                    message = "request timeout has occurred, please try again later!";
                    break;

                case "UNAUTHORIZED" or "AUTH_NOT_AUTHENTICATED":
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "User not authorized to perform this operation!";
                    break;

                case "NOT_FOUND":
                    statusCode = HttpStatusCode.NotFound;
                    message = error.Message;
                    break;
            }
        }

        return error.WithMessage(message)
            .WithCode(error.Code)
            .SetExtension("statusCode", statusCode)
            .SetExtension("stackTrace", null)
            .WithException(null); //Hide important call stack information
    }
}