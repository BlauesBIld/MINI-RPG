using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>() != null)
        {
            other.GetComponentInParent<Player>().PickUpItem(GetComponentInParent<DroppedItemController>());
        }
    }
}
