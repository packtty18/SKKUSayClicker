using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class PokemonDetailUI : MonoBehaviour
{
    [Header("Basic Info")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI heightText;
    [SerializeField] private TextMeshProUGUI weightText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Type Relations")]
    [SerializeField] private Transform typeContainer;
    [SerializeField] private Transform weakTypeContainer;
    [SerializeField] private Transform resistanceTypeContainer;
    [SerializeField] private Transform immuneTypeContainer;
    [SerializeField] private PokemonTypeTag typeTagPrefab;

    [Header("Stat Sliders")]
    [SerializeField] private PokemonStatSlider hpSlider;
    [SerializeField] private PokemonStatSlider attackSlider;
    [SerializeField] private PokemonStatSlider defenseSlider;
    [SerializeField] private PokemonStatSlider specialAttackSlider;
    [SerializeField] private PokemonStatSlider specialDefenseSlider;
    [SerializeField] private PokemonStatSlider speedSlider;

    private const int MaxStatValue = 255;

    private PokemonData _currentPokemonData;
    private CancellationTokenSource _cts;

    private void Start()
    {
        PokemonManager.Instance.OnDataChanged.Subscribe(SetPokemonData);
    }

    public void SetPokemonData(PokemonData pokemonData, CancellationToken cancellationToken = default)
    {
        SetPokemonDataAsync(pokemonData, cancellationToken).Forget();
    }

    /// <summary>
    /// 포켓몬 데이터를 받아 설명창 UI를 설정합니다
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
            // 기본 정보 설정
            SetBasicInfo();

            // 타입 관계 설정
            await SetTypeRelationsAsync(_cts.Token);

            // 스탯 설정
            SetStats();
        }
        catch (System.OperationCanceledException)
        {
            Debug.Log("Pokemon detail panel setup was cancelled");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to setup pokemon detail panel: {ex.Message}");
        }
    }

    /// <summary>
    /// 기본 정보 설정 (이름, ID, 키, 몸무게)
    /// </summary>
    private void SetBasicInfo()
    {
        if (nameText != null)
        {
            nameText.text = _currentPokemonData.Name.ToUpper();
        }

        if (heightText != null)
        {
            // API에서 높이는 데시미터 단위로 반환됨 (1 = 0.1m)
            float heightInMeters = _currentPokemonData.Height / 10f;
            heightText.text = $"{heightInMeters:F1}m";
        }

        if (weightText != null)
        {
            // API에서 무게는 헥토그램 단위로 반환됨 (1 = 0.1kg)
            float weightInKg = _currentPokemonData.Weight / 10f;
            weightText.text = $"{weightInKg:F1}kg";
        }

        if(descriptionText != null)
        {
            descriptionText.text = _currentPokemonData.Description;
        }
    }

    /// <summary>
    /// 타입 관계 설정 (약점, 저항, 면역)
    /// </summary>
    private async UniTask SetTypeRelationsAsync(CancellationToken cancellationToken)
    {
        // 포켓몬의 타입들을 가져옴
        List<string> pokemonTypes = new List<string>();
        foreach (var type in _currentPokemonData.Types)
        {
            pokemonTypes.Add(type.TypeName);
        }

        foreach (Transform child in typeContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var type in _currentPokemonData.Types)
        {
            PokemonTypeTag typeTag = Instantiate(typeTagPrefab, typeContainer);
            typeTag.SetType(type.TypeName);
        }

        // 타입 관계 계산
        TypeRelations relations = CalculateTypeRelations(pokemonTypes);

        // 약점 타입 표시
        SetupTypeContainer(weakTypeContainer, relations.WeakTo);

        // 저항 타입 표시
        SetupTypeContainer(resistanceTypeContainer, relations.ResistantTo);

        // 면역 타입 표시
        SetupTypeContainer(immuneTypeContainer, relations.ImmuneTo);

        await UniTask.Yield(cancellationToken);
    }

    /// <summary>
    /// 타입 컨테이너에 타입 태그들을 생성합니다
    /// </summary>
    private void SetupTypeContainer(Transform container, List<string> types)
    {
        if (container == null) return;

        // 기존 태그 제거
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // 타입이 없으면 "None" 표시
        if (types == null || types.Count == 0)
        {
            PokemonTypeTag noneTag = Instantiate(typeTagPrefab, container);
            noneTag.SetType("None");
            return;
        }

        // 새 타입 태그 생성
        foreach (string type in types)
        {
            PokemonTypeTag typeTag = Instantiate(typeTagPrefab, container);
            typeTag.SetType(type);
        }
    }

    /// <summary>
    /// 스탯 슬라이더 설정
    /// </summary>
    private void SetStats()
    {
        foreach (var stat in _currentPokemonData.Stats)
        {
            string statName = stat.StatName.ToLower();
            int baseStat = stat.BaseStat;

            switch (statName)
            {
                case "hp":
                    SetStatSlider(hpSlider, "HP", baseStat);
                    break;
                case "attack":
                    SetStatSlider(attackSlider, "Attack", baseStat);
                    break;
                case "defense":
                    SetStatSlider(defenseSlider, "Defense", baseStat);
                    break;
                case "special-attack":
                    SetStatSlider(specialAttackSlider, "Sp. Atk", baseStat);
                    break;
                case "special-defense":
                    SetStatSlider(specialDefenseSlider, "Sp. Def", baseStat);
                    break;
                case "speed":
                    SetStatSlider(speedSlider, "Speed", baseStat);
                    break;
            }
        }
    }

    /// <summary>
    /// 개별 스탯 슬라이더 설정
    /// </summary>
    private void SetStatSlider(PokemonStatSlider statSlider, string statName, int value)
    {
        if (statSlider != null)
        {
            statSlider.SetStat(statName, value, MaxStatValue);
        }
    }

    /// <summary>
    /// 포켓몬 타입 기반으로 타입 관계 계산
    /// </summary>
    private TypeRelations CalculateTypeRelations(List<string> pokemonTypes)
    {
        TypeRelations relations = new TypeRelations();

        // 타입별 효과 배수 저장
        Dictionary<string, float> typeEffectiveness = new Dictionary<string, float>();

        // 모든 타입에 대해 1.0으로 초기화
        foreach (string attackType in TypeChart.AllTypes)
        {
            typeEffectiveness[attackType] = 1.0f;
        }

        // 각 포켓몬 타입에 대해 효과 배수 계산
        foreach (string defenseType in pokemonTypes)
        {
            if (TypeChart.TypeEffectiveness.TryGetValue(defenseType, out var effectiveness))
            {
                foreach (var kvp in effectiveness)
                {
                    typeEffectiveness[kvp.Key] *= kvp.Value;
                }
            }
        }

        // 효과 배수에 따라 분류
        foreach (var kvp in typeEffectiveness)
        {
            if (kvp.Value > 1.0f)
            {
                relations.WeakTo.Add(kvp.Key);
            }
            else if (kvp.Value < 1.0f && kvp.Value > 0.0f)
            {
                relations.ResistantTo.Add(kvp.Key);
            }
            else if (kvp.Value == 0.0f)
            {
                relations.ImmuneTo.Add(kvp.Key);
            }
        }

        return relations;
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}

public class TypeRelations
{
    public List<string> WeakTo = new List<string>();
    public List<string> ResistantTo = new List<string>();
    public List<string> ImmuneTo = new List<string>();
}
