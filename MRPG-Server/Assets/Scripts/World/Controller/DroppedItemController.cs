using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DroppedItemController : MonoBehaviour
{
    public Item item;

    public GameObject colliderObjectForPickUp;
    public Vector3 forceDirection;
    public WorldController world;

    private void Awake()
    {
        SetRandomForce();
    }

    private void Start()
    {
        DropInForceDirectionInFrontOfOrigin();
    }
    
    private void SetRandomForce()
    {
        forceDirection =  transform.up * Random.Range(160,200) + transform.forward * Random.Range(80f, 120f) + transform.right * Random.Range(-100f, 100f);
    }
    
    private void DropInForceDirectionInFrontOfOrigin()
    {
        GetComponent<Rigidbody>().AddForce(forceDirection);
    }
}
