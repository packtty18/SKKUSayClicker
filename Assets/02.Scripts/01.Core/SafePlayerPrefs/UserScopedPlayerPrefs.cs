using System.Collections.Generic;
using UnityEngine;


//플레이어 Pref 관리
public static class UserScopedPlayerPrefs
{
    private static string BuildKey(string userId, string domain, string dataType)
        => PlayerPrefsKeyBuilder.GameData(userId, domain, dataType);

    //아이디를 생성했을때 등록
    private static void Register(string userId, string key)
    {
        PlayerPrefsIndexManagement.RegisterUser(userId);
        PlayerPrefsIndexManagement.RegisterKey(userId, key);
    }

    //해당 유저를 삭제
    public static void DeleteUser(string userId)
    {
        PlayerPrefsIndexManagement.ClearUser(userId);
    }

    //해당 계정의 해당 도메인을 모두 삭제
    public static void DeleteDomain(string userId, string domain)
    {
        //01. 모든 키 가져오기
        var keys = PlayerPrefsIndexManagement.GetKeys(userId);
        var removedKeys = new List<string>();

        foreach (var key in keys)
        {
            //특정 조건의 키만 선별
            if (key.StartsWith($"{userId}_{domain}_"))
            {
                //해당 키 삭제
                PlayerPrefs.DeleteKey(key);
                //삭제된 키 목록에 저장
                removedKeys.Add(key);
                Debug.Log($"[Prefs] Delete Domain Key: {key}");
            }
        }

        //해당 유저정보에게서 해당 키 제거
        PlayerPrefsIndexManagement.Unregister(userId, removedKeys);
    }


    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void SetInt(string userId, string domain, string dataType, int value)
    {
        string key = BuildKey(userId, domain, dataType);
        PlayerPrefs.SetInt(key, value);
        Register(userId, key);
        PlayerPrefs.Save();

        Debug.Log($"[Prefs] SetInt {key} = {value}");
    }

    public static int GetInt(string userId, string domain, string dataType, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(BuildKey(userId, domain, dataType), defaultValue);
    }

    public static void SetFloat(string userId, string domain, string dataType, float value)
    {
        string key = BuildKey(userId, domain, dataType);
        PlayerPrefs.SetFloat(key, value);
        Register(userId, key);
        PlayerPrefs.Save();

        Debug.Log($"[Prefs] SetFloat {key} = {value}");
    }

    public static float GetFloat(string userId, string domain, string dataType, float defaultValue = 0f)
    {
        return PlayerPrefs.GetFloat(BuildKey(userId, domain, dataType), defaultValue);
    }

    public static void SetString(string userId, string domain, string dataType, string value)
    {
        string key = BuildKey(userId, domain, dataType);
        PlayerPrefs.SetString(key, value);
        Register(userId, key);
        PlayerPrefs.Save();

        Debug.Log($"[Prefs] SetString {key} = {value}");
    }

    public static string GetString(string userId, string domain, string dataType, string defaultValue = "")
    {
        return PlayerPrefs.GetString(BuildKey(userId, domain, dataType), defaultValue);
    }


    //디버깅용 데이터 가져오기
    public static void DebugAllData()
    {
        Debug.Log("[Prefs] --- Dumping All PlayerPrefs Data ---");

        // 1. 모든 유저 ID 가져오기
        var allUserIds = PlayerPrefsIndexManagement.GetAllUserIds();

        foreach (var userId in allUserIds)
        {
            Debug.Log($"[Prefs] User: {userId}");

            // 2. 해당 유저의 모든 키 가져오기
            var keys = PlayerPrefsIndexManagement.GetKeys(userId);

            foreach (var key in keys)
            {
                // 3. 타입별로 값 가져오기
                if (PlayerPrefs.HasKey(key))
                {
                    string value = PlayerPrefs.GetString(key, "<unknown>");
                    Debug.Log($"[Prefs]  Key: {key} → Value: {value}");
                }
            }
        }

        Debug.Log("[Prefs] --- End Dump ---");
    }

}
