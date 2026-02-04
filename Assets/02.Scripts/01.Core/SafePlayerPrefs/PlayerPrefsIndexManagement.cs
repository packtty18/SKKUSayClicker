using System.Collections.Generic;
using UnityEngine;

//유저별 키 관리 & 유저 목록 관리
public static class PlayerPrefsIndexManagement
{
    //새로운 유저 등록, 글로벌 유저 인덱스 관리
    public static void RegisterUser(string userId)
    {
        string globalIndexKey = "GlobalUserIndex";
        var wrapper = JsonUtility.FromJson<KeyListWrapper>(
            PlayerPrefs.GetString(globalIndexKey, "{\"keys\":[]}")
        );

        if (!wrapper.keys.Contains(userId))
        {
            wrapper.keys.Add(userId);
            PlayerPrefs.SetString(globalIndexKey, JsonUtility.ToJson(wrapper));
            PlayerPrefs.Save();
        }
    }

    //특정 유저에게 키 추가
    public static void RegisterKey(string userId, string key)
    {
        var keys = GetKeys(userId);

        if (!keys.Contains(key))
        {
            keys.Add(key);
            SaveKeys(userId, keys);
            PlayerPrefs.Save();
        }
    }

    //특정 유저의 특정 키 삭제
    public static void Unregister(string userId, List<string> keysToRemove)
    {
        var keys = GetKeys(userId);

        foreach (var key in keysToRemove)
            keys.Remove(key);

        SaveKeys(userId, keys);
    }


    //해당 유저가 가진 모든 키 제거
    public static List<string> GetKeys(string userId)
    {
        string indexKey = PlayerPrefsKeyBuilder.UserIndex(userId);
        if (!PlayerPrefs.HasKey(indexKey))
            return new List<string>();

        string json = PlayerPrefs.GetString(indexKey);
        return JsonUtility.FromJson<KeyListWrapper>(json).keys;
    }

    //모든 유저의 아이디 반환
    public static List<string> GetAllUserIds()
    {
        var userIds = new List<string>();

        string globalIndexKey = "GlobalUserIndex";
        string json = PlayerPrefs.GetString(globalIndexKey, "{\"keys\":[]}");
        var wrapper = JsonUtility.FromJson<KeyListWrapper>(json);
        userIds.AddRange(wrapper.keys);

        return userIds;
    }

    //모든 유저 삭제
    public static void ClearUser(string userId)
    {
        var keys = GetKeys(userId);
        foreach (var key in keys)
        {
            PlayerPrefs.DeleteKey(key);
            Debug.Log($"[PrefsIndex] Delete Key: {key}");
        }

        PlayerPrefs.DeleteKey(PlayerPrefsKeyBuilder.UserIndex(userId));
        Debug.Log($"[PrefsIndex] Clear User: {userId}");
    }

    //유저키를 JSON으로 저장
    private static void SaveKeys(string userId, List<string> keys)
    {
        var wrapper = new KeyListWrapper { keys = keys };
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(PlayerPrefsKeyBuilder.UserIndex(userId), json);
        PlayerPrefs.Save();
    }


    [System.Serializable]
    private class KeyListWrapper
    {
        public List<string> keys = new List<string>();
    }
}
