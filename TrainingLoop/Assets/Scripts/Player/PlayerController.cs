using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int SunflowerSeeds = 0;

    [SerializeField]
    private int lives;
    public int Lives
    {
        get => lives;
        set
        {
            lives += value;

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

        }
    }

    [SerializeField]
    private int MaxLives = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "SunflowerSeed":
                PickupSunflowerSeed(collision.gameObject);
                break;
            case "Rocket":
                TakeDamage(collision.gameObject);
                Destroy(collision.gameObject);
                break;
            case "DamageSource":
                TakeDamage(collision.gameObject);
                break;
            default:
                break;
        }
    }

    public void TakeDamage(GameObject damageSource)
    {
        Lives -= GetValue(damageSource);
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
}
