using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WormController : MonoBehaviour
{
    public SpriteRenderer RocketImage;
    public GameObject RocketPrefab;

    public Transform TempObjectsRoot;

    public Transform Player;

    public float TimeToShoot;
    public float RocketAppearTime;
    private float shootTimer;

    private void Awake()
    {
        // Random small offset
        shootTimer = Random.value * 1.5f;

        shootTimer = 0f;
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;

        if(shootTimer >=RocketAppearTime)
        {
            if(shootTimer >= TimeToShoot)
            {
                Shoot(); // After scope returns the alpha below is changed to 0
            }

            var c = Color.white;
            var fadeDuration = TimeToShoot - RocketAppearTime;
            var currentTime = shootTimer - RocketAppearTime;
            c.a = currentTime / fadeDuration;
            RocketImage.color = c;
        }    
    }

    private void Shoot()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player").transform;

        Instantiate(    RocketPrefab,
                        RocketImage.transform.position,
                        RocketImage.transform.rotation,
                        TempObjectsRoot)
            .GetComponent<Rocket>().SetTarget(Player);

        shootTimer = 0f;
    }
}
