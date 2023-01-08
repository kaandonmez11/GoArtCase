namespace KaanDonmez.ComponentInheritances
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class AnimatedButtons : MonoBehaviour
    {
        [SerializeField] private Button registerButton;
        [SerializeField] private Button loginButton;
        [SerializeField] private Button resetPasswordButton;

        public void SetRegisterButtonText(string text) => SetButtonText(registerButton, text);
        public void SetLoginButtonText(string text) => SetButtonText(loginButton, text);
        public void SetResetPasswordButtonText(string text) => SetButtonText(resetPasswordButton, text);

        public void SetEnableAll(bool enable)
        {
            registerButton.interactable = enable;
            loginButton.interactable = enable;
            resetPasswordButton.interactable = enable;
        }
        
        private void SetButtonText(Button button, string text)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = text;
        }

    }
}