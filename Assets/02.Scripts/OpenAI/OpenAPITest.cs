using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenAPITest : MonoBehaviour
{

    //api키 숨기는 방법
    //1. 환경변수 이용
    //2, 깃이그노어 사용(채택)
    [Title("UI 요소")]
    [SerializeField] private TextMeshProUGUI _resultTextUI;
    [SerializeField] private TMP_InputField _promptTextField;
    [SerializeField] private Button _sendButton;

    [Title("api key 필수")]
    [SerializeField, Required] private ApiKeyConfig config;

    private async void Start()
    {
        var api = new OpenAIClient(config.OpenAIKey);
        var messages = new List<Message>
        {
            new Message(Role.User, "너는 누구야?")
        };

        var chatRequest = new ChatRequest(messages, Model.GPT4oMini);
        var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
        var choice = response.FirstChoice;
        Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice.Message} | Finish Reason: {choice.FinishReason}");
        _resultTextUI.text = choice.Message;
    }

}
