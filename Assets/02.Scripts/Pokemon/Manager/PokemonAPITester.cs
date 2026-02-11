using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Threading;
using UnityEngine;

public class PokemonAPITester : MonoBehaviour
{

    [SerializeField] private int pokemonId = 1;
    [SerializeField] private string pokemonName = "pikachu";
    [SerializeField] private int listLimit = 20;
    [SerializeField] private int listOffset = 0;

    private PokemonAPI _apiService;
    private CancellationTokenSource _cancellationTokenSource;

    private void Awake()
    {
        _apiService = new PokemonAPI();
    }

    private void Start()
    {
        // 사용 예제들 (필요한 것만 활성화하세요)
        // FetchPokemonById().Forget();
        // FetchPokemonByName().Forget();
        // FetchPokemonList().Forget();
    }

    /// <summary>
    /// ID로 포켓몬 정보 가져오기 예제
    /// </summary>
    [Button]
    public async UniTaskVoid FetchPokemonById()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        try
        {
            PokemonData pokemonData = await _apiService.GetPokemonByIdAsync(pokemonId, _cancellationTokenSource.Token);
            OnPokemonDataReceived(pokemonData);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Pokemon fetch was cancelled");
        }
        catch (Exception ex)
        {
            OnApiError(ex.Message);
        }
    }

    /// <summary>
    /// 이름으로 포켓몬 정보 가져오기 예제
    /// </summary>
    [Button]
    public async UniTaskVoid FetchPokemonByName()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        try
        {
            PokemonData pokemonData = await _apiService.GetPokemonByNameAsync(pokemonName, _cancellationTokenSource.Token);
            OnPokemonDataReceived(pokemonData);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Pokemon fetch was cancelled");
        }
        catch (Exception ex)
        {
            OnApiError(ex.Message);
        }
    }

    /// <summary>
    /// 포켓몬 목록 가져오기 예제
    /// </summary>
    [Button]
    public async UniTaskVoid FetchPokemonList()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        try
        {
            PokemonListResponse listResponse = await _apiService.GetPokemonListAsync(
                listLimit,
                listOffset,
                _cancellationTokenSource.Token
            );
            OnPokemonListReceived(listResponse);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Pokemon list fetch was cancelled");
        }
        catch (Exception ex)
        {
            OnApiError(ex.Message);
        }
    }

    private void OnPokemonDataReceived(PokemonData pokemonData)
    {
        Debug.Log($"=== Pokemon Data Received ===");
        Debug.Log($"ID: {pokemonData.Id}");
        Debug.Log($"Name: {pokemonData.Name}");
        Debug.Log($"Height: {pokemonData.Height}");
        Debug.Log($"Weight: {pokemonData.Weight}");
        Debug.Log($"Base Experience: {pokemonData.BaseExperience}");

        Debug.Log($"\nTypes:");
        foreach (var type in pokemonData.Types)
        {
            Debug.Log($"  - Slot {type.Slot}: {type.TypeName}");
        }

        Debug.Log($"\nStats:");
        foreach (var stat in pokemonData.Stats)
        {
            Debug.Log($"  - {stat.StatName}: {stat.BaseStat} (Effort: {stat.Effort})");
        }

        Debug.Log($"\nSprites:");
        Debug.Log($"  - Front Default: {pokemonData.Sprites.FrontDefault}");
        Debug.Log($"  - Front Female: {pokemonData.Sprites.FrontFemale}");
        Debug.Log($"  - Back Default: {pokemonData.Sprites.BackDefault}");
        Debug.Log($"  - Back Female: {pokemonData.Sprites.BackFemale}");
    }

    private void OnPokemonListReceived(PokemonListResponse listResponse)
    {
        Debug.Log($"=== Pokemon List Received ===");
        Debug.Log($"Total Count: {listResponse.count}");
        Debug.Log($"Next Page: {listResponse.next}");
        Debug.Log($"Previous Page: {listResponse.previous}");
        Debug.Log($"\nPokemon in this page:");

        foreach (var pokemon in listResponse.results)
        {
            Debug.Log($"  - {pokemon.name}: {pokemon.url}");
        }
    }

    private void OnApiError(string errorMessage)
    {
        Debug.LogError($"API Error: {errorMessage}");
    }

    private void OnDestroy()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }
}
