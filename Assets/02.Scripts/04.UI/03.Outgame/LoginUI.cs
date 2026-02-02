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

    // 검증 상태 추적
    private bool _isEmailValid = false;
    private bool _isPasswordValid = false;
    private bool _isPasswordConfirmValid = false;

    private void Start()
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

    private void OnLoginClicked()
    {
        string email = _emailField.text;
        string password = _passwordField.text;

        SAuthResult result = _account.TryLogin(email, password);
        
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

    private void OnRegisterClicked()
    {
        string email = _emailField.text;
        string password = _passwordField.text;
        string confirmPassword = _confirmField.text;

        // TryRegisterWithAllErrors를 사용하여 모든 에러를 한 번에 표시
        SAuthResult result = _account.TryRegister(email, password, confirmPassword);
        
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

    // 이메일 필드 검증
    private void OnEmailTextChanged(string value)
    {
        if (_mode == SceneMode.Login)
        {
            // 로그인 모드: 이메일 형식만 검증
            var emailSpec = new EmailSpecification();
            _isEmailValid = emailSpec.IsSatisfiedBy(value);
            
            if (!_isEmailValid)
            {
                ShowMessage(emailSpec.ErrorMessage, Color.yellow);
            }
            else
            {
                ClearMessage();
            }
        }
        else
        {
            // 회원가입 모드: 전체 이메일 검증 (중복 포함)
            var emailValidator = new EmailValidator(_account.Repository);
            var result = emailValidator.Validate(value);
            _isEmailValid = result.IsValid;
            
            if (!_isEmailValid)
            {
                ShowMessage(result.FirstError, Color.yellow);
            }
            else
            {
                ClearMessage();
            }
        }

        UpdateButtonStates();
    }

    // 비밀번호 필드 검증
    private void OnPasswordTextChanged(string value)
    {
        if (_mode == SceneMode.Login)
        {
            // 로그인 모드: 비밀번호 형식만 검증
            var passwordSpec = new PasswordSpecification();
            _isPasswordValid = passwordSpec.IsSatisfiedBy(value);
            
            if (!_isPasswordValid && !string.IsNullOrEmpty(value))
            {
                ShowMessage(passwordSpec.ErrorMessage, Color.yellow);
            }
            else if (_isEmailValid && _isPasswordValid)
            {
                ClearMessage();
            }
        }
        else
        {
            // 회원가입 모드: 전체 비밀번호 검증
            var passwordValidator = new PasswordValidator();
            var result = passwordValidator.Validate(value);
            _isPasswordValid = result.IsValid;
            
            if (!_isPasswordValid && !string.IsNullOrEmpty(value))
            {
                ShowMessage(result.FirstError, Color.yellow);
            }
            else if (_isEmailValid && _isPasswordValid)
            {
                ClearMessage();
            }

            // 비밀번호 확인 필드도 재검증
            if (!string.IsNullOrEmpty(_confirmField.text))
            {
                OnPasswordConfirmTextChanged(_confirmField.text);
            }
        }

        UpdateButtonStates();
    }

    // 비밀번호 확인 필드 검증 (회원가입 모드만)
    private void OnPasswordConfirmTextChanged(string value)
    {
        if (_mode != SceneMode.Register)
            return;

        var matchSpec = new PasswordMatchSpecification();
        _isPasswordConfirmValid = matchSpec.IsSatisfiedBy((_passwordField.text, value));

        if (!_isPasswordConfirmValid && !string.IsNullOrEmpty(value))
        {
            ShowMessage(matchSpec.ErrorMessage, Color.yellow);
        }
        else if (_isEmailValid && _isPasswordValid && _isPasswordConfirmValid)
        {
            ClearMessage();
        }

        UpdateButtonStates();
    }

    // 버튼 활성화 상태 업데이트
    private void UpdateButtonStates()
    {
        if (_mode == SceneMode.Login)
        {
            // 로그인 버튼: 이메일 + 비밀번호 검증
            _loginButton.interactable = _isEmailValid && _isPasswordValid;
        }
        else
        {
            // 회원가입 확인 버튼: 이메일 + 비밀번호 + 비밀번호 확인 검증
            _confirmButton.interactable = _isEmailValid && _isPasswordValid && _isPasswordConfirmValid;
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
        if (_mode == SceneMode.Login)
        {
            _emailField.text = AccountManager.Instance.GetLastEmail();
        }
        else
        {
            _emailField.text = "";
        }

        _passwordField.text = "";
        _confirmField.text = "";

        // 검증 상태 초기화
        _isEmailValid = false;
        _isPasswordValid = false;
        _isPasswordConfirmValid = false;

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
