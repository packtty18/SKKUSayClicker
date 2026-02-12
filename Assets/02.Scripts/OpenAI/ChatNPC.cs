using OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Models;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ChatNPC : MonoBehaviour
{
    
    [Title("UI 요소")]
    [SerializeField] private TextMeshProUGUI _resultTextUI;
    [SerializeField] private TMP_InputField _promptTextField;
    [SerializeField] private Button _sendButton;
    [SerializeField] private AudioSource _audioSource;

    [Title("api key 필수")]
    [SerializeField, Required] private ApiKeyConfig config;

    private List<Message> _messages = new List<Message>();

    private void Start()
    {
        //NPC모드 지침 추가(역할, 목적, 표현)
        string systemMessage = string.Empty;
        systemMessage += "역할 : 너는 게임 NPC이며 자신을 실제 게임 세상속 마법사(여)로 표현해야 한다.";
        systemMessage += "목적 : 실제 사람처럼 대화하는 게임 NPC 모드";
        systemMessage += "표현 : 항상 100글자 이내로 답변하며 판타지 마법에 대한 지식을 갖출것";

        _messages.Add(new Message(Role.System, systemMessage));

        _sendButton.onClick.RemoveAllListeners();
        _sendButton.onClick.AddListener(Send);
    }

    private async void Send()
    {
        string prompt = _promptTextField.text;
        if(string.IsNullOrEmpty(prompt))
        {
            return;
        }


        var api = new OpenAIClient(config.OpenAIKey);
        _messages.Add(new Message(Role.User, prompt));

        _promptTextField.text = string.Empty;
        _sendButton.interactable = false;

        var chatRequest = new ChatRequest(_messages, Model.GPT4oMini);
        var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
        var choice = response.FirstChoice;

        _messages.Add(new Message(Role.Assistant, choice.Message));

        Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice.Message} | Finish Reason: {choice.FinishReason}");
        _resultTextUI.text = choice.Message;

        SpeechRequest speech = new SpeechRequest(
            choice.Message, 
            Model.TTS_GPT_4o_Mini,
            Voice.Alloy
        );
        //TTS
        var speechClip = await api.AudioEndpoint.GetSpeechAsync(speech);
        _audioSource.PlayOneShot(speechClip);
        Debug.Log(speechClip);


        _sendButton.interactable = true;

    }
}
