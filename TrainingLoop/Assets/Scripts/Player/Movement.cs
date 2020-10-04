using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public int ExtraJumps;
    private int eJmp;
    public int currrentExtraJumps
    {
        get => eJmp;
        set
        {
            if (value > 0)
                JumpArrow.enabled = true;
            else
                JumpArrow.enabled = false;

            eJmp = value;
        }
    }

    public Image JumpArrow;

    public float speed = 1f;
    public float maxSpeed = 2f;
    public float JumpStregth = 4f;

    public Animator animator;

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
    public CircleCollider2D headDetection;

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

    private void Awake()
    {
        currrentExtraJumps = ExtraJumps;
    }

    private void Update()
    {
        if (canJump)
            currrentExtraJumps = ExtraJumps;

        var wasGrounded = isGrounded; // FIX

        DetectGround();
        DetectGroundOrPlatform();

        // Movement input.
        movement = Input.GetAxisRaw("Horizontal") * speed;
        // Check for jump input.
        if (Input.GetButtonDown("Jump") && (canJump || currrentExtraJumps > 0))
        {
            if (!canJump)
                currrentExtraJumps--;
            jump = true;
        }

        // Check for a wall
        if (movement != 0f)
        {
            Collider2D headCollider = Physics2D.OverlapCircle(
                LocalOffsetPosition(headDetection.offset),
                headDetection.radius + 0.1f,
                platformMask);

            if (headCollider != null)
                movement = 0f;
        }

        if (isGrounded)
        {
            // Rotate to top
            var angle = Mathf.Atan2(Up.y, Up.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (!wasGrounded)
                transform.position -= transform.up * 0.1f; // FIX
        }
        else
        {
            // Rotate to world up.
            var angle = Mathf.Atan2(1f, 0f) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (movement == 0 && canJump)
            transform.SetParent(Rotator);
        else
            transform.SetParent(null);

        // Control the animator.
        if (movement != 0f)
            animator.speed = 1f;
        else
            animator.speed = 0f;

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
            //drag = -Rigidbody2D.velocity * horizontalDrag * Time.fixedDeltaTime;
            drag = new Vector2(-Rigidbody2D.velocity.x * horizontalDrag * Time.fixedDeltaTime, 0f);
        }
        else
        {
            force = new Vector2(movement * lerp * speed * Time.fixedDeltaTime, 0f);
            drag = new Vector2(-Rigidbody2D.velocity.x * horizontalDrag * Time.fixedDeltaTime, 0f);
        }

        //Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + force, Color.green);
        //Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + drag, Color.red);
        Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + Rigidbody2D.velocity, Color.yellow);

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
            JumpInDirection(transform.up, JumpStregth);
        }
    }

    private void DetectGround()
    {
        var radius = 0.05f;
        if (isGrounded)
            radius = 0.2f;

        Collider2D colliderFront = Physics2D.OverlapCircle(LocalOffsetPosition(frontDetection.offset), frontDetection.radius + radius, groundMask);
        Collider2D colliderBack = Physics2D.OverlapCircle(LocalOffsetPosition(backDetection.offset), backDetection.radius + radius, groundMask);

        if (colliderFront != null || colliderBack != null)
        {
            if (Rigidbody2D.velocity.y < 0f)
            {
                isGrounded = true;
            }

        }
        else
            isGrounded = false;
    }

    private void DetectGroundOrPlatform()
    {
        var radius = 0.05f;
        if (canJump)
            radius = 0.2f;

        Collider2D colliderFront = Physics2D.OverlapCircle(LocalOffsetPosition(frontDetection.offset), frontDetection.radius + radius, platformMask);
        Collider2D colliderBack = Physics2D.OverlapCircle(LocalOffsetPosition(backDetection.offset), backDetection.radius + radius, platformMask);

        if (colliderFront != null || colliderBack != null)
        {
            if (Rigidbody2D.velocity.y < 0f)
                canJump = true;
        }
        else
            canJump = false;
    }

    private void OnDrawGizmosSelected()
    {
        var radius1 = 0.1f;

        var radius2 = 0.05f;
        if (canJump)
            radius2 = 0.2f;

        Gizmos.DrawWireSphere(LocalOffsetPosition(headDetection.offset), headDetection.radius + radius1);
        Gizmos.DrawWireSphere(LocalOffsetPosition(frontDetection.offset), frontDetection.radius + radius2);
        Gizmos.DrawWireSphere(LocalOffsetPosition(backDetection.offset), backDetection.radius + radius2);
    }

    private Vector2 LocalOffsetPosition(Vector2 relativeOffset)
    {
        var flipped = isFlipped ? -1f : 1f;

        var offsetPosition = Rigidbody2D.position;
        offsetPosition += (Vector2)transform.right * relativeOffset.x * flipped;
        offsetPosition += (Vector2)transform.up * relativeOffset.y;

        return offsetPosition;
    }

    public void JumpInDirection(Vector2 direction, float stregth)
    {
        Rigidbody2D.velocity = direction.normalized * stregth;
        jump = false;
        isGrounded = false;
        canJump = false;
    }
}
