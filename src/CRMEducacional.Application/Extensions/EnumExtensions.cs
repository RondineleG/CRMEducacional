namespace CRMEducacional.Application.Extensions;

public static class EnumExtensions
{
    public static TEnum ParseEnumFromDescription<TEnum>(string description) where TEnum : struct
    {
        foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute
                && attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase)
                && Enum.TryParse<TEnum>(field.Name, out var value))
            {
                return value;
            }
        }
        throw new ArgumentException($"N�o foi poss�vel encontrar um valor correspondente para '{description}' em {typeof(TEnum).Name}.");
    }

    public static string ToDescriptionString<TEnum>(this TEnum @enum)
    {
        if (@enum is null) throw new ArgumentNullException(nameof(@enum));
        var info = @enum.GetType().GetField(@enum.ToString()!)!;
        var attributes = (DescriptionAttribute[])
            info.GetCustomAttributes(typeof(DescriptionAttribute), false)!;
        if (attributes.Length > 0) return attributes[0].Description;
        return @enum.ToString()!;
    }
}