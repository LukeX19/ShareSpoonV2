namespace ShareSpoon.Api.Middlewares
{
    public class TimingMiddleware
    {
        private readonly ILogger<TimingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public TimingMiddleware(ILogger<TimingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var start = DateTime.UtcNow;
            await _next.Invoke(httpContext);
            _logger.LogInformation($"Request \"{httpContext.Request.Path}\": {(DateTime.UtcNow - start).TotalMilliseconds} ms");
        }
    }
}
