using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    /// <summary>Sends a packet to a client via TCP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to a client via UDP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    /// <summary>Sends a packet to all clients via TCP.</summary>
    /// <param name="_packet">The packet to send.</param>
    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }
    /// <summary>Sends a packet to all clients except one via TCP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    /// <summary>Sends a packet to all clients via UDP.</summary>
    /// <param name="_packet">The packet to send.</param>
    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    /// <summary>Sends a packet to all clients except one via UDP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }

    #region Packets
    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }
    
    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTCPData(_toClient, _packet);
        }
    }
    
    public static void PlayerServerMovement(Player _player, Vector3 _movingDirection)
    {
        using (Packet _packet = new Packet((int) ServerPackets.playerServerMovement))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);
            _packet.Write(_movingDirection);

            SendUDPDataToAll(_player.id, _packet);
        }
    }
    
    public static void PlayerDisconnected(int _playerId)
    {
        using (Packet _packet = new Packet((int) ServerPackets.playerDisconnected))
        {
            _packet.Write(_playerId);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PlatformCreate(int _toClient, PlatformController _hexaPlatform)
    {
        using (Packet _packet = new Packet((int) ServerPackets.platformCreate))
        {
            _packet.Write(_hexaPlatform.GetID());
            _packet.Write((int) _hexaPlatform.platformSize);
            _packet.Write(_hexaPlatform.gameObject.transform.position);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void WallCreate(int _toClient, WallController _wallController)
    {
        using (Packet _packet = new Packet((int) ServerPackets.wallCreate))
        {
            _packet.Write(_wallController.platformController.id);
            _packet.Write((int) _wallController.neighborPlatformDirection);
            _packet.Write(_wallController.transform.position);
            _packet.Write(WallController.ROTATIONSY[_wallController.neighborPlatformDirection]);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void RemoveWall(int _toClient, int _parentPlatformId, int _neighborNumber)
    {
        using (Packet _packet = new Packet((int) ServerPackets.wallRemove))
        {
            _packet.Write(_parentPlatformId);
            _packet.Write(_neighborNumber);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void CreateEnvironmentObj(int _toClient, EnvironmentObjectController _environmentObject)
    {
        using (Packet _packet = new Packet((int) ServerPackets.createEnvironmentObj))
        {
            _packet.Write(_environmentObject.id);
            _packet.Write(_environmentObject.platform.id);
            _packet.Write(_environmentObject.GetLocalIdOnPlatform());
            _packet.Write(_environmentObject.path);
            _packet.Write(_environmentObject.transform.position);
            _packet.Write(_environmentObject.transform.rotation);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void PortalCreate(int _toClient, PortalController _portal)
    {
        using (Packet _packet = new Packet((int) ServerPackets.portalCreate))
        {
            _packet.Write(_portal.id);
            _packet.Write(_portal.portalName);
            _packet.Write(_portal.transform.position);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void PortalRemove(int _toClient, PortalController _portal)
    {
        using (Packet _packet = new Packet((int) ServerPackets.portalRemove))
        {
            _packet.Write(_portal.id);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void PlayerEnterDungeon(int _clientThatEnteredDungeon, int _portalId)
    {
        using (Packet _packet = new Packet((int) ServerPackets.playerEnterDungeon))
        {
            _packet.Write(_clientThatEnteredDungeon);
            _packet.Write(_portalId);
            
            SendTCPDataToAll(_packet);
        }
    }
    
    public static void SpawnChest(int _toClient, ChestController _chest)
    {
        
        using (Packet _packet = new Packet((int) ServerPackets.spawnChest))
        {
            _packet.Write(_chest.id);
            _packet.Write((int) _chest.chestType);
            _packet.Write(_chest.transform.position);
            
            SendTCPData(_toClient, _packet);
        }
    }
    
    public static void OpenChest(int _toClient, int _chestId)
    {
        using (Packet _packet = new Packet((int) ServerPackets.openChest))
        {
            _packet.Write(_chestId);
            
            SendTCPData(_toClient, _packet);
        }
    }
    
    public static void SpawnItemWithForce(int _toClient, DroppedItemController _droppedItem, Vector3 _origin, Vector3 _force)
    {
        using (Packet _packet = new Packet((int) ServerPackets.spawnItem))
        {
            _packet.Write(_droppedItem.item.prefabPath);
            _packet.Write(_droppedItem.world.name);
            _packet.Write(_origin);
            _packet.Write(_force);
            
            Debug.Log("seas");
            
            SendTCPData(_toClient, _packet);
        }
    }

    public static void SendHealthUpdateForDestructableEnvObject(List<Player> _players, EnvironmentObjectController _envObj)
    {
        using (Packet _packet = new Packet((int) ServerPackets.updateHealthOfDestructableEnvObj))
        {
            _packet.Write(_envObj.platform.id);
            _packet.Write(_envObj.GetLocalIdOnPlatform());
            _packet.Write(_envObj.GetComponent<DestructibleObject>().currentHealth);
            foreach (Player _player in _players)
            {
                SendTCPData(_player.id, _packet);
            }
        }
    }

    public static void DestroyDestructableEnvObject(List<Player> _players, EnvironmentObjectController _envObj)
    {
        using (Packet _packet = new Packet((int) ServerPackets.destroyDestructableEnvObject))
        {
            _packet.Write(_envObj.platform.id);
            _packet.Write(_envObj.GetLocalIdOnPlatform());
            foreach (Player _player in _players)
            {
                SendTCPData(_player.id, _packet);
            }
        }
    }

    public static void PlayerPicksUpItem(int _toClient, Player _playerWhoPickedUpTheItem, DroppedItemController _droppedItem)
    {
        using (Packet _packet = new Packet((int) ServerPackets.playerPicksUpItem))
        {
            _packet.Write(_droppedItem.world.droppedItems.IndexOf(_droppedItem));
            _packet.Write(_playerWhoPickedUpTheItem.id);
            SendTCPData(_toClient, _packet);
        }
    }
    
    #endregion
}
