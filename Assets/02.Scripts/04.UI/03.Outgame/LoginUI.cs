using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    private AccountManager _account => AccountManager.Instance;
    private enum SceneMode { Login, Register }
    private SceneMode _mode = SceneMode.Login;

    [Header("UI References")]
    [SerializeField] private GameObject _passwordConfirmObject;

    [SerializeField] private Button _registerButton;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;

    [SerializeField] private TMP_InputField _idField;
    [SerializeField] private TMP_InputField _passwordField;
    [SerializeField] private TMP_InputField _confirmField;
    [SerializeField] private TextMeshProUGUI _messageText;

    private void Start()
    {
        RefreshUI();
        AddButtonEvents();
    }

    private void AddButtonEvents()
    {
        _registerButton.onClick.AddListener(() => SwitchMode(SceneMode.Register));
        _loginButton.onClick.AddListener(OnLoginClicked);
        _confirmButton.onClick.AddListener(OnRegisterClicked);
        _cancelButton.onClick.AddListener(() => SwitchMode(SceneMode.Login));
    }


    private void OnLoginClicked()
    {
        var result = _account.Login(_idField.text, _passwordField.text);
        HandleLoginResult(result);
    }

    private void OnRegisterClicked()
    {
        var result = _account.Register(
            _idField.text,
            _passwordField.text,
            _confirmField.text);

        HandleRegisterResult(result);
    }

    private void HandleLoginResult(ELoginResult result)
    {
        switch (result)
        {
            case ELoginResult.Success:
                SceneManager.LoadScene(1);
                break;
            case ELoginResult.InvalidIdFormat:
                ShowMessage("아이디 형식이 올바르지 않습니다.");
                break;
            case ELoginResult.AccountNotFound:
                ShowMessage("존재하지 않는 계정입니다.");
                break;
            case ELoginResult.InvalidPassword:
                ShowMessage("비밀번호가 올바르지 않습니다.");
                break;
        }
    }

    private void HandleRegisterResult(ERegisterResult result)
    {
        switch (result)
        {
            case ERegisterResult.Success:
                SwitchMode(SceneMode.Login);
                ShowMessage("계정 생성이 완료되었습니다.");
                break;
            case ERegisterResult.InvalidIdFormat:
                ShowMessage("아이디 형식이 올바르지 않습니다.");
                break;
            case ERegisterResult.DuplicatedId:
                ShowMessage("이미 존재하는 아이디입니다.");
                break;
            case ERegisterResult.InvalidPassword:
                ShowMessage("비밀번호가 적절하지 않습니다.");
                break;
            case ERegisterResult.PasswordMismatch:
                ShowMessage("비밀번호가 서로 다릅니다.");
                break;
        }
    }
    private void ShowMessage(string message)
    {
        _messageText.text = message;
    }

    private void SwitchMode(SceneMode mode)
    {
        _mode = mode;
        RefreshUI();
    }

    private void RefreshUI()
    {
        _registerButton.gameObject.SetActive(_mode == SceneMode.Login);
        _loginButton.gameObject.SetActive(_mode == SceneMode.Login);
        _passwordConfirmObject.SetActive(_mode == SceneMode.Register);
        _confirmButton.gameObject.SetActive(_mode == SceneMode.Register);
        _cancelButton.gameObject.SetActive(_mode == SceneMode.Register);

        _idField.text = PlayerPrefs.GetString("LastId", "");
        _passwordField.text = "";
        _confirmField.text = "";
        _messageText.text = "";
    }
}
