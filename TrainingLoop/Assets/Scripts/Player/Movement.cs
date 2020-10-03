using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 1f;
    public float maxSpeed = 2f;
    public float JumpStregth = 4f;


    private float movement;
    private bool isFlipped = false;
    //[SerializeField]
    private bool isGrounded = false;
    private bool canJump = false;
    private bool jump = false;

    public LayerMask groundMask;
    public LayerMask platformMask;
    public CircleCollider2D frontDetection;
    public CircleCollider2D backDetection;

    public float horizontalDrag = 2f;

    private Vector3 vc;
    private Vector3 worldCenter
    {
        get
        {
            if (vc == null)
                vc = GameController.Instance.WorldCenter;

            return vc;
        }
    }

    public Transform Rotator;

    public Vector2 Up
    {
        get
        {
            return (worldCenter - transform.position).normalized;
        }
    }
    public Vector2 Right
    {
        get
        {
            return -(new Vector2(-Up.y, Up.x).normalized);
        }
    }

    private Rigidbody2D rb;
    public Rigidbody2D Rigidbody2D
    {
        get
        {
            if (rb == null)
                rb = GetComponent<Rigidbody2D>();

            return rb;
        }
    }

    private void Update()
    {
        DetectGround();
        DetectGroundOrPlatform();

        // Movement input.
        movement = Input.GetAxisRaw("Horizontal") * speed;
        // Check for jump input.
        if (Input.GetButtonDown("Jump") && canJump)
            jump = true;

        if (isGrounded)
        {
            // Rotate to top
            var angle = Mathf.Atan2(Up.y, Up.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            // Rotate to world up.
            var angle = Mathf.Atan2(1f, 0f) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (movement == 0 && isGrounded)
            transform.SetParent(Rotator);
        else
            transform.SetParent(null);

        Debug.DrawLine((Vector2)transform.position, (Vector2)worldCenter);
        Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + ((Vector2)transform.right * movement).normalized, Color.blue);
    }

    private void FixedUpdate()
    {
        float t = Rigidbody2D.velocity.x / maxSpeed;

        float lerp = 0f;

        if (t >= 0f)
            lerp = Mathf.Lerp(maxSpeed, 0f, t);
        else if (t < 0f)
            lerp = Mathf.Lerp(maxSpeed, 0f, Mathf.Abs(t));

        Vector2 force, drag;

        if (isGrounded)
        {
            force = Right * movement * lerp * speed * Time.fixedDeltaTime;
            drag = -Rigidbody2D.velocity * horizontalDrag * Time.fixedDeltaTime;
        }
        else
        {
            force = new Vector2(movement * lerp * speed * Time.fixedDeltaTime, 0f);
            drag = new Vector2(-rb.velocity.x * horizontalDrag * Time.fixedDeltaTime, 0f);
        }

        Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + force, Color.green);
        Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + drag, Color.red);

        Rigidbody2D.AddForce(force, ForceMode2D.Force);
        Rigidbody2D.AddForce(drag, ForceMode2D.Force);

        if (movement >= .1f && isFlipped)
        {
            Vector2 flipScale = new Vector2(-1f, 1f);
            //animator.transform.localScale *= flipScale;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isFlipped = false;
        }
        else if (movement <= -.1f && !isFlipped)
        {
            Vector2 flipScale = new Vector2(-1f, 1f);
            //animator.transform.localScale *= flipScale;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isFlipped = true;
        }

        if (jump)
        {
            //Vector2 vel = rb.velocity;
            //vel.y = JumpStregth;

            rb.velocity = transform.up * JumpStregth;
            jump = false;
            isGrounded = false;
            canJump = false;
            //animator.SetBool("IsGrounded", isGrounded);
            //animator.SetTrigger("Jump");
        }
    }

    private void DetectGround()
    {
        Collider2D colliderFront = Physics2D.OverlapCircle(rb.position + frontDetection.offset, frontDetection.radius + 0.5f, groundMask);
        Collider2D colliderBack = Physics2D.OverlapCircle(rb.position + backDetection.offset, backDetection.radius + 0.5f, groundMask);

        if (colliderFront != null || colliderBack != null)
        {
            if (rb.velocity.y < 0f)
            {
                isGrounded = true;
            }
                
        }
        else
            isGrounded = false;
    }

    private void DetectGroundOrPlatform()
    {
        Collider2D colliderFront = Physics2D.OverlapCircle(rb.position + frontDetection.offset, frontDetection.radius + 0.5f, platformMask);
        Collider2D colliderBack = Physics2D.OverlapCircle(rb.position + backDetection.offset, backDetection.radius + 0.5f, platformMask);

        if (colliderFront != null || colliderBack != null)
        {
            if (rb.velocity.y < 0f)
            {
                canJump = true;
            }

        }
        else
            canJump = false;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawWireSphere((Vector2)rb.position, 5f);
    //}
}
