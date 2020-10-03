using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int SunflowerSeeds = 0;

    public float damageKnockback;
    public float invulTime;

    [SerializeField]
    private int lives;
    public int Lives
    {
        get => lives;
        set
        {
            lives = value;

            if (lives <= 0)
            {
                lives = 0;
                Die();
            }
            else if (lives > MaxLives)
            {
                lives = MaxLives;
                Debug.LogWarning("gained more than max lives!");
            }

            // TODO: Update UI

            // TODO: Throwback
            // TODO Invincibility frames

        }
    }

    public int MaxLives = 3;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    switch (collision.gameObject.tag)
    //    {
    //        case "SunflowerSeed":
    //            PickupSunflowerSeed(collision.gameObject);
    //            break;
    //        case "Rocket":
    //            var contacts = collision.;
    //            TakeDamage(collision.gameObject, [0]);
    //            Destroy(collision.gameObject);
    //            break;
    //        case "DamageSource":
    //            TakeDamage(collision.gameObject);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "SunflowerSeed":
                PickupSunflowerSeed(collision.gameObject);
                break;
            case "Rocket":
                TakeDamage(collision.gameObject, collision.GetContact(0).point);
                Destroy(collision.gameObject);
                break;
            case "DamageSource":
                TakeDamage(collision.gameObject, collision.GetContact(0).point);
                break;
            default:
                break;
        }
    }

    public void TakeDamage(GameObject damageSource, Vector3 point)
    {
        Lives -= GetValue(damageSource);
        Movement.JumpInDirection(transform.position - point, damageKnockback);

    }

    private void PickupSunflowerSeed(GameObject seed)
    {
        SunflowerSeeds += GetValue(seed);
        Destroy(seed);
    }

    private void Die()
    {
        GameController.Instance.GameOver();
    }

    private int GetValue(GameObject gameObject)
    {
        var component = gameObject.GetComponent<ValueHolder>();
        if (component == null)
        {
            Debug.LogWarning("ValueHolder not found!");
            return 0;
        }
            

        return component.Value;
    }

    private Movement m;
    public Movement Movement
    {
        get
        {
            if (m == null)
                m = GetComponent<Movement>();

            return m;
        }
    }
}
