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

    [SerializeField] private TMP_InputField _emailField;
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

        _emailField.onValueChanged.AddListener(OnEmailTextChanged);
    }


    private void OnLoginClicked()
    {
        string email = _emailField.text;
        string password = _passwordField.text;
        SAuthResult result =  AccountManager.Instance.TryLogin(email, password);
        ShowMessage(result.ErrorMessage);
    }

    private void OnRegisterClicked()
    {
        string email = _emailField.text;
        string password = _passwordField.text;
        string confirmPassword = _confirmField.text;
        SAuthResult result = AccountManager.Instance.TryRegister(email, password, confirmPassword);
        ShowMessage(result.ErrorMessage);
    }

    public void OnEmailTextChanged(string value)
    {
        EmailValidator validator = new EmailValidator(AccountManager.Instance.Repository);
        ValidationResult result = validator.Validate(value);

        ShowMessage(result.FirstError);

        if(_mode == SceneMode.Login)
        {
            _loginButton.interactable = result.IsValid;
        }
        else
        {
            _registerButton.interactable = result.IsValid;
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

        _emailField.text = PlayerPrefs.GetString("LastId", "");
        _passwordField.text = "";
        _confirmField.text = "";
        _messageText.text = "";
    }
}
