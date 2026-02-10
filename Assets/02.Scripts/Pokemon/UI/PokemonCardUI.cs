using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonCardUI : MonoBehaviour
{
    [Header("Card Root Objects")]
    [SerializeField] private GameObject frontSide;
    [SerializeField] private GameObject backSide;

    [Header("Front Side - 앞면")]
    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image frontSpriteImage;
    [SerializeField] private Transform typeContainer;
    [SerializeField] private PokemonTypeTag typeTagPrefab;

    [Header("Back Side - 뒷면")]
    [SerializeField] private Image backSpriteImage;
    [SerializeField] private TextMeshProUGUI hpStatText;
    [SerializeField] private TextMeshProUGUI attackStatText;
    [SerializeField] private TextMeshProUGUI defenseStatText;
    [SerializeField] private TextMeshProUGUI specialAttackStatText;
    [SerializeField] private TextMeshProUGUI specialDefenseStatText;
    [SerializeField] private TextMeshProUGUI speedStatText;

    [Header("Animation Settings")]
    [SerializeField] private float flipDuration = 0.3f;

    private PokemonData _currentPokemonData;
    private CancellationTokenSource _cts;
    private bool _isShowingFront = true;

    private void Awake()
    {
        // 초기 상태 설정
        ShowFront();
    }

    /// <summary>
    /// 포켓몬 데이터를 받아 UI를 설정합니다
    /// </summary>
    public async UniTask SetPokemonDataAsync(PokemonData pokemonData, CancellationToken cancellationToken = default)
    {
        if (pokemonData == null)
        {
            Debug.LogError("PokemonData is null");
            return;
        }

        _currentPokemonData = pokemonData;

        // 기존 작업 취소
        _cts?.Cancel();
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        try
        {
            // 앞면 설정
            await SetupFrontSideAsync(_cts.Token);

            // 뒷면 설정
            await SetupBackSideAsync(_cts.Token);
        }
        catch (System.OperationCanceledException)
        {
            Debug.Log("Pokemon card setup was cancelled");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to setup pokemon card: {ex.Message}");
        }
    }

    /// <summary>
    /// 앞면 UI 설정
    /// </summary>
    private async UniTask SetupFrontSideAsync(CancellationToken cancellationToken)
    {
        // ID와 이름 설정
        idText.text = $"No.{_currentPokemonData.Id:000}";
        nameText.text = _currentPokemonData.Name.ToUpper();

        // 전면 이미지 로드
        if (!string.IsNullOrEmpty(_currentPokemonData.Sprites.FrontDefault))
        {
            Sprite frontSprite = await PokemonSpriteLoader.Instance.LoadSpriteAsync(
                _currentPokemonData.Sprites.FrontDefault,
                cancellationToken
            );
            frontSpriteImage.sprite = frontSprite;
        }

        // 타입 태그 설정
        SetupTypeTagsOnFront();
    }

    /// <summary>
    /// 뒷면 UI 설정
    /// </summary>
    private async UniTask SetupBackSideAsync(CancellationToken cancellationToken)
    {
        // 뒷면 이미지 로드
        if (!string.IsNullOrEmpty(_currentPokemonData.Sprites.BackDefault))
        {
            Sprite backSprite = await PokemonSpriteLoader.Instance.LoadSpriteAsync(
                _currentPokemonData.Sprites.BackDefault,
                cancellationToken
            );
            backSpriteImage.sprite = backSprite;
        }

        // 스탯 설정
        SetupStats();
    }

    /// <summary>
    /// 앞면에 포켓몬 타입 태그들을 생성합니다
    /// </summary>
    private void SetupTypeTagsOnFront()
    {
        // 기존 타입 태그 제거
        foreach (Transform child in typeContainer)
        {
            Destroy(child.gameObject);
        }

        // 새 타입 태그 생성
        foreach (var type in _currentPokemonData.Types)
        {
            PokemonTypeTag typeTag = Instantiate(typeTagPrefab, typeContainer);
            typeTag.SetType(type.TypeName);
        }
    }

    /// <summary>
    /// 뒷면에 포켓몬 스탯을 설정합니다
    /// </summary>
    private void SetupStats()
    {
        foreach (var stat in _currentPokemonData.Stats)
        {
            string statValue = $"{stat.BaseStat}";

            switch (stat.StatName.ToLower())
            {
                case "hp":
                    hpStatText.text = statValue;
                    break;
                case "attack":
                    attackStatText.text = statValue;
                    break;
                case "defense":
                    defenseStatText.text = statValue;
                    break;
                case "special-attack":
                    specialAttackStatText.text = statValue;
                    break;
                case "special-defense":
                    specialDefenseStatText.text = statValue;
                    break;
                case "speed":
                    speedStatText.text = statValue;
                    break;
            }
        }
    }

    public void DoFlip()
    {
        FlipCard().Forget();
    }
    /// <summary>
    /// 카드를 뒤집습니다
    /// </summary>
    [Button]
    public async UniTaskVoid FlipCard()
    {
        if (_isShowingFront)
        {
            await FlipToBackAsync();
        }
        else
        {
            await FlipToFrontAsync();
        }
    }

    /// <summary>
    /// 카드 앞면으로 뒤집기
    /// </summary>
    private async UniTask FlipToFrontAsync()
    {
        await AnimateFlip(false, true);
        _isShowingFront = true;
    }

    /// <summary>
    /// 카드 뒷면으로 뒤집기
    /// </summary>
    private async UniTask FlipToBackAsync()
    {
        await AnimateFlip(true, false);
        _isShowingFront = false;
    }

    /// <summary>
    /// 카드 뒤집기 애니메이션
    /// </summary>
    private async UniTask AnimateFlip(bool showFront, bool showBack)
    {
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 middleScale = new Vector3(0f, startScale.y, startScale.z);
        Vector3 endScale = Vector3.one;

        // 절반까지 축소
        while (elapsed < flipDuration / 2f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (flipDuration / 2f);
            transform.localScale = Vector3.Lerp(startScale, middleScale, t);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        // 면 전환
        frontSide.SetActive(showFront);
        backSide.SetActive(showBack);

        // 원래 크기로 복구
        elapsed = 0f;
        while (elapsed < flipDuration / 2f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (flipDuration / 2f);
            transform.localScale = Vector3.Lerp(middleScale, endScale, t);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        transform.localScale = endScale;
    }

    /// <summary>
    /// 앞면 표시
    /// </summary>
    public void ShowFront()
    {
        frontSide.SetActive(true);
        backSide.SetActive(false);
        _isShowingFront = true;
    }

    /// <summary>
    /// 뒷면 표시
    /// </summary>
    public void ShowBack()
    {
        frontSide.SetActive(false);
        backSide.SetActive(true);
        _isShowingFront = false;
    }

    /// <summary>
    /// 현재 표시 중인 면 확인
    /// </summary>
    public bool IsShowingFront => _isShowingFront;

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
