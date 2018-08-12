using UnityEngine;
using System.Collections;

public class ViewController : MonoBehaviour
{

    public static ViewController instance;

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


    public virtual void ReStartButton()
    {

    }
    public virtual void QuitButton()
    {

    }

    public virtual void ShowIntro()
    {

    }

    public virtual void ShowScore()
    {

    }
    public virtual void ShowGameOver()
    {

    }

    public virtual void ShowCounter(int seconds)
    {

    }


    public static bool IsValidViewController()
    {
        return instance != null;
    }
}
