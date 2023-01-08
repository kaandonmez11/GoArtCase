namespace KaanDonmez.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Serialization;
    using UnityEngine.UI;
    using Prefabs;
    using Bundle;

    public class ARUIManager : MonoBehaviour
    {
        public static ARUIManager Instance;

        [SerializeField] private ObjectPlacer placer;
        [SerializeField] private Button downloadListButton;

        [FormerlySerializedAs("downloadlistButtonImage")] [FormerlySerializedAs("downloadButtonImage")] [SerializeField]
        private GameObject downloadListButtonImage;

        [FormerlySerializedAs("downloadButtonText")] [SerializeField]
        private GameObject downloadListButtonText;

        [SerializeField] private Transform placeableButtonsParent;
        [SerializeField] private ObjectButton objectButtonPrefab;

        private List<ObjectButton> spawnedObjectButtons;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("ARUIManager already in use!");
                Destroy(this);
                return;
            }

            Instance = this;

            spawnedObjectButtons = new List<ObjectButton>();
            downloadListButton.onClick.AddListener(() => StartCoroutine(DownloadListClick()));
        }

        private IEnumerator DownloadListClick()
        {
            downloadListButton.interactable = false;
            downloadListButtonImage.SetActive(true);
            downloadListButtonText.SetActive(false);
            Task task = BundleJsonDeserializer.Instance.GetDatas();
            yield return new WaitUntil(() => task.IsCompleted);
            Destroy(downloadListButton.gameObject);
        }

        
        public void OnObjectDatasDownloaded(ObjectDatas datas)
        {
            foreach (ObjectData data in datas.objectDatas)
            {
                ObjectButton bt = Instantiate(objectButtonPrefab, placeableButtonsParent);
                bt.Init(data);
                spawnedObjectButtons.Add(bt);
            }
        }
    }
}