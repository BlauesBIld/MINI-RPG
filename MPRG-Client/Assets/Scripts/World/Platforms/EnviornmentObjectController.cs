using System;
using UnityEngine;
using UnityEngine.Serialization;

public class EnviornmentObjectController : MonoBehaviour
{
    public int id;
    public PlatformController platform;
    public string name;
    
    public void SetNameInScene()
    {
        gameObject.name = name + "_" + id;
    }

    public void InitializeParameters(int _id, PlatformController _platform, string _name)
    {
        id = _id;
        platform = _platform;
        name = _name;
    }

    private void OnDestroy()
    {
        platform.envObjs.Remove(this);
    }

    public int GetLocalIdOnPlatform()
    {
        return platform.envObjs.IndexOf(this);
    }
}
