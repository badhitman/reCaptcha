﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace reCaptcha;

/// <summary>
/// For reCAPTCHA Android
/// </summary>
public class ReCaptchaAndroidResponseModel : AbstractReCaptchaResponseModel
{
    /// <summary>
    /// Имя пакета приложения, в котором была решена reCAPTCHA
    /// </summary>
    public string? apk_package_name { get; set; } // the package name of the app where the reCAPTCHA was solved
}
