using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public GameObject portalTemplate;
    public GameObject portalParentObject;

    public Dictionary<int, PortalController> portals = new Dictionary<int, PortalController>();

    public static PortalManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("PortalManager instance already exists, destroying object!");
            Destroy(this);
        }
    }
    
    void Start()
    {
        SpawnElkanahPortal();
    }

    private void SpawnElkanahPortal()
    {
        PortalController _elkanahPortal = Instantiate(portalTemplate).GetComponent<PortalController>();
        _elkanahPortal.Initialize("Elkanah");
        portals.Add(_elkanahPortal.id, _elkanahPortal);
        _elkanahPortal.transform.position = new Vector3(22f, 0f, 0f);
        
        foreach (Client _client in Server.clients.Values)
        {
            ServerSend.PortalCreate(_client.id, _elkanahPortal);
        }
    }

    public void SendAllInformationToPlayer(int _id)
    {
        foreach (PortalController _portal in portals.Values)
        {
            ServerSend.PortalCreate(_id, _portal);
        }
    }

    public string GetWorldNameFromPortalId(int _portalId)
    {
        return portals[_portalId].portalName;
    }
}
