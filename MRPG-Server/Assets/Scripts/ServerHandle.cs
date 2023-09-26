using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
        Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        Vector3 _position = _packet.ReadVector3();
        Vector3 _movingDirection = _packet.ReadVector3();
        Vector3 _rotatingDirection = _packet.ReadVector3();

        Server.clients[_fromClient].player.SetMovement(_position, _movingDirection, _rotatingDirection);
    }

    public static void RequestUnlockNewPlatform(int _fromClient, Packet _packet)
    {
        int _platformId = _packet.ReadInt();
        int _neighborNr = _packet.ReadInt();

        PlatformManager.instance.InstantiateNewNeighborPlatform(_platformId, _neighborNr);
    }

    public static void RequestToEnterDungeon(int _fromClient, Packet _packet)
    {
        int _portalId = _packet.ReadInt();
        string _portalName = _packet.ReadString();

        Server.clients[_fromClient].EnterDungeon(_portalId);
    }

    public static void RequestToOpenChest(int _fromclient, Packet _packet)
    {
        int _chestId = _packet.ReadInt();

        Server.clients[_fromclient].player.currentWorld.FindChest(_chestId).Open();
    }
    public static void RequestUpdateDestructibleEnvObjectHealth(int _fromclient, Packet _packet)
    {
        int _platformId = _packet.ReadInt();
        int _localEnvObjIdOnPlatform = _packet.ReadInt();
        int _healthDiff = _packet.ReadInt();
        
        PlatformManager.instance.platforms[_platformId-1].environmentObjects[_localEnvObjIdOnPlatform].GetComponent<DestructibleObject>().ReduceHealth(_healthDiff);
    }
}
