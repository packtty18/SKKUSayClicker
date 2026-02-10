using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonTypeTag : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private Image backgroundImage;

    /// <summary>
    /// 타입별 색상 매핑
    /// </summary>
    private static readonly Dictionary<string, Color> TypeColors = new Dictionary<string, Color>
        {
            { "normal", new Color(0.66f, 0.66f, 0.47f) },      // #A8A878
            { "fire", new Color(0.93f, 0.51f, 0.19f) },        // #F08030
            { "water", new Color(0.40f, 0.56f, 0.93f) },       // #6890F0
            { "electric", new Color(0.98f, 0.82f, 0.21f) },    // #F8D030
            { "grass", new Color(0.47f, 0.78f, 0.30f) },       // #78C850
            { "ice", new Color(0.60f, 0.85f, 0.85f) },         // #98D8D8
            { "fighting", new Color(0.75f, 0.19f, 0.16f) },    // #C03028
            { "poison", new Color(0.63f, 0.25f, 0.63f) },      // #A040A0
            { "ground", new Color(0.88f, 0.75f, 0.42f) },      // #E0C068
            { "flying", new Color(0.66f, 0.56f, 0.94f) },      // #A890F0
            { "psychic", new Color(0.98f, 0.33f, 0.53f) },     // #F85888
            { "bug", new Color(0.66f, 0.75f, 0.13f) },         // #A8B820
            { "rock", new Color(0.72f, 0.63f, 0.22f) },        // #B8A038
            { "ghost", new Color(0.44f, 0.35f, 0.60f) },       // #705898
            { "dragon", new Color(0.44f, 0.22f, 0.98f) },      // #7038F8
            { "dark", new Color(0.44f, 0.35f, 0.28f) },        // #705848
            { "steel", new Color(0.72f, 0.72f, 0.82f) },       // #B8B8D0
            { "fairy", new Color(0.93f, 0.60f, 0.67f) }        // #EE99AC
        };

    /// <summary>
    /// 타입을 설정하고 UI를 업데이트합니다
    /// </summary>
    public void SetType(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
        {
            return;
        }

        string lowerType = typeName.ToLower();

        // 타입 텍스트 설정
        if (typeText != null)
        {
            typeText.text = typeName.ToUpper();
        }

        // 타입에 맞는 배경색 설정
        if (backgroundImage != null && TypeColors.TryGetValue(lowerType, out Color color))
        {
            backgroundImage.color = color;
        }
        else if (backgroundImage != null)
        {
            // 기본 색상 (회색)
            backgroundImage.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }
}
