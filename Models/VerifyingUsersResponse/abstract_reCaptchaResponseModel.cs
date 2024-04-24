////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace reCaptcha;

/// <inheritdoc/>
public abstract class AbstractReCaptchaResponseModel
{
    /// <summary>
    /// был ли этот запрос действительным маркером reCAPTCHA для вашего сайта
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; } = false;

    /// <summary>
    /// timestamp загрузки проверки (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
    /// </summary>
    [JsonProperty("challenge_ts")]
    public DateTime ChallengeTs { get; set; }

    /// <summary>
    /// Ошибки (коды ошибок)
    /// </summary>
    [JsonProperty("error-codes")]
    public string[]? ErrorСodes { get; set; }

    /// <summary>
    /// Преобразовать в строковое представление
    /// </summary>
    /// <returns>Строковое представление</returns>
    public override string ToString()
    {
        string ret_val = (Success ? "Success" : "Not success") + " - " + ChallengeTs.ToString() + Environment.NewLine;
        if (ErrorСodes != null)
            foreach (string s in ErrorСodes)
                ret_val += "  ERR:" + (ReCaptchaVerifyingErrorCodes.AvailableCodes.TryGetValue(s, out string? value) ? value : s);

        return ret_val;
    }
}
