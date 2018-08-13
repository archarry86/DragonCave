using UnityEngine;
using System.Collections;

public class StatueTreasure : MonoBehaviour
{


    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log("OnCollisionEnter2D");
        ProcessCollision();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        //Debug.Log("OnCollisionStay2D");
        ProcessCollision();

    }

    private void ProcessCollision()
    {
        //Player has won

        LevelController.instance.PlayerHasWon();
    }

}
