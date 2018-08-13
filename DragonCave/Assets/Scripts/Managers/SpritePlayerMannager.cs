using UnityEngine;
using System.Collections;

public class SpritePlayerMannager : MonoBehaviour
{

    public static SpritePlayerMannager instance;
    public Sprite[] PlayerSprites = new Sprite[System.Enum.GetValues(typeof(PlayerStates)).Length];


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
