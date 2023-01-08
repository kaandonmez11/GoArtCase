namespace KaanDonmez.Bundle
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Networking;

    public class BundleDownloader : MonoBehaviour
    {
        public static BundleDownloader Instance;

        [SerializeField] private UnityEvent<GameObject> onAssetDownloaded;
        
        internal float downloadPercent { get; private set; }
        
        private Dictionary<string, AssetBundle> alreadyDownloadedBundles;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("BundleDownloader already in use!");
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            alreadyDownloadedBundles = new Dictionary<string, AssetBundle>();
        }

        public void DownloadAsset(string bundleName, string assetName, string bundleUrl)
        {
            StartCoroutine(DownloadAssetRoutine(bundleName, assetName, bundleUrl));
        }

        private IEnumerator DownloadAssetRoutine(string bundleName, string assetName, string bundleUrl)
        {
            if (alreadyDownloadedBundles.ContainsKey(bundleName))
            {
                onAssetDownloaded?.Invoke(alreadyDownloadedBundles[bundleName].LoadAsset(assetName) as GameObject);
                yield break;
            }

            using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl))
            {
                www.SendWebRequest();

                while (www.result == UnityWebRequest.Result.InProgress)
                {
                    downloadPercent = www.downloadProgress;
                    yield return null;
                }

                if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogWarning("Error : " + www.error);
                }
                else
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                    alreadyDownloadedBundles.Add(bundleName, bundle);

                    GameObject go = bundle.LoadAsset(assetName) as GameObject;
                    onAssetDownloaded?.Invoke(go);

                    yield return new WaitForEndOfFrame();
                }

                www.Dispose();
            }
        }

        private void OnApplicationQuit()
        {
            Caching.ClearCache();
        }
    }
}