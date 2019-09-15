using System.ComponentModel.DataAnnotations;

namespace reCaptcha
{
    public class reCaptchaVerifyRequestModel
    {
        [Required]
        public string Action { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
