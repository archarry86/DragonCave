using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour
{

     void Start()
    {
        
    }



    void OnTriggerEnter2D(Collider2D col)
    {
        
        //Debug.Log("DeadZone OnTriggerEnter2D");
        ProcessCollision(col);
    }

    void OnTriggerStay2D(Collider2D col)
    {
       
        //Debug.Log("DeadZone OnTriggerStay2D");
        ProcessCollision(col);

    }

    private void ProcessCollision(Collider2D col)
    {


        if (LevelController.instance.gameState != GameStates.Playing)
            return;


        //validar que se ha el player
        if (col.transform.gameObject.layer != 9)
            return;
        LevelController.instance.KillPlayerDueDeadZone();
    }

 
}
