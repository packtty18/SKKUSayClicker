using NUnit.Framework;
using UnityEngine;

public readonly struct AccountResult
{
    public readonly bool IsSuccess;
    public readonly string Message;
    public readonly string Email;

    public AccountResult(bool success, string message = "", string email = "")
    {
        IsSuccess = success;
        Message = message;
        Email = email;
    }
}
