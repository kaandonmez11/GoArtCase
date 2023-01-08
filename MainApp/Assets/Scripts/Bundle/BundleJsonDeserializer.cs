namespace KaanDonmez.Bundle
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Events;
    using FirebaseScripts;

    public class BundleJsonDeserializer : MonoBehaviour
    {
        public static BundleJsonDeserializer Instance;
        
        [SerializeField] private UnityEvent<ObjectDatas> onDataDownloaded;
        
        internal ObjectDatas datas { get; private set; }

        private const string fileName = "/AssetDatas.json";

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("BundleJsonDeserializer already in use!");
                Destroy(this);
                return;
            }

            Instance = this;
        }


        public async Task GetDatas()
        {
            string jsonUrl = await FirebaseManager.Instance.GetJsonURL();

            WebClient client = new WebClient();
            await client.DownloadFileTaskAsync(new Uri(jsonUrl), Application.persistentDataPath + fileName);

            datas = JsonUtility.FromJson<ObjectDatas>(
                await File.ReadAllTextAsync(Application.persistentDataPath + fileName));

            File.Delete(Application.persistentDataPath + fileName);

            onDataDownloaded?.Invoke(datas);
        }
    }
}