using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    // Use this for initialization
    void Awake()
    {

        if(instance == null)
        {
            instance = this;
        }
        else{
            Destroy(this);
            return;
        }
    }

   public void LoopSound(string name)
    {

    }

    public void StopSound(string name)
    {

    }

    public void StartSound(string name)
    {

    }
}
