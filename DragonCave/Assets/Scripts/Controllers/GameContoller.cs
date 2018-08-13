using UnityEngine;
using System.Collections;

public abstract class GameContoller : MonoBehaviour
{
    protected LevelInfo levelInformation;

    public Transform playerTransform;

    protected IPlayerStatus playerInfo;

    protected GameStates gameState;

    public int CurrentLevel = 0;

    // Use this for initialization
    void Start()
    {
        playerInfo = playerTransform.GetComponent<IPlayerStatus>();
        //todo should be _InitScreen
        gameState = GameStates.Playing;
        ProcessStart();
    }

    // Update is called once per frame
    void Update()
    {

        switch (gameState)
        {
            case GameStates._InitScreen:
                break;
            case GameStates.Playing:
                ProcessUpdate();
                break;
            case GameStates.PlayerHasWon:
                //score

                
                break;
            case GameStates.NextLevelLoading:

                break;
            case GameStates.ScoreScreen:
                break;
        }

        GameControllerUpdate();

    }

    public void ProcessUpdate()
    {

        if (levelInformation!= null && levelInformation.IsCurrentCheckPointActivated(playerTransform.position))
        {
            levelInformation.ActivateEvent();
            levelInformation.NextCheckPoint();
        }
    }

    public void OnGUI()
    {
        ProcessOnGUI();
    }


    public virtual void StartPlaying()
    {
        gameState = GameStates.Playing;
    }

    public virtual void ProcessStart()
    {

    }

    public virtual void ProcessOnGUI()
    {

    }
    public virtual void GameControllerUpdate()
    {

    }



}
