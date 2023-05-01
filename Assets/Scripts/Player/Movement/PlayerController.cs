using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Scripts")]
    public MenuManager menuManager;
    public PlayerControls playerControls;
    
    private Rigidbody rb;
    private Collider col;
    private Vector3 movementDirection;

    [Header("Layers")]
    [SerializeField] private LayerMask ground;

    [Header("Movement")]
    [SerializeField] private float speed;
    
    [Header("Jump")]
    [SerializeField] private float jumpForce = 15f;
    private const float _lowVelocity = 0.1f;
    private float _velocityLastFrame;
    private bool _jumping = false;

    //[SerializeField] private float gravityScale = 3.0f; // adjust this to control the strength of the downward force

    [Header("Grow & Shrink")]
    [SerializeField] private float smallScale;
    [SerializeField] private float normalScale;
    [SerializeField] private float bigScale;

    private bool isBig = false;
    private bool isSmall = false;
    private bool isNormal = true;

    // Check if Player has entered trigger to activate size 
    private bool hasTriggered = false;

    // Check if can Grow
    private bool canGrow = false;

    // Wall Check
    [Header("Wall Check")]
    [SerializeField] private Transform wallCheck;

    [Header("Main Menu Canvas")]
    [SerializeField] private Canvas mainMenuCanvas;
    //[SerializeField] private GameObject PauseMenuCanvas;
   
    [Header("Animation")]
    public Animator animator;

    [Header("Feedbacks")]
    public MMFeedbacks JumpFeedback;
    public MMFeedbacks LandingFeedback;
    public MMFeedbacks GrowFeedback;
    public MMFeedbacks ShrinkFeedback;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        Physics.gravity = Vector3.down * 30;
        Animator animator = transform.GetChild(0).GetComponent<Animator>();

        playerControls.Ground.Jump.performed += _ =>
        {
            if (!mainMenuCanvas.isActiveAndEnabled && !_jumping)
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
            // Move the Player
            Vector3 currentPosition = transform.position;
            currentPosition.x += movementInput * speed * Time.deltaTime;
            transform.position = currentPosition;          
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

        // if jumping, were going down last frame, and have now reached an almost null velocity
        if (_jumping && (_velocityLastFrame < -1) && (Mathf.Abs(rb.velocity.y) < _lowVelocity))
        {
            // then we just landed, we reset our state
            _jumping = false;
            LandingFeedback?.PlayFeedbacks();
        }

        // we store our velocity
        _velocityLastFrame = rb.velocity.y;

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
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _jumping = true;
            JumpFeedback?.PlayFeedbacks();
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
                GrowFeedback?.PlayFeedbacks();
                transform.localScale = new Vector3(normalScale, normalScale, normalScale);              
                isSmall = false;
                isNormal = true;
                isBig = false;              
            }
            else if (isNormal && canGrow)
            {
                GrowFeedback?.PlayFeedbacks();
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
                ShrinkFeedback?.PlayFeedbacks();
                transform.localScale = new Vector3(normalScale, normalScale, normalScale);
                isBig = false;
                isNormal = true;             
            }
            else if (isNormal)
            {
                ShrinkFeedback?.PlayFeedbacks();
                transform.localScale = new Vector3(smallScale, smallScale, smallScale);
                isNormal = false;
                isSmall = true;              
            }
        }       
    }

    // Trigger to detarmin if Player can use Grow/Shrink
    public void SetHasTriggered(bool value)
    {
        hasTriggered = value;
    }
   
    // Trigger to determain if player can grow
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
