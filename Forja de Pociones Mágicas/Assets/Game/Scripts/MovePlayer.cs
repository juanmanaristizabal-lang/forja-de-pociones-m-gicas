using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D))]
public class MovePlayer : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float fuerzaSalto = 8f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundcheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    public Animator animator;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

        animator.SetFloat("Movement", Mathf.Abs(rb.linearVelocity.x));

        if (moveInput.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (moveInput.x > 0)
            transform.localScale = new Vector3(1, 1, 1);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundcheckRadius, groundLayer);
        
        animator.SetBool("isJumping", !isGrounded);
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if(context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundcheckRadius);
        }
    }
}
