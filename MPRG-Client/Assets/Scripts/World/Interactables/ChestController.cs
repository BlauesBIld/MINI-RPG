using System;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public int id;

    public GameObject keyPopUp;
    public bool opened = false;

    public PlayerController playerNearChest;
    public Camera playerNearChestCamera;

    [NonSerialized]
    public Animator chestAnimator;
    
    private void Start()
    {
        chestAnimator = GetComponentInChildren<Animator>();
        keyPopUp.SetActive(false);
    }

    private void Update()
    {
        if (playerNearChest != null)
        {
            if (Input.GetKeyDown(KeyCode.F) && chestAnimator.GetBool("opened") == false)
            {
                ClientSend.RequestToOpenChest(this);
            }

            SetRotationForKeyPopUp();
        }
    }

    private void SetRotationForKeyPopUp()
    {
        Quaternion _targetRotationForKeyPopUp =
            Quaternion.LookRotation(GetLookingDirectionWithSameYCoordinateAsKeyPopUp());
        Quaternion _keyPopUpRotation = keyPopUp.transform.rotation;
        _keyPopUpRotation = Quaternion.RotateTowards(_keyPopUpRotation, _targetRotationForKeyPopUp,
            (Quaternion.Angle(_keyPopUpRotation, _targetRotationForKeyPopUp) / 100 + 1) * 90f * Time.deltaTime);
        keyPopUp.transform.rotation = _keyPopUpRotation;
    }

    public void OpenChest()
    {
        chestAnimator.SetBool("opened", true);
        keyPopUp.SetActive(false);
    }

    private void OnTriggerEnter(Collider _enteringCollider)
    {
        if (_enteringCollider.GetComponent<PlayerController>() != null)
        {
            playerNearChest = _enteringCollider.GetComponent<PlayerController>();
            playerNearChestCamera = playerNearChest.GetComponentInChildren<Camera>();
            if (chestAnimator.GetBool("opened") == false)
            {
                keyPopUp.SetActive(true);
                keyPopUp.transform.rotation = Quaternion.LookRotation(GetLookingDirectionWithSameYCoordinateAsKeyPopUp());
            }
        }
    }

    private void OnTriggerExit(Collider _exitingCollider)
    {
        if (_exitingCollider.GetComponent<PlayerController>() != null && opened == false)
        {
            playerNearChest = null;
            playerNearChestCamera = null;
            if (chestAnimator.GetBool("opened"))
            {
                keyPopUp.SetActive(false);
            }
        }
    }

    private Vector3 GetLookingDirectionWithSameYCoordinateAsKeyPopUp()
    {
        var _keyPopUpPosition = keyPopUp.transform.position;
        Vector3 _direction =
            (playerNearChestCamera.transform.position + playerNearChest.transform.position)/2 -
            _keyPopUpPosition;
        _direction.y = 0;
        return _direction;
    }
}