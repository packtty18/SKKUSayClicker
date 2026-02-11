using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class pokemonInputUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button searchButton;

    private PokemonManager manager => PokemonManager.Instance;

    private void Awake()
    {
        SetupButtons();
    }
    private void SetupButtons()
    {
        if (searchButton != null)
        {
            searchButton.onClick.AddListener(() => OnSearchButtonClicked().Forget());
        }

        // 엔터키로도 검색 가능
        if (inputField != null)
        {
            inputField.onSubmit.AddListener((value) => OnSearchButtonClicked().Forget());
        }
    }

    /// <summary>
    /// 검색 버튼 클릭 이벤트
    /// </summary>
    private async UniTaskVoid OnSearchButtonClicked()
    {
        if (manager == null || manager.IsLoading)
        {
            return;
        }

        string input = inputField?.text?.Trim();
        inputField.text = "";
        if (string.IsNullOrEmpty(input))
        {
            Debug.LogWarning("Input field is empty");
            return;
        }

        // 숫자인지 문자열인지 확인
        if (int.TryParse(input, out int pokemonId))
        {
            await manager.LoadPokemonById(pokemonId);
        }
        else
        {
            await manager.LoadPokemonByName(input);
        }
    }

    private void OnDestroy()
    {
        // 버튼 리스너 제거
        if (searchButton != null)
        {
            searchButton.onClick.RemoveAllListeners();
        }
        if (inputField != null)
        {
            inputField.onSubmit.RemoveAllListeners();
        }
    }
}
