using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour, IPlayerStatus, IRestartable
{
    RaycastHit2D hit;
    private Vector3 NormalOfCollision = Vector3.zero;
    RaycastHit2D hit2;
    private Vector3 NormalOfCollision2 = Vector3.zero;

    private Vector3 initialpos = Vector3.zero;

    private TimeSpan nextWallSliding = TimeSpan.Zero;

    private DateTime datewalljump = DateTime.MinValue;

    public int WallJumpingMsFrecuency = 100;

    private int frame = 0;

    private bool wallsliding = false;

    public float minGroundNormalY = .65f;

    public float Gravitywallsliding = 0.25f;

    public float Gravity = 1f;

    public PlayerStates playerState;

    public float HorizontalMovementdelta = 0.5f;

    public Vector3 JumpVector = new Vector3(0, 0.25f, 0);

    public float JumpWallVectorPercentage = 1.1f;

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

    private SpriteRenderer spriteRender = null;

    public Transform LeftCheck;

    public Transform RightCheck;


    private ContactFilter2D filter2D = new ContactFilter2D();
    // Use this for initialization
    void Start()
    {
        initialpos = this.transform.position;

        spriteRender = this.GetComponent<SpriteRenderer>();

        nextWallSliding = TimeSpan.FromMilliseconds(WallJumpingMsFrecuency);

        playerState = PlayerStates.OnGround;

        this.rigidbody2D = this.GetComponent<Rigidbody2D>();

        //TODO provisional
        // Camera.main.transform.parent = this.transform;


        filter2D.useLayerMask = true;
        filter2D.useTriggers = false;
        filter2D.SetLayerMask(LayerMask.GetMask("Ground"));
    }


    // Update is called once per frame
    void Update()
    {

        CheckBoxCollider();
        try
        {
            if (LevelController.instance.gameState != GameStates.Playing)
            {
                this.rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                return;
            }



            wallsliding = false;
            frame++;
            switch (playerState)
            {

                case PlayerStates.Falling:
                    //ProcessFalling();
                    CheckMovingButtons();
                    CheckWallSliding();
                    CheckOnTheFloor();

                    AddHorizontalMovement();

                    break;
                case PlayerStates.Jumpling:

                    CheckMovingButtons();
                    ProcessJump();
                    CheckWallSliding();
                    if (!wallsliding)
                        AddHorizontalMovement();
                    else
                    {
                        playerState = PlayerStates.Falling;
                        this.CurrentJumpVector = Vector3.zero;

                    }

                    break;
                case PlayerStates.OnGround:

                    CheckMovingButtons();

                    var _checkJumping = CheckJumping();
                    //Debug.Log(frame+" CheckJumping " + _checkJumping);
                    if (!_checkJumping)
                        CheckOnTheFloor();


                    AddHorizontalMovement();




                    break;


            }
        }
        finally
        {
            SetPlayerSprite();
        }

    }

    private void CheckBoxCollider()
    {
        NormalOfCollision = Vector3.zero;
        var _collider = this.GetComponent<Collider2D>();
        var aux = this.transform.position;
        var _position = aux + new Vector3(0, -0.1f, 0);// Vector3.down ;

        hit = Physics2D.BoxCast(_position, _collider.bounds.size, 0, Vector2.down, 0.3f, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            NormalOfCollision = hit.normal;
        }

        var end = _collider.bounds.size.y / 2 * Vector3.down;
        hit2 = Physics2D.Linecast(this.transform.position, _position + end, filter2D.layerMask.value);

        if (hit2.collider != null)
        {
            NormalOfCollision2 = hit2.normal;
        }
    }

    private void SetPlayerSprite()
    {
        try
        {
            var ls = this.transform.localScale;
            if (horizontalMovement == Vector3.left)
            {

                ls.x = Mathf.Abs(ls.x) * -1;
            }
            else
            {

                ls.x = Mathf.Abs(ls.x);
            }

            this.transform.localScale = ls;
            if (playerState == PlayerStates.OnGround)
            {
                if (horizontalMovement == Vector3.zero)
                    spriteRender.sprite = SpritePlayerMannager.instance.PlayerSprites[(int)PlayerStates.OnGround];
                else
                {
                    spriteRender.sprite = SpritePlayerMannager.instance.PlayerSprites[SpritePlayerMannager.instance.PlayerSprites.Length - 1];
                }
            }
            else
            {
                spriteRender.sprite = SpritePlayerMannager.instance.PlayerSprites[(int)playerState];
            }


        }
        finally
        {

        }
    }

    private void CheckWallSliding()
    {


        var _collider = this.GetComponent<Collider2D>();
        this.rigidbody2D.gravityScale = Gravity;
        var checker = RightCheck;
        if (horizontalMovement == Vector3.left)
            checker = LeftCheck;

        RaycastHit2D rayhits = Physics2D.BoxCast(
              checker.position + horizontalMovement / 5,
              _collider.bounds.size,
              0,
              horizontalMovement,
             0.1f,
              LayerMask.GetMask("Ground"));




        if (rayhits.collider != null)
        {
            wallsliding = true;

            if (datewalljump == DateTime.MinValue || (DateTime.Now - datewalljump) >= nextWallSliding)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space))
                {
                    datewalljump = DateTime.Now;
                    playerState = PlayerStates.Jumpling;
                    //AudioController.instance.PlaySound(Sounds.Jump);
                    CurrentJumpVector = this.JumpVector * JumpWallVectorPercentage;
                }

            }
            else
            {
                this.rigidbody2D.gravityScale = Gravitywallsliding;
            }
        }
        else
        {
            datewalljump = DateTime.MinValue;
        }

        // Vector3 direction = horizontalMovement;
        // direction.x *= (this.transform.localScale.x / 2); // + 0.1f;
        //
        // RaycastHit2D rayhits = Physics2D.BoxCast(
        //     this.transform.position + direction,
        //     new Vector2(this.transform.localScale.x / 2, this.transform.localScale.y), 
        //     0, 
        //     new Vector2(direction.x / 2, 0), 
        //    Mathf.Abs(direction.x / 2 ),
        //     LayerMask.GetMask("Ground"));
        //
        // if (rayhits.collider != null)
        // {
        //     wallsliding = true;
        //     if ( Input.GetKeyDown(KeyCode.Space)  || Input.GetKey(KeyCode.Space))
        //     {
        //         if((DateTime.Now - datewalljump) >= nextWallSliding)
        //         {
        //             datewalljump = DateTime.Now;
        //             playerState = PlayerStates.Jumpling;
        //             //AudioController.instance.PlaySound(Sounds.Jump);
        //             CurrentJumpVector = this.JumpVector * JumpWallVectorPercentage;
        //         }
        // 
        //     }
        //     else
        //     {
        //         this.rigidbody2D.gravityScale = Gravitywallsliding;
        //     }
        // }
        //


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
            //AudioController.instance.PlaySound(Sounds.Jump);
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
        this.rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        Vector3 moveAlongGround = horizontalMovement;
        if (CurrentNormal != Vector3.up && CurrentNormal != Vector3.zero)
            Debug.Log("CurrentNormal " + CurrentNormal);

        if (CurrentNormal != Vector3.zero)
        {
            Vector2 auxnormal = new Vector2(CurrentNormal.y, -CurrentNormal.x);
            moveAlongGround.x = moveAlongGround.x * auxnormal.x;

        }

        if (moveAlongGround != Vector3.zero)
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
        this.rigidbody2D.gravityScale = wallsliding ? Gravitywallsliding : Gravity;

        var _collider = this.GetComponent<Collider2D>();
        CurrentNormal = Vector3.zero;
        isonfloor = false;


        var mask = LayerMask.GetMask("Ground");

        RaycastHit2D rayhits = Physics2D.BoxCast(
            this.transform.position + Vector3.down / 10,
              _collider.bounds.size,
            0,
           Vector3.down,
            0.01f,
            LayerMask.GetMask("Ground"));

        if (rayhits.collider != null)
        {
            isonfloor = true;
            if (rayhits.normal.y > minGroundNormalY)
                CurrentNormal = rayhits.normal;
            this.rigidbody2D.gravityScale = 0.01f;
        }
        if (!isonfloor)
            Debug.Log("isonfloor " + isonfloor);

        // isonfloor = all != null && all.Length > 0;
        //var colider2d =  Physics2D.OverlapBox(this.transform.position+ _down, new Vector2(this.transform.localScale.x, 0.1f), LayerMask.GetMask("Ground")); ;
        //
        //if(colider2d != null)
        //{
        //
        //}



        //Debug.DrawRay(this.transform.position, _down, Color.red);

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
        var _collider = this.GetComponent<Collider2D>();
        GUI.Label(new Rect(0, 0, Screen.width, 40), NormalOfCollision.ToString() + " " + hit.distance + " | 2= " + NormalOfCollision2 + " " + hit2.distance + " | " + filter2D.layerMask.value + " " + _collider.bounds.size);
    }
    public bool IsAlive()
    {
        return playerState != PlayerStates.Dead;
    }

    public void Dead()
    {
        playerState = PlayerStates.Dead;
    }

    public void PlayerHasWon()
    {
        playerState = PlayerStates.HasWon;
    }


    public void Restart()
    {
        this.transform.position = initialpos;
        this.playerState = PlayerStates.OnGround;
    }
}
