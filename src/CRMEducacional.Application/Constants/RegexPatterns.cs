namespace CRMEducacional.Application.Constants;

public static class RegexPatterns
{
    public const string Celular = @"^(\d{2})?9[1-9]\d{7}$";

    public const string CEP = @"^\d{8}$";

    public const string Email = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    public const string ResidencialComercial = @"^(\d{2})?[2-9]\d{7}$";

    public const string RG = @"^[0-9A-Za-z]{5,9}$";
}