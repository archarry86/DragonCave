using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour,IPlayerStatus
{
    private int frame = 0;

    public float minGroundNormalY = .65f;

    public PlayerStates playerState;

    public float HorizontalMovementdelta = 0.5f;

    public Vector3 JumpVector = new Vector3(0, 0.25f, 0);

    public Vector3 gravity = new Vector3(0, -9.81f, 0);

    private Vector3 CurrentJumpVector = Vector3.zero;

    private Vector3 CurrentNormal = Vector3.zero;

    private bool isonfloor = false;

    private Vector3 horizontalMovement;

    public Vector3 HorizontalMovement
    {
        get
        {
            return horizontalMovement;
        }
    }


    private new Rigidbody2D rigidbody2D;

    // Use this for initialization
    void Start()
    {
        playerState = PlayerStates.OnGround;

        this.rigidbody2D = this.GetComponent<Rigidbody2D>();

        //TODO provisional
        Camera.main.transform.parent = this.transform;
    }


    // Update is called once per frame
    void Update()
    {
        frame++;
        switch (playerState)
        {

            case PlayerStates.Falling:
                //ProcessFalling();
                CheckMovingButtons();
                CheckOnTheFloor();
                AddHorizontalMovement();

                break;
            case PlayerStates.Jumpling:

                CheckMovingButtons();
                ProcessJump();
                AddHorizontalMovement();


                break;
            case PlayerStates.OnGround:

                CheckMovingButtons();

                var _checkJumping = CheckJumping();
                //Debug.Log(frame+" CheckJumping " + _checkJumping);
                if (_checkJumping)
                    return;

                CheckOnTheFloor();
                AddHorizontalMovement();

               
              

                break;
       

        }
    }

    private void ProcessJump()
    {
        Vector3 playerposition = this.rigidbody2D.position;
        playerposition += this.CurrentJumpVector + Time.deltaTime * gravity;

        this.rigidbody2D.position = playerposition;

        this.CurrentJumpVector = this.CurrentJumpVector + gravity * Time.deltaTime;

        if (this.CurrentJumpVector.y <= 0)
        {
            //Debug.Log(frame + " CurrentJumpVector <= 0 " + this.CurrentJumpVector);
            playerState = PlayerStates.Falling;
            this.CurrentJumpVector = Vector3.zero;

        }

    }

   

    private bool CheckJumping()
    {
        if (playerState == PlayerStates.Jumpling ||
            playerState == PlayerStates.Falling ||
            playerState == PlayerStates.Dead)
            return false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerState = PlayerStates.Jumpling;
            CurrentJumpVector = this.JumpVector;
            return true;
        }

        return false;
    }



    private void CheckMovingButtons()
    {
        horizontalMovement = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalMovement = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalMovement = Vector3.right;
        }

       // this.transform.Translate(horizontalMovement * HorizontalMovementdelta * Time.deltaTime);

       

    }



    private void AddHorizontalMovement()
    {
        this.rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation ;

        Vector3 moveAlongGround = new Vector3(CurrentNormal.y * horizontalMovement.x , -CurrentNormal.x* horizontalMovement.y); 
        if(moveAlongGround != Vector3.zero)
        {
            Vector3 pos = rigidbody2D.position;
            rigidbody2D.position = pos + moveAlongGround * HorizontalMovementdelta * Time.deltaTime;

        }
        else
        {
            this.rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

     

    }

    private void CheckOnTheFloor()
    {
        this.rigidbody2D.gravityScale = 1;
        CurrentNormal = Vector3.up;
        isonfloor = false;
        Vector3 _down = Vector3.down;
        _down.y *= (this.transform.localScale.y / 2); // + 0.1f;
    
        var mask = LayerMask.GetMask("Ground");
        //int layerMask = 1 << 8;

        //Ray ray = new Ray(this.transform.position, _down);

        //isonfloor = Physics.Raycast(ray, Mathf.Abs(_down.y));

      // var all = Physics2D.RaycastAll(this.transform.position, _down, Mathf.Abs(_down.y), mask);
      // if(all != null && all.Length > 0)
      // {
      //  
      //
      //   
      // }

        RaycastHit2D rayhits = Physics2D.BoxCast(this.transform.position + _down, new Vector2(this.transform.localScale.x / 2, 0.1f), 0, new Vector2(0,-0.1f), 0.1f, LayerMask.GetMask("Ground")); ;
        if (rayhits.collider != null)
        {
            isonfloor = true;
            if(rayhits.normal.y > minGroundNormalY)
            CurrentNormal = rayhits.normal;
            this.rigidbody2D.gravityScale = 0;
        }

        // isonfloor = all != null && all.Length > 0;
        //var colider2d =  Physics2D.OverlapBox(this.transform.position+ _down, new Vector2(this.transform.localScale.x, 0.1f), LayerMask.GetMask("Ground")); ;
        //
        //if(colider2d != null)
        //{
        //
        //}



        Debug.DrawRay(this.transform.position, _down, Color.red);
      
        switch (playerState)
        {
            case PlayerStates.OnGround:
                if (!isonfloor)
                {
                    playerState = PlayerStates.Falling;
                 
                }
                break;
            case PlayerStates.Falling:
                if (isonfloor)
                {
                    playerState = PlayerStates.OnGround;
                }
                break;


            case PlayerStates.Jumpling:

                break;
            case PlayerStates.Dead:
                break;

        }
    }


    void OnGUI()
    {

        var _deltavector = new Vector3(-(this.transform.localScale.x), (this.transform.localScale.y / 2) + 1, 0);

        var posonscreen = Camera.main.WorldToScreenPoint(this.transform.position + _deltavector);


        GUI.Label(new Rect(0, 0, Screen.width, Screen.height),"Normal " +CurrentNormal + " RgbPos "+rigidbody2D.position+ " Playerstate = " + this.playerState + " , CurremtJumpVector = " + this.CurrentJumpVector);


        var currentcolor = GUI.color;
        GUI.color = Color.red;
        GUI.Label(new Rect(posonscreen.x, Screen.height - posonscreen.y, 100, 50), playerState.ToString());
        GUI.color = currentcolor;


        
       
    }

    public bool IsAlive()
    {
        return playerState != PlayerStates.Dead;
    }

    public void Dead()
    {
         playerState = PlayerStates.Dead;
    }
}
