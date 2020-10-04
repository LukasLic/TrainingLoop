using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : GenericSingleton<GameController>
{
    public Vector3 WorldCenter = Vector3.zero;
    public PlayerController Player;
    public Transform Wheel;
    public Transform[] ObjectsRoots;

    public GameObject VictoryScreen;
    public GameObject GameOverScreen;

    public SmoothCamera SmoothCamera;

    private Vector3 startingPosition;

    public void GameOver()
    {
        StopGame();

        GameOverScreen.SetActive(true);
        StartCoroutine(FadeIn(GameOverScreen.GetComponent<RawImage>().material));
    }

    public void Victory()
    {
        StopGame();

        VictoryScreen.SetActive(true);
        StartCoroutine(FadeIn(VictoryScreen.GetComponent<RawImage>().material));
    }

    

    private void Update()
    {
        //if (Time.realtimeSinceStartup >= 12f && VictoryScreen.activeSelf == false)
            //Victory();
    }

    public override void Awake()
    {
        base.Awake();

        if (Player == null)
            Player = FindObjectOfType<PlayerController>();

        startingPosition = Player.transform.position;

        StartNewGame();
    }

    private void StopGame()
    {
        Player.gameObject.SetActive(false);
        Player.enabled = false;
        SmoothCamera.enabled = false;
    }

    public void StartNewGame()
    {
        // Clean world.
        for (int i = 0; i < ObjectsRoots.Length; i++)
            for (int j = 0; j < ObjectsRoots[i].childCount; j++)
                Destroy(ObjectsRoots[i].GetChild(j).gameObject);

        // Reset the loop.
        Wheel.rotation = Quaternion.identity;

        // Prepare player.
        Player.enabled = true;
        Player.Lives = Player.MaxLives;
        Player.SunflowerSeeds = 0;
        Player.BodyRenderer.color = Color.white;
        Player.transform.position = startingPosition;
        Player.transform.rotation = Quaternion.identity;
        Player.gameObject.SetActive(true);

        // Clean victory/gameover UI;
        GameOverScreen.SetActive(false);
        VictoryScreen.SetActive(false);

        // Reset camera.
        SmoothCamera.enabled = true;
        SmoothCamera.transform.position = new Vector3(0f, 0f, SmoothCamera.transform.position.z);
    }

    IEnumerator FadeIn(Material m)
    {
        var ic = m.color;
        ic.a = 0f;
        m.color = ic;

        var steps = 0;

        for (float ft = 0f; ft <= 1f; ft += 0.05f * steps)
        {
            Color c = m.color;
            c.a += Mathf.Min(ft * steps, 1f);
            m.color = c;

            steps++;

            yield return new WaitForSeconds(.05f);
        }

        var lc = m.color;
        lc.a = 1f;
        m.color = lc;
    }

    public void ResetGameRequest()
    {
        StartNewGame();
    }
}
