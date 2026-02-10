using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetWeatherTest : MonoBehaviour
{
    private const string API_KEY = "bf796a22cf926c9d58b2698a776e29a1";
    private const string BASE_URL =
        "https://api.openweathermap.org/data/2.5/weather";

    private void Start()
    {
        StartGet().Forget();
    }

    private async UniTask StartGet()
    {
        float lat = 37.4038f;
        float lon = 127.1056f;

        string url =
            $"{BASE_URL}?lat={lat}&lon={lon}&units=metric&appid={API_KEY}&units=metric&lang=kr";

        string json = await GetWebText(url);
        if (string.IsNullOrEmpty(json))
            return;

        ParseWeather(json);
    }

    private void ParseWeather(string json)
    {
        WeatherData data =
            JsonUtility.FromJson<WeatherData>(json);

        if (data == null || data.cod != 200)
        {
            Debug.LogWarning("[Weather] Invalid response");
            return;
        }

        Debug.Log($"City : {data.name}");
        Debug.Log($"Weather : {data.weather[0].description}");
        Debug.Log($"Temp : {data.main.temp} °C");
        Debug.Log($"Humidity : {data.main.humidity}%");
        Debug.Log($"Wind : {data.wind.speed} m/s");

        // Example: sunrise time conversion
        var sunrise =
            System.DateTimeOffset
                .FromUnixTimeSeconds(data.sys.sunrise)
                .ToLocalTime();

        Debug.Log($"Sunrise : {sunrise}");
    }

    private async UniTask<string> GetWebText(string url)
    {
        using UnityWebRequest request = UnityWebRequest.Get(url);

        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                $"[Weather] HTTP {request.responseCode} : {request.error}");
            return string.Empty;
        }

        return request.downloadHandler.text;
    }
}
