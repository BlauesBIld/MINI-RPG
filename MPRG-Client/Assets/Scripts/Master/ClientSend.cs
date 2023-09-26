using UnityEngine;

public class ClientSend : MonoBehaviour
{
    
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    
    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(Vector3 _position, Vector3 _movingDirection, Vector3 _rotatingDirection)
    {
        using (Packet _packet = new Packet((int) ClientPackets.playerMovement))
        {
            _packet.Write(_position);
            _packet.Write(_movingDirection);
            _packet.Write(_rotatingDirection);

            SendUDPData(_packet);
        }
    }

    public static void RequestUnlockNewPlatform(PlatformController _platform, int _directionToNeighborPlatform)
    {
        using (Packet _packet = new Packet((int) ClientPackets.requestUnlockNewPlatform))
        {
            _packet.Write(_platform.id);
            _packet.Write(_directionToNeighborPlatform);
            
            SendTCPData(_packet);
        }
    }

    public static void RequestToEnterDungeon(PortalController _portal)
    {
        using (Packet _packet = new Packet((int) ClientPackets.requestToEnterDungeon))
        {
            _packet.Write(_portal.id);
            _packet.Write(_portal.name);
            
            SendTCPData(_packet);
        }
    }

    public static void RequestToOpenChest(ChestController _chest)
    {
        using (Packet _packet = new Packet((int) ClientPackets.requestToOpenChest))
        {
            _packet.Write(_chest.id);
            
            SendTCPData(_packet);
        }
    }

    public static void RequestDestructibleEnvObjectHealthUpdate(int _platformId, int _localEnvObjIdOnPlatform, int _healthUpdateDiff)
    {
        using (Packet _packet = new Packet((int) ClientPackets.requestUpdateDestructibleHealth))
        {
            _packet.Write(_platformId);
            _packet.Write(_localEnvObjIdOnPlatform);
            _packet.Write(_healthUpdateDiff);
            
            SendTCPData(_packet);
        }
    }
    
    #endregion
}
