using UnityEngine;

public class PortalController : MonoBehaviour
{
    public int id;
    public string name;
    
    private bool isNearDungeon = false;
    
    public void Initialize(int _portalId, string _portalName)
    {
        id = _portalId;
        name = _portalName;
    }

    private void OnTriggerEnter(Collider _enteringCollider)
    {
        if (_enteringCollider.GetComponent<PlayerController>() != null)
        {
            isNearDungeon = true;
        }
    }

    private void OnTriggerExit(Collider _exitingTrigger)
    {
        if (_exitingTrigger.GetComponent<PlayerController>() != null)
        {
            isNearDungeon = false;
        }
    }

    private void Update()
    {
        if (isNearDungeon && Input.GetKeyDown(KeyCode.F))
        {
            ClientSend.RequestToEnterDungeon(this);
        }
    }
}
