using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructibleObject : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ReduceHealth(int _damage)
    {
        currentHealth -= _damage;
        SendHealthUpdateForDestructableObjectToAllPlayers();
        if (currentHealth <= 0)
        {
            SendDestroyDestructableObject();
            DropLoot();
            Destroy(gameObject);
        }
    }


    public abstract void SendHealthUpdateForDestructableObjectToAllPlayers();
    public abstract void SendDestroyDestructableObject();
    public abstract void DropLoot();
}
