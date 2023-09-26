using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager instance;

    public List<PlatformController> platforms = new List<PlatformController>();

    public GameObject platformTemplate;
    public GameObject wallTemplate;
    public GameObject parentObjectForPlatforms;
    public GameObject templateObjectForEnvironmentObjects;
    
    public delegate void GenerationMethod(PlatformController _platform);
    
    public List<GenerationMethod> generationMethods = new List<GenerationMethod>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("PlatformManager instance already exists, destroying object!");
            Destroy(this);
        }

        InitializeGenerationMethods();
    }

    private void InitializeGenerationMethods()
    {
        generationMethods = new List<GenerationMethod>()
        {
            GenerateRandomBush,
            GenerateRandomStone,
            GenerateRandomTree,
        };
    }

    private void Start()
    {
        AddBasePlatform();
    }

    private void AddBasePlatform()
    {
        PlatformController _basePlatform = Instantiate(platformTemplate).GetComponent<PlatformController>();
        _basePlatform.id = 1;
        _basePlatform.InitializeNeighborsAndWallsWithNullValues();
        _basePlatform.SetRestOfMissingNeighborsAsWalls();
        _basePlatform.GenerateBasePlatformEnvironment();
    }

    public void InstantiateNewNeighborPlatform(int _parentPlatformId, int _neighborNumber)
    {
        PlatformController _newPlatform = platforms[_parentPlatformId-1]
            .InstantiateNewNeighborPlatform((NeighborPlatformDirection) _neighborNumber);
        
        RemoveRedundantWallsAndAddAsNeighborToPlatformsNextToThem(_newPlatform);
        
        SendInformationAboutPlatformToAllPlayers(_parentPlatformId, _neighborNumber, _newPlatform);
    }

    private void RemoveRedundantWallsAndAddAsNeighborToPlatformsNextToThem(PlatformController _newPlatform)
    {
        foreach (PlatformController _platform in platforms)
        {
            int _neighborNumber = _platform.GetNeighborNumber(_newPlatform);
            if (_neighborNumber != -1)
            {
                _platform.SetNeighborOnBothPlatforms(_newPlatform, (NeighborPlatformDirection) _neighborNumber);
                _platform.UpdateWalls();
                _newPlatform.UpdateWalls();
                foreach (Client _client in Server.clients.Values)
                {
                    if (_client.player != null)
                    {
                        ServerSend.RemoveWall(_client.id, _platform.id, _neighborNumber);
                    }
                }
            }
        }
    }

    private void SendInformationAboutPlatformToAllPlayers(int _parentPlatformId, int _neighborNumber, PlatformController _newPlatform)
    {

        foreach (Client _client in Server.clients.Values)
        {
            if (_client.player != null)
            {
                ServerSend.RemoveWall(_client.id, _parentPlatformId, _neighborNumber);
                SendInformationAboutPlatform(_client.id, _newPlatform);
            }
        }
    }

    public void SendAllInformationToPlayer(int _clientId)
    {
        foreach (PlatformController _platform in platforms)
        {
            SendInformationAboutPlatform(_clientId, _platform);
        }
    }

    private void SendInformationAboutPlatform(int _clientId, PlatformController _platform)
    {
        
        ServerSend.PlatformCreate(_clientId, _platform);
        
        SendInformationAboutWalls(_clientId, _platform);

        SendInformationAboutEnvObjs(_clientId, _platform);
    }

    private static void SendInformationAboutEnvObjs(int _clientId, PlatformController _platform)
    {
        foreach (EnvironmentObjectController _environmentObject in _platform.environmentObjects)
        {
            ServerSend.CreateEnvironmentObj(_clientId, _environmentObject);
        }
    }

    private static void SendInformationAboutWalls(int _clientId, PlatformController _platform)
    {
        foreach (KeyValuePair<NeighborPlatformDirection, WallController> _wallKeyValuePair in _platform.walls)
        {
            if (_wallKeyValuePair.Value != null)
            {
                ServerSend.WallCreate(_clientId, _wallKeyValuePair.Value);
            }
        }
    }

    public static void GenerateRandomBush(PlatformController _platform)
    {
        _platform.AddEnvironmentObject("bushes/Bush/" + Random.Range(1,14), GetRandomVector3OnPlatform(_platform), GetRandomRotationOnY());
    }

    public static void GenerateRandomTree(PlatformController _platform)
    {
        _platform.AddEnvironmentObject("trees/BasicTree/" + Random.Range(1,6), GetRandomVector3OnPlatform(_platform), GetRandomRotationOnY());
    }

    public static void GenerateRandomStone(PlatformController _platform)
    {
        _platform.AddEnvironmentObject("stones/SmallStone/" + Random.Range(1,8), GetRandomVector3OnPlatform(_platform), GetRandomRotationOnY());
    }

    private static Quaternion GetRandomRotationOnY()
    {
        return Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);
    }
    
    private static Vector3 GetRandomVector3OnPlatform(PlatformController _platform)
    {
        Vector3 _randomVector3OnPlatform = _platform.transform.position + new Vector3(Random.Range(-26f, 26f), 0f, Random.Range(-26f, 26f));
        
        while (Vector3.Distance(_randomVector3OnPlatform, _platform.transform.position) > 25f)
        {
            _randomVector3OnPlatform = _platform.transform.position + new Vector3(Random.Range(-26f, 26f), 0f, Random.Range(-26f, 26f));
        }

        return _randomVector3OnPlatform;
    }
}