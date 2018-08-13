using UnityEngine;
using System.Collections;

public class StatueTreasure : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(" StatueTreasure OnTriggerEnter2D");
        ProcessCollision(col);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //Debug.Log(" StatueTreasure OnTriggerStay2D");
        ProcessCollision(col);

    }

    private void ProcessCollision(Collider2D col)
    {
        //Player has won

        LevelController.instance.PlayerHasWon();
    }

}
