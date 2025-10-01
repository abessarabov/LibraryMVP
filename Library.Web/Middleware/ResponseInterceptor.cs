namespace Library.Web.Middleware
{
    public class ResponseInterceptor
    {
        private const string CorrelationIdHeader = "X-Correlation-Id";
        private readonly RequestDelegate _next;

        public ResponseInterceptor(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<ResponseInterceptor> logger)
        {
            string correlationId;

            if (context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationHeader))
            {
                correlationId = correlationHeader!;
            }
            else
            {
                correlationId = Guid.NewGuid().ToString("N");
                context.Request.Headers.Append(CorrelationIdHeader, correlationId);
            }

            context.Response.Headers[CorrelationIdHeader] = correlationId;
            context.Items[CorrelationIdHeader] = correlationId;

            using (logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = correlationId
            }))
            {
                await _next(context);
            }
        }
    }
}
