using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerSeed : MonoBehaviour
{
    private bool pickedUp = false;
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickedUp)
            return;

        switch (collision.gameObject.tag)
        {
            case "Player":
                collision.gameObject.GetComponent<PlayerController>().PickupSunflowerSeed(value);
                pickedUp = true;
                Destroy(gameObject);
                break;
            default:
                break;
        }

        //foreach (var collider in gameObject.GetComponents<Collider2D>())
        //{
        //    collider.enabled = false;
        //}
    }
}
