using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRGb : MonoBehaviour
{
    public float AddedForcescalar = 0.0f;
    public float ForceTimescalar = 0.0f;

    private Vector2 addedForce = Vector2.zero;
    private float ForceTime = -1;

    public float OntransitionTime = -1;

    public float minGroundNormalY = .3f;

    private int pressjump = 0;

    private readonly Vector2 unitvector = new Vector2(1f, 1f);


    public int HorizontaleForce = 1;

    private Vector2 velocity = Vector2.zero;

    private Vector2 groundnormal = Vector2.zero;

    private Vector2 movealongground = Vector2.zero;

    public float JumpingForce = 1;

    private Vector2 vJumpingForce = Vector2.up;

    public float JumpingShortforce = 0.5f;

    private Vector2 vJumpingShortforce = Vector2.down;

    private Vector2 VerticalForce = Vector2.zero;

    public Vector2 SizeCollision = new Vector2(1, 1);

    public Vector2 BodySize = new Vector2(1, 1);

    public PlayerStates playerState = PlayerStates.OnGround;

    public PlayerStates nextplayerState = PlayerStates.OnGround;

    private bool onGround = false;

    private bool touchWall = false;

    private Rigidbody2D body;

    private float shell = 0.09f;

    // Use this for initialization
    void Start()
    {
        float _x = this.GetComponent<CapsuleCollider2D>().bounds.size.x * 1.1f;// * 0.7f;
        float _y = this.GetComponent<CapsuleCollider2D>().bounds.size.y;// * 0.5f;// * 0.5f;
        SizeCollision = new Vector2(_x , _y);

         _x = this.GetComponent<CapsuleCollider2D>().bounds.size.x;// * 0.7f;
         _y = this.GetComponent<CapsuleCollider2D>().bounds.size.y;// * 0.5f;// * 0.5f;

        BodySize = new Vector2(_x, _y);

        body = this.GetComponent<Rigidbody2D>();

        groundnormal = unitvector;

        vJumpingForce = Vector2.up * JumpingForce;
        vJumpingShortforce = Vector2.down * JumpingShortforce;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(playerState == PlayerStates.OnTransition)
        {
            if ( Time.time > OntransitionTime)
            {
                FinishTransition();
            }
            else
                return;
        }

        Vector2 forward = this.transform.forward;
        //Debug.Log(Time.time + " " + forward);

        float defgrav = 20f;
        //Debug.Log(Time.time + " " + playerState + " " + touchWall);
        var lastVelocity = velocity;

        #region Input 

        velocity = Vector2.zero;

        //default
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity = Vector2.left;
            var aux =   this.transform.rotation.eulerAngles;// = Quaternion.EulerAngles(new Vector3(0, 180, 0));
            aux.y = 180f;
            this.transform.eulerAngles =( aux);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity = Vector2.right;
            var aux = this.transform.rotation.eulerAngles;// = Quaternion.EulerAngles(new Vector3(0, 180, 0));
            aux.y = 0f;
            this.transform.eulerAngles = (aux);
        }


        if(addedForce.x != 0 || addedForce.y != 0){

            if(Time.time < ForceTime)
            {
                velocity = addedForce;
            }
            else
            {
                addedForce = Vector2.zero;
                ForceTime = -1;
            }
           
        }

        switch (playerState)
        {
            // case PlayerStates._Initial:
            //     break;

            case PlayerStates.OnGround:

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (pressjump > 10)
                    {
                        throw new UnityException("FAIL!!!!!!!!!!");
                    }

                    VerticalForce = vJumpingForce;
                    playerState = PlayerStates.Jumpling;
                    pressjump = (pressjump++);
                    defgrav = 0.0f;
                }



                break;

            // break;
            // case PlayerStates.Dead:
            //     break;
            // case PlayerStates.HasWon:
            //     break;
            case PlayerStates.Falling:


                if (touchWall && Input.GetKeyDown(KeyCode.Space))
                {
                    this.AddForce(-velocity , ForceTimescalar);
                    VerticalForce = vJumpingForce * 1.1f;
                    playerState = PlayerStates.Jumpling;
                    pressjump = 100;
                    defgrav = 0.0f;
                }

                if (touchWall)
                    defgrav = 0.25f;

                break;
            case PlayerStates.Jumpling:

                if (pressjump < 5)
                {
                    pressjump++;
                    if (!Input.GetKey(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space))
                    {
                        VerticalForce = VerticalForce + vJumpingShortforce;
                    }

                }


                if (touchWall && VerticalForce.sqrMagnitude <0.1f && Input.GetKeyDown(KeyCode.Space))
                {
                    this.AddForce(-velocity, ForceTimescalar);

                    VerticalForce = vJumpingForce * 1.1f;
                    playerState = PlayerStates.Jumpling;
                    pressjump = 100;
                }




                break;
                // case PlayerStates.WallSliding:
                //     break;


        }
        #endregion

        #region check collisions 


        int layer = 1 << LayerMask.NameToLayer("Ground");

        var pos = new Vector2(this.transform.position.x, this.transform.position.y);

        touchWall = false;

        //var hitwall = Physics2D.Linecast(pos, pos + (velocity) * 1.8f, layer);
        //var velaux = new Vector2(velocity.x, velocity.y).normalized;
        
        var hitwall  = Physics2D.BoxCast(pos+(velocity * 0.4F)  ,   SizeCollision,0, velocity,0.2f, layer);

        if (velocity != Vector2.zero &&  hitwall.collider != null )
        {
            var zangle =  hitwall.collider.gameObject.transform.rotation.eulerAngles.z;
            if ( ((hitwall.normal.x ==-1 || hitwall.normal.x == 1) && hitwall.normal.y == 0))
            {
                touchWall = true;

                

                if (playerState == PlayerStates.OnGround && ((zangle != 90.0f) && (zangle != 270.0f))  )
                {
                    touchWall = false;
                }

                if(playerState == PlayerStates.Falling)
                {
                    defgrav = 0.25f;
                }
            }


           // Debug.Log(Time.time + " wall normal " + hitwall.normal + " " + playerState + " " + hitwall.distance + " " + zangle + " "+ touchWall);

        }


        //Debug.Log(Time.time + " touchWall " + touchWall);
        Vector2 vaux = (SizeCollision.x *0.5f) *velocity;// velocity * 1.5f;// * 0.5f;

        var scale = (SizeCollision.y * 0.5f) *(  playerState == PlayerStates.OnGround ? 1.5f : 1.25f);

        var hitground = Physics2D.Linecast(pos + (vaux), pos + (vaux) + (Vector2.down * scale), layer);

        if(hitground.collider == null)
        {

            var _scale = (SizeCollision.y * 0.5f) * (playerState == PlayerStates.OnGround ? 1.5f : 1.25f);
            hitground = Physics2D.Linecast(pos - (vaux), pos -  vaux+(Vector2.down * _scale), layer);

        }

        //the object is not touching the fround


        if (hitground.collider != null)
        {
          
           if (hitground.normal.y > minGroundNormalY  )
            {
                groundnormal = hitground.normal;
                defgrav = 0;

                switch (playerState)
                {

                    case PlayerStates.OnGround:

                        break;
                    case PlayerStates.Dead:
                        break;
                    case PlayerStates.HasWon:
                        break;
                    case PlayerStates.Falling:
                        playerState = PlayerStates.OnGround;
                        break;
                    case PlayerStates.Jumpling:
                        break;
                    case PlayerStates.WallSliding:
                        playerState = PlayerStates.OnGround;
                        break;
                }
            }
        }
        else
        {
            switch (playerState)
            {

                case PlayerStates.OnGround:

                    playerState = PlayerStates.Falling;

                    break;
                case PlayerStates.Dead:
                    break;
                case PlayerStates.HasWon:
                    break;
                case PlayerStates.Falling:
                    break;
                case PlayerStates.Jumpling:
                 
                    break;
                case PlayerStates.WallSliding:
                    playerState = PlayerStates.Falling;
                    break;
            }

        }
        #endregion


        #region Movement




        switch (playerState)
        {
            // case PlayerStates._Initial:
            //     break;
            // case PlayerStates.OnGround:
            //
            //     CheckOnGround();
            //
            //     if (!onGround)
            //     {
            //         playerState = PlayerStates.Falling;
            //     }
            //
            //     break;
            // case PlayerStates.Dead:
            //     break;
            // case PlayerStates.HasWon:
            //     break;
            // case PlayerStates.Falling:
            //     break;
            case PlayerStates.Jumpling:
                defgrav = 0;
                ProcessJumping();

                break;
                // case PlayerStates.WallSliding:
                //     break;
                //default:
                //    break;


        }


        body.gravityScale = defgrav;
        //Debug.Log(Time.time + " velocity " + velocity + " touchWall " + touchWall + " groundnormal "+ groundnormal+ " hitwallnormal  " + hitwall.normal + "  hitwalldistance  " + hitwall.distance);

        if (  velocity != Vector2.zero )
        {
            if (velocity.x == Vector2.right.x)
            {
                movealongground = new Vector2(this.groundnormal.y, -this.groundnormal.x);
            }

            if (velocity.x == Vector2.left.x)
            {
                movealongground = new Vector2(-(this.groundnormal.y), -(-this.groundnormal.x));
            }

            if (hitground.collider == null)
            {
                movealongground = velocity;
            }


            
           
            body.position = body.position +( (!touchWall) ? ((movealongground.normalized * HorizontaleForce) * Time.deltaTime):(movealongground.normalized * (hitwall.distance +( -shell) )));


        }
        else
        {
            movealongground = velocity;
        }


        #endregion

        if (hitground.collider != null)
        {
            body.velocity = Vector2.zero;
        }


        var vel = body.velocity;
        vel.x = 0;
        if (touchWall && (playerState == PlayerStates.Falling ) && body.velocity.y <-10)// && body.velocity.y>)
        {
      
            vel.y = -10f;
            //vel.x = vel.x*(velocity.x/ );
            body.velocity = vel;
        }

        if (!touchWall && (playerState == PlayerStates.Falling) && body.velocity.y < -100)
        {
    
            vel.y = -100;

        }


        body.velocity = vel;

        //Debug.Log(Time.time +" "+ playerState+" "+ body.velocity);

    }

    private void ProcessJumping()
    {


        var aux = body.position + VerticalForce;
        //aux.x = body.position.x;
        //body.MovePosition(aux);
        body.position = aux;
        VerticalForce = VerticalForce + Physics2D.gravity * Time.deltaTime;

        if (VerticalForce.y < 0)
        {
            this.playerState = PlayerStates.Falling;
            VerticalForce = Vector2.zero;
            pressjump = 0;
        }
    }

    void PhysicsProcess()
    {

        switch (playerState)
        {
            case PlayerStates._Initial:
                break;
            case PlayerStates.OnGround:

                CheckOnGround();

                if (!onGround)
                {
                    playerState = PlayerStates.Falling;
                }

                break;
            case PlayerStates.Dead:
                break;
            case PlayerStates.HasWon:
                break;
            case PlayerStates.Falling:
                break;
            case PlayerStates.Jumpling:
                break;
            case PlayerStates.WallSliding:
                break;
        }
    }

    void CheckOnGround()
    {


    }

    void TouchWall()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 pos = this.transform.position;// new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 forward = this.transform.right;
      
        Gizmos.DrawLine(pos+ Vector2.down *0.10f, pos + Vector2.down * 0.10f + (forward * 10));

        Gizmos.color = Color.red;
         pos = new Vector2(this.transform.position.x, this.transform.position.y);
        Gizmos.DrawLine(pos, pos + (movealongground * 5));

        //var hitwall = Physics2D.BoxCast(pos, SizeCollision, 0, velocity.normalized, layer);
        //var hitwall = Physics2D.BoxCast(pos + (velocity) * 0.5f, SizeCollision, 0, velocity, 1, layer);

        Gizmos.DrawWireCube(pos + (velocity * 0.4F), SizeCollision);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos , pos + groundnormal);


        var scale = (SizeCollision.y * 0.5f) * (playerState == PlayerStates.OnGround ? 1.5f : 1.5f);
        Gizmos.color = Color.yellow;
        Vector2 vaux = (SizeCollision.x * 0.5f) * velocity;// velocity * 1.5f;// * 0.5f;

       

        Gizmos.DrawLine(pos + (vaux), pos + (vaux) + (Vector2.down * scale));
        //   hitground = Physics2D.Linecast(pos - (vaux), pos - (vaux) + (Vector2.down * scale), layer);
        var _scale = (SizeCollision.y * 0.5f) * 1.8f;
        Gizmos.DrawLine(pos - (vaux), pos - (vaux) + (Vector2.down * _scale));

    }


    public void OnTransition(PlayerStates _next, float param = -1f)
    {
        this.playerState = PlayerStates.OnTransition;

        if(param > 0)
        OntransitionTime = Time.time + param;

        nextplayerState = _next;
    }


    public void FinishTransition()
    {
        this.playerState = nextplayerState;
        OntransitionTime = -1;
    }

    public void AddForce(Vector2 force,float time )
    {   if (time == 0 || AddedForcescalar == 0)
            return;
        ForceTime = Time.time+ time;
        addedForce = force * AddedForcescalar;

    }

}