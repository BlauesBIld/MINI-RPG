using UnityEngine;

public class WallController : MonoBehaviour
{
    public PlatformController platform;
    private int directionToNeighborPlatform;
    
    private bool isNearWall = false;

    private void OnTriggerEnter(Collider _enteringCollider)
    {
        if (_enteringCollider.GetComponent<PlayerController>() != null)
        {
            isNearWall = true;
        }
    }

    private void OnTriggerExit(Collider _exitingTrigger)
    {
        if (_exitingTrigger.GetComponent<PlayerController>() != null)
        {
            isNearWall = false;
        }
    }

    private void Update()
    {
        if (isNearWall && Input.GetKeyDown(KeyCode.F))
        {
            ClientSend.RequestUnlockNewPlatform(platform, directionToNeighborPlatform);
        }
    }

    public void InitializeNewWall(int _wallPlatformId, int _directionToNeighborPlatform)
    {
        platform = ((HomeWorldController)HomeWorldController.instance).platforms[_wallPlatformId];
        transform.parent = platform.transform;
        directionToNeighborPlatform = _directionToNeighborPlatform;
        platform.walls[_directionToNeighborPlatform] = this;
    }
}
