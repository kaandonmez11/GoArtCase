using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class JSONExporter : EditorWindow
{
    [MenuItem("Tools/KaanDonmez/JSON Exporter")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(JSONExporter));
        window.titleContent.text = "JSON Exporter";
    }

    private enum SavePathType
    {
        StreamingAssets,
        Assets,
        Resources,
        Custom
    }

    private SavePathType _savePathType;
    private string savePath = "";
    private Vector2 scrollPos;

    [SerializeField] private ObjectDatas datas;

    void OnGUI()
    {
        if (datas == null)
        {
            datas = new ObjectDatas();
            datas.objectDatas = new List<ObjectData>();
        }

        _savePathType = (SavePathType)EditorGUILayout.EnumPopup("Save path", _savePathType);

        if (_savePathType == SavePathType.Custom)
        {
            savePath = EditorGUILayout.TextField("Path : ", savePath);
        }


        EditorGUILayout.LabelField("Asset List");
        int newCount = Mathf.Max(0, EditorGUILayout.IntField("Size", datas.objectDatas.Count));
        EditorGUILayout.Space(5);

        while (newCount < datas.objectDatas.Count)
            datas.objectDatas.RemoveAt(datas.objectDatas.Count - 1);
        while (newCount > datas.objectDatas.Count)
            datas.objectDatas.Add(new ObjectData());

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < datas.objectDatas.Count; i++)
        {
            EditorGUILayout.LabelField("Element " + i);
            ObjectData buffer = datas.objectDatas[i];
            buffer.bundleName = EditorGUILayout.TextField("Bundle Name : ", buffer.bundleName);
            buffer.assetName = EditorGUILayout.TextField("Asset Name : ", buffer.assetName);
            buffer.downloadUrl = EditorGUILayout.TextField("URL : ", buffer.downloadUrl);
            EditorGUILayout.Space(5);
            datas.objectDatas[i] = buffer;
        }
        EditorGUILayout.EndScrollView();


        if (GUILayout.Button("Export JSON"))
        {
            switch (_savePathType)
            {
                case SavePathType.StreamingAssets:
                    savePath = Application.streamingAssetsPath;
                    break;
                case SavePathType.Assets:
                    savePath = Application.dataPath;
                    break;
                case SavePathType.Resources:
                    savePath = Application.dataPath + "/Resources";
                    break;
                case SavePathType.Custom:
                    break;
            }
            
            File.WriteAllText(savePath + "/AssetDatas.json", JsonUtility.ToJson(datas, true));
            Debug.Log("JSON saved to - " + savePath);
            
        }
    }

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