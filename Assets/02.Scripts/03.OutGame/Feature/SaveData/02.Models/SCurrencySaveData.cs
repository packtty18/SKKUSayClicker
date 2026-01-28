public struct SCurrencySaveData
{
    public float[] Currencies;

    // 재화 기본값
    public static SCurrencySaveData Default => new SCurrencySaveData()
    {
        Currencies = new float[(int)ECurrentcyData.Count]
    };
}