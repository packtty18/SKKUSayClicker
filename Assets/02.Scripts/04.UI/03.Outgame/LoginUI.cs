using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    private AccountManager _account => AccountManager.Instance;

    private enum SceneMode { Login, Register, Ready }
    private SceneMode _mode = SceneMode.Login;

    [Header("ReadyUI References")]
    [SerializeField] private GameObject _OnLogInUIs;
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _logOutButton;

    [Header("LogInUI References")]
    [SerializeField] private GameObject _OnLogOutUIs;
    [SerializeField] private GameObject _passwordConfirmObject;
    [SerializeField] private Button _registerButton;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private TMP_InputField _emailField;
    [SerializeField] private TMP_InputField _passwordField;
    [SerializeField] private TMP_InputField _confirmField;
    [SerializeField] private TextMeshProUGUI _messageText;


    private void Awake()
    {
        if(_account.IsLogin)
        {
            SwitchMode(SceneMode.Ready);
        }
        else
        {
            SwitchMode(SceneMode.Login);
        }

        RefreshUI();
        AddButtonEvents();
    }

    private void AddButtonEvents()
    {
        _gameStartButton.onClick.AddListener(OnGameStartClicked);

        _logOutButton.onClick.AddListener(() => SwitchMode(SceneMode.Login));
        _logOutButton.onClick.AddListener(OnLogOutClicked);
        

        _registerButton.onClick.AddListener(() => SwitchMode(SceneMode.Register));
        _loginButton.onClick.AddListener(OnLoginClicked);
        _confirmButton.onClick.AddListener(OnRegisterClicked);
        _cancelButton.onClick.AddListener(() => SwitchMode(SceneMode.Login));

        // 입력 필드 변경 이벤트
        _emailField.onValueChanged.AddListener(OnEmailTextChanged);
        _passwordField.onValueChanged.AddListener(OnPasswordTextChanged);
        _confirmField.onValueChanged.AddListener(OnPasswordConfirmTextChanged);
    }

    private void OnGameStartClicked()
    {
        MySceneManager.Instance.ChangeScene(ESceneType.Game);
    }

    private void OnLogOutClicked()
    {
        _account.Logout();
        RefreshUI();
    }

    private async void OnLoginClicked()
    {
        string email = _emailField.text;
        string password = _passwordField.text;

        AccountResult result = await _account.TryLoginAsync(email, password);
        
        if (result.IsSuccess)
        {
            ShowMessage("로그인 성공!", Color.green);
            SwitchMode(SceneMode.Ready);
        }
        else
        {
            ShowMessage(result.Message, Color.red);
        }
    }

    private async void OnRegisterClicked()
    {
        string email = _emailField.text;
        string password = _passwordField.text;
        string confirmPassword = _confirmField.text;

        // TryRegisterWithAllErrors를 사용하여 모든 에러를 한 번에 표시
        AccountResult result = await _account.TryRegisterAsync(email, password, confirmPassword);
        
        if (result.IsSuccess)
        {
            ShowMessage("회원가입 성공!", Color.green);
            SwitchMode(SceneMode.Login);
        }
        else
        {
            ShowMessage(result.Message, Color.red);
        }
    }

    private void OnEmailTextChanged(string value)
    {
        UpdateInputValidation();
    }

    private void OnPasswordTextChanged(string value)
    {
        UpdateInputValidation();
    }

    private void OnPasswordConfirmTextChanged(string value)
    {
        UpdateInputValidation();
    }

    private void UpdateInputValidation()
    {
        ValidationResult result;

        if (_mode == SceneMode.Login)
        {
            result = _account.ValidateLoginInput(
                _emailField.text,
                _passwordField.text
            );

            _loginButton.interactable = result.IsValid;
        }
        else
        {
            result = _account.ValidateRegisterInput(
                _emailField.text,
                _passwordField.text,
                _confirmField.text
            );

            _confirmButton.interactable = result.IsValid;
        }

        if (!result.IsValid)
        {
            ShowMessage(result.FirstError, Color.yellow);
        }
        else
        {
            ClearMessage();
        }
    }

    private void ShowMessage(string message, Color color)
    {
        _messageText.text = message;
        _messageText.color = color;
    }

    private void ClearMessage()
    {
        _messageText.text = "";
    }

    private void SwitchMode(SceneMode mode)
    {
        _mode = mode;

        if(mode == SceneMode.Ready)
        {
            _OnLogInUIs.SetActive(true);
            _OnLogOutUIs.SetActive(false);
        }
        else
        {
            _OnLogInUIs.SetActive(false);
            _OnLogOutUIs.SetActive(true);
            RefreshUI();
        }
           
    }

    private void RefreshUI()
    {
        // UI 표시/숨김
        _registerButton.gameObject.SetActive(_mode == SceneMode.Login);
        _loginButton.gameObject.SetActive(_mode == SceneMode.Login);
        _passwordConfirmObject.SetActive(_mode == SceneMode.Register);
        _confirmButton.gameObject.SetActive(_mode == SceneMode.Register);
        _cancelButton.gameObject.SetActive(_mode == SceneMode.Register);

        // 필드 초기화
        _emailField.text = "";
        _passwordField.text = "";
        _confirmField.text = "";

        // 버튼 상태 초기화
        _loginButton.interactable = false;
        _confirmButton.interactable = false;

        // 메시지 초기화
        ClearMessage();

        // 마지막 이메일이 있으면 자동 검증
        if (!string.IsNullOrEmpty(_emailField.text))
        {
            OnEmailTextChanged(_emailField.text);
        }
    }
}
