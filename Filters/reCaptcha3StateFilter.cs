////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace reCaptcha;

/// <inheritdoc/>
[AttributeUsage(AttributeTargets.All)]
public class ReCaptcha3StateFilter : Attribute, IActionFilter
{
    ILogger AppLogger { get; }
    readonly IMemoryCache cache;

    /// <inheritdoc/>
    public ReCaptcha3StateFilter(IMemoryCache memoryCache, ILoggerFactory loggerFactory)
    {
        AppLogger = loggerFactory.CreateLogger(GetType().Name + "Logger");
        cache = memoryCache;
    }

    /// <summary>
    /// перед отработкой метода контроллера
    /// </summary>
    public void OnActionExecuting(ActionExecutingContext ax_context)
    {
        HttpContext context = ax_context.HttpContext;

        if (!context.Session.IsAvailable)
            AppLogger.LogDebug("Запрос reCaptcha статуса невозможен. Сессия: !context.Session.IsAvailable");
        else
        {
            string? client_id = context.Session.Get<string>("ClientId");
            if (string.IsNullOrWhiteSpace(client_id))
            {
                AppLogger.LogError("Сессии клиента не назначен ID");
                return;
            }

            AppLogger.LogDebug("Запрос reCaptcha статуса. reCaptchaTokenName: " + client_id);

            if (!cache.TryGetValue(client_id, out byte[] reCaptchaBody))
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