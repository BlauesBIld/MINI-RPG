using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public string name;

    [NonSerialized] public GameObject playerParent;
    public GameObject itemParent;
    public GameObject interactAblesParent;

    public List<ChestController> chests;
    public List<DroppedItemController> droppedItems;

    private void Awake()
    {
        playerParent = new GameObject("Players");
        playerParent.transform.parent = this.transform;
    }

    public List<Player> GetPlayers()
    {
        return GetComponentsInChildren<Player>().ToList();
    }

    public ChestController FindChest(int _id)
    {
        foreach (ChestController _chest in chests)
        {
            if (_chest.id == _id)
            {
                return _chest;
            }
        }

        return null;
    }

    public ChestController SpawnChest(Vector3 _position)
    {
        ChestController _newChest =
            Instantiate(WorldManager.instance.chestTemplate, _position, Quaternion.identity,
                interactAblesParent.transform).GetComponent<ChestController>();
        SetParametersForChest(_newChest);
        chests.Add(_newChest);
        SendInformationAboutNewChestToPlayersInThisWorld(_newChest);
        return _newChest;
    }

    private void SendInformationAboutNewChestToPlayersInThisWorld(ChestController _chest)
    {
        foreach (Player _player in GetPlayers())
        {
            ServerSend.SpawnChest(_player.id, _chest);
        }
    }

    private void SetParametersForChest(ChestController _chest)
    {
        SetIdForChest(_chest);

        _chest.currentWorld = this;
    }

    private void SetIdForChest(ChestController _chest)
    {
        if (chests.Count <= 0)
        {
            _chest.id = 1;
        }
        else
        {
            _chest.id = chests[chests.Count - 1].id + 1;
        }
    }

    public void SendAllInformationAboutWorldToPlayer(int _clientId)
    {
        foreach (ChestController _chest in chests)
        {
            ServerSend.SpawnChest(_clientId, _chest);
        }
    }

    public void DropItem(Item _item, Transform _itemSpawnLoc)
    {
        DroppedItemController _droppedItem = InitializeDroppedItemControllerWithParams(_item, _itemSpawnLoc);

        droppedItems.Add(_droppedItem);

        foreach (Player _player in GetPlayers())
        {
            for (int i = 0; i < _item.amount; i++)
            {
                ServerSend.SpawnItemWithForce(_player.id, _droppedItem, _itemSpawnLoc.position, _droppedItem.forceDirection);
            }
        }
    }

    private DroppedItemController InitializeDroppedItemControllerWithParams(Item _item, Transform _itemSpawnLoc)
    {
        DroppedItemController _droppedItem =
            Instantiate(WorldManager.instance.droppedItemTemplate, _itemSpawnLoc.position, _itemSpawnLoc.rotation,
                itemParent.transform).GetComponent<DroppedItemController>();
        _droppedItem.item = _item;
        _droppedItem.world = this;
        return _droppedItem;
    }
}