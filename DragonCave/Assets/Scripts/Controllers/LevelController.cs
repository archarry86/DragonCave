using UnityEngine;
using System.Collections;
using System;

public class LevelController : GameContoller
{
   public static LevelController instance;


    public LevelInfo[] levels;

    public override void ProcessStart()
    {
        if(instance == null)
        instance = this;
        else
        {
            Destroy(this);
            return;
        }
        if(levels!= null && levels.Length > 0)
        levelInformation = levels[0];


        AudioController.instance.LoopSound(Sounds.Theme);
    }

    public void NextLevel()
    {
        CurrentLevel++;

        if(CurrentLevel < levels.Length)
        {
            //load new level
            LoadNextLevel();
        }
        else
        {
            ViewController.instance.ShowThanksForPlaying();
        }

    }

    private void LoadNextLevel()
    {
        ViewController.instance.ShowLevelTransition();
        levelInformation= levels[CurrentLevel];
        Camera.main.transform.position = levelInformation.initialCameraPosition;
        playerTransform.position = levelInformation.initialPlayerPosition;
        this.StartPlaying();
    }



    //public override void MyUpdate()
    //{

    //}

    public override void ProcessOnGUI()
    {

        switch (gameState)
        {
            case GameStates._InitScreen:
                break;
            case GameStates.Playing:

                if (playerInfo.IsAlive())
                {

                }
                else
                {

                }

                break;
            case GameStates.PlayerHasWon:

                break;
            case GameStates.PlayerHasDead:

                break;
            case GameStates.NextLevelLoading:

                break;
            case GameStates.ScoreScreen:
                break;
        }
      
    }

    public void KillPlayerDueDeadZone()
    {
       // if (gameState == GameStates.Playing && !playerInfo.IsAlive())
       //     return;
       //
       // gameState = GameStates.PlayerHasDead;
       // var player =  playerTransform.GetComponent<Player>();
       //
       // player.Dead();


    }

    public void PlayerHasWon()
    {
        if (gameState != GameStates.Playing)
            return;

        gameState = GameStates.PlayerHasWon;
        //show score 
        ViewController.instance.ShowLevelPassed();
        ViewController.instance.ShowScore();

        AudioController.instance.StopSound(Sounds.Theme);
        AudioController.instance.PlaySound(Sounds.Victory);
    }

    public void PlayerHasDead()
    {
        

      
        //show score 
        ViewController.instance.ShowPlayerHasDead();
        ViewController.instance.ShowScore();
    }



}
