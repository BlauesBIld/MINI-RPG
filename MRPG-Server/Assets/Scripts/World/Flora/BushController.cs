using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushController : DestructibleObject
{
    public override void SendHealthUpdateForDestructableObjectToAllPlayers()
    {
        throw new System.NotImplementedException();
    }
    public override void SendDestroyDestructableObject()
    {
        ServerSend.DestroyDestructableEnvObject(gameObject.GetComponentInParent<WorldController>().GetPlayers(), gameObject.GetComponent<EnvironmentObjectController>());
    }

    public override void DropLoot()
    {
        
    }
}
