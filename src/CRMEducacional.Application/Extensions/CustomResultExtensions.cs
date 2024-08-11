namespace CRMEducacional.Application.Extensions;

public static class CustomResultExtensions
{
    public static CustomResult<T> FromValidationResult<T>(this CustomResult<T> customResult, CustomValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            customResult.AddError(error);
        }
        return customResult;
    }
}