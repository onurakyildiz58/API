using System.Net;

namespace WnT.API.Middleware
{
    public class ExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> logger;
        private readonly RequestDelegate requestDelegate;

        public ExceptionHandler(ILogger<ExceptionHandler> logger, RequestDelegate requestDelegate)
        {
            this.logger = logger;
            this.requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await requestDelegate(httpContext);
            }
            catch(Exception e)
            {
                var Id = Guid.NewGuid();

                string message = $"{Id} : {e.Message}";
                logger.LogError(e, message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = Id,
                    ErrorMessage = "bozuldu bu başka zaman gel",
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }


    }
}
