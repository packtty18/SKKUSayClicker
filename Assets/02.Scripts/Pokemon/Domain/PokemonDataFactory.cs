using System;
using System.Collections.Generic;

public static class PokemonDataFactory
{
    public static PokemonData CreateFromApiResponse(
        PokemonApiResponse response,
        string description)
    {
        if (response == null)
        {
            throw new ArgumentNullException(nameof(response));
        }

        var types = ConvertTypes(response.types);
        var stats = ConvertStats(response.stats);
        var sprites = ConvertSprites(response.sprites);
        var gifSprites = ConvertGifSprites(response.sprites);

        return new PokemonData(
            response.id,
            response.name,
            response.height,
            response.weight,
            description,
            response.base_experience,
            types,
            stats,
            sprites,
            gifSprites
        );
    }

    private static List<PokemonType> ConvertTypes(List<TypeSlot> typeSlots)
    {
        var types = new List<PokemonType>();

        if (typeSlots == null)
            return types;

        foreach (var typeSlot in typeSlots)
        {
            if (typeSlot?.type != null)
            {
                types.Add(new PokemonType(
                    typeSlot.slot,
                    typeSlot.type.name
                ));
            }
        }

        return types;
    }

    private static List<PokemonStat> ConvertStats(List<StatInfo> statInfos)
    {
        var stats = new List<PokemonStat>();

        if (statInfos == null)
            return stats;

        foreach (var statInfo in statInfos)
        {
            if (statInfo?.stat != null)
            {
                stats.Add(new PokemonStat(
                    statInfo.stat.name,
                    statInfo.base_stat,
                    statInfo.effort
                ));
            }
        }

        return stats;
    }

    private static PokemonSprites ConvertSprites(SpriteRoot spriteRoot)
    {
        if (spriteRoot == null)
        {
            return new PokemonSprites(null, null, null, null);
        }

        return new PokemonSprites(
            spriteRoot.front_default,
            spriteRoot.back_default,
            spriteRoot.front_female,
            spriteRoot.back_female
        );
    }

    private static PokemonGifSprites ConvertGifSprites(SpriteRoot spriteRoot)
    {
        if (spriteRoot == null)
            return new PokemonGifSprites(null, null);

        string frontGif = null;
        string backGif = null;

        var gen5 = spriteRoot.versions?
            .generation_v?
            .black_white?
            .animated;

        if (gen5 != null)
        {
            frontGif = gen5.front_default;
            backGif = gen5.back_default;
        }

        if (string.IsNullOrEmpty(frontGif))
        {
            frontGif = spriteRoot.other?.showdown?.front_default;
        }

        if (string.IsNullOrEmpty(backGif))
        {
            backGif = spriteRoot.other?.showdown?.back_default;
        }

        return new PokemonGifSprites(frontGif, backGif);
    }

}
