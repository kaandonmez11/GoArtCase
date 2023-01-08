namespace KaanDonmez.Prefabs
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Bundle;
    using UI;

    public class ObjectButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI objectNameText;
        [SerializeField] private Button shareButton;
        [SerializeField] private Button placeButton;

        public void Init(ObjectData data)
        {
            objectNameText.text = data.assetName;

            shareButton.onClick.AddListener(() => SetMessageInput(data));
            placeButton.onClick.AddListener(() =>
                {
                    BundleDownloader.Instance.DownloadAsset(data.bundleName, data.assetName, data.downloadUrl);
                }
            );
        }

        public void SetActive(bool isActive)
        {
            shareButton.interactable = isActive;
            placeButton.interactable = isActive;
        }
        
        private void SetMessageInput(ObjectData data)
        {
            MessageUIManager.Instance.SetMessageInput(GetLinkedText(data.assetName, data.downloadUrl));
        }
        private string GetLinkedText(string assetName, string link)
        {
            return
                $"Hi! Have you seen this {assetName} object? <link=\"{link}\"><b><color=green><u>Click and view!<u></color></b></link>";
        }

    }
}