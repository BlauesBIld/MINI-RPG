using System.Collections.Generic;
using UnityEngine;

public enum PlatformSize
{
    SMALL = 1,
    BIG,
    HUGE
}

public class PlatformController : MonoBehaviour
{
    public int id;
    public PlatformSize platformSize;
    public Dictionary<int, WallController> walls = new Dictionary<int, WallController>();
    public List<EnviornmentObjectController> envObjs = new List<EnviornmentObjectController>();

    public GameObject parentObjectForEnvObjs;

    private void Awake()
    {
        InitializeWallsWithNullValues();
    }

    private void InitializeWallsWithNullValues()
    {
        for (int i = 1; i < 7; i++)
        {

            walls[i] = null;
        }
    }

    public void InitializeNewPlatform(int platformId, int platformSize)
    {
        SetParameters(platformId, platformSize);
        ((HomeWorldController)HomeWorldController.instance).platforms[id] = this;
    }

    private void SetParameters(int platformId, int platformSize)
    {
        id = platformId;
        this.platformSize = (PlatformSize) platformSize;
    }

    public void AddEnvObj(EnviornmentObjectController _enviornmentObject)
    {
        envObjs.Add(_enviornmentObject);
        _enviornmentObject.transform.parent = parentObjectForEnvObjs.transform;
    }
}
