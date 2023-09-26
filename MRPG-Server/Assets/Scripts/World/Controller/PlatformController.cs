using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;
using Newtonsoft.Json;

public enum NeighborPlatformDirection
{
    TOP = 1,
    TOPRIGHT,
    TOPLEFT,
    BOTTOM,
    BOTTOMLEFT,
    BOTTOMRIGHT,
}

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

    public GameObject parentEnvironmentObject;

    public static Dictionary<NeighborPlatformDirection, Vector3> DISTANCES =
        new Dictionary<NeighborPlatformDirection, Vector3>();

    public Dictionary<NeighborPlatformDirection, PlatformController> neighbors =
        new Dictionary<NeighborPlatformDirection, PlatformController>();

    public Dictionary<NeighborPlatformDirection, WallController> walls =
        new Dictionary<NeighborPlatformDirection, WallController>();

    public List<EnvironmentObjectController> environmentObjects = new List<EnvironmentObjectController>();

    static PlatformController()
    {
        DISTANCES[NeighborPlatformDirection.TOP] = new Vector3(52f, 0f, 0f);
        DISTANCES[NeighborPlatformDirection.TOPRIGHT] = new Vector3(26f, 0f, -45f);
        DISTANCES[NeighborPlatformDirection.TOPLEFT] = new Vector3(26f, 0f, 45f);
        DISTANCES[NeighborPlatformDirection.BOTTOM] = new Vector3(-52f, 0f, 0f);
        DISTANCES[NeighborPlatformDirection.BOTTOMRIGHT] = new Vector3(-26f, 0f, -45f);
        DISTANCES[NeighborPlatformDirection.BOTTOMLEFT] = new Vector3(-26f, 0f, 45f);
    }

    private void Start()
    {
        PlatformManager.instance.platforms.Add(this);
        transform.parent = PlatformManager.instance.parentObjectForPlatforms.transform;
        InitializePlatformChest();
    }

    public void InitializeNeighborsAndWallsWithNullValues()
    {
        foreach (NeighborPlatformDirection _neighborDirection in (NeighborPlatformDirection[])Enum.GetValues(
            typeof(NeighborPlatformDirection)))
        {
            neighbors[_neighborDirection] = null;
            walls[_neighborDirection] = null;
        }
    }

    public PlatformController InstantiateNewNeighborPlatform(NeighborPlatformDirection neighborNumber)
    {
        PlatformController _newPlatform =
            Instantiate(PlatformManager.instance.platformTemplate).GetComponent<PlatformController>();
        _newPlatform.id = PlatformManager.instance.platforms[PlatformManager.instance.platforms.Count - 1].id + 1;
        _newPlatform.InitializeNeighborsAndWallsWithNullValues();
        SetNeighborOnBothPlatforms(_newPlatform, neighborNumber);
        _newPlatform.SetRestOfMissingNeighborsAsWalls();
        _newPlatform.transform.position = transform.position + DISTANCES[neighborNumber];
        UpdateWalls();

        _newPlatform.GenerateRandomEnvironmentObjectsWithPositions();

        return _newPlatform;
    }

    public static NeighborPlatformDirection GetOppositeNeigborDirection(NeighborPlatformDirection _neighborNumber)
    {
        return (int)_neighborNumber > 3 ? _neighborNumber - 3 : _neighborNumber + 3;
    }

    public void UpdateWalls()
    {
        foreach (NeighborPlatformDirection _neighborDirection in (NeighborPlatformDirection[])Enum.GetValues(
            typeof(NeighborPlatformDirection)))
        {
            if (neighbors[_neighborDirection] == null)
            {
                if (walls[_neighborDirection] == null)
                {
                    walls[_neighborDirection] = Instantiate(PlatformManager.instance.wallTemplate)
                        .GetComponent<WallController>();
                }
            }
            else
            {
                if (walls[_neighborDirection] != null)
                {
                    Destroy(walls[_neighborDirection].gameObject);
                    walls[_neighborDirection] = null;
                }
            }
        }
    }

    public void SetNeighborOnBothPlatforms(PlatformController _newPlatform,
        NeighborPlatformDirection _newNeighborDirection)
    {
        NeighborPlatformDirection _oppositeDirection = GetOppositeNeigborDirection(_newNeighborDirection);

        neighbors[_newNeighborDirection] = _newPlatform;
        _newPlatform.neighbors[_oppositeDirection] = this;
    }

    public void SetRestOfMissingNeighborsAsWalls()
    {
        foreach (KeyValuePair<NeighborPlatformDirection, PlatformController> neighborKeyValuePair in neighbors)
        {
            if (neighborKeyValuePair.Value == null)
            {
                walls[neighborKeyValuePair.Key] =
                    Instantiate(PlatformManager.instance.wallTemplate).GetComponent<WallController>();
                walls[neighborKeyValuePair.Key].InitializePlatformAndDirection(this, neighborKeyValuePair.Key);
            }
        }
    }

    public int GetID()
    {
        return id;
    }

    public int GetNeighborNumber(PlatformController _platform)
    {
        foreach (KeyValuePair<NeighborPlatformDirection, Vector3> distancePair in DISTANCES)
        {
            if (_platform.gameObject.transform.position == transform.position + distancePair.Value)
            {
                return (int)distancePair.Key;
            }
        }

        return -1;
    }

    public void GenerateRandomEnvironmentObjectsWithPositions()
    {
        int amountOfObjects;
        int minForSmall = 20, maxForSmall = 30;

        switch (platformSize)
        {
            case PlatformSize.BIG:
                amountOfObjects = Random.Range(minForSmall * 3, maxForSmall * 3);
                break;
            case PlatformSize.HUGE:
                amountOfObjects = Random.Range(minForSmall * 7, maxForSmall * 7);
                break;
            default:
                amountOfObjects = Random.Range(minForSmall, maxForSmall);
                break;
        }

        for (int i = 0; i < amountOfObjects; i++)
        {
            PlatformManager.instance.generationMethods[Random.Range(0, 3)](this);
        }
    }

    public void GenerateBasePlatformEnvironment()
    {
        List<BasePlatformEnvObj> _basePlatformEnvObjects =
            JsonConvert.DeserializeObject<List<BasePlatformEnvObj>>(
                File.ReadAllText(Application.streamingAssetsPath + "/BasePlatformObjects.json"));
        foreach (BasePlatformEnvObj _basePlatformEnvObj in _basePlatformEnvObjects)
        {   
            AddEnvironmentObject(_basePlatformEnvObj.name,
                new Vector3(_basePlatformEnvObj.x, _basePlatformEnvObj.y, _basePlatformEnvObj.z), Quaternion.identity);
        }

        //Stones somehow didnt get saved in the JSON file, so they will be created randomly each time the server is created, cause im too lazy to add them in the JSON file.
        for (int i = 0; i < 12; i++)
        {
            PlatformManager.GenerateRandomStone(this);
        }
    }

    public void AddEnvironmentObject(string _name, Vector3 _position, Quaternion _rotation)
    {
        EnvironmentObjectController _envObj = Instantiate(Resources.Load<GameObject>("Prefabs/World/"+_name.Split("/")[1]))
            .GetComponent<EnvironmentObjectController>();
        environmentObjects.Add(_envObj);
        SetParametersForEnvironmentObject(_envObj, _name, _position, _rotation);
    }

    private void SetParametersForEnvironmentObject(EnvironmentObjectController _envObj, string _path, Vector3 _position,
        Quaternion _rotation)
    {
        SetIdForEnvironmentObject(_envObj);
        _envObj.path = _path;
        _envObj.transform.position = new Vector3(_position.x, 0f, _position.z);
        _envObj.transform.parent = parentEnvironmentObject.transform;
        _envObj.transform.rotation = _rotation;
        _envObj.SetNameInScene();
    }

    private void SetIdForEnvironmentObject(EnvironmentObjectController _envObj)
    {
        _envObj.platform = this;
        
        if (environmentObjects.Count <= 0)
        {
            _envObj.id = Int32.Parse(id + "" + _envObj.GetLocalIdOnPlatform());
        }
        else
        {
            _envObj.id = Int32.Parse(id + "" + _envObj.GetLocalIdOnPlatform());
        }
    }

    private void InitializePlatformChest()
    {
        ChestController _chest = WorldManager.instance.worlds["HomeWorld"].SpawnChest(transform.position);

        //TODO: Remove this once chest is finished with testing
        _chest.AddItemToDroppingList(new Item());
        _chest.AddItemToDroppingList(new Item());
        _chest.AddItemToDroppingList(new Item());
    }
}