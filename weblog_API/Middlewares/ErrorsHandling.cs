namespace weblog_API.Middlewares;

public class CustomException : Exception
{
    public int ErrorCode { get; set; }

    public CustomException(string message, int errorCode):base(message)
    {
        ErrorCode = errorCode;
    }
}

public class ErrorsHandling
{
    private readonly RequestDelegate _next;

    public ErrorsHandling(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }   
        catch (CustomException e)
        {
            context.Response.StatusCode = e.ErrorCode;
            await context.Response.WriteAsJsonAsync(new {message = e.Message});
        }
    }
}