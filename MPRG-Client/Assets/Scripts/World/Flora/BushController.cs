using UnityEngine;

public class BushController : DestructibleObject
{
    private void OnTriggerEnter(Collider enteringCollider)
    {
        if (enteringCollider.GetComponent<PlayerController>() != null)
        {
            enteringCollider.GetComponent<PlayerManager>().moveSpeed /= 1.4f;
        }
    }

    private void OnTriggerExit(Collider exitingCollider)
    {
        if (exitingCollider.GetComponent<PlayerController>() != null)
        {
            exitingCollider.GetComponent<PlayerManager>().moveSpeed *= 1.4f;
        }
    }
    public override void RequestHealthUpdate(int _healthDiff)
    {
        EnviornmentObjectController _envObject = GetComponent<EnviornmentObjectController>();
        ClientSend.RequestDestructibleEnvObjectHealthUpdate(_envObject.platform.id, _envObject.GetLocalIdOnPlatform(), _healthDiff);
    }
}