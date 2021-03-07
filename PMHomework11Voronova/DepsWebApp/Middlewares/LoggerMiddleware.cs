using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace DepsWebApp.Middlewares
{
    /// <summary>
    /// Middleware that logs an information about request and response
    /// </summary>
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<LoggerMiddleware> _logger;

        /// <summary>
        /// Constructor of logging middleware.
        /// </summary>
        /// <param name="next">Request delegate.</param>
        /// <param name="logger">Logger.</param>
        public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Method that invoke a middleware.
        /// Loggs a request and response.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <returns>Task.</returns>
        public async Task Invoke(HttpContext context)
        {
            var request = await ObtainRequestBody(context.Request);
            _logger.LogInformation($"\n - Request -\n" +
                                   $" Path : {context.Request.Path}\n" +
                                   $" Content Type : {context.Request.ContentType}\n" +
                                   $" Request Body : {await ObtainRequestBody(context.Request)}\n");

            var originalBodyStream = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            var statusCode = context.Response.StatusCode;
            var body = await ObtainResponseBody(context);
            
            _logger.LogInformation($"\n - Response -\n" +
                                   $" Path : {context.Request.Path}\n" +
                                   $" Content Type : {context.Response.ContentType}\n" +
                                   $" Status Code : {context.Response.StatusCode}\n" +
                                   $" Response Body : {await ObtainResponseBody(context)}\n");

            await responseBody.CopyToAsync(originalBodyStream);
        }

        /// <summary>
        /// Read a request body via stream reader.
        /// </summary>
        /// <param name="request">A Request.</param>
        /// <returns>Request body as string.</returns>
        private static async Task<string> ObtainRequestBody(HttpRequest request)
        {
            if (request.Body == null)
                return string.Empty;

            request.EnableBuffering();
            var encoding = GetEncodingFromContentType(request.ContentType);
            string bodyStr;

            using (var reader = new StreamReader(request.Body, encoding, true, 1024, true))
            {
                bodyStr = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            request.Body.Seek(0, SeekOrigin.Begin); 
            return bodyStr;
        }

        /// <summary>
        /// Read a response body via stream reader.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Response body as string.</returns>
        private static async Task<string> ObtainResponseBody(HttpContext context)
        {
            var response = context.Response;
            response.Body.Seek(0, SeekOrigin.Begin);
            var encoding = GetEncodingFromContentType(response.ContentType);

            using (var reader = new StreamReader(response.Body, encoding, detectEncodingFromByteOrderMarks: false, bufferSize: 4096, leaveOpen: true))
            {
                var text = await reader.ReadToEndAsync().ConfigureAwait(false);
                response.Body.Seek(0, SeekOrigin.Begin);
                return text;
            }
        }


        /// <summary>
        /// Get encoding from content type.
        /// </summary>
        /// <param name="content">Content type as string.</param>
        /// <returns>Encoding.</returns>
        private static Encoding GetEncodingFromContentType(string content)
        {
            if (string.IsNullOrEmpty(content))
                return Encoding.UTF8;

            ContentType contentType;

            try
            {
                contentType = new ContentType(content);
            }
            catch (FormatException)
            {
                return Encoding.UTF8;
            }

            if (string.IsNullOrEmpty(contentType.CharSet))
                return Encoding.UTF8;

            return Encoding.GetEncoding(contentType.CharSet, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
        }
    }
}
