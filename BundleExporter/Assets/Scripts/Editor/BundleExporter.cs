using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEngine;

public class BundleExporter
{
    [MenuItem("Tools/KaanDonmez/Export Bundles")]
    public static void ExportBundles()
    {
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/Bundles",
            BuildAssetBundleOptions.None,
            BuildTarget.Android);
    }
}