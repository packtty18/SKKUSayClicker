using UnityEngine;


public class LocalCurrencyRepository : ICurrencyRepository
{
    public void Save(SCurrencySaveData saveData)
    {
        // 어떻게든 Save한다.
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;
            PlayerPrefs.SetString(type.ToString(), saveData.Currencies[i].ToString("G17"));
        }
    }

    public SCurrencySaveData Load()
    {
        SCurrencySaveData data = SCurrencySaveData.Default;

        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;

            if (PlayerPrefs.HasKey(type.ToString()))
            {
                data.Currencies[i] =
                    float.Parse(PlayerPrefs.GetString(type.ToString(), "0"));
            }
        }

        return data;
    }

    public void DeleteSave()
    {
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;
            PlayerPrefs.SetString(type.ToString(), "0");
        }
    }
}