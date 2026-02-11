using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PokemonManager : LocalSingleton<PokemonManager>
{
    [Header("Input Controls")]
    
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    [Header("Settings")]
    [SerializeField] private int minPokemonId = 1;
    [SerializeField] private int maxPokemonId = 1025; // 최신 포켓몬 번호

    private PokemonAPI _apiService;
    private PokemonData _currentPokemonData;
    private CancellationTokenSource _cts;
    private bool _isLoading = false;
    public bool IsLoading => _isLoading;

    public SafeEvent<PokemonData, CancellationToken> OnDataChanged = new SafeEvent<PokemonData, CancellationToken>();


    protected override void Init()
    {
        _apiService = new PokemonAPI();
        SetupButtons();
    }

    private void Start()
    {
        // 초기 포켓몬 로드 (피카츄)
        LoadPokemonById(25).Forget();
    }

    /// <summary>
    /// 버튼 이벤트 설정
    /// </summary>
    private void SetupButtons()
    {
        if (previousButton != null)
        {
            previousButton.onClick.AddListener(() => OnPreviousButtonClicked().Forget());
        }

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(() => OnNextButtonClicked().Forget());
        }
    }

    

    /// <summary>
    /// 이전 버튼 클릭 이벤트
    /// </summary>
    private async UniTaskVoid OnPreviousButtonClicked()
    {
        if (_isLoading || _currentPokemonData == null) return;

        int previousId = _currentPokemonData.Id - 1;
        if (previousId >= minPokemonId)
        {
            await LoadPokemonById(previousId);
        }
    }

    /// <summary>
    /// 다음 버튼 클릭 이벤트
    /// </summary>
    private async UniTaskVoid OnNextButtonClicked()
    {
        if (_isLoading || _currentPokemonData == null) return;

        int nextId = _currentPokemonData.Id + 1;
        if (nextId <= maxPokemonId)
        {
            await LoadPokemonById(nextId);
        }
    }

    /// <summary>
    /// ID로 포켓몬 로드
    /// </summary>
    public async UniTask LoadPokemonById(int pokemonId)
    {
        if (_isLoading) return;

        // 유효성 검사
        if (pokemonId < minPokemonId || pokemonId > maxPokemonId)
        {
            Debug.LogWarning($"Invalid Pokemon ID: {pokemonId}. Must be between {minPokemonId} and {maxPokemonId}");
            return;
        }

        // 기존 작업 취소
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        _isLoading = true;
        UpdateButtonStates(false);

        try
        {
            // API에서 포켓몬 데이터 가져오기
            PokemonData pokemonData = await _apiService.GetPokemonByIdAsync(pokemonId, _cts.Token);

            _currentPokemonData = pokemonData;
            UpdateNavigationButtons();

            _isLoading = false;
            OnDataChanged?.Invoke(pokemonData, _cts.Token);
            Debug.Log($"Successfully loaded Pokemon: {pokemonData.Name} (ID: {pokemonData.Id})");
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Pokemon loading was cancelled");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load Pokemon by ID {pokemonId}: {ex.Message}");
        }
        finally
        {
            UpdateNavigationButtons();
        }
    }

    /// <summary>
    /// 이름으로 포켓몬 로드
    /// </summary>
    public async UniTask LoadPokemonByName(string pokemonName)
    {
        if (_isLoading) return;
        if (string.IsNullOrWhiteSpace(pokemonName)) return;

        // 기존 작업 취소
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        _isLoading = true;
        UpdateButtonStates(false);

        try
        {
            // API에서 포켓몬 데이터 가져오기
            PokemonData pokemonData = await _apiService.GetPokemonByNameAsync(pokemonName.ToLower(), _cts.Token);

            _currentPokemonData = pokemonData;
            UpdateNavigationButtons();

            _isLoading = false;
            OnDataChanged?.Invoke(pokemonData, _cts.Token);
            Debug.Log($"Successfully loaded Pokemon: {pokemonData.Name} (ID: {pokemonData.Id})");
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Pokemon loading was cancelled");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load Pokemon by name '{pokemonName}': {ex.Message}");
        }
        finally
        {
            
            
            UpdateNavigationButtons();
        }
    }

    /// <summary>
    /// 내비게이션 버튼 상태 업데이트 (이전/다음)
    /// </summary>
    private void UpdateNavigationButtons()
    {
        if (_currentPokemonData == null)
        {
            UpdateButtonStates(false);
            return;
        }

        bool canGoPrevious = _currentPokemonData.Id > minPokemonId;
        bool canGoNext = _currentPokemonData.Id < maxPokemonId;

        if (previousButton != null)
        {
            previousButton.interactable = canGoPrevious && !_isLoading;
        }

        if (nextButton != null)
        {
            nextButton.interactable = canGoNext && !_isLoading;
        }

        //if (searchButton != null)
        //{
        //    searchButton.interactable = !_isLoading;
        //}
    }

    /// <summary>
    /// 모든 버튼 상태 업데이트
    /// </summary>
    private void UpdateButtonStates(bool interactable)
    {
        //if (searchButton != null)
        //{
        //    searchButton.interactable = interactable;
        //}

        if (previousButton != null)
        {
            previousButton.interactable = interactable;
        }

        if (nextButton != null)
        {
            nextButton.interactable = interactable;
        }
    }

    /// <summary>
    /// 현재 포켓몬 데이터 반환
    /// </summary>
    public PokemonData GetCurrentPokemonData() => _currentPokemonData;


    protected override void OnDestroy()
    {
        // 버튼 리스너 제거

        if (previousButton != null)
        {
            previousButton.onClick.RemoveAllListeners();
        }

        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
        }

       
        // CancellationToken 정리
        _cts?.Cancel();
        _cts?.Dispose();

        base.OnDestroy();
    }

    
}