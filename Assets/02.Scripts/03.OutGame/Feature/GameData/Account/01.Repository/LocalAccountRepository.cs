using UnityEngine;

//저장과 불러오기 만을 담당(검증 없음)
public class LocalAccountRepository : IAccountRepository
{
    private const string PasswordSuffix = "_PW";

    public bool Exists(string id)
    {
        bool exists = PlayerPrefs.HasKey(GetPasswordKey(id));
        Debug.Log($"[AccountRepository] Exists({id}) = {exists}");
        return exists;
    }

    //ID를 통한 패스워드 해쉬 가져오기
    public string LoadPasswordHash(string id)
    {
        if (!Exists(id))
        {
            Debug.LogWarning($"[AccountRepository] Load failed. ID not found: {id}");
            return string.Empty;
        }

        string hash = PlayerPrefs.GetString(GetPasswordKey(id), string.Empty);
        Debug.Log($"[AccountRepository] Password hash loaded for ID: {id}");
        return hash;
    }

    //해당 ID에 해쉬로 저장
    public void Save(string id, string passwordHash)
    {
        PlayerPrefs.SetString(GetPasswordKey(id), passwordHash);
        PlayerPrefs.Save();

        Debug.Log($"[AccountRepository] Account saved. ID: {id}");
    }

    //수정이 필요
    public void DeleteSave(string id)
    {
        if (!Exists(id))
        {
            Debug.LogWarning($"[AccountRepository] Delete failed. ID not found: {id}");
            return;
        }

        PlayerPrefs.DeleteKey(GetPasswordKey(id));
        PlayerPrefs.Save();

        Debug.Log($"[AccountRepository] Account deleted. ID: {id}");
    }

    private string GetPasswordKey(string id)
    {
        return $"{id}{PasswordSuffix}";
    }

    public void DeleteAllSave()
    {
        throw new System.NotImplementedException();
    }
}
