using UnityEngine;
using System.Collections;
using System;

public class WallFloorMovement : MonoBehaviour, ISwitchListener
{
    public TimeSpan yellowTime = TimeSpan.FromMilliseconds(5000);

    private DateTime iniDate = DateTime.MaxValue;

    public Vector3 direction;

    private float velscale;

    public float velnormalscale;

    public float yellowswitchvelscale;

    public float velrapidscale;


    public MovingWalStates movingState;

    public SwitchTypes switchType;

    protected ContactFilter2D contactFilter;

    private static RaycastHit2D[] chacheRays = new RaycastHit2D[10];

    // Use this for initialization
    void Start()
    {
        movingState = MovingWalStates._Still;

        velscale = velnormalscale;

        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }


    // Update is called once per frame
    void Update()
    {
        DrawGuizmos();
        switch (movingState)
        {
            case MovingWalStates._Still:
                break;
            case MovingWalStates.Crashed:
                break;
            case MovingWalStates.Moving:
                //CheckColiision
                if (CheckGroundCollission())
                {
                    movingState = MovingWalStates.Crashed;
                }
                else
                {
                    this.transform.position += direction * Time.deltaTime * velscale;
                }
                break;
            case MovingWalStates.OnSwitchBehavior:

                break;
        }
    }

    private bool CheckGroundCollission()
    {
        bool result = false;

       Vector3 ls = this.transform.localScale / 2;// * this.direction;
        ls.x *= this.direction.x;
        ls.y *= this.direction.y;
        ls.z *= this.direction.z;
        
        int rays = Physics2D.Raycast(this.transform.position + ls, this.direction, contactFilter, chacheRays, 1);

        result = rays > 0;

        return result;
    }

    private void DrawGuizmos()
    {
       

        Vector3 ls = this.transform.localScale / 2;// * this.direction;
        ls.x *= this.direction.x;
        ls.y *= this.direction.y;
        ls.z *= this.direction.z;

        Gizmos.DrawSphere(this.transform.position + ls, 3);
      
    }

    public void StartMoving()
    {
        this.movingState = MovingWalStates.Moving;
    }

    public void SwitchOn(SwitchTypes switchType)
    {

        this.switchType = switchType;
        this.movingState = MovingWalStates.OnSwitchBehavior;
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
}