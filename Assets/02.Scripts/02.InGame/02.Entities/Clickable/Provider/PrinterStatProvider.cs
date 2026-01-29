using UnityEngine;

public class PrinterStatProvider : IPrinterStatProvider
{
    public float ManualClick => 10f;

    public float CritRate => 0.1f;

    public float CritIncreaseRate => 1.2f;

    public float ProductTime => 100f;

    public float ValueByTime => 1f;
}
