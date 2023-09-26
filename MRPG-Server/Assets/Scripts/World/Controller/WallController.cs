using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public PlatformController platformController;
    public NeighborPlatformDirection neighborPlatformDirection;

    public static Dictionary<NeighborPlatformDirection, Vector3> DISTANCES =
        new Dictionary<NeighborPlatformDirection, Vector3>();

    public static Dictionary<NeighborPlatformDirection, float> ROTATIONSY =
        new Dictionary<NeighborPlatformDirection, float>();

    static WallController()
    {
        DISTANCES[NeighborPlatformDirection.TOP] = new Vector3(27f, 0f, 0f);
        DISTANCES[NeighborPlatformDirection.TOPRIGHT] = new Vector3(13.47f, 0f, -23.25f);
        DISTANCES[NeighborPlatformDirection.TOPLEFT] = new Vector3(13.47f, 0f, 23.25f);
        DISTANCES[NeighborPlatformDirection.BOTTOM] = new Vector3(-27f, 0f, 0f);
        DISTANCES[NeighborPlatformDirection.BOTTOMRIGHT] = new Vector3(-13.47f, 0f, -23.25f);
        DISTANCES[NeighborPlatformDirection.BOTTOMLEFT] = new Vector3(-13.47f, 0f, 23.25f);

        ROTATIONSY[NeighborPlatformDirection.TOP] = 0f;
        ROTATIONSY[NeighborPlatformDirection.TOPRIGHT] = 60f;
        ROTATIONSY[NeighborPlatformDirection.TOPLEFT] = -60f;
        ROTATIONSY[NeighborPlatformDirection.BOTTOM] = 180f;
        ROTATIONSY[NeighborPlatformDirection.BOTTOMRIGHT] = 120f;
        ROTATIONSY[NeighborPlatformDirection.BOTTOMLEFT] = -120f;
    }

    public void InitializePlatformAndDirection(PlatformController _platformController,
        NeighborPlatformDirection _neighborPlatformDirection)
    {
        platformController = _platformController;
        neighborPlatformDirection = _neighborPlatformDirection;

        transform.parent = platformController.transform;
        transform.position += DISTANCES[_neighborPlatformDirection];
        transform.rotation = Quaternion.Euler(0f, ROTATIONSY[_neighborPlatformDirection], 0f);
    }
}