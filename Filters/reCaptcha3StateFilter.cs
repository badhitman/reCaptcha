using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;

namespace reCaptcha.Filters
{
    public class reCaptcha3StateFilter : Attribute, IActionFilter
    {
        ILogger AppLogger { get; }
        IMemoryCache cache;

        //IHttpContextAccessor accessor
        public reCaptcha3StateFilter(IMemoryCache memoryCache, ILoggerFactory loggerFactory)
        {
            AppLogger = loggerFactory.CreateLogger(GetType().Name + "Logger");
            cache = memoryCache;
        }

        /// <summary>
        /// перед отработкой метода контроллера
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext ax_context)
        {
            HttpContext context = ax_context.HttpContext;
            byte[] reCaptchaBody;
            
            if (!context.Session.IsAvailable)
                AppLogger.LogDebug("Запрос reCaptcha статуса невозможен. Сессия: !context.Session.IsAvailable");
            else
            {
                string client_id = context.Session.Get<string>("ClientId");
                if (string.IsNullOrWhiteSpace(client_id))
                {
                    AppLogger.LogError("Сессии клиента не назначен ID");
                    return;
                }

                AppLogger.LogDebug("Запрос reCaptcha статуса. reCaptchaTokenName: " + client_id);

                if (!cache.TryGetValue(client_id, out reCaptchaBody))
                {
                    AppLogger.LogWarning("Запрашиваемый reCaptchaTokenName [" + client_id + "] не обнаружен в кеше");
                    return;
                }
                else
                    cache.Remove(client_id);

                AppLogger.LogDebug("Запрашиваемый reCaptchaTokenName [" + client_id + "] прочитан из кеша " + reCaptchaBody.Length + " bytes");
                if (reCaptchaBody is null || reCaptchaBody.Length == 0)
                {
                    AppLogger.LogError("Запрашиваемый reCaptchaTokenName [" + client_id + "] пустой. В нём не записано ни каких данных проверки");
                    return;
                }
                context.Session.Set(GetType().Name, reCaptchaBody);
            }
        }

        /// <summary>
        /// после выработки метода контроллера
        /// </summary>
        public void OnActionExecuted(ActionExecutedContext ax_context)
        {
            ax_context.HttpContext.Session.Remove(GetType().Name);
        }
    }
}
