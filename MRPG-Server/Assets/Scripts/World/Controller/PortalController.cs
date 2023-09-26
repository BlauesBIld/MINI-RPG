using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public int id;
    public string portalName;

    public void Start()
    {
        transform.parent = PortalManager.instance.portalParentObject.transform;
    }

    public void Initialize(string _name)
    {
        portalName = _name;
        gameObject.name = _name;

        if (PortalManager.instance.portals.Count > 0)
        {
            id = PortalManager.instance.portals[PortalManager.instance.portals.Count - 1].id + 1;
        }
        else
        {
            id = 1;
        }
    }
}