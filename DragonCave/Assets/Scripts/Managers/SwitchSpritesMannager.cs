using UnityEngine;
using System.Collections;

public class SwitchSpritesMannager : MonoBehaviour
{

    public static SwitchSpritesMannager instance;
    public Sprite[] SwtitchSprites = new Sprite[System.Enum.GetValues(typeof(SwitchTypes)).Length * 2];


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
       

    }
}

