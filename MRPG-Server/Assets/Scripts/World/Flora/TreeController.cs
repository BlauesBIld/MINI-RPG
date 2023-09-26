using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public enum TreeType
{
    basic,
    christmas
}

public class TreeController : DestructibleObject
{
    public TreeType treeType;

    public override void SendHealthUpdateForDestructableObjectToAllPlayers()
    {
        ServerSend.SendHealthUpdateForDestructableEnvObject(
            gameObject.GetComponentInParent<WorldController>().GetPlayers(),
            gameObject.GetComponent<EnvironmentObjectController>());
    }

    public override void SendDestroyDestructableObject()
    {
        ServerSend.DestroyDestructableEnvObject(gameObject.GetComponentInParent<WorldController>().GetPlayers(),
            gameObject.GetComponent<EnvironmentObjectController>());
    }

    public override void DropLoot()
    {
        GetComponentInParent<WorldController>().DropItem(new Item("Wood Log", Random.Range(3, 5), "/Materials/WoodLog"),
            this.transform);
    }
}