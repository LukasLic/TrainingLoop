using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D rb;
    private Transform target;

    public GameObject ExplosionPrefab;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    //private void Awake()
    //{
    //    SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(gameObject, collision.GetContact(0).point);
                break;
            default:
                break;
        }

        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);

        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void Update()
    {
        if(target != null)
        {
            var dir = transform.position - target.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            var dir = (target.position - transform.position).normalized;
            rb.velocity += (Vector2)dir * Speed * Time.fixedDeltaTime;
        }
    }
}
