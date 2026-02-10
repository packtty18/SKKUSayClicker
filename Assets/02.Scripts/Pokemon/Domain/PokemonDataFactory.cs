using System.Collections.Generic;

public static class PokemonDataFactory
{
    public static PokemonData CreateFromApiResponse(PokemonApiResponse response)
    {
        if (response == null)
        {
            throw new System.ArgumentNullException(nameof(response));
        }

        var types = ConvertTypes(response.types);
        var stats = ConvertStats(response.stats);
        var sprites = ConvertSprites(response.sprites);

        return new PokemonData(
            response.id,
            response.name,
            response.height,
            response.weight,
            response.base_experience,
            types,
            stats,
            sprites
        );
    }

    private static List<PokemonType> ConvertTypes(List<TypeSlot> typeSlots)
    {
        var types = new List<PokemonType>();

        if (typeSlots == null) return types;

        foreach (var typeSlot in typeSlots)
        {
            if (typeSlot?.type != null)
            {
                types.Add(new PokemonType(typeSlot.slot, typeSlot.type.name));
            }
        }

        return types;
    }

    private static List<PokemonStat> ConvertStats(List<StatInfo> statInfos)
    {
        var stats = new List<PokemonStat>();

        if (statInfos == null) return stats;

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

    private static PokemonSprites ConvertSprites(SpriteUrls spriteUrls)
    {
        if (spriteUrls == null)
        {
            return new PokemonSprites(null, null, null, null);
        }

        return new PokemonSprites(
            spriteUrls.front_default,
            spriteUrls.front_shiny,
            spriteUrls.back_default,
            spriteUrls.back_shiny
        );
    }
}