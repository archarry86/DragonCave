using UnityEngine;
using System.Collections;

public class ViewController : MonoBehaviour
{

    public static ViewController instance;

    public Transform player;

    public Canvas canvas;
    [TextArea]
    public string InitScreen;
    [TextArea]
    public string congratulations =  "Congratulations you have won";
    [TextArea]
    public string DevelopBy = "";

    public int DamageRecived = 0;

    private string Counter = "";

    private LevelController levelController;
    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        levelController = LevelController.instance;
        //if(canvas == null)
        //{
        //    Destroy(this);
        //    return;
        //}

    }

    void OnGUI()
    {
        //var levelinfo= LevelController.instance.levels[LevelController.instance.CurrentLevel];
        // var message = "currentIndexCheckPoint" + levelinfo.currentIndexCheckPoint;

        switch (levelController.gameState)
        {
            case GameStates._InitScreen:

                GUI.Box(new Rect(10,10, Screen.width-10, Screen.height/ 4), InitScreen);

                if(GUI.Button(new Rect((Screen.width / 2) -100, 10 + Screen.height / 4 , 200 , 50), "Start"))
                {
                    levelController.StartPlaying();
                }

                break;
            case GameStates.Playing:

               

                Color current = GUI.color;

                GUI.color = Color.cyan;
                GUI.Box(new Rect(20, 20, 300, 20), "DamageRecived : " + DamageRecived);

                if (!string.IsNullOrEmpty(Counter))
                {
                    GUI.color = Color.red;
                    GUI.Box(new Rect(Screen.width/2-75, 80, 150, 20), Counter);
                    Counter = "";
                }

                if (GUI.Button(new Rect(0,Screen.height -50 , 100, 50), " ReStart "))
                {
                    levelController.ReStartGame();
                }


                GUI.color = current;
                break;
            case GameStates.PlayerHasDead:
                break;
            case GameStates.PlayerHasWon:

                GUI.Box(new Rect(10, 10, Screen.width - 10, Screen.height / 4), congratulations+System.Environment.NewLine+"With Damage "+DamageRecived + System.Environment.NewLine +""+DevelopBy);

                if (GUI.Button(new Rect((Screen.width / 2) -100, 10 + Screen.height / 4, 200 , 50), " TryAgain ? "))
                {
                    levelController.ReStartGame();
                }

                break;
            case GameStates.NextLevelLoading:
                break;
            case GameStates.ScoreScreen:
                break;
        }

    }

    private void OnGUIDebugMessage(ref string message)
    {
        GUI.color = Color.cyan;
        GUI.Label(new Rect(0, 0, 100, 100), message);
    }

    public virtual void StartButton()
    {
        //Debug.Log("View StartButton");
    }

    public virtual void ReStartButton()
    {
        //Debug.Log("View ReStartButton");
    }
    public virtual void QuitButton()
    {
        //Debug.Log("View QuitButton");
    }

    public virtual void NexLevelButton()
    {
        //Debug.Log("View NexLevelButton");
    }

    public virtual void ShowIntro()
    {
        //Debug.Log("View ShowIntro");
    }

    public virtual void ShowScore()
    {
        //Debug.Log("View ShowScore");
    }
    public virtual void ShowGameOver()
    {
        //Debug.Log("View ShowGameOver");
    }

    public virtual void ShowThanksForPlaying()
    {
        //Debug.Log("View ShowThanksForPlaying");
    }

    public virtual void ShowCounter(int seconds)
    {
        //Debug.Log(seconds);
        //Debug.Log("View "+ seconds);
        Counter = seconds + " s";
    }

    public virtual void ShowLevelTransition()
    {
        //Debug.Log("View ShowLevelTransition ");
    }
    public virtual void StopLevelTransition()
    {
        //Debug.Log("View StopLevelTransition ");
    }


    public virtual void ShowLevelPassed()
    {
        Debug.Log("View ShowLevelPassed ");
    }

    public virtual void ShowPlayerHasDead()
    {
        Debug.Log("View ShowPlayerHasDead ");
    }

    public static bool IsValidViewController()
    {
        return instance != null;
    }

    public void Restart()
    {
        DamageRecived = 0;
    }
}
