using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Threading;
using UnityEngine;

public class PokemonCardTester : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PokemonCardUI pokemonCardUI;

    [Header("Test Settings")]
    [SerializeField] private int testPokemonId = 25; // 피카츄

    private PokemonAPI _apiService;
    private CancellationTokenSource _cts;

    private void Awake()
    {
        _apiService = new PokemonAPI();
    }

    private void Start()
    {
        // 자동으로 포켓몬 로드 (테스트용)
        LoadPokemonCard(testPokemonId).Forget();
    }

    /// <summary>
    /// ID로 포켓몬 카드 로드
    /// </summary>
    [Button]
    public async UniTaskVoid LoadPokemonCard(int pokemonId)
    {
        // 기존 작업 취소
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        try
        {
            // API에서 포켓몬 데이터 가져오기
            PokemonData pokemonData = await _apiService.GetPokemonByIdAsync(pokemonId, _cts.Token);

            // 카드 UI에 데이터 설정
            await pokemonCardUI.SetPokemonDataAsync(pokemonData, _cts.Token);

            Debug.Log($"Successfully loaded pokemon card: {pokemonData.Name}");
        }
        catch (System.OperationCanceledException)
        {
            Debug.Log("Pokemon card loading was cancelled");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load pokemon card: {ex.Message}");
        }
    }

    /// <summary>
    /// 이름으로 포켓몬 카드 로드
    /// </summary>
    [Button]
    public async UniTaskVoid LoadPokemonCardByName(string pokemonName)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        try
        {
            PokemonData pokemonData = await _apiService.GetPokemonByNameAsync(pokemonName, _cts.Token);
            await pokemonCardUI.SetPokemonDataAsync(pokemonData, _cts.Token);

            Debug.Log($"Successfully loaded pokemon card: {pokemonData.Name}");
        }
        catch (System.OperationCanceledException)
        {
            Debug.Log("Pokemon card loading was cancelled");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load pokemon card: {ex.Message}");
        }
    }

    /// <summary>
    /// 카드 뒤집기
    /// </summary>
    [Button]
    public void FlipCard()
    {
        pokemonCardUI.FlipCard().Forget();
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    // UI 버튼에서 호출할 수 있는 공개 메서드들
    public void OnLoadPikachuButtonClicked()
    {
        LoadPokemonCard(25).Forget(); // 피카츄
    }

    public void OnLoadCharizardButtonClicked()
    {
        LoadPokemonCard(6).Forget(); // 리자몽
    }

    public void OnLoadMewtwoButtonClicked()
    {
        LoadPokemonCard(150).Forget(); // 뮤츠
    }

    public void OnFlipCardButtonClicked()
    {
        FlipCard();
    }
}

