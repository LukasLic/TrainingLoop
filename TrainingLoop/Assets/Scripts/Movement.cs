using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 1f;
    public float maxSpeed = 2f;
    private float movement;
    private bool isFlipped = false;

    public float horizontalDrag = 2f;

    private Vector3 worldCenter
    {
        get
        {
            return GameController.Instance.WorldCenter;
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
        Debug.DrawLine((Vector2)transform.position, (Vector2)worldCenter);
        //Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + (Right * maxSpeed), Color.green);

        //var movement = Right * speed * Time.deltaTime;
        //transform.position += (Vector3)movement;

        //movement = 1 * speed;
        movement = Input.GetAxisRaw("Horizontal") * speed;
        Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + (Right * movement).normalized, Color.blue);

        // Rotate to top
        var angle = Mathf.Atan2(Up.y, Up.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);



        if (movement == 0/* && Rigidbody2D.velocity.magnitude <= 0.1f*/)
            transform.SetParent(Rotator);
        else
            transform.SetParent(null);
    }

    private void FixedUpdate()
    {
        float t = Rigidbody2D.velocity.x / maxSpeed;

        float lerp = 0f;

        if (t >= 0f)
            lerp = Mathf.Lerp(maxSpeed, 0f, t);
        else if (t < 0f)
            lerp = Mathf.Lerp(maxSpeed, 0f, Mathf.Abs(t));

        //Vector2 force = new Vector2(movement * lerp * speed * Time.fixedDeltaTime, 0f);
        Vector2 force = Right * movement * lerp * speed * Time.fixedDeltaTime;
        //Vector2 drag = new Vector2(-Rigidbody2D.velocity.x * horizontalDrag * Time.fixedDeltaTime, 0f);
        Vector2 drag = -Rigidbody2D.velocity * horizontalDrag * Time.fixedDeltaTime;

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
    }
}
