using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;

    public WorldController currentWorld;
    
    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        gameObject.name = "Player (" + username + ")";
        WorldManager.instance.MovePlayerToWorld(this, "HomeWorld");
    }

    private void Move(Vector3 _movingDirection)
    {
        ServerSend.PlayerServerMovement(this, _movingDirection);
    }

    public void SetMovement(Vector3 _position, Vector3 _movingDirection, Vector3 _rotatingDirection)
    {
        transform.position = _position;
        Move(_movingDirection);
    }

    public void PickUpItem(DroppedItemController _droppedItem)
    {
        foreach (Player _player in currentWorld.GetPlayers())
        {
            ServerSend.PlayerPicksUpItem(_player.id, this, _droppedItem);
        }
    }
}
