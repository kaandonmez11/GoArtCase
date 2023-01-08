namespace KaanDonmez.ComponentInheritances
{
    using TMPro;
    using UnityEngine.EventSystems;
    using Bundle;
    using System.Threading.Tasks;

    public class ClickableText : TextMeshProUGUI, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (textInfo.linkCount > 0)
            {
                string link = textInfo.linkInfo[0].GetLinkID();
                if (!string.IsNullOrEmpty(link))
                {
                    CheckDatasAndDownload(link);
                }
            }
        }

        private async Task CheckDatasAndDownload(string link)
        {
            if (BundleJsonDeserializer.Instance.datas == null)
            {
                await BundleJsonDeserializer.Instance.GetDatas();
            }

            ObjectData data = BundleJsonDeserializer.Instance.datas.objectDatas.Find(data => data.downloadUrl == link);

            BundleDownloader.Instance.DownloadAsset(data.bundleName, data.assetName, data.downloadUrl);
        }
    }
}