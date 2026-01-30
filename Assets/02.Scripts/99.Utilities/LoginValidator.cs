using System.Text.RegularExpressions;
public static class LoginValidator
{
    private static readonly Regex EmailRegex = new(@"^[\w.-]+@[\w.-]+\.\w+$");
    private static readonly Regex PasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^A-Za-z0-9])(?!.*\d)\S{7,20}$");

    public static bool IsValidEmail(string email)
    {
        return !string.IsNullOrEmpty(email) && EmailRegex.IsMatch(email);
    }

    public static bool IsValidPassword(string password)
    {
        return !string.IsNullOrEmpty(password) && PasswordRegex.IsMatch(password);
    }
}