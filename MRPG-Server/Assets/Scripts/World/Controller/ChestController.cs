using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ChestType
{
    Normal = 1,
    Rare,
    Legendary,
    White,
    Violet,
    Boss
}

public class ChestController : InteractableController
{
    public int id;

    public ChestType chestType;
    public WorldController currentWorld;
    public GameObject itemSpawnLoc;

    private List<Item> itemsToBeDropped = new List<Item>();

    private float itemSpawnDelay = 2.2f;
    private float itemSpawnDelayIterationOffset = 0.5f;
    
    public void Open()
    {
        DropItems(itemsToBeDropped);
        
        foreach (Player _player in currentWorld.GetPlayers())
        {
            ServerSend.OpenChest(_player.id, id);
        }
    }

    public IEnumerator DropItem(Item _item, int _iterationNr = 0)
    {
        yield return new WaitForSeconds(itemSpawnDelay + itemSpawnDelayIterationOffset * _iterationNr);
        currentWorld.DropItem(_item, itemSpawnLoc.transform);
    }

    public void DropItems(List<Item> _items)
    {
        for (int i = 0; i < itemsToBeDropped.Count; i++)
        {
            StartCoroutine(DropItem(itemsToBeDropped[i], i));
        }
    }

    private Vector3 GetRandomPositionInFrontOfChest()
    {
        float _chestX = transform.position.x;
        float _chestZ = transform.position.z;

        Vector3 _randomPositionInFrontOfChest = new Vector3(Random.Range(_chestX - 2, _chestX + 2), 999f,
            Random.Range(_chestZ - 2, _chestZ + 2));
        RaycastHit _hit;
        
        
        if (Physics.Raycast(_randomPositionInFrontOfChest, -Vector3.up, out _hit, Mathf.Infinity))
        {
            _randomPositionInFrontOfChest = Vector3.Scale(_randomPositionInFrontOfChest, transform.forward);
            _randomPositionInFrontOfChest.y = _hit.point.y;
        }

        return _randomPositionInFrontOfChest;
    }

    public void AddItemToDroppingList(Item _item)
    {
        itemsToBeDropped.Add(_item);
    }
}