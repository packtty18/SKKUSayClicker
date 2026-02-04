using NUnit.Framework;
using UnityEngine;

public readonly struct SAccountResult
{
    public readonly bool IsSuccess;
    public readonly string Message;
    public readonly string Email;

    public SAccountResult(bool success, string message = "", string email = "")
    {
        IsSuccess = success;
        Message = message;
        Email = email;
    }
}
