using System;
using System.Collections.Generic;
using UnityEngine;

public class HomeWorldController : WorldController
{
    public HomeWorldController homeWorldInstance;
    
    public Dictionary<int, PlatformController> platforms = new Dictionary<int, PlatformController>();

    public void RemoveWallFromGame(int _platformId, int _directionToNeighborPlatform)
    {
        if (platforms[_platformId] != null && platforms[_platformId]
            .walls[_directionToNeighborPlatform] != null)
        {
            Destroy(platforms[_platformId].walls[_directionToNeighborPlatform].gameObject);
            platforms[_platformId].walls[_directionToNeighborPlatform] = null;
        }
    }
}