using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    // Use this for initialization
    private AudioSource[] sounds = new AudioSource[0];

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


       var _soundsenum =  System.Enum.GetValues(typeof(Sounds));
        sounds = new AudioSource[_soundsenum.Length];
        int i = 0;
        foreach (var sound in _soundsenum)
        {
            var _clip = Resources.Load<AudioClip>(sound.ToString());

            AudioSource adsrc = gameObject.AddComponent<AudioSource>() ;
            adsrc.name = sound.ToString();
            adsrc.clip = _clip;
            sounds[i] = adsrc;
            i++;
        }

    }

    public void LoopSound(Sounds name)
    {
        AudioSource adsrc = sounds[(int)name];
        if (adsrc.isPlaying)
        {
            adsrc.Stop();
        }
        adsrc.loop = true;
        adsrc.Play();
    }

    public void StopSound(Sounds name)
    {
        AudioSource adsrc = sounds[(int)name];
        adsrc.Stop();
    }

    public void PlaySound(Sounds name)
    {
        AudioSource adsrc = sounds[(int)name];
        if (adsrc.isPlaying)
        {
            adsrc.Stop();
        }

        adsrc.Play();
    }
}
