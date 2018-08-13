using UnityEngine;
using System.Collections;

public class PlayerTracker : MonoBehaviour
{
    public Transform player;

    public int MaxXDistance = 1;
    public int MaxYDistance = 2;

    private Player palyercontroller;
    // Use this for initialization
    void Start()
    {
        palyercontroller = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

        var position = Camera.main.transform.position;

        if (palyercontroller.HorizontalMovement == Vector3.right || palyercontroller.HorizontalMovement == Vector3.zero)
        {

            var result = player.position - Camera.main.transform.position;

    

            if (result.x > MaxXDistance)
            {

                position.x = player.position.x - MaxXDistance;

            }

            if (Mathf.Abs(result.y) > MaxXDistance)
            {
                position.y = result.y >= 0 ? player.position.y - MaxXDistance : player.position.y + MaxXDistance;
            }

      
        }
        else if (palyercontroller.HorizontalMovement == Vector3.left)
        {
            var result =  Camera.main.transform.position - player.position ;

            if (result.x > MaxXDistance)
            {

                position.x = player.position.x + MaxXDistance;

            }

            if (Mathf.Abs(result.y) > MaxXDistance)
            {
                position.y = result.y>=0? player.position.y - MaxXDistance : player.position.y + MaxXDistance;
            }

            Camera.main.transform.position = position;
        }

        Camera.main.transform.position = position;

    }
}
