﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace reCaptcha;

/// <summary>
/// reCaptcha проверка
/// </summary>
public static class ReCaptchaVerifier
{
    /// <summary>
    /// Проверка reCaptcha
    /// </summary>
    /// <param name="secret">Общий ключ между вашим сайтом и reCAPTCHA</param>
    /// <param name="response">Маркер ответа пользователя, предоставляемый клиентской интеграцией reCAPTCHA на вашем сайте</param>
    /// <param name="remoteip">IP адрес удалённого клиента (который проходит проверку)</param>
    /// <returns>Ответ/результат проверки reCaptcha</returns>
    public static ReCaptcha3ResponseModel? ReCaptcha3SiteVerify(string secret, string response, string? remoteip = null)
    {
        byte[] respBytes = ReCaptchaSiteVerify(secret, response, remoteip).Result;
        return DeserializeFromStream(new MemoryStream(respBytes), typeof(ReCaptcha3ResponseModel)) as ReCaptcha3ResponseModel;
    }

    /// <summary>
    /// Проверка reCaptcha
    /// </summary>
    /// <param name="secret">Общий ключ между вашим сайтом и reCAPTCHA</param>
    /// <param name="response">Маркер ответа пользователя, предоставляемый клиентской интеграцией reCAPTCHA на вашем сайте</param>
    /// <param name="remoteip">IP адрес удалённого клиента (который проходит проверку)</param>
    /// <returns>Ответ/результат проверки reCaptcha</returns>
    public async static Task<ReCaptcha3ResponseModel?> ReCaptcha3SiteVerifyAsync(string secret, string response, string? remoteip = null)
    {
        byte[] respBytes = await ReCaptchaSiteVerify(secret, response, remoteip);
        respBytes = await RunSave(() => respBytes, []);
        return await RunSave(() => DeserializeFromStream(new MemoryStream(respBytes), typeof(ReCaptcha3ResponseModel)) as ReCaptcha3ResponseModel, null);
    }

    /// <summary>
    /// Проверка reCaptcha
    /// </summary>
    /// <param name="secret">Общий ключ между вашим сайтом и reCAPTCHA</param>
    /// <param name="response">Маркер ответа пользователя, предоставляемый клиентской интеграцией reCAPTCHA на вашем сайте</param>
    /// <param name="remoteip">IP адрес удалённого клиента (который проходит проверку)</param>
    /// <returns>Ответ/результат проверки reCaptcha</returns>
    public static ReCaptcha2ResponseModel? ReCaptcha2SiteVerify(string secret, string response, string? remoteip = null)
    {
        byte[] respBytes = ReCaptchaSiteVerify(secret, response, remoteip).Result;
        return DeserializeFromStream(new MemoryStream(respBytes), typeof(ReCaptcha2ResponseModel)) as ReCaptcha2ResponseModel;
    }

    /// <summary>
    /// Проверка reCaptcha
    /// </summary>
    /// <param name="secret">Общий ключ между вашим сайтом и reCAPTCHA</param>
    /// <param name="response">Маркер ответа пользователя, предоставляемый клиентской интеграцией reCAPTCHA на вашем сайте</param>
    /// <param name="remoteip">IP адрес удалённого клиента (который проходит проверку)</param>
    /// <returns>Ответ/результат проверки reCaptcha</returns>
    public async static Task<ReCaptcha2ResponseModel?> ReCaptcha2SiteVerifyAsync(string secret, string response, string? remoteip = null)
    {
        byte[]? bytes_response = await ReCaptchaSiteVerify(secret, response, remoteip);
        byte[] respBytes = await RunSave(() => bytes_response, []);
        return await RunSave(() => DeserializeFromStream(new MemoryStream(respBytes), typeof(ReCaptcha2ResponseModel)) as ReCaptcha2ResponseModel, null);
    }

    /// <summary>
    /// Проверка reCaptcha
    /// </summary>
    /// <param name="secret">Общий ключ между вашим сайтом и reCAPTCHA</param>
    /// <param name="response">Маркер ответа пользователя, предоставляемый клиентской интеграцией reCAPTCHA на вашем сайте</param>
    /// <param name="remoteip">IP адрес удалённого клиента (который проходит проверку)</param>
    /// <returns>Ответ/результат проверки reCaptcha</returns>
    static async Task<byte[]> ReCaptchaSiteVerify(string secret, string response, string? remoteip = null)
    {
        try
        {
            List<KeyValuePair<string, string>>? values = new List<KeyValuePair<string, string>>
            {
                new("secret", secret),
                new("response", response)
            };

            if (!string.IsNullOrWhiteSpace(remoteip))
                values.Add(new KeyValuePair<string, string>("remoteip", remoteip));// Необязательный. IP-адрес пользователя

            FormUrlEncodedContent? content = new(values);

            using HttpClient client = new() { BaseAddress = new Uri("https://www.google.com/recaptcha/api/siteverify") };
            using HttpRequestMessage httpRequest = new(HttpMethod.Post, "https://www.google.com/recaptcha/api/siteverify")
            {
                Content = new FormUrlEncodedContent(values)
            };

            HttpResponseMessage? resp_msg = client.Send(httpRequest);
            return await resp_msg.Content.ReadAsByteArrayAsync();
        }
        catch
        {
            return [];
        }
    }

    /// <summary>
    /// Десереализовать объект из stream
    /// </summary>
    /// <param name="stream">Поток для десериализации</param>
    /// <param name="type">Тип данных, в который следует десериализовать поток</param>
    /// <returns>Объект, десериализованный из потока</returns>
    public static object? DeserializeFromStream(Stream stream, Type type)
    {
        JsonSerializer serializer = new();
        try
        {
            using StreamReader sr = new(stream);
            using JsonTextReader jsonTextReader = new(sr);
            return serializer.Deserialize(jsonTextReader, type);
        }
        catch
        {
            return null;
        }
    }

    private static Task<T> RunSave<T>(Func<T> func, T def)
    {
        return Task.Run(() =>
        {
            try
            {
                return func.Invoke();
            }
            catch
            {
                return def;
            }
        });
    }
}