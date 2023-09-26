using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : DestructibleObject
{
    public override void SendHealthUpdateForDestructableObjectToAllPlayers()
    {
        ServerSend.SendHealthUpdateForDestructableEnvObject(gameObject.GetComponentInParent<WorldController>().GetPlayers(), gameObject.GetComponent<EnvironmentObjectController>());
    }
    public override void SendDestroyDestructableObject()
    {
        ServerSend.DestroyDestructableEnvObject(gameObject.GetComponentInParent<WorldController>().GetPlayers(), gameObject.GetComponent<EnvironmentObjectController>());
    }

    public override void DropLoot()
    {
        
    }
}
