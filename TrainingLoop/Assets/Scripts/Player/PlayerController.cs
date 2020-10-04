using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private int sls;
    public int SunflowerSeeds
    {
        get => sls;
        set
        {
            sls = value;
            SunflowerSeedsText.text = sls.ToString();
        }
    }

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
                lives = 0;
            else if (lives > MaxLives)
            {
                lives = MaxLives;
                Debug.LogWarning("gained more than max lives!");
            }

            for (int i = 0; i < LifeImages.Length; i++)
            {
                if (i < lives)
                {
                    LifeImages[i]./*GetComponent<Animator>().*/SetBool("Visible", true);
                    //LifeImages[i].enabled = true;
                }
                else
                {
                    LifeImages[i]./*GetComponent<Animator>().*/SetBool("Visible", false);
                    //LifeImages[i].enabled = false;
                }
            }

            if (lives <= 0)
                Die();

        }
    }

    public int MaxLives = 3;

    public Text SunflowerSeedsText;
    public Animator[] LifeImages;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //switch (collision.gameObject.tag)
    //    //{
    //    //    case "SunflowerSeed":
    //    //        PickupSunflowerSeed(collision.gameObject);
    //    //        break;
    //    //    default:
    //    //        break;
    //    //}
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    switch (collision.gameObject.tag)
    //    {
    //        case "SunflowerSeed":
    //            break;
    //        case "Rocket":
    //            TakeDamage(collision.gameObject, collision.GetContact(0).point);
    //            Destroy(collision.gameObject);
    //            break;
    //        case "DamageSource":
    //            TakeDamage(collision.gameObject, collision.GetContact(0).point);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    public void TakeDamage(GameObject damageSource, Vector3 point)
    {
        Lives -= GetValue(damageSource);
        Movement.JumpInDirection(transform.position - point, damageKnockback);

        if (gameObject.activeSelf)
            StartCoroutine(RedFlash());
    }

    public void PickupSunflowerSeed(GameObject seed)
    {
        SunflowerSeeds += GetValue(seed);
        seed.SetActive(false);
        Destroy(seed);
    }
    public void PickupSunflowerSeed(int amount = 1)
    {
        SunflowerSeeds += amount;
    }

    //private void Awake()
    //{
    //    UpdateUi();
    //}

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

    IEnumerator RedFlash()
    {
        BodyRenderer.material.color = Color.red;
        var steps = 0;

        for (float ft = 0f; ft <= 1f; ft += 0.05f * steps)
        {
            Color c = BodyRenderer.material.color;
            c.g += Mathf.Min(ft * steps, 1f);
            c.b += Mathf.Min(ft * steps, 1f);
            c.g = ft;
            c.b = ft;
            BodyRenderer.material.color = c;

            steps++;

            yield return new WaitForSeconds(.05f);
        }

        BodyRenderer.material.color = Color.white;
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

    public SpriteRenderer BodyRenderer;

    //public void UpdateUi()
    //{
    //    Lives = Lives;
    //    SunflowerSeeds = SunflowerSeeds;
    //}
}
