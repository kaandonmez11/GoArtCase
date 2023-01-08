using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARCore;

public class NetworkChecker : MonoBehaviour
{
    [SerializeField] private GameObject unreachablePanel;

    private bool lastReachabilityState = true;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (lastReachabilityState != (Application.internetReachability == NetworkReachability.NotReachable))
        {
            lastReachabilityState = Application.internetReachability == NetworkReachability.NotReachable;
            
            unreachablePanel.SetActive(lastReachabilityState);
        }
        
    }
}
