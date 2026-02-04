using UnityEngine;

public enum EFloatTextType
{ 
    Printer,
    PrinterCritical,
    Money,
    Prestigy
}

public struct SFloaterTextContext
{
    public double Value;
    public EFloatTextType Type;

    public SFloaterTextContext(double value, EFloatTextType type)
    {
        Value = value;
        Type = type;
    }
}
