using UnityEngine;

public class PrinterStatProvider : IPrinterStatProvider
{
    private DataManager _data => DataManager.Instance;

    public float ManualClick =>
        _data.GetDataValue(EClickData.ManualClickValue);

    public float CritRate =>
        _data.GetDataValue(EClickData.CritRate);

    public float CritIncreaseRate =>
        _data.GetDataValue(EClickData.CritIncreaesRate);

    public float ProductTime =>
        _data.GetDataValue(EPrinterData.DefaultProductTime);

    public float ValueByTime =>
        _data.GetDataValue(EPrinterData.ValueByTime);
}
