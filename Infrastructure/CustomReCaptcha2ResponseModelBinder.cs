////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace reCaptcha.Infrastructure
{
    public class CustomReCaptcha2ResponseModelBinder : IModelBinder
    {
        private readonly IModelBinder fallbackBinder;
        public CustomReCaptcha2ResponseModelBinder(IModelBinder fallbackBinder)
        {
            this.fallbackBinder = fallbackBinder;
        }
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            if (bindingContext.FieldName == "g-recaptcha-response")
                bindingContext.FieldName = "g_recaptcha_response";

            return fallbackBinder.BindModelAsync(bindingContext);
        }
    }
}
