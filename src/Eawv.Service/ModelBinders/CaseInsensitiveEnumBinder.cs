// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Eawv.Service.ModelBinders;

public class CaseInsensitiveEnumBinder<T> : IModelBinder
    where T : struct, Enum
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

        if (Enum.TryParse<T>(value, ignoreCase: true, out var result))
        {
            bindingContext.Result = ModelBindingResult.Success(result);
        }
        else
        {
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, $"Invalid value '{value}' for enum type '{typeof(T).Name}'.");
        }

        return Task.CompletedTask;
    }
}
