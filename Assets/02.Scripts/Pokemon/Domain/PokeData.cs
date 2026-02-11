using System;
using System.Collections.Generic;
using UnityEngine;

#region ======================= DOMAIN =======================

[Serializable]
public class PokemonData
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private int height;
    [SerializeField] private int weight;
    [SerializeField] private string description;
    [SerializeField] private int baseExperience;
    [SerializeField] private List<PokemonType> types;
    [SerializeField] private List<PokemonStat> stats;
    [SerializeField] private PokemonSprites sprites;
    [SerializeField] private PokemonGifSprites gifSprites;

    public int Id => id;
    public string Name => name;
    public int Height => height;
    public int Weight => weight;
    public string Description => description;
    public int BaseExperience => baseExperience;
    public IReadOnlyList<PokemonType> Types => types;
    public IReadOnlyList<PokemonStat> Stats => stats;
    public PokemonSprites Sprites => sprites;
    public PokemonGifSprites GifSprites => gifSprites;

    public PokemonData(
        int id,
        string name,
        int height,
        int weight,
        string description,
        int baseExperience,
        List<PokemonType> types,
        List<PokemonStat> stats,
        PokemonSprites sprites,
        PokemonGifSprites gifSprites)
    {
        this.id = id;
        this.name = name;
        this.height = height;
        this.weight = weight;
        this.description = description ?? "No description available.";
        this.baseExperience = baseExperience;
        this.types = types ?? new List<PokemonType>();
        this.stats = stats ?? new List<PokemonStat>();
        this.sprites = sprites;
        this.gifSprites = gifSprites;
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
    [SerializeField] private string backDefault;
    [SerializeField] private string frontFemale;
    [SerializeField] private string backFemale;

    public string FrontDefault => frontDefault;
    public string BackDefault => backDefault;
    public string FrontFemale => frontFemale;
    public string BackFemale => backFemale;

    public PokemonSprites(
        string frontDefault,
        string backDefault,
        string frontFemale,
        string backFemale)
    {
        this.frontDefault = frontDefault;
        this.backDefault = backDefault;
        this.frontFemale = frontFemale;
        this.backFemale = backFemale;
    }
}

[Serializable]
public class PokemonGifSprites
{
    [SerializeField] private string frontDefaultGif;
    [SerializeField] private string backDefaultGif;

    public string FrontDefaultGif => frontDefaultGif;
    public string BackDefaultGif => backDefaultGif;

    public PokemonGifSprites(
        string frontDefaultGif,
        string backDefaultGif)
    {
        this.frontDefaultGif = frontDefaultGif;
        this.backDefaultGif = backDefaultGif;
    }
}

#endregion


#region ======================= API DTO =======================

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
    public SpriteRoot sprites;
    public SpeciesReference species;
}

[Serializable]
public class SpeciesReference
{
    public string name;
    public string url;
}

[Serializable]
public class PokemonSpeciesResponse
{
    public int id;
    public string name;
    public List<FlavorTextEntry> flavor_text_entries;
}

[Serializable]
public class FlavorTextEntry
{
    public string flavor_text;
    public LanguageReference language;
    public VersionReference version;
}

[Serializable]
public class LanguageReference
{
    public string name;
    public string url;
}

[Serializable]
public class VersionReference
{
    public string name;
    public string url;
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

#region ===== Sprite DTO =====

[Serializable]
public class SpriteRoot
{
    public string front_default;
    public string back_default;
    public string front_female;
    public string back_female;
    public OtherSprites other;
    public Versions versions; 
}

[Serializable]
public class OtherSprites
{
    public ShowdownSprites showdown;
}

[Serializable]
public class ShowdownSprites
{
    public string front_default;
    public string back_default;
}

#endregion


#region ===== List Response =====

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

#endregion

#endregion


[Serializable]
public class Versions
{
    public GenerationV generation_v;
}

[Serializable]
public class GenerationV
{
    public BlackWhite black_white;
}

[Serializable]
public class BlackWhite
{
    public AnimatedSprites animated;
}

[Serializable]
public class AnimatedSprites
{
    public string front_default;
    public string back_default;
}
