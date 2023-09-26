using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldController : MonoBehaviour
{
    public static WorldController instance;
    
    [NonSerialized] public List<ChestController> chests = new List<ChestController>();
    public List<DroppedItemController> droppedItems = new List<DroppedItemController>();

    public GameObject interactablesParent;

    private string droppedItemPrefabTemplatePath = "Prefabs/Items/DroppedItem";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public GameObject SpawnEnvironmentObj(string _assetName, Vector3 _transformPosition, Quaternion _transformRotation)
    {
        GameObject spawningEnvironmentObject = Instantiate(Resources.Load<GameObject>("Prefabs/" + _assetName));

        SetBasicParametersForSpawningObj(spawningEnvironmentObject, _assetName, _transformPosition,
            _transformRotation);

        return spawningEnvironmentObject;
    }

    private void SetBasicParametersForSpawningObj(GameObject _spawningEnvironmentObject, string _assetName,
        Vector3 _transformPosition, Quaternion _transformRotation)
    {
        SetAssetNameFromPath(_spawningEnvironmentObject, _assetName);
        SetAssetPosition(_spawningEnvironmentObject, _transformPosition);
        _spawningEnvironmentObject.transform.rotation = Quaternion.Euler(0, 0, 0) * _transformRotation;
        _spawningEnvironmentObject.transform.parent = this.transform;
    }

    private void SetAssetPosition(GameObject _spawningEnvironmentObject, Vector3 _transformPosition)
    {
        _spawningEnvironmentObject.transform.position = _transformPosition;
    }
    private static void SetAssetNameFromPath(GameObject spawningEnvironmentObject, string assetName)
    {
        spawningEnvironmentObject.name = assetName.Split('/')[assetName.Split('/').Length - 1];
    }

    public ChestController FindChest(int _chestId)
    {
        foreach (ChestController _chest in chests)
        {
            if (_chest.id == _chestId)
            {
                return _chest;
            }
        }

        return null;
    }

    public ChestController SpawnChest(int _chestId, int _chestType, Vector3 _position, Quaternion _rotation)
    {
        ChestController _newChest =
            Instantiate(Resources.Load<GameObject>("Prefabs/Chests/" + GetChestTypeFromInt(_chestType)), _position,
                _rotation, interactablesParent.transform).GetComponent<ChestController>();
        _newChest.id = _chestId;
        chests.Add(_newChest);
        return _newChest;
    }

    public DroppedItemController SpawnItemWithForce(string _itemPrefabPath, Vector3 _origin, Vector3 _force)
    {
        DroppedItemController _droppedItem =
            Instantiate(Resources.Load<GameObject>(droppedItemPrefabTemplatePath), _origin, Quaternion.identity,
                interactablesParent.transform).GetComponent<DroppedItemController>();
        
        GameObject _itemToBeDisplayed = Resources.Load<GameObject>("Prefabs/Items" + _itemPrefabPath);

        _droppedItem.GetComponent<Rigidbody>().AddForce(_force);
        _droppedItem.SetMeshForFloatingItem(_itemToBeDisplayed.GetComponent<MeshRenderer>().sharedMaterials,
            _itemToBeDisplayed.GetComponent<MeshFilter>().sharedMesh);
        
        droppedItems.Add(_droppedItem);
        
        return _droppedItem;
    }

    private static string GetChestTypeFromInt(int _chestType)
    {
        switch (_chestType)
        {
            case 1: return "SilverChest";
            case 2: return "RareChest";
            case 3: return "GoldenChest";
            case 4: return "WhiteChest";
            case 5: return "VioletChest";
            default: return "GoldenChest";
        }
    }

    public void PlayerPicksUpItem(int _playerId, int _droppedItemIndex)
    {
        droppedItems[_droppedItemIndex].SetPlayerWhoIsPickingUp(_playerId);
    }
}
