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

    public void ReduceHealth(int _healthDiff)
    {
        RequestHealthUpdate(_healthDiff);
    }

    public abstract void RequestHealthUpdate(int _healthDiff);

}
