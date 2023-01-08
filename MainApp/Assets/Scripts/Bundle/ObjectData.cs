namespace KaanDonmez.Bundle
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class ObjectData
    {
        [SerializeField] public string bundleName;
        [SerializeField] public string assetName;
        [SerializeField] public string downloadUrl;
    }

    [Serializable]
    public class ObjectDatas
    {
        [SerializeField] public List<ObjectData> objectDatas;

        public ObjectDatas()
        {
            objectDatas = new List<ObjectData>();
        }
    }
}