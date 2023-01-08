using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    internal static float GetMappedValue(float value, float currentMin, float currentMax, float targetMin,
        float targetMax)
    {
        return (value - currentMin) / (currentMax - currentMin) * (targetMax - targetMin) + targetMin;
    }

    internal static void SetColorWithMPB(GameObject target, Color color)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        Renderer renderer = target.GetComponent<Renderer>();
        renderer.GetPropertyBlock(mpb);

        if (mpb.GetColor("_Color") != color)
        {
            mpb.SetColor("_Color", color);
            renderer.SetPropertyBlock(mpb);
        }
    }


    internal static void SetScaleZ(this Transform transform, float value)
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value);
    }
}