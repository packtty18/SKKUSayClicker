using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class PokemonAPI
{
    private const string BaseUrl = "https://pokeapi.co/api/v2";
    private const string PokemonEndpoint = "/pokemon";
    private const string SpeciesEndpoint = "/pokemon-species";

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

    /// <summary>
    /// 포켓몬 종 정보를 가져옵니다 (설명 포함)
    /// </summary>
    public async UniTask<PokemonSpeciesResponse> GetPokemonSpeciesAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid Pokemon ID. ID must be greater than 0.", nameof(id));
        }

        string url = $"{BaseUrl}{SpeciesEndpoint}/{id}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest().WithCancellation(cancellationToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"Failed to fetch Pokemon species: {request.error}");
            }

            try
            {
                PokemonSpeciesResponse speciesResponse = JsonUtility.FromJson<PokemonSpeciesResponse>(request.downloadHandler.text);
                return speciesResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse Pokemon species response: {ex.Message}", ex);
            }
        }
    }

    private async UniTask<PokemonData> FetchPokemonDataAsync(string url, CancellationToken cancellationToken)
    {
        PokemonApiResponse apiResponse;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest().WithCancellation(cancellationToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"Failed to fetch Pokemon data: {request.error}");
            }

            try
            {
                apiResponse = JsonUtility.FromJson<PokemonApiResponse>(request.downloadHandler.text);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse Pokemon data: {ex.Message}", ex);
            }
        }

        // Species 정보 가져오기 (설명 포함)
        string description = "";
        try
        {
            PokemonSpeciesResponse speciesResponse = await GetPokemonSpeciesAsync(apiResponse.id, cancellationToken);
            description = ExtractDescription(speciesResponse);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Failed to fetch species description: {ex.Message}");
            description = "No description available.";
        }

        // PokemonData 생성
        PokemonData pokemonData = PokemonDataFactory.CreateFromApiResponse(apiResponse, description);
        return pokemonData;
    }

    /// <summary>
    /// Species 응답에서 영어 설명을 추출합니다
    /// </summary>
    private string ExtractDescription(PokemonSpeciesResponse speciesResponse)
    {
        if (speciesResponse.flavor_text_entries == null || speciesResponse.flavor_text_entries.Count == 0)
        {
            return "No description available.";
        }

        // 영어 설명 찾기
        foreach (var entry in speciesResponse.flavor_text_entries)
        {
            if (entry.language != null && entry.language.name == "en")
            {
                // 개행 문자 제거 및 정리
                string cleanedText = entry.flavor_text
                    .Replace("\n", " ")
                    .Replace("\f", " ")
                    .Replace("  ", " ")
                    .Trim();
                return cleanedText;
            }
        }

        // 영어 설명이 없으면 첫 번째 설명 반환
        string firstText = speciesResponse.flavor_text_entries[0].flavor_text
            .Replace("\n", " ")
            .Replace("\f", " ")
            .Replace("  ", " ")
            .Trim();
        return firstText;
    }
}
