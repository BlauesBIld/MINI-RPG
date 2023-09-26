using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemInHandController : MonoBehaviour
{
    public GameObject currentItem;
    public GameObject playerCam;

    public GameObject handBone;
    public GameObject neckBone;

    public Action currentMethod;

    // Start is called before the first frame update
    void Start()
    {
        currentMethod = Punch;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Punch()
    {
        Physics.Raycast(neckBone.transform.position, playerCam.transform.forward, out RaycastHit _hit, 5f);
        if (_hit.collider != null)
        {
            if (_hit.collider.gameObject.GetComponent<DestructibleObject>() != null)
            {
                _hit.collider.gameObject.GetComponent<DestructibleObject>().ReduceHealth(20);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            currentMethod();
        }
    }
}