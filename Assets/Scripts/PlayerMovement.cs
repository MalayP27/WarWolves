using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float jumpPower;
    [SerializeField] private float groundCheckDistance = 0.1f; // Allow adjustable ground check distance
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private LayerMask treeLayer; 
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCD;
    private float horizontalInput;

    [SerializeField] private Vector3 playerScale = new Vector3(1.5f, 1.5f, 1.5f); // Adjustable player scale

    private bool canDoubleJump = false; // Tracks whether the player can perform a double jump
    public bool canJump = true; // Track if the player can jump

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        
        // Flip player based on direction
        if (horizontalInput > 0.01f)
        {
            transform.localScale = playerScale;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-playerScale.x, playerScale.y, playerScale.z);
        }

        // Set animation parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
        
        // Wall jump cooldown logic
        if (wallJumpCD > 0.2f)
        {   
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onTree() && isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
            {
                body.gravityScale = 1.3f;
            }

            if (canJump && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
            {
                if (isGrounded())
                {
                    Jump();
                    canDoubleJump = true; // Allow double jump after the first jump
                }
                else if (canDoubleJump && body.velocity.y < 0) // Allow double jump only when falling
                {
                    DoubleJump();
                    canDoubleJump = false; // Disable double jump after using it
                }
            }
        }
        else
        {
            wallJumpCD += Time.deltaTime;
        }
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        TriggerJumpAnimation();
    }

    private void DoubleJump()
    {
        // Trigger a double jump (apply a slightly reduced jump power if desired)
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        TriggerJumpAnimation();
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, groundCheckDistance, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onTree()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, treeLayer);
        return raycastHit.collider != null;
    }

    private void TriggerJumpAnimation()
    {
        anim.SetTrigger("jump");
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && !onTree();
    }
}
