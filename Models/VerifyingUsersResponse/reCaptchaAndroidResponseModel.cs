﻿using System;
using System.Collections.Generic;
using System.Text;

namespace reCaptcha.Models.VerifyingUsersResponse
{
    /// <summary>
    /// For reCAPTCHA Android
    /// </summary>
    public class reCaptchaAndroidResponseModel : abstract_reCaptchaResponseModel
    {
        public string apk_package_name { get; set; } // the package name of the app where the reCAPTCHA was solved
    }
}
