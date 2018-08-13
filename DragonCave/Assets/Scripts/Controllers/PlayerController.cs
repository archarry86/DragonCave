using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour,IPlayerStatus
{
    public PlayerStates playerState;

    // Use this for initialization
    void Start()
    {
        playerState = PlayerStates._Initial;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsAlive()
    {
        return playerState != PlayerStates.Dead;
    }
}
