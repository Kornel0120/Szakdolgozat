using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementRigidBody : MonoBehaviour
{
    public float playerHeight = 1f;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    public float Speed = 2f;
    [SerializeField] float groundMultiplier = 2f;
    [SerializeField] float airMultiplier = 1f;
    [SerializeField] float slopeMultiplier = 3f;

    [Header("Jumping")]
    public float jumpForce = 1f;

    [Header("KeyBinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Drag")]
    [SerializeField] float groundDrag = 0.5f;
    [SerializeField] float airDrag = 0.2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    float groundDistance = 0.1f;
    

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position + new Vector3(0f,0.25f,0f), Vector3.down, out slopeHit, 1.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                Debug.DrawLine(transform.position + new Vector3(0f, 0.25f, 0f), transform.position + Vector3.down * 1.5f, Color.red);
                return true;  
            }
            else
            {
                Debug.DrawLine(transform.position + new Vector3(0f, 0.25f, 0f), transform.position + Vector3.down * 1.5f, Color.red);
                return false;
            }
                
        }
        Debug.DrawLine(transform.position + new Vector3(0f, 0.25f, 0f), transform.position + Vector3.down * 1.5f, Color.red);
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        PlayerInput();
        DragControl();

        if(Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void PlayerInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void MovePlayer()
    {
        if(isGrounded && !OnSlope())
            rb.AddForce(moveDirection.normalized * Speed * groundMultiplier, ForceMode.Acceleration);
        else if(isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * Speed * slopeMultiplier, ForceMode.Acceleration);
        }
        else if(!isGrounded)
            rb.AddForce(moveDirection.normalized * Speed * airMultiplier, ForceMode.Acceleration);
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void DragControl()
    {
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }
    
}
