using System.Collections.Generic;
using UnityEngine;

public class ElkanahController : MonoBehaviour
{
    private List<PortalStoneForElkanahController> portalStones = new List<PortalStoneForElkanahController>();

    public GameObject centerObject;

    // Start is called before the first frame update
    void Start()
    {
        int iteratorForId = 0;
        foreach (PortalStoneForElkanahController _portalStone in GetComponentsInChildren<PortalStoneForElkanahController>())
        {
            _portalStone.id = iteratorForId;
            _portalStone.SetCenter(centerObject.transform.position);
            portalStones.Add(_portalStone);
            
            iteratorForId++;
        }
    }
}