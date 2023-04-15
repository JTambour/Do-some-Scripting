using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;

    private Rigidbody rb;
    private Collider col;
    private Vector3 movementDirection;

    [SerializeField] private LayerMask ground;

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








    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Start()
    {
        playerControls.Ground.Jump.performed += _ => Jump();
        playerControls.Ground.Grow.performed += _ => Grow();
        playerControls.Ground.Shrink.performed += _ => Shrink();
    }

    void Update()
    {
        // Read the moovement value
        float movementInput = playerControls.Ground.Move.ReadValue<float>();

        // Move the Player
        Vector3 currentPosition = transform.position;
        currentPosition.x += movementInput * speed * Time.deltaTime;
        transform.position = currentPosition;



        // Set the movement direction
        movementDirection = new Vector3(0f, 0f, movementInput);

        // Rotate the player based on movement direction
        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            /*Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = targetRotation;*/
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

    private void Grow()
    {
        if (isSmall)
        {
            transform.localScale = new Vector3(normalScale, normalScale, normalScale);
            isSmall = false;
            isNormal = true;
        }
        else if (isNormal)
        {
            transform.localScale = new Vector3(bigScale, bigScale, bigScale);
            isNormal = false;
            isBig = true;
        }
    }

    private void Shrink()
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
