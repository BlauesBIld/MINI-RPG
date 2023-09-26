using System;
using UnityEngine;

public class TreeController : DestructibleObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public override void RequestHealthUpdate(int _healthDiff)
    {
        EnviornmentObjectController _envObject = GetComponent<EnviornmentObjectController>();
        ClientSend.RequestDestructibleEnvObjectHealthUpdate(_envObject.platform.id, _envObject.GetLocalIdOnPlatform(), _healthDiff);
    }
}
