using UnityEngine;
using System.Collections;

public class ViewController : MonoBehaviour
{

    public static ViewController instance;

    public Transform player;

    public Canvas canvas;

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

        //if(canvas == null)
        //{
        //    Destroy(this);
        //    return;
        //}

    }

    void OnGUI()
    {
       var levelinfo= LevelController.instance.levels[LevelController.instance.CurrentLevel];
        var message = "currentIndexCheckPoint" + levelinfo.currentIndexCheckPoint;

       // OnGUIDebugMessage(ref message);
    }

    private void OnGUIDebugMessage(ref string message)
    {
        GUI.color = Color.cyan;
        GUI.Label(new Rect(0, 0, 100, 100), message);
    }

    public virtual void StartButton()
    {
        Debug.Log("View StartButton");
    }

    public virtual void ReStartButton()
    {
        Debug.Log("View ReStartButton");
    }
    public virtual void QuitButton()
    {
        Debug.Log("View QuitButton");
    }

    public virtual void NexLevelButton()
    {
        Debug.Log("View NexLevelButton");
    }

    public virtual void ShowIntro()
    {
        Debug.Log("View ShowIntro");
    }

    public virtual void ShowScore()
    {
        Debug.Log("View ShowScore");
    }
    public virtual void ShowGameOver()
    {
        Debug.Log("View ShowGameOver");
    }

    public virtual void ShowThanksForPlaying()
    {
        Debug.Log("View ShowThanksForPlaying");
    }

    public virtual void ShowCounter(int seconds)
    {
        //Debug.Log(seconds);
        Debug.Log("View "+ seconds);
    }

    public virtual void ShowLevelTransition()
    {
        Debug.Log("View ShowLevelTransition ");
    }
    public virtual void StopLevelTransition()
    {
        Debug.Log("View StopLevelTransition ");
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
}
