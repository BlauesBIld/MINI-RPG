using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint) Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }


    public static void PlayerServerMovement(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Vector3 _movingDirection = _packet.ReadVector3();
        //TODO: Detschn: Solve this crap - Future me: Idk what's crap here
        if (GameManager.players.ContainsKey(_id) && !GameManager.players[_id].IsMainPlayer())
        {
            GameManager.players[_id].SetMovingParameter(_position, _movingDirection);
        }
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        if (GameManager.players.ContainsKey(_id))
        {
            Destroy(GameManager.players[_id].gameObject);
            GameManager.players.Remove(_id);
        }
    }

    public static void PlatformCreate(Packet _packet)
    {
        int _platformId = _packet.ReadInt();
        int _platformSize = _packet.ReadInt();
        Vector3 _platformPosition = _packet.ReadVector3();

        GameObject _newPlatform =
            HomeWorldController.instance.SpawnEnvironmentObj("World/Platform", _platformPosition, Quaternion.identity);
        _newPlatform.GetComponent<PlatformController>().InitializeNewPlatform(_platformId, _platformSize);
    }

    public static void WallCreate(Packet _packet)
    {
        int _wallPlatformId = _packet.ReadInt();
        int _wallDirectionToNeighbor = _packet.ReadInt();
        Vector3 _wallPosition = _packet.ReadVector3();
        float _wallRotationY = _packet.ReadFloat();
        
        GameObject _newWall = HomeWorldController.instance.SpawnEnvironmentObj("World/Wall", _wallPosition, Quaternion.Euler(-90f, 0f, _wallRotationY));
        _newWall.GetComponent<WallController>().InitializeNewWall(_wallPlatformId, _wallDirectionToNeighbor);
    }

    public static void WallRemove(Packet _packet)
    {
        int _platformId = _packet.ReadInt();
        int _directionToNeighborPlatform = _packet.ReadInt();

        ((HomeWorldController)HomeWorldController.instance).RemoveWallFromGame(_platformId, _directionToNeighborPlatform);
    }

    public static void CreateEnvironmentObj(Packet _packet)
    {
        int _envObjId = _packet.ReadInt();
        int _envObjPlatformId = _packet.ReadInt();
        int _envObjLocalIdOnPlatform = _packet.ReadInt();
        string _assetName = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        PlatformController _envObjPlatform =
            ((HomeWorldController) WorldController.instance).platforms[_envObjPlatformId];
        
        GameObject _newEnvironmentObj = HomeWorldController.instance.SpawnEnvironmentObj(_assetName, _position, _rotation * Quaternion.Euler(-90, 0, 0));
        
        _newEnvironmentObj.AddComponent<EnviornmentObjectController>().InitializeParameters(_envObjId, _envObjPlatform, _assetName);
        _newEnvironmentObj.GetComponent<EnviornmentObjectController>().SetNameInScene();
        ((HomeWorldController)HomeWorldController.instance).platforms[_envObjPlatformId].AddEnvObj(_newEnvironmentObj.GetComponent<EnviornmentObjectController>());
    }

    public static void PortalCreate(Packet _packet)
    {
        int _portalId = _packet.ReadInt();
        string _portalName = _packet.ReadString();
        Vector3 _portalPosition = _packet.ReadVector3();

        GameObject _newPortal =
            HomeWorldController.instance.SpawnEnvironmentObj("Portals/" + _portalName, _portalPosition, Quaternion.identity);
        _newPortal.GetComponent<PortalController>().Initialize(_portalId, _portalName);
    }

    public static void PortalRemove(Packet _packet)
    {
        throw new System.NotImplementedException();
    }

    public static void PlayerEnterDungeon(Packet _packet)
    {
        int _clientId = _packet.ReadInt();
        int _portalId = _packet.ReadInt();

        GameManager.instance.PlayerSwitchesWorld(_clientId, _portalId);
    }
    
    public static void SpawnChest(Packet _packet)
    {
        int _chestId = _packet.ReadInt();
        int _chestType = _packet.ReadInt();
        Vector3 _chestPosition = _packet.ReadVector3();

        HomeWorldController.instance.SpawnChest(_chestId, _chestType, _chestPosition, Quaternion.identity);
    }
    
    public static void OpenChest(Packet _packet)
    {
        int _chestId = _packet.ReadInt();
        
        ChestController _chest = HomeWorldController.instance.FindChest(_chestId);
        
        if (_chest != null)
        {
            _chest.OpenChest();
        }
        else
        {
            Debug.Log("Tried to open Chest, which does not exist!");
        }
    }

    public static void SpawnItem(Packet _packet)
    {
        string _itemPrefabPath = _packet.ReadString();
        Vector3 _origin = _packet.ReadVector3();
        Vector3 _force = _packet.ReadVector3();

        WorldController.instance.SpawnItemWithForce(_itemPrefabPath, _origin, _force);
    }
    public static void UpdateHealthOfDestructibleEnvObj(Packet _packet)
    {
        int _platformId = _packet.ReadInt();
        int _envObjLocalId = _packet.ReadInt();
        int _currentHealth = _packet.ReadInt();
        
        ((HomeWorldController)HomeWorldController.instance).platforms[_platformId].envObjs[_envObjLocalId].GetComponent<DestructibleObject>().currentHealth = _currentHealth;
    }
    public static void DestroyDestructibleEnvObj(Packet _packet)
    {
        int _platformId = _packet.ReadInt();
        int _envObjLocalId = _packet.ReadInt();

        Destroy(((HomeWorldController)HomeWorldController.instance).platforms[_platformId].envObjs[_envObjLocalId].gameObject);
    }

    public static void PlayerPicksUpItem(Packet _packet)
    {
        int _droppedItemIndex = _packet.ReadInt();
        int _playerId = _packet.ReadInt();

        WorldController.instance.PlayerPicksUpItem(_playerId, _droppedItemIndex);
    }
}