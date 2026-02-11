using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UniGif;

public class PokemonCardUI : MonoBehaviour
{
    [Header("Card Root Objects")]
    [SerializeField] private GameObject frontSide;
    [SerializeField] private GameObject backSide;

    [Header("Front")]
    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RawImage frontRawImage;
    [SerializeField] private Image maleImage;
    [SerializeField] private Image femaleImage;

    [Header("Back")]
    [SerializeField] private RawImage backRawImage;
    [SerializeField] private Image malebackImage;
    [SerializeField] private Image femalebackImage;

    [Header("Animation")]
    [SerializeField] private float flipDuration = 0.3f;

    private PokemonData _currentPokemonData;
    private CancellationTokenSource _cts;
    private CancellationTokenSource _gifPlayCts;

    private List<GifTexture> _frontFrames;
    private List<GifTexture> _backFrames;

    [SerializeField, ReadOnly]
    private bool _isShowingFront = true;

    private void Start()
    {
        PokemonManager.Instance.OnDataChanged.Subscribe(SetPokemonData);
        ShowFront();
    }

    public void SetPokemonData(PokemonData data, CancellationToken token = default)
    {
        SetPokemonDataAsync(data, token).Forget();
    }

    public async UniTask SetPokemonDataAsync(PokemonData data, CancellationToken token = default)
    {
        if (data == null)
        {
            Debug.LogError("PokemonData is null");
            return;
        }

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = CancellationTokenSource.CreateLinkedTokenSource(token);

        _currentPokemonData = data;

        try
        {
            await SetupFrontSideAsync(_cts.Token);
            await SetupBackSideAsync(_cts.Token);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Card setup cancelled");
        }

        ShowFront();
        PlayCurrentSideGif();
    }

    #region Setup

    private async UniTask SetupFrontSideAsync(CancellationToken token)
    {
        idText.text = $"No.{_currentPokemonData.Id:000}";
        nameText.text = _currentPokemonData.Name.ToUpper();

        // GIF Load
        if (!string.IsNullOrEmpty(_currentPokemonData.GifSprites.FrontDefaultGif))
        {
            _frontFrames = await PokemonGifLoader.Instance
                .LoadGifAsync(_currentPokemonData.GifSprites.FrontDefaultGif, token);

            if (_frontFrames?.Count > 0)
                frontRawImage.texture = _frontFrames[0].m_texture2d;
        }

        if (!string.IsNullOrEmpty(_currentPokemonData.Sprites.FrontDefault))
        {
            var sprite = await PokemonSpriteLoader.Instance
                .LoadSpriteAsync(_currentPokemonData.Sprites.FrontDefault, token);

            maleImage.sprite = sprite;
        }

        if (!string.IsNullOrEmpty(_currentPokemonData.Sprites.FrontFemale))
        {
            var sprite = await PokemonSpriteLoader.Instance
                .LoadSpriteAsync(_currentPokemonData.Sprites.FrontFemale, token);

            femaleImage.gameObject.SetActive(true);
            femaleImage.sprite = sprite;
        }
        else
        {
            femaleImage.gameObject.SetActive(false);
        }
    }

    private async UniTask SetupBackSideAsync(CancellationToken token)
    {
        // GIF Load
        if (!string.IsNullOrEmpty(_currentPokemonData.GifSprites.BackDefaultGif))
        {
            _backFrames = await PokemonGifLoader.Instance
                .LoadGifAsync(_currentPokemonData.GifSprites.BackDefaultGif, token);

            if (_backFrames?.Count > 0)
                backRawImage.texture = _backFrames[0].m_texture2d;
        }

        // Male
        if (!string.IsNullOrEmpty(_currentPokemonData.Sprites.BackDefault))
        {
            var sprite = await PokemonSpriteLoader.Instance
                .LoadSpriteAsync(_currentPokemonData.Sprites.BackDefault, token);

            malebackImage.sprite = sprite;
        }

        // 🔥 수정: BackFemale 사용해야 함
        if (!string.IsNullOrEmpty(_currentPokemonData.Sprites.BackFemale))
        {
            var sprite = await PokemonSpriteLoader.Instance
                .LoadSpriteAsync(_currentPokemonData.Sprites.BackFemale, token);

            femalebackImage.gameObject.SetActive(true);
            femalebackImage.sprite = sprite;
        }
        else
        {
            femalebackImage.gameObject.SetActive(false);
        }
    }

    #endregion

    #region GIF Playback

    private void PlayCurrentSideGif()
    {
        _gifPlayCts?.Cancel();
        _gifPlayCts = new CancellationTokenSource();

        if (_isShowingFront && _frontFrames != null)
            PlayGif(frontRawImage, _frontFrames, _gifPlayCts.Token).Forget();
        else if (!_isShowingFront && _backFrames != null)
            PlayGif(backRawImage, _backFrames, _gifPlayCts.Token).Forget();
    }

    private async UniTaskVoid PlayGif(
        RawImage target,
        List<GifTexture> frames,
        CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                foreach (var frame in frames)
                {
                    target.texture = frame.m_texture2d;

                    await UniTask.Delay(
                        TimeSpan.FromSeconds(frame.m_delaySec),
                        cancellationToken: token);
                }
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("GIF playback stopped");
        }
    }

    #endregion

    #region Flip

    [Button]
    public void DoFlipCard()
    {
        FlipCard().Forget();
    }
    public async UniTaskVoid FlipCard()
    {
        if (_isShowingFront)
            await FlipToBackAsync();
        else
            await FlipToFrontAsync();
    }

    private async UniTask FlipToFrontAsync()
    {
        await AnimateFlip(true, false);
        _isShowingFront = true;
        PlayCurrentSideGif();
    }

    private async UniTask FlipToBackAsync()
    {
        await AnimateFlip(false, true);
        _isShowingFront = false;
        PlayCurrentSideGif();
    }

    private async UniTask AnimateFlip(bool showFront, bool showBack)
    {
        float elapsed = 0f;
        Vector3 start = transform.localScale;
        Vector3 mid = new Vector3(0f, start.y, start.z);

        while (elapsed < flipDuration / 2f)
        {
            elapsed += Time.deltaTime;
            transform.localScale =
                Vector3.Lerp(start, mid, elapsed / (flipDuration / 2f));
            await UniTask.Yield();
        }

        frontSide.SetActive(showFront);
        backSide.SetActive(showBack);

        elapsed = 0f;
        while (elapsed < flipDuration / 2f)
        {
            elapsed += Time.deltaTime;
            transform.localScale =
                Vector3.Lerp(mid, Vector3.one, elapsed / (flipDuration / 2f));
            await UniTask.Yield();
        }
    }

    #endregion

    private void ShowFront()
    {
        frontSide.SetActive(true);
        backSide.SetActive(false);
        _isShowingFront = true;
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _gifPlayCts?.Cancel();
    }
}
