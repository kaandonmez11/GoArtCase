namespace KaanDonmez.UI
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Prefabs;
    using FirebaseScripts;

    public class MessageUIManager : MonoBehaviour
    {
        public static MessageUIManager Instance;

        [SerializeField] private Transform contentParent;
        [SerializeField] private MessagePrefab messagePrefab;
        [Space] [SerializeField] private Button chatButton;
        [SerializeField] private TextMeshProUGUI chatButtonText;
        [Space] [SerializeField] private Button sendButton;
        [SerializeField] private GameObject messagePanel;
        [SerializeField] private TMP_InputField usernameField;
        [SerializeField] private TMP_InputField messageField;

        private bool isChatClosed = true;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("MessageUIManager already in use!");
                Destroy(this);
                return;
            }

            Instance = this;
        }
        private void Start()
        {
            sendButton.onClick.AddListener(SendButtonClick);
            chatButton.onClick.AddListener(OpenChatButtonClick);
            usernameField.onEndEdit.AddListener((text) => UsernameFieldOnEndEdit(text));

            FirebaseMessageManager.Instance.onMessagesUpdated += OnMessagesUpdated;
        }

        public void SetMessageInput(string message)
        {
            if (isChatClosed)
            {
                chatButton.onClick?.Invoke();
            }

            messageField.text = message;
        }
        
        private void OnMessagesUpdated(Message message)
        {
            if (isChatClosed)
            {
                chatButton.onClick?.Invoke();
            }

            AddMessageToChat(message);
        }

        private async Task UsernameFieldOnEndEdit(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                Dictionary<string, Message> messages =
                    await FirebaseMessageManager.Instance.GetOrCreatePM(text);

                foreach (Transform child in contentParent)
                {
                    Destroy(child.gameObject);
                }

                foreach (var message in messages)
                {
                    AddMessageToChat(message.Value);
                }
            }
            else
            {
                usernameField.text = string.Empty;
            }
        }
        
        private void AddMessageToChat(Message message)
        {
            MessagePrefab msg = Instantiate(messagePrefab, contentParent);
            msg.Init(message);
        }

        private void OpenChatButtonClick()
        {
            messagePanel.SetActive(isChatClosed);
            chatButtonText.text = (isChatClosed ? "Close" : "Open") + " Chat";
            isChatClosed = !isChatClosed;
        }

        private void SendButtonClick()
        {
            if (string.IsNullOrEmpty(usernameField.text) || string.IsNullOrEmpty(messageField.text))
            {
                return;
            }

            FirebaseMessageManager.Instance.SendMessageTo(usernameField.text, messageField.text);
        }

        
    }
}