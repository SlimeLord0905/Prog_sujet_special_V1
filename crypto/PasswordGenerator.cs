using System.Security.Cryptography;
using System.Text;

namespace Crypto;

public class PasswordGenerator
{
    public const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
    public const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string Digits = "0123456789";
    public const string Symbols = "!@#$%?&*()";

    public static string GeneratePassword(
        int length,
        bool withSymbols = true, bool withDigits = true,
        bool withUpper = true, bool withLower = true)
    {
        if (!(withLower || withUpper || withDigits || withSymbols))
        {
            throw new ArgumentException("at least 1 of the character types must be true");
        }

        string characters = withLower ? LowercaseLetters : "";
        if (withUpper)
            characters += UppercaseLetters;

        if (withDigits)
            characters += Digits;

        if (withSymbols)
            characters += Symbols;


        StringBuilder password = new StringBuilder();
        for (int j = 0; j < length; j++)
        {
            int index = RandomNumberGenerator.GetInt32(0, characters.Length);
            password.Append(characters[index]);
        }

        return password.ToString();
    }

    public static bool ValidatePassword(
        string password,
        bool withSymbols = true, bool withDigits = true,
        bool withUpper = true, bool withLower = true)
    {
        bool isValid = !withLower || password.Any(x => LowercaseLetters.Contains(x));
        if (withUpper)
            isValid &= password.Any(x => UppercaseLetters.Contains(x));
        
        if (withDigits)
            isValid &= password.Any(x => Digits.Contains(x));
        
        if (withSymbols)
            isValid &= password.Any(x => Symbols.Contains(x));
        return isValid;
    }
}