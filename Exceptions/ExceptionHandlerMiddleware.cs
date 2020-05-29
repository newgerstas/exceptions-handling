using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExceptionStrategy.Exceptions
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IErrorVocabularyProvider errorVocabularyProvider;
        private readonly ILogger<ExceptionHandlerMiddleware> logger;

        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            IErrorVocabularyProvider errorVocabularyProvider,
            ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;

            this.errorVocabularyProvider = errorVocabularyProvider
                ?? throw new System.ArgumentNullException(nameof(errorVocabularyProvider));
                
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            int responseStatus = 500;
            object body = null;

            // get request id to identify error through many API calls
            var requestId = context.Request.Headers["RequestId"].FirstOrDefault()
                ?? Guid.NewGuid().ToString();

            // log error
            logger.LogError(requestId, exception);

            if (exception is ABCException abcException)
            {
                var language = context.Request.Headers["Accept-Language"];
                var translator = errorVocabularyProvider.Create(language);

                // translate exception
                var error = translator.Translate(abcException);
                responseStatus = error.Status;

                body = new
                {
                    ErrorCode = abcException.Code.ToString(),
                    ErrorMessage = error.Message,
                    RequestId = requestId
                };
            }
            else
            {
                // if we can't translate error - fallback to default one
                body = "Sorry a server error has occurred";
            }

            context.Response.StatusCode = responseStatus;
            context.Response.ContentType = "application/json";

            var text = JsonConvert.SerializeObject(body);
            await context.Response.WriteAsync(text);
        }
    }
}