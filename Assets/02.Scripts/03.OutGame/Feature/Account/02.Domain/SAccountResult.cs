using NUnit.Framework;
using UnityEngine;

public readonly struct SAccountResult
{
    public readonly bool IsSuccess;
    public readonly string Message;
    public readonly Account Account;

    public SAccountResult(bool success, string message = "", Account value = null)
    {
        IsSuccess = success;
        Message = message;
        Account = value;
    }
}
