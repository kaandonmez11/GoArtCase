namespace KaanDonmez.UI
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using ComponentInheritances;
    using FirebaseScripts;

    public class AuthUIManager : MonoBehaviour
    {
        public static AuthUIManager Instance { get; private set; }

        [Header("General")] [SerializeField] private Animator buttonsAnimator;
        [SerializeField] private AnimatedButtons animatedButtons;
        [SerializeField] private InfoText infoText;

        [Space] [Header("Fields")] [SerializeField]
        private TMP_InputField usernameField;

        [SerializeField] private TMP_InputField emailField;
        [SerializeField] private TMP_InputField passwordField;
        [Space] [SerializeField] private Animator usernameFieldAnimator;
        [SerializeField] private Animator emailFieldAnimator;
        [SerializeField] private Animator passwordFieldAnimator;

        [Space] [Header("Buttons")] [SerializeField]
        private Button loginButton;

        [SerializeField] private Button registerButton;
        [SerializeField] private Button resetPasswordButton;
        [SerializeField] private Button backButton;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("UIManager already in use!");
                Destroy(this);
                return;
            }

            Instance = this;

            loginButton.onClick.AddListener(SendLoginRequest);
            loginButton.onClick.AddListener(() => animatedButtons.SetEnableAll(false));
            registerButton.onClick.AddListener(RegisterToMid);
            resetPasswordButton.onClick.AddListener(ResetToUp);
        }

        public void DisplayMessage(string message, Color color)
        {
            infoText.text = message;
            infoText.color = color;
        }

        public void SetEnableAllButtons(bool enable, bool includeBack = false)
        {
            registerButton.interactable = enable;
            loginButton.interactable = enable;
            resetPasswordButton.interactable = enable;

            backButton.interactable = includeBack && enable;
        }
        
        private void ResetToUp()
        {
            buttonsAnimator.SetTrigger("r2Up");
            loginButton.interactable = false;
            registerButton.interactable = false;
            backButton.interactable = true;

            usernameFieldAnimator.SetTrigger("fadeOut");
            passwordFieldAnimator.SetTrigger("fadeOut");
            emailFieldAnimator.SetTrigger("fadeIn");

            resetPasswordButton.onClick.RemoveListener(ResetToUp);
            resetPasswordButton.onClick.AddListener(SendResetPasswordRequest);
            resetPasswordButton.onClick.AddListener(ResetToDown);

            backButton.onClick.AddListener(ResetToDown);
        }
        private void ResetToDown()
        {
            buttonsAnimator.SetTrigger("r2Down");
            loginButton.interactable = true;
            registerButton.interactable = true;
            backButton.interactable = false;

            usernameFieldAnimator.SetTrigger("fadeIn");
            passwordFieldAnimator.SetTrigger("fadeIn");
            emailFieldAnimator.SetTrigger("fadeOut");

            resetPasswordButton.onClick.RemoveListener(ResetToDown);
            resetPasswordButton.onClick.RemoveListener(SendResetPasswordRequest);
            resetPasswordButton.onClick.AddListener(ResetToUp);

            backButton.onClick.RemoveListener(ResetToDown);
        }
        private void RegisterToMid()
        {
            buttonsAnimator.SetTrigger("s2Mid");
            loginButton.interactable = false;
            resetPasswordButton.interactable = false;
            backButton.interactable = true;

            emailFieldAnimator.SetTrigger("fadeIn");

            registerButton.onClick.RemoveListener(RegisterToMid);
            registerButton.onClick.AddListener(RegisterToSide);
            registerButton.onClick.AddListener(SendRegisterRequest);

            backButton.onClick.AddListener(RegisterToSide);
        }
        private void RegisterToSide()
        {
            buttonsAnimator.SetTrigger("s2Side");
            loginButton.interactable = true;
            resetPasswordButton.interactable = true;
            backButton.interactable = false;

            emailFieldAnimator.SetTrigger("fadeOut");

            registerButton.onClick.RemoveListener(RegisterToSide);
            registerButton.onClick.RemoveListener(SendRegisterRequest);
            registerButton.onClick.AddListener(RegisterToMid);

            backButton.onClick.RemoveListener(RegisterToSide);
        }

        private void SendLoginRequest() => FirebaseManager.Instance.Login(usernameField.text, passwordField.text);
        private void SendRegisterRequest() => FirebaseManager.Instance.Register(usernameField.text, emailField.text, passwordField.text);
        private void SendResetPasswordRequest() => FirebaseManager.Instance.ResetPassword(emailField.text);
        

    }
}