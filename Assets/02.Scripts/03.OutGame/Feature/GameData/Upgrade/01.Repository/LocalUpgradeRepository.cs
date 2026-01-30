using UnityEngine;

public class LocalUpgradeRepository : IUpgradeRepository
{
    public void Save(SUpgradeSaveData upgrade)
    {
        for (int i = 0; i < (int)EUpgradeType.Count; i++)
        {
            var type = (EUpgradeType)i;
            PlayerPrefs.SetString(type.ToString(), upgrade.Level[i].ToString());
        }
    }

    public SUpgradeSaveData Load()
    {
        SUpgradeSaveData data = SUpgradeSaveData.Default;

        for (int i = 0; i < (int)EUpgradeType.Count; i++)
        {
            var type = (EUpgradeType)i;

            if (PlayerPrefs.HasKey(type.ToString()))
            {
                data.Level[i] =
                    int.Parse(PlayerPrefs.GetString(type.ToString(), "0"));
            }
        }

        return data;
    }
    public void DeleteAllSave()
    {
        for (int i = 0; i < (int)EUpgradeType.Count; i++)
        {
            var type = (EUpgradeType)i;
            PlayerPrefs.SetString(type.ToString(), "0");
        }
    }

}
