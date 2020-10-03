using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : GenericSingleton<GameController>
{
    public Vector3 WorldCenter = Vector3.zero;
    public PlayerController Player;

    public void GameOver()
    {

    }

    public override void Awake()
    {
        base.Awake();

        if (Player == null)
            Player = FindObjectOfType<PlayerController>();

        StartGame();
    }

    public void StartGame()
    {
        Player.Lives = Player.MaxLives;
    }
}
