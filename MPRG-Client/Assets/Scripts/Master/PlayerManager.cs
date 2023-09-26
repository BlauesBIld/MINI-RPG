using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth = 100f;
    public SkinnedMeshRenderer model;

    public float moveSpeed = 20f;
    public float rotateSpeed = 20f;
    
    private float forwardVelocity = 0f;
    private float sideVelocity = 0f;
    
    public Vector3 movingDirection = Vector3.zero;
    public Vector3 rotatingDirection = Vector3.zero;
    
    public bool[] movementInputs;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
    }
    
    private void FixedUpdate()
    {
        if (!IsMainPlayer())
        {
            SetRotatingSpeed();
            GetComponent<CharacterController>().Move(movingDirection * moveSpeed * Time.fixedDeltaTime);
            transform.Rotate(rotatingDirection * rotateSpeed * Time.fixedDeltaTime);
        }
        BindAnimatorParameters();
    }

    public void Die()
    {
        model.enabled = false;
    }

    
    public void SetMovingParameter(Vector3 _position, Vector3 _movingDirection)
    {
        transform.position = _position;
        movingDirection = _movingDirection;
    }

    private void BindAnimatorParameters()
    {
        GetComponent<Animator>().SetFloat("forwardVelocity", forwardVelocity);
        GetComponent<Animator>().SetFloat("rotatingVelocity", rotatingDirection.y);
        GetComponent<Animator>().SetFloat("sideVelocity", sideVelocity);
    }

    public void UpdateMovingDirection(Vector3 _movingDirection)
    {
        movingDirection = _movingDirection;
    }

    public void UpdateRotatingDirection(Vector3 _rotatingDirection)
    {
        rotatingDirection = _rotatingDirection;
    }
    
    public void SetRotatingSpeed()
    {
        if (forwardVelocity > 0f || sideVelocity > 0f)
        {
            rotateSpeed = Mathf.Abs((sideVelocity+forwardVelocity) * moveSpeed * 10f);
        }
        else
        {
            rotateSpeed = 100f;
        }
    }

    public void UpdateForwardVelocity(float _forwardVelocity)
    {
        forwardVelocity = _forwardVelocity;
    }

    public void UpdateSideVelocity(float _sideVelocity)
    {
        sideVelocity = _sideVelocity;
    }

    public bool IsMainPlayer()
    {
        return GetComponent<PlayerController>() != null;
    }
}
