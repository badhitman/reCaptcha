////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;

namespace reCaptcha.Infrastructure
{
    public class CustomReCaptcha2ResponseBinderProvider : IModelBinderProvider
    {
        private IModelBinder binder;

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            ILoggerFactory logger = context.Services.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            if (context.Metadata.ModelType == typeof(string) && context.Metadata.Name == "g_recaptcha_response")
                binder = new CustomReCaptcha2ResponseModelBinder(new SimpleTypeModelBinder(typeof(string), logger));
            else
                return null;
           
            return binder;
        }
    }
}
