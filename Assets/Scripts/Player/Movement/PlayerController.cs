using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public PlayerControls playerControls;
    public PauseMenuUI pauseMenuUI;

    private Rigidbody rb;
    private Collider col;
    private Vector3 movementDirection;

    [SerializeField] private LayerMask ground;

    /*[Header("Cameras")]
    public CinemachineVirtualCameraBase mainMenuCamera;
    public CinemachineVirtualCameraBase playerCamera;*/

    [Header("Movement")]
    [SerializeField] private float speed;
    
    [Header("Jump")]
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravityScale = 3.0f; // adjust this to control the strength of the downward force

    [Header("Grow & Shrink")]
    [SerializeField] private float smallScale;
    [SerializeField] private float normalScale;
    [SerializeField] private float bigScale;

    private bool isBig = false;
    private bool isSmall = false;
    private bool isNormal = true;

    // Check if Player has entered trigger to activate size 
    private bool hasTriggered = false;

    // Chec if can Grow
    private bool canGrow = false;

    // Wall Check
    [Header("Wall Check")]
    [SerializeField] private Transform wallCheck;

    [Header("Main Menu Canvas")]
    [SerializeField] private Canvas mainMenuCanvas;
    //[SerializeField] private GameObject PauseMenuCanvas;
   
    [Header("Animation")]
    public Animator animator;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        Animator animator = transform.GetChild(0).GetComponent<Animator>();

        playerControls.Ground.Jump.performed += _ =>
        {
            if (!mainMenuCanvas.isActiveAndEnabled)
            {
                Jump();
            }
        };

        playerControls.Ground.Grow.performed += _ =>
        {
            if (!mainMenuCanvas.isActiveAndEnabled)
            {
                Grow();
            }
        };

        playerControls.Ground.Shrink.performed += _ =>
        {
            if (!mainMenuCanvas.isActiveAndEnabled)
            {
                Shrink();
            }
        };
    }

    
    void Update()
    {      
        // Canvas is active, disable player movement
        if (mainMenuCanvas.isActiveAndEnabled)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        float movementInput = playerControls.Ground.Move.ReadValue<float>();

        // Check if wall
        if (!isWall())
        {
            // Read the moovement value
            //float movementInput = playerControls.Ground.Move.ReadValue<float>();

            // Move the Player
            Vector3 currentPosition = transform.position;
            currentPosition.x += movementInput * speed * Time.deltaTime;
            transform.position = currentPosition;

            // Set the movement direction
           // movementDirection = new Vector3(0f, 0f, movementInput);
        }

        movementDirection = new Vector3(0f, 0f, movementInput);

        // Rotate the player based on movement direction
        if (movementDirection != Vector3.zero)
        {          
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = targetRotation;
        }

        if (movementDirection == Vector3.zero)
        {          
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = targetRotation;
        }

        // Animations
        if (movementDirection == Vector3.zero)
        {
            // Idle
            animator.SetFloat("Speed", 0);
        }
        else
        {
            // Walk
            animator.SetFloat("Speed", 1);
        }
        
    }

    private void FixedUpdate()
    {
        // apply a downward force if the player is falling
        if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.down * rb.mass * gravityScale);
        }
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Jump()
    {
       if (IsGrounded())
        {
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        Vector2 feetPos = transform.position;
        feetPos.y -= col.bounds.extents.y;
        return Physics.CheckSphere(feetPos, .1f, ground);
    }

    public bool isWall()
    {
        return Physics.CheckSphere(wallCheck.transform.position, 0.1f, ground);
    }

    private void Grow()
    {   
        if (hasTriggered)
        {
            if (isSmall)
            {
                transform.localScale = new Vector3(normalScale, normalScale, normalScale);              
                isSmall = false;
                isNormal = true;
                isBig = false;
            }
            else if (isNormal && canGrow)
            {
                transform.localScale = new Vector3(bigScale, bigScale, bigScale);
                isNormal = false;
                isBig = true;
            }
        }   
    }

    private void Shrink()
    {
        if (hasTriggered)
        {
            if (isBig)
            {
                transform.localScale = new Vector3(normalScale, normalScale, normalScale);
                isBig = false;
                isNormal = true;           
            }
            else if (isNormal)
            {
                transform.localScale = new Vector3(smallScale, smallScale, smallScale);
                isNormal = false;
                isSmall = true;
            }
        }       
    }

    public void SetHasTriggered(bool value)
    {
        hasTriggered = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GrowTrigger"))
        {
            canGrow = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GrowTrigger"))
        {
            canGrow = false;
        }
    }

   
}
