using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using static UniGif;

public class PokemonGifLoader : MonoBehaviour
{
    private static PokemonGifLoader _instance;

    // URL → GIF Frame List Cache
    private readonly Dictionary<string, List<GifTexture>> _gifCache
        = new Dictionary<string, List<GifTexture>>();

    public static PokemonGifLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("PokemonGifLoader");
                _instance = go.AddComponent<PokemonGifLoader>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region Public API

    /// <summary>
    /// URL에서 GIF 프레임 로드 (캐싱 지원)
    /// </summary>
    public async UniTask<List<GifTexture>> LoadGifAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentException("GIF url is null or empty");

        if (_gifCache.TryGetValue(url, out var cached))
        {
            Debug.Log($"GIF Cache Hit: {url}");
            return cached;
        }

        byte[] bytes = await DownloadAsync(url, cancellationToken);
        var frames = await DecodeAsync(bytes, cancellationToken);

        if (frames == null || frames.Count == 0)
            throw new Exception("GIF decode failed");

        _gifCache[url] = frames;

        Debug.Log($"GIF Cached: {url}");

        return frames;
    }

    #endregion

    #region Download

    private async UniTask<byte[]> DownloadAsync(
        string url,
        CancellationToken token)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest().WithCancellation(token);

            if (request.result != UnityWebRequest.Result.Success)
                throw new Exception($"GIF download failed: {request.error}");

            return request.downloadHandler.data;
        }
    }

    #endregion

    #region Decode

    private UniTask<List<GifTexture>> DecodeAsync(
        byte[] bytes,
        CancellationToken token)
    {
        var tcs = new UniTaskCompletionSource<List<GifTexture>>();

        StartCoroutine(
            UniGif.GetTextureListCoroutine(
                bytes,
                (textures, loopCount, width, height) =>
                {
                    tcs.TrySetResult(textures);
                }
            )
        );

        token.Register(() =>
        {
            tcs.TrySetCanceled();
        });

        return tcs.Task;
    }

    #endregion

    #region Cache Management

    public void ClearCache()
    {
        foreach (var list in _gifCache.Values)
        {
            foreach (var frame in list)
            {
                if (frame.m_texture2d != null)
                    Destroy(frame.m_texture2d);
            }
        }

        _gifCache.Clear();
        Debug.Log("GIF Cache Cleared");
    }

    public void RemoveFromCache(string url)
    {
        if (_gifCache.TryGetValue(url, out var list))
        {
            foreach (var frame in list)
            {
                if (frame.m_texture2d != null)
                    Destroy(frame.m_texture2d);
            }

            _gifCache.Remove(url);
        }
    }

    #endregion

    private void OnDestroy()
    {
        ClearCache();
    }
}
