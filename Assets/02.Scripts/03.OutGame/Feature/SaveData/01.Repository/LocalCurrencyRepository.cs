using UnityEngine;


public class LocalCurrencyRepository : ICurrencyRepository
{
    public void Save(SCurrencySaveData saveData)
    {
        // 어떻게든 Save한다.
        for (int i = 0; i < (int)ECurrentcyData.Count; i++)
        {
            var type = (ECurrentcyData)i;
            PlayerPrefs.SetString(type.ToString(), saveData.Currencies[i].ToString("G17"));
        }
    }

    public SCurrencySaveData Load()
    {
        SCurrencySaveData data = SCurrencySaveData.Default;

        for (int i = 0; i < (int)ECurrentcyData.Count; i++)
        {
            if (PlayerPrefs.HasKey(i.ToString()))
            {
                data.Currencies[i] = float.Parse(PlayerPrefs.GetString(i.ToString(), "0"));
            }
        }

        return data;
    }
}