using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public BoxCollider2D TriggerArea;
    public BoxCollider2D DamageArea;

    public float TimeToActivate = 0.5f;
    public float TimeActivated = 1f;

    private bool activated = false;
    private float timer = 999f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Activated", false);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Triggered to get activated but not activated
        if (!activated && timer < TimeToActivate + TimeActivated)
        {
            // Should activate
            if (timer >= TimeToActivate)
            {
                activated = true;
                animator.SetBool("Activated", true);
                DamageArea.enabled = true;
            }
        }
        // Activated and should deactivate
        else if (activated && timer > TimeToActivate + TimeActivated)
        {
            activated = false;
            animator.SetBool("Activated", false);
            DamageArea.enabled = false;
            TriggerArea.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated)
            return;

        switch (collision.gameObject.tag)
        {
            case "Player":
                TriggerArea.enabled = false;
                timer = 0f;
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!activated)
            return;

        switch (collision.gameObject.tag)
        {
            case "Player":
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(gameObject, collision.GetContact(0).point);
                break;
            default:
                break;
        }
    }
}
