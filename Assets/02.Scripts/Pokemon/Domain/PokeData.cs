using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 포켓몬의 기본 정보를 담는 데이터 클래스
/// </summary>
[Serializable]
public class PokemonData
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private int height;
    [SerializeField] private int weight;
    [SerializeField] private int baseExperience;
    [SerializeField] private List<PokemonType> types;   //타입은 여러개일수 있다. 1.electric
    [SerializeField] private List<PokemonStat> stats;   //스텟도 여러개일수 있다.
    [SerializeField] private PokemonSprites sprites;

    public int Id => id;
    public string Name => name;
    public int Height => height;
    public int Weight => weight;
    public int BaseExperience => baseExperience;
    public IReadOnlyList<PokemonType> Types => types;
    public IReadOnlyList<PokemonStat> Stats => stats;
    public PokemonSprites Sprites => sprites;

    public PokemonData(int id, string name, int height, int weight, int baseExperience,
        List<PokemonType> types, List<PokemonStat> stats, PokemonSprites sprites)
    {
        this.id = id;
        this.name = name;
        this.height = height;
        this.weight = weight;
        this.baseExperience = baseExperience;
        this.types = types ?? new List<PokemonType>();
        this.stats = stats ?? new List<PokemonStat>();
        this.sprites = sprites;
    }
}

[Serializable]
public class PokemonType
{
    [SerializeField] private int slot;
    [SerializeField] private string typeName;

    public int Slot => slot;
    public string TypeName => typeName;

    public PokemonType(int slot, string typeName)
    {
        this.slot = slot;
        this.typeName = typeName;
    }
}

[Serializable]
public class PokemonStat
{
    [SerializeField] private string statName;
    [SerializeField] private int baseStat;
    [SerializeField] private int effort;

    public string StatName => statName;
    public int BaseStat => baseStat;
    public int Effort => effort;

    public PokemonStat(string statName, int baseStat, int effort)
    {
        this.statName = statName;
        this.baseStat = baseStat;
        this.effort = effort;
    }
}

[Serializable]
public class PokemonSprites
{
    [SerializeField] private string frontDefault;
    [SerializeField] private string frontShiny;
    [SerializeField] private string backDefault;
    [SerializeField] private string backShiny;

    public string FrontDefault => frontDefault;
    public string FrontShiny => frontShiny;
    public string BackDefault => backDefault;
    public string BackShiny => backShiny;

    public PokemonSprites(string frontDefault, string frontShiny, string backDefault, string backShiny)
    {
        this.frontDefault = frontDefault;
        this.frontShiny = frontShiny;
        this.backDefault = backDefault;
        this.backShiny = backShiny;
    }
}


//api에 담기위한 dto
[Serializable]
public class PokemonApiResponse
{
    public int id;
    public string name;
    public int height;
    public int weight;
    public int base_experience;
    public List<TypeSlot> types;
    public List<StatInfo> stats;
    public SpriteUrls sprites;
}

[Serializable]
public class TypeSlot
{
    public int slot;
    public TypeData type;
}

[Serializable]
public class TypeData
{
    public string name;
    public string url;
}

[Serializable]
public class StatInfo
{
    public int base_stat;
    public int effort;
    public StatData stat;
}

[Serializable]
public class StatData
{
    public string name;
    public string url;
}

[Serializable]
public class SpriteUrls
{
    public string front_default;
    public string front_shiny;
    public string back_default;
    public string back_shiny;
}

[Serializable]
public class PokemonListResponse
{
    public int count;
    public string next;
    public string previous;
    public List<PokemonListItem> results;
}

[Serializable]
public class PokemonListItem
{
    public string name;
    public string url;
}