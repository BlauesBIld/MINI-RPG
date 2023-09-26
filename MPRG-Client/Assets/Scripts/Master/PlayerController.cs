using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpSpeed = 10f;
    public float gravity = -20f;
    public float yVelocity = 0f;

    private float distToGround = 0f;

    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void Update()
    {
        SetMovementInputs();
        SetRotatingSpeed();
        Move();
    }

    private void SetRotatingSpeed()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            playerManager.rotatingDirection = new Vector3(0f, Input.GetAxis("Mouse X") * 25f, 0f);
        }
    }

    private void FixedUpdate()
    {
        SendPositionAndMovingDirectionToServer();
    }

    private void SendPositionAndMovingDirectionToServer()
    {
        ClientSend.PlayerMovement(transform.position, playerManager.movingDirection, playerManager.rotatingDirection);
    }

    public void Move()
    {
        GetComponent<CharacterController>()
            .Move(playerManager.movingDirection * playerManager.moveSpeed * Time.deltaTime);
        transform.Rotate(playerManager.rotatingDirection * playerManager.rotateSpeed * Time.deltaTime);

        playerManager.UpdateSideVelocity(Vector3.Dot(GetComponent<CharacterController>().velocity.normalized,
            transform.right));
        playerManager.UpdateForwardVelocity(Vector3.Dot(GetComponent<CharacterController>().velocity.normalized,
            transform.forward));
    }

    private void SetMovementInputs()
    {
        playerManager.movementInputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space)
        };

        SetMovingAndRotatingDirection();
    }

    private void SetMovingAndRotatingDirection()
    {
        Vector3 _inputMovingDirection = Vector3.zero;
        Vector3 _inputRotatingDirection = Vector3.zero;

        if (playerManager.movementInputs[0])
        {
            _inputMovingDirection += transform.forward;
        }

        if (playerManager.movementInputs[1])
        {
            _inputMovingDirection -= transform.forward;
        }

        if (playerManager.movementInputs[2])
        {
            _inputMovingDirection -= transform.right;
        }

        if (playerManager.movementInputs[3])
        {
            _inputMovingDirection += transform.right;
        }

        HandleJump();

        playerManager.UpdateRotatingDirection(_inputRotatingDirection.normalized);
        playerManager.UpdateMovingDirection(_inputMovingDirection.normalized);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    private void HandleJump()
    {
        if (IsGrounded())
        {
            yVelocity = 0f;
            if (playerManager.movementInputs[4])
            {
                yVelocity = jumpSpeed;
            }
        }

        yVelocity += gravity * Time.deltaTime;
        GetComponent<CharacterController>().Move(new Vector3(0f, yVelocity, 0f) * Time.deltaTime);
    }
}