using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class PokemonAPI
{
    private const string BaseUrl = "https://pokeapi.co/api/v2";
    private const string PokemonEndpoint = "/pokemon";
    private const string BaseImageUrl = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon"; // + /id

    /// <summary>
    /// ID로 포켓몬 정보를 가져옵니다
    /// </summary>
    public async UniTask<PokemonData> GetPokemonByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid Pokemon ID. ID must be greater than 0.", nameof(id));
        }

        string url = $"{BaseUrl}{PokemonEndpoint}/{id}";
        return await FetchPokemonDataAsync(url, cancellationToken);
    }

    /// <summary>
    /// 이름으로 포켓몬 정보를 가져옵니다
    /// </summary>
    public async UniTask<PokemonData> GetPokemonByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Invalid Pokemon name. Name cannot be null or empty.", nameof(name));
        }

        string url = $"{BaseUrl}{PokemonEndpoint}/{name.ToLower()}";
        return await FetchPokemonDataAsync(url, cancellationToken);
    }

    /// <summary>
    /// 포켓몬 목록을 가져옵니다 (페이지네이션 지원)
    /// </summary>
    public async UniTask<PokemonListResponse> GetPokemonListAsync(int limit, int offset, CancellationToken cancellationToken = default)
    {
        if (limit <= 0 || offset < 0)
        {
            throw new ArgumentException("Invalid pagination parameters.");
        }

        string url = $"{BaseUrl}{PokemonEndpoint}?limit={limit}&offset={offset}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest().WithCancellation(cancellationToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"Failed to fetch Pokemon list: {request.error}");
            }

            try
            {
                PokemonListResponse listResponse = JsonUtility.FromJson<PokemonListResponse>(request.downloadHandler.text);
                return listResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse Pokemon list response: {ex.Message}", ex);
            }
        }
    }

    private async UniTask<PokemonData> FetchPokemonDataAsync(string url, CancellationToken cancellationToken)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest().WithCancellation(cancellationToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"Failed to fetch Pokemon data: {request.error}");
            }

            try
            {
                PokemonApiResponse apiResponse = JsonUtility.FromJson<PokemonApiResponse>(request.downloadHandler.text);
                PokemonData pokemonData = PokemonDataFactory.CreateFromApiResponse(apiResponse);
                return pokemonData;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse Pokemon data: {ex.Message}", ex);
            }
        }
    }
}
