using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObjectController : MonoBehaviour
{
    public int id;
    public PlatformController platform;
    public string path;

    
    public void SetNameInScene()
    {
        gameObject.name = path.Split('/')[1] + "_" + id;
    }

    public void InitializeParameters(int _id, PlatformController _platform, int _localIdOnPlatform, string _name)
    {
        id = _id;
        platform = _platform;
        name = _name;
    }

    void OnDestroy()
    {
        platform.environmentObjects.Remove(this);
    }

    public int GetLocalIdOnPlatform()
    {
        return platform.environmentObjects.IndexOf(this);
    }
    
}
