////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace reCaptcha;

/// <inheritdoc/>
/// <inheritdoc/>
public class CustomReCaptcha2ResponseModelBinder(IModelBinder fallbackBinder) : IModelBinder
{
    /// <inheritdoc/>
    public Task BindModelAsync(ModelBindingContext? bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        if (bindingContext.FieldName == "g-recaptcha-response")
            bindingContext.FieldName = "g_recaptcha_response";

        return fallbackBinder.BindModelAsync(bindingContext);
    }
}