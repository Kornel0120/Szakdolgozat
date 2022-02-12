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
    //private Vector3 PlayerMovementInput;
    //private Vector2 PlayerMouseInput;
    //private float xRot;

    //[SerializeField] private LayerMask GroundMask;
    //[SerializeField] private Transform GroundCheck;
    //[SerializeField] private Transform playerCamera;
    //[SerializeField] private Rigidbody body;

    //[SerializeField] private float Speed;
    //[SerializeField] private float Sensitivity;
    //[SerializeField] private float Jumpforce;

    //public bool isGrabable = false;
    //public bool isGrabed = false;
    //GameObject GrabableObject;

    //void Update()
    //{
    //    PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    //    PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

    //    MovePlayer();
    //    MovePlayerCamera();
    //    if (isGrabable == true)
    //    {
    //        isGrabed = true;
    //        PickUpObject();
    //    }   
    //    else
    //    {
    //        isGrabed = false;
    //        DropDownObject();
    //    }

    //}

    //private void MovePlayer()
    //{
    //    Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * Speed;
    //    body.velocity = new Vector3(MoveVector.x, body.velocity.y, MoveVector.z);

    //    if(Input.GetKeyDown(KeyCode.Space))
    //    {
    //        if(Physics.CheckSphere(GroundCheck.position, 0.01f, GroundMask))
    //            body.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
    //    }

    //}

    //private void MovePlayerCamera()
    //{
    //    xRot -= PlayerMouseInput.y * Sensitivity;

    //    if(xRot <= 45 && xRot >= -45)
    //    {
    //        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
    //        playerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    //    }
    //    else
    //    {
    //        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
    //    }
    //}

    //private void PickUpObject()
    //{
    //    //if (Input.GetKeyDown(KeyCode.E))
    //    //{
    //    //    GrabableObject.GetComponent<PickUp>().ObjectPickUp();
    //    //}    
    //}

    //private void DropDownObject()
    //{
    //    //if (Input.GetKeyDown(KeyCode.E))
    //    //{
    //    //    GrabableObject.GetComponent<PickUp>().ObjectDropDown();
    //    //}
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.CompareTag("Grabbable"))
    //    {
    //        isGrabable = true;
    //        GrabableObject = collision.gameObject;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    isGrabable = false;
    //}
}
