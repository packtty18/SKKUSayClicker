using System.Security.Cryptography;
using System.Text;

public static class StringEncoder
{
    public static string Hash(string text)
    {
        using SHA256 sha = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        byte[] hash = sha.ComputeHash(bytes);

        StringBuilder sb = new StringBuilder(64);

        for (int i = 0; i < hash.Length; i++)
            sb.Append(hash[i].ToString("x2"));

        return sb.ToString();
    }
}
