using Lean.Pool;
using System.Collections;
using UnityEngine;

public class Utils : MonoBehaviour 
{
    public static IEnumerator DestroyAfterTime(float delay, GameObject gameObject)
    {
        yield return new WaitForSeconds(delay);

        ObjectDestroy(gameObject);
    }

    public static void ObjectDestroy(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out IPoolable pool))
        {
            LeanPool.Despawn(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static string FormattedString(double damage)
    {
        // 1,000 -> 1k
        // 12,000,000 -> 12M
        string[] _suffixes =
        {
            "", "K", "M", "B", "T",
            "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj",
            "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at",
            "au", "av", "aw", "ax", "ay", "az",
            "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj",
            "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt",
            "bu", "bv", "bw", "bx", "by", "bz"
        };

        if (damage < 1000)
            return damage.ToString("N0");

        int suffixIndex = 0;

        // 1200
        // -> 1.2K
        double value = damage;
        while (value >= 1000 && suffixIndex < _suffixes.Length - 1)
        {
            value /= 1000;
            suffixIndex++;
        }

        if (value >= 100)
            return $"{value:F0}{_suffixes[suffixIndex]}";
        //if (value >= 10)
        //    return $"{value:F1}{_suffixes[suffixIndex]}";
        return $"{value:F1}{_suffixes[suffixIndex]}";
    }

}
