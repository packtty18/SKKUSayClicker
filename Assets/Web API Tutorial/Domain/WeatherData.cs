using System;
using UnityEngine;

/// <summary>
/// Root DTO for OpenWeather current weather API
/// </summary>
[Serializable]
public class WeatherData
{
    public CoordDTO coord;
    public WeatherDTO[] weather;
    public MainDTO main;
    public WindDTO wind;
    public CloudsDTO clouds;
    public SysDTO sys;

    public string name;   // city name
    public int timezone;  // seconds from UTC
    public long dt;       // data time (unix)
    public int cod;       // http result code
}

[Serializable]
public class CoordDTO
{
    public float lon;
    public float lat;
}

[Serializable]
public class WeatherDTO
{
    public int id;
    public string main;
    public string description;
    public string icon;
}

[Serializable]
public class MainDTO
{
    public float temp;
    public float feels_like;
    public float temp_min;
    public float temp_max;
    public int pressure;
    public int humidity;
}

[Serializable]
public class WindDTO
{
    public float speed;
    public int deg;
}

[Serializable]
public class CloudsDTO
{
    public int all;
}

[Serializable]
public class SysDTO
{
    public string country;
    public long sunrise;
    public long sunset;
}
