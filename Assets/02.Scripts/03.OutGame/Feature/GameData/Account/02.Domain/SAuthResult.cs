using NUnit.Framework;
using UnityEngine;

public readonly struct SAuthResult
{
    public readonly bool Success;
    public readonly string ErrorMessage;
    public readonly Account Account;

    public SAuthResult(bool v, string message = "", Account value = null)
    {
        Success = v;
        ErrorMessage = message;
        Account = value;
    }
}
