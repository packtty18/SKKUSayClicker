//저장시킬 키를 생성. 각 사용처 별로 키 생성
public static class PlayerPrefsKeyBuilder
{
    public static string Account(string userId)
        => $"Account_{userId}";

    public static string GameData(string userId, string domain, string dataType)
        => $"{userId}_{domain}_{dataType}";

    public static string UserIndex(string userId)
        => $"UserIndex_{userId}";
}
