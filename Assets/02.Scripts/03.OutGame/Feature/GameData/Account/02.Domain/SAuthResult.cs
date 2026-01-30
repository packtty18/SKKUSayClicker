using NUnit.Framework;
using UnityEngine;

public readonly struct SAuthResult
{
    public readonly bool IsSuccess;
    public readonly string Message;
    public readonly Account Account;

    public SAuthResult(bool v, string message = "", Account value = null)
    {
        IsSuccess = v;
        Message = message;
        Account = value;
    }
}
