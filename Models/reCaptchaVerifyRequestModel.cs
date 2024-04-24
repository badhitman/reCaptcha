////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////
using System.ComponentModel.DataAnnotations;

namespace reCaptcha;

/// <inheritdoc/>
public class ReCaptchaVerifyRequestModel
{
    /// <inheritdoc/>
    [Required]
    public required string Action { get; set; }

    /// <inheritdoc/>
    [Required]
    public required string Token { get; set; }
}
