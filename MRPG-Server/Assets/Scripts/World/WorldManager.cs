using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;

    public GameObject homeWorld;

    public GameObject droppedItemTemplate;
    public GameObject chestTemplate;

    public Dictionary<string, WorldController> worlds = new Dictionary<string, WorldController>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("WorldManager Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        worlds.Add("HomeWorld", homeWorld.GetComponent<WorldController>());

        SetLayerConfigurations();
    }

    public void SendClientIntoDungeon(Client _client, int _portalId)
    {
        MovePlayerToWorld(_client.player, PortalManager.instance.GetWorldNameFromPortalId(_portalId));
    }

    public void MovePlayerToWorld(Player _player, string _worldName)
    {
        if (worlds.ContainsKey(_worldName))
        {
            _player.currentWorld = worlds[_worldName];
            _player.transform.parent = worlds[_worldName].playerParent.transform;
        }
        else
        {
            InitializeNewWorld(_worldName);
            MovePlayerToWorld(_player, _worldName);
        }
    }

    private void InitializeNewWorld(string _worldName)
    {
        GameObject _newWorld = new GameObject(_worldName);
        _newWorld.AddComponent<WorldController>();

        worlds.Add(_worldName, _newWorld.GetComponent<WorldController>());
    }


    public void SendAllInformationToPlayer(int _id)
    {
        Server.clients[_id].player.currentWorld.SendAllInformationAboutWorldToPlayer(_id);
    }

    private void SetLayerConfigurations()
    {
        Physics.IgnoreLayerCollision(6, 6);
        Physics.IgnoreLayerCollision(6, 1);
    }

    public WorldController GetWorldControllerFromPlayer(int _playerId)
    {
        Player playerToFind = Server.clients[_playerId].player;
        if (playerToFind == null)
        {
            Debug.LogError("Player with id: " + _playerId + " not online!");
            return null;
        }
        foreach (KeyValuePair<string, WorldController> world in worlds)
        {
            if (world.Value.GetPlayers().Contains(playerToFind))
            {
                return world.Value;
            }
        }
        return null;
    }
}
