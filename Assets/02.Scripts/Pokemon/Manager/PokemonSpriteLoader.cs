using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class PokemonSpriteLoader : MonoBehaviour
{
    private static PokemonSpriteLoader _instance;
    private readonly Dictionary<string, Sprite> _spriteCache = new Dictionary<string, Sprite>();

    public static PokemonSpriteLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("PokemonSpriteLoader");
                _instance = go.AddComponent<PokemonSpriteLoader>();
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

    /// <summary>
    /// URL에서 스프라이트를 로드합니다 (캐싱 지원)
    /// </summary>
    public async UniTask<Sprite> LoadSpriteAsync(string url, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentException("URL is null or empty", nameof(url));
        }

        // 캐시에서 확인
        if (_spriteCache.TryGetValue(url, out Sprite cachedSprite))
        {
            return cachedSprite;
        }

        // 다운로드
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            await request.SendWebRequest().WithCancellation(cancellationToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"Failed to load sprite: {request.error}");
            }

            try
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );

                // 캐시에 저장
                _spriteCache[url] = sprite;
                return sprite;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create sprite: {ex.Message}", ex);
            }
        }
    }

    /// <summary>
    /// 여러 스프라이트를 동시에 로드합니다
    /// </summary>
    public async UniTask<Sprite[]> LoadSpritesAsync(string[] urls, CancellationToken cancellationToken = default)
    {
        if (urls == null || urls.Length == 0)
        {
            throw new ArgumentException("URLs array is null or empty", nameof(urls));
        }

        UniTask<Sprite>[] tasks = new UniTask<Sprite>[urls.Length];
        for (int i = 0; i < urls.Length; i++)
        {
            tasks[i] = LoadSpriteAsync(urls[i], cancellationToken);
        }

        return await UniTask.WhenAll(tasks);
    }

    /// <summary>
    /// 캐시를 클리어합니다
    /// </summary>
    public void ClearCache()
    {
        foreach (var sprite in _spriteCache.Values)
        {
            if (sprite != null && sprite.texture != null)
            {
                Destroy(sprite.texture);
                Destroy(sprite);
            }
        }
        _spriteCache.Clear();
    }

    /// <summary>
    /// 특정 URL의 캐시를 제거합니다
    /// </summary>
    public void RemoveFromCache(string url)
    {
        if (_spriteCache.TryGetValue(url, out Sprite sprite))
        {
            if (sprite != null && sprite.texture != null)
            {
                Destroy(sprite.texture);
                Destroy(sprite);
            }
            _spriteCache.Remove(url);
        }
    }

    private void OnDestroy()
    {
        ClearCache();
    }
}