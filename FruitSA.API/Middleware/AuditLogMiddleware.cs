using FruitSA.API.Models;
using FruitSA.Model;
using System.Text;
using Newtonsoft.Json.Linq;

namespace FruitSA.API.Middleware
{
    public class AuditLogMiddleware
    {
        private const string ControllerKey = "controller";
        private const string IdKey = "id";

        private readonly RequestDelegate _next;
        private static string userEmail = "";
        public AuditLogMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task InvokeAsync(HttpContext httpContext, AppDbContext dbContext)
        {
            await _next(httpContext);

            var request = httpContext.Request;

            if (request.Path.StartsWithSegments("/api"))
            {
                request.RouteValues.TryGetValue(ControllerKey, out var controllerValue);
                var controllerName = (string)(controllerValue ?? string.Empty);

                var changedValue = await GetChangedValues(request).ConfigureAwait(false);
                if(controllerName == "Auth")
                {
                    dynamic data = JObject.Parse(changedValue);
                    userEmail = data.Email;
                }
               
                var auditLog = new AuditLog
                {
                    UserEmail = userEmail,
                    EntityName = controllerName,
                    Action = request.Method,
                    Timestamp = DateTime.UtcNow,
                    Changes = changedValue
                };

                dbContext.AuditLogs.Add(auditLog);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task<string> GetChangedValues(HttpRequest request)
        {
            var changedValue = string.Empty;

            switch (request.Method)
            {
                case "POST":
                case "PUT":
                    changedValue = await ReadRequestBody(request, Encoding.UTF8).ConfigureAwait(false);
                    break;

                case "DELETE":
                    request.RouteValues.TryGetValue(IdKey, out var idValueObj);
                    changedValue = (string?)idValueObj ?? string.Empty;
                    break;

                default:
                    break;
            }

            return changedValue;
        }

        private static async Task<string> ReadRequestBody(HttpRequest request, Encoding? encoding = null)
        {
            request.Body.Position = 0;
            var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8);
            var requestBody = await reader.ReadToEndAsync().ConfigureAwait(false);
            request.Body.Position = 0;

            return requestBody;
        }
    }

}
