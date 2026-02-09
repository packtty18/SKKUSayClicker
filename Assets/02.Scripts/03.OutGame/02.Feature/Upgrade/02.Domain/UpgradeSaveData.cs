using Firebase.Firestore;
using System;

[Serializable]
[FirestoreData]
public class UpgradeSaveData
{
    [FirestoreProperty]
    public int[] Levels { get; private set; }
    [FirestoreProperty]
    public long LastSavedAt { get; private set; }

    public UpgradeSaveData() { }

    public UpgradeSaveData(UpgradeSaveData data)
    {
        Levels = data.Levels;
        LastSavedAt = data.LastSavedAt;
    }

    //로드용도
    public UpgradeSaveData(int[] levels, long lastSavedAt)
    {
        Levels = levels;
        LastSavedAt = lastSavedAt;
    }

    //세이브 용도 , 초기 상태 생성하기
    public static UpgradeSaveData CreateDefault()
    {
        return new UpgradeSaveData(new int[(int)EUpgradeType.Count],
            DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }

    //현재 저장된 데이터를 세이브 데이터로 바꾸기
    public static UpgradeSaveData FromRuntime(IReadOnlyUpgrade[] upgrade)
    {
        int[] target = new int[upgrade.Length];
        for (int i = 0; i < upgrade.Length; i++)
        {
            target[i] = upgrade[i].Level;
        }

        return new UpgradeSaveData(
            target,
             DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        );
    }
}
