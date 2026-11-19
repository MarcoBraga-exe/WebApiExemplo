namespace WebApiExemplo.Application.Services;

/// <summary>Validates and normalizes CPF values using their check digits.</summary>
public static class CpfValidator
{
    public static bool IsValid(string? cpf)
    {
        var digits = Normalize(cpf);
        if (digits.Length != 11 || digits.Distinct().Count() == 1)
        {
            return false;
        }

        var firstDigit = CalculateDigit(digits[..9], 10);
        var secondDigit = CalculateDigit(digits[..9] + firstDigit, 11);
        return digits[9] == firstDigit && digits[10] == secondDigit;
    }

    public static string Normalize(string? cpf) => new string((cpf ?? string.Empty).Where(char.IsDigit).ToArray());

    private static char CalculateDigit(string baseNumber, int initialWeight)
    {
        var total = baseNumber.Select((digit, index) => (digit - '0') * (initialWeight - index)).Sum();
        var remainder = total % 11;
        return remainder < 2 ? '0' : (char)('0' + 11 - remainder);
    }
}
