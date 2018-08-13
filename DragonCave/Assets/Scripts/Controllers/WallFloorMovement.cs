using UnityEngine;
using System.Collections;
using System;

public class WallFloorMovement : MonoBehaviour, ISwitchListener
{
    public int MilisecondsYeloowTime = 5000;
    private TimeSpan yellowTime;

    private DateTime iniDate = DateTime.MinValue;

    public Vector3 direction;

    private float velscale;

    public float velnormalscale;

    public float yellowswitchvelscale;

    private float velrapidscale;


    public MovingWalStates movingState;

    public SwitchTypes switchType;

 
    // Use this for initialization
    void Start()
    {
        movingState = MovingWalStates._Still;

        velscale = velnormalscale;

        velrapidscale = velnormalscale * 5;

        yellowTime = TimeSpan.FromMilliseconds(MilisecondsYeloowTime);

       
    }


    // Update is called once per frame
    void Update()
    {
        
        switch (movingState)
        {
            case MovingWalStates._Still:
                break;
            case MovingWalStates.Crashed:
                break;
            case MovingWalStates.Moving:
                //CheckColiision
              
                this.transform.position += direction * Time.deltaTime * velscale;
           
                break;
            case MovingWalStates.OnSwitchBehavior:
                SwitchBehavior();
                break;
        }
    }

    private bool CheckGroundCollission()
    {
        bool result = false;
        BoxCollider2D boxcollider = this.GetComponent<BoxCollider2D>();
        Vector3 ls = (boxcollider.size) / 2;// * this.direction;
        ls.x *= this.direction.x;

        ls.y *= this.direction.y;
        ls.z *= this.direction.z;


        return result;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        // DrawGuizmos();
    }
    private void DrawGuizmos()
    {


        BoxCollider2D boxcollider = this.GetComponent<BoxCollider2D>();
        Vector3 ls = (boxcollider.size) / 2;// * this.direction;
        ls.x *= this.direction.x;
        ls.y *= this.direction.y;
        ls.z *= this.direction.z;

        Gizmos.DrawSphere(this.transform.position + ls, 0.5f);

    }

    public void StartMoving()
    {
        this.movingState = MovingWalStates.Moving;
    }

    public void SwitchOn(SwitchTypes switchType)
    {

        this.switchType = switchType;
        this.movingState = MovingWalStates.OnSwitchBehavior;
         Debug.Log("switchType= " + this.switchType + ", movingState=" + this.movingState);
    }

    public void SwitchBehavior()
    {
        switch (switchType)
        {
            case SwitchTypes.Blue:
                this.movingState = MovingWalStates._Still;
                break;
            case SwitchTypes.Green:
                break;
            case SwitchTypes.Red:
                this.velscale = velrapidscale;
                this.movingState = MovingWalStates.Moving;
                break;
            case SwitchTypes.Yellow:

                //Cause I have not used couroutines
                if (iniDate == DateTime.MinValue)
                {
                    this.velscale = yellowswitchvelscale;
                    iniDate = DateTime.Now;
                    return;
                }

                if (!IsOnYellowSwitchBehavior())
                {
                    iniDate = DateTime.MinValue;
                    this.velscale = velrapidscale;
                    this.movingState = MovingWalStates.Moving;
                }
                else
                {
                    this.transform.position += direction * Time.deltaTime * velscale;
                }
                break;
        }
    }

    private bool IsOnYellowSwitchBehavior()
    {
        return (DateTime.Now - iniDate) <= yellowTime;
    }


    void OnGUI()
    {

        switch (movingState)
        {
            case MovingWalStates._Still:
                break;
            case MovingWalStates.Crashed:
                break;
            case MovingWalStates.Moving:

                break;
            case MovingWalStates.OnSwitchBehavior:
                switch (switchType)
                {
                    case SwitchTypes.Blue:
                        ;
                        break;
                    case SwitchTypes.Green:
                        break;
                    case SwitchTypes.Red:

                        break;
                    case SwitchTypes.Yellow:
                        //SHOW ON CANVAS TIMING
                        var result = (DateTime.Now - iniDate);
                        if (result <= yellowTime)
                        {
                            if (ViewController.IsValidViewController())
                                ViewController.instance.ShowCounter((int)yellowTime.TotalSeconds - (int)result.TotalSeconds);
                        }
                        break;
                }
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        ProcessCollision(col);
    }

    void OnCollisionStay2D(Collision2D col)
    {
       // Debug.Log("OnCollisionStay2D");
        ProcessCollision(col);

    }

    private void ProcessCollision(Collision2D col)
    {
        if (col.gameObject.layer == 8 && movingState == MovingWalStates.Moving)
        {
            Debug.Log("ProcessCollision");
            this.movingState = MovingWalStates.Crashed;
        }
    }
}