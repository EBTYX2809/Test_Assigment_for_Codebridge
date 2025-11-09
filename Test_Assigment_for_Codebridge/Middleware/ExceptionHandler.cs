using System.Net;
using System.Text.Json;

namespace Test_Assigment_for_Codebridge.Middleware;

public class ExceptionsHandler
{
    private readonly RequestDelegate _next;
    public ExceptionsHandler(RequestDelegate next)
    {
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
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";

        var response = httpContext.Response;
        var errorDetails = new ErrorResponse();

        switch (exception)
        {
            case ArgumentNullException argumentNullException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorDetails = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = argumentNullException.Message
                };
                break;

            case ArgumentException argumentException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorDetails = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = argumentException.Message
                };
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                errorDetails = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = "Unknown error."
                };
                break;
        }

        var result = JsonSerializer.Serialize(errorDetails);
        return httpContext.Response.WriteAsync(result);
    }

}
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
}
