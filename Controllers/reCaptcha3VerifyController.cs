﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace reCaptcha;

/// <inheritdoc/>
public abstract class ReCaptcha3VerifyController : Controller
{
    /// <summary>
    /// Описание внутренней ошибки index post запроса
    /// </summary>
    private string err_verificator { get; set; } = "Требуются POST данные модели 'reCaptchaVerifyRequestModel' {[Required] string Action; [Required] string Token;}";

    private ILogger? AppLogger;

    /// <summary>
    /// Private API key reCaptcha
    /// </summary>
    public abstract string ReCaptchaV3PrivatKey { get; }

    /// <summary>
    /// Срок (в минутах) хранения (положительного) результата проверки reCaptcha токена.
    /// Информация о прохождении проверки reCaptcha сохраняется во временном кеше - только в случае если проверка пройдена удачно.
    /// </summary>
    public virtual short CacheSuccessVerifyResultLifetimeMinutes { get; protected set; } = 2;

    /// <summary>
    /// Заглушка для вывода ошибки
    /// </summary>
    /// <returns></returns>
    public JsonResult Index()
    {
        return Json(new ReCaptcha2ResponseModel() { Hostname = null, Success = false, ChallengeTs = DateTime.MinValue, ErrorСodes = [err_verificator] });
    }

    /// <inheritdoc/>
    [HttpPost]
    public IActionResult Index(ReCaptchaVerifyRequestModel verify_model, [FromServices] IMemoryCache memoryCache, [FromServices] ILoggerFactory loggerFactory)
    {
        AppLogger = loggerFactory.CreateLogger(GetType().Name + "Logger");
        if (!ModelState.IsValid)
        {
            err_verificator = "ModelState NOT Valid";
            AppLogger.LogError(err_verificator + Environment.NewLine + "session:" + HttpContext.Session?.Id);
            return Index();
        }

        ReCaptcha3ResponseModel? reCaptchavStatus = ReCaptchaVerifier.ReCaptcha3SiteVerify(ReCaptchaV3PrivatKey, verify_model.Token, HttpContext.Connection.RemoteIpAddress.ToString());

        if (reCaptchavStatus is null)
        {
            err_verificator = "parse reCaptcha api server - is null";
            AppLogger.LogError(err_verificator);
            return Index();
        }

        if (reCaptchavStatus.Success)
        {
            string? client_id = HttpContext.Session.Get<string>("ClientId");
            if (string.IsNullOrWhiteSpace(client_id))
            {
                client_id = Guid.NewGuid().ToString();
                HttpContext.Session.Set("ClientId", client_id);
            }

            memoryCache.Set(client_id, reCaptchavStatus, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheSuccessVerifyResultLifetimeMinutes)
            });
        }
        else
            AppLogger.LogDebug("reCaptcha v3 verify controller error");

        return Json(reCaptchavStatus);
    }
}