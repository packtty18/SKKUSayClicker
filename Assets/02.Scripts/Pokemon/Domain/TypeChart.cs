using System.Collections.Generic;

public static class TypeChart
{
    /// <summary>
    /// 모든 포켓몬 타입 목록
    /// </summary>
    public static readonly List<string> AllTypes = new List<string>
        {
            "normal", "fire", "water", "electric", "grass", "ice",
            "fighting", "poison", "ground", "flying", "psychic", "bug",
            "rock", "ghost", "dragon", "dark", "steel", "fairy"
        };

    /// <summary>
    /// 타입별 공격 효과 배수
    /// Key: 방어 타입 (받는 타입)
    /// Value: Dictionary<공격 타입, 효과 배수>
    /// 
    /// 효과 배수:
    /// 2.0 = 효과가 굉장함 (약점)
    /// 1.0 = 보통 효과
    /// 0.5 = 효과가 별로 (저항)
    /// 0.0 = 효과 없음 (면역)
    /// </summary>
    public static readonly Dictionary<string, Dictionary<string, float>> TypeEffectiveness = new Dictionary<string, Dictionary<string, float>>
        {
            // Normal 타입이 받는 데미지
            {
                "normal", new Dictionary<string, float>
                {
                    { "fighting", 2.0f },
                    { "ghost", 0.0f }
                }
            },

            // Fire 타입이 받는 데미지
            {
                "fire", new Dictionary<string, float>
                {
                    { "fire", 0.5f },
                    { "water", 2.0f },
                    { "grass", 0.5f },
                    { "ice", 0.5f },
                    { "ground", 2.0f },
                    { "bug", 0.5f },
                    { "rock", 2.0f },
                    { "steel", 0.5f },
                    { "fairy", 0.5f }
                }
            },

            // Water 타입이 받는 데미지
            {
                "water", new Dictionary<string, float>
                {
                    { "fire", 0.5f },
                    { "water", 0.5f },
                    { "electric", 2.0f },
                    { "grass", 2.0f },
                    { "ice", 0.5f },
                    { "steel", 0.5f }
                }
            },

            // Electric 타입이 받는 데미지
            {
                "electric", new Dictionary<string, float>
                {
                    { "electric", 0.5f },
                    { "ground", 2.0f },
                    { "flying", 0.5f },
                    { "steel", 0.5f }
                }
            },

            // Grass 타입이 받는 데미지
            {
                "grass", new Dictionary<string, float>
                {
                    { "fire", 2.0f },
                    { "water", 0.5f },
                    { "electric", 0.5f },
                    { "grass", 0.5f },
                    { "ice", 2.0f },
                    { "poison", 2.0f },
                    { "ground", 0.5f },
                    { "flying", 2.0f },
                    { "bug", 2.0f }
                }
            },

            // Ice 타입이 받는 데미지
            {
                "ice", new Dictionary<string, float>
                {
                    { "fire", 2.0f },
                    { "ice", 0.5f },
                    { "fighting", 2.0f },
                    { "rock", 2.0f },
                    { "steel", 2.0f }
                }
            },

            // Fighting 타입이 받는 데미지
            {
                "fighting", new Dictionary<string, float>
                {
                    { "flying", 2.0f },
                    { "psychic", 2.0f },
                    { "bug", 0.5f },
                    { "rock", 0.5f },
                    { "dark", 0.5f },
                    { "fairy", 2.0f }
                }
            },

            // Poison 타입이 받는 데미지
            {
                "poison", new Dictionary<string, float>
                {
                    { "grass", 0.5f },
                    { "fighting", 0.5f },
                    { "poison", 0.5f },
                    { "ground", 2.0f },
                    { "psychic", 2.0f },
                    { "bug", 0.5f },
                    { "fairy", 0.5f }
                }
            },

            // Ground 타입이 받는 데미지
            {
                "ground", new Dictionary<string, float>
                {
                    { "water", 2.0f },
                    { "electric", 0.0f },
                    { "grass", 2.0f },
                    { "ice", 2.0f },
                    { "poison", 0.5f },
                    { "rock", 0.5f }
                }
            },

            // Flying 타입이 받는 데미지
            {
                "flying", new Dictionary<string, float>
                {
                    { "electric", 2.0f },
                    { "grass", 0.5f },
                    { "ice", 2.0f },
                    { "fighting", 0.5f },
                    { "ground", 0.0f },
                    { "bug", 0.5f },
                    { "rock", 2.0f }
                }
            },

            // Psychic 타입이 받는 데미지
            {
                "psychic", new Dictionary<string, float>
                {
                    { "fighting", 0.5f },
                    { "psychic", 0.5f },
                    { "bug", 2.0f },
                    { "ghost", 2.0f },
                    { "dark", 2.0f }
                }
            },

            // Bug 타입이 받는 데미지
            {
                "bug", new Dictionary<string, float>
                {
                    { "fire", 2.0f },
                    { "grass", 0.5f },
                    { "fighting", 0.5f },
                    { "ground", 0.5f },
                    { "flying", 2.0f },
                    { "rock", 2.0f }
                }
            },

            // Rock 타입이 받는 데미지
            {
                "rock", new Dictionary<string, float>
                {
                    { "normal", 0.5f },
                    { "fire", 0.5f },
                    { "water", 2.0f },
                    { "grass", 2.0f },
                    { "fighting", 2.0f },
                    { "poison", 0.5f },
                    { "ground", 2.0f },
                    { "flying", 0.5f },
                    { "steel", 2.0f }
                }
            },

            // Ghost 타입이 받는 데미지
            {
                "ghost", new Dictionary<string, float>
                {
                    { "normal", 0.0f },
                    { "fighting", 0.0f },
                    { "poison", 0.5f },
                    { "bug", 0.5f },
                    { "ghost", 2.0f },
                    { "dark", 2.0f }
                }
            },

            // Dragon 타입이 받는 데미지
            {
                "dragon", new Dictionary<string, float>
                {
                    { "fire", 0.5f },
                    { "water", 0.5f },
                    { "electric", 0.5f },
                    { "grass", 0.5f },
                    { "ice", 2.0f },
                    { "dragon", 2.0f },
                    { "fairy", 2.0f }
                }
            },

            // Dark 타입이 받는 데미지
            {
                "dark", new Dictionary<string, float>
                {
                    { "fighting", 2.0f },
                    { "psychic", 0.0f },
                    { "bug", 2.0f },
                    { "ghost", 0.5f },
                    { "dark", 0.5f },
                    { "fairy", 2.0f }
                }
            },

            // Steel 타입이 받는 데미지
            {
                "steel", new Dictionary<string, float>
                {
                    { "normal", 0.5f },
                    { "fire", 2.0f },
                    { "grass", 0.5f },
                    { "ice", 0.5f },
                    { "fighting", 2.0f },
                    { "poison", 0.0f },
                    { "ground", 2.0f },
                    { "flying", 0.5f },
                    { "psychic", 0.5f },
                    { "bug", 0.5f },
                    { "rock", 0.5f },
                    { "dragon", 0.5f },
                    { "steel", 0.5f },
                    { "fairy", 0.5f }
                }
            },

            // Fairy 타입이 받는 데미지
            {
                "fairy", new Dictionary<string, float>
                {
                    { "fighting", 0.5f },
                    { "poison", 2.0f },
                    { "bug", 0.5f },
                    { "dragon", 0.0f },
                    { "dark", 0.5f },
                    { "steel", 2.0f }
                }
            }
        };
}