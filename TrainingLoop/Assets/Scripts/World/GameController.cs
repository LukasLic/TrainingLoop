using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Round
{
    public string Name;
    public float Duration;
    public float SeedTime;
    public GameObject ActiveObjects;
}

public class GameController : GenericSingleton<GameController>
{
    public Vector3 WorldCenter = Vector3.zero;
    public PlayerController Player;
    public Transform Wheel;
    public Transform[] ObjectsRoots;
    public Transform WorldRoot;

    public Transform SeedSpawners;

    public GameObject VictoryScreen;
    public GameObject GameOverScreen;

    public SmoothCamera SmoothCamera;

    private Vector3 startingPosition;

    public Round[] Rounds;

    private bool Paused = false;
    private int CurrentRound = 0;
    private float RoundTimer = 0f;
    private int seedsSpawned;

    [Header("Prefabs")]
    public GameObject SeedPrefab;

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
        if (Paused)
            return;

        RoundTimer += Time.deltaTime;

        if (RoundTimer >= Rounds[CurrentRound].SeedTime * seedsSpawned)
            SpawnSeed();

        if (RoundTimer >= Rounds[CurrentRound].Duration)
            EndRound();
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
        Paused = true;
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

        // Reset rounds.
        CurrentRound = 0;
        RoundTimer = 0f;
        Paused = false;

        // Start round
        StartRound();
    }

    private void EndRound()
    {
        // Puase Player and Camera
        Player.gameObject.SetActive(false);
        Player.enabled = false;
        SmoothCamera.enabled = false;

        // Increase round if possible.
        CurrentRound++;
        if (CurrentRound >= Rounds.Length)
            CurrentRound = Rounds.Length - 1;

        // Clean world.
        for (int i = 0; i < ObjectsRoots.Length; i++)
            for (int j = 0; j < ObjectsRoots[i].childCount; j++)
                Destroy(ObjectsRoots[i].GetChild(j).gameObject);

        // Pause game
        Paused = true;

        Debug.LogError("OPEN SHOP");
    }

    private void StartRound()
    {
        // Reset camera.
        SmoothCamera.enabled = true;
        SmoothCamera.transform.position = new Vector3(0f, 0f, SmoothCamera.transform.position.z);

        // Reset the loop.
        Wheel.rotation = Quaternion.identity;

        // Activate Player
        Player.enabled = true;
        Player.BodyRenderer.color = Color.white;
        Player.transform.position = startingPosition;
        Player.transform.rotation = Quaternion.identity;
        Player.gameObject.SetActive(true);

        // Unpause
        Paused = false;

        // Other
        seedsSpawned = 0;

        // TODO: Spawn objects

        Debug.LogError(Rounds[CurrentRound].Name);
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

    public void FinishShopping()
    {
        StartRound();
    }

    private void SpawnSeed()
    {
        var r = UnityEngine.Random.value;
        var ch = (float)SeedSpawners.childCount * r;

        var pos = SeedSpawners.GetChild((int)ch).position;

        var rb = Instantiate(SeedPrefab, pos, Quaternion.identity, WorldRoot).GetComponent<Rigidbody2D>();

        var r_v = UnityEngine.Random.value;
        rb.velocity = (new Vector2(8f * r_v, 14f));

        seedsSpawned++;
    }
}
