////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace reCaptcha;

/// <inheritdoc/>
public static class SessionExtensions
{
    /// <inheritdoc/>
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    /// <inheritdoc/>
    public static T? Get<T>(this ISession session, string key)
    {
        string? value = session.GetString(key);

        return value is null
            ? default
            : JsonConvert.DeserializeObject<T>(value);
    }
}