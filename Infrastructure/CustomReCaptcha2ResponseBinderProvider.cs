////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace reCaptcha;

/// <inheritdoc/>
public class CustomReCaptcha2ResponseBinderProvider : IModelBinderProvider
{
    private IModelBinder? binder;

    /// <inheritdoc/>
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ILoggerFactory logger = (ILoggerFactory)context.Services.GetRequiredService(typeof(ILoggerFactory));
        if (context.Metadata.ModelType == typeof(string) && context.Metadata.Name == "g_recaptcha_response")
            binder = new CustomReCaptcha2ResponseModelBinder(new SimpleTypeModelBinder(typeof(string), logger));
        else
            return null;

        return binder;
    }
}