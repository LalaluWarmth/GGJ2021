using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Movement : MonoBehaviour
{
    //components
    private Transform trs;
    private Collider2D coll;
    private Animator anim;

    private Vector2 mousePos;

    // private SpriteRenderer playerSprite;
    private Rigidbody2D rBody;
    private Animator animator;


    //inputs
    private int leftInput;
    private int rightInput;
    private int upInput;
    private int downInput;


    //situation
    public Vector2 Velocity;

    //horizontal
    public float horizontalMax;
    public float horizontalSpeed;

    private float dir;

    //vertical
    public float jumpMax;
    public float dropMax;
    public float verticalSpeed;

    public float dropSpeed;

    //groundCheck
    public bool isGround;

    public bool isJumping;

    //GroundCheck
    public Transform[] groundC;

    private LayerMask ground;

    // public Vector2 ropeHook;
    // public float swingForce = 4f;
    public bool groundCheck;

    // public bool isSwinging;
    //HeadCheck
    public Transform headC;

    public bool headCheck;
    //Hook
    // public GameObject assetHook;
    // public GameObject Hook;

    public bool moveable;


    void Awake()
    {
        ground = (1 << 10) | (1 << 11);
        moveable = false;
        trs = GetComponent<Transform>();
        // playerSprite = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
        Velocity = new Vector2(0.0f, 0.0f);
        // anim = GetComponent<Animator>();
    }

    void Start()
    {
    }

    void Update()
    {
        movementInput();
    }

    void FixedUpdate()
    {
        //groundC[0].position = new Vector2(trs.position.x + 0.4f,trs.position.y - 0.5f);
        //groundC[1].position = new Vector2(trs.position.x - 0.4f,trs.position.y - 0.5f);
        
        movementUpdate();
        // anim.SetBool("isGround",isGround);
        // anim.SetFloat("verticalSpeed",Velocity.y);
        // if(dir>0) anim.SetFloat("direction",dir);
        // if(dir<0) anim.SetFloat("direction",0);
        if (dir > 0)
        {
            transform.localScale=new Vector3(1,1,1);
        }

        if (dir < 0)
        {
            transform.localScale=new Vector3(-1,1,1);
        }
        // anim.SetBool("running",Input.GetKey(KeyCode.A)|Input.GetKey(KeyCode.D));
    }

    private void movementInput()
    {
        if (moveable == true)
        {
            leftInput = (Input.GetKey(KeyCode.A) == true) ? -1 : 0;
            rightInput = (Input.GetKey(KeyCode.D) == true) ? 1 : 0;
            upInput = (Input.GetKeyDown(KeyCode.W) == true) ? 1 : 0;
            downInput = (Input.GetKey(KeyCode.S) == true) ? -1 : 0;
        }
        else
        {
            leftInput = 0;
            rightInput = 0;
            upInput = 0;
            downInput = 0;
        }
    }

    private void movementUpdate()
    {
        horizontalUpdate();
        verticalUpdate();
        rBody.velocity = new Vector2(Velocity.x, Velocity.y);
        //rBody.MovePosition(new Vector2(trs.position.x + Velocity.x * Time.fixedDeltaTime, trs.position.y + Velocity.y * Time.fixedDeltaTime));
    }

    private void horizontalUpdate()
    {
        if (horizontalSpeed != 0) dir = Mathf.Abs(horizontalSpeed) / horizontalSpeed;
        //dir = (horizontalSpeed == 0) ? 1.0f : Mathf.Abs(horizontalSpeed) / horizontalSpeed;
        if (leftInput + rightInput != 0)
            horizontalSpeed = horizontalSpeed + horizontalMax * (leftInput + rightInput) / 12.0f;
        else
            horizontalSpeed -= dir * Mathf.Min(horizontalMax / 6.0f, horizontalSpeed * dir);
        if (horizontalSpeed * dir >= horizontalMax) horizontalSpeed = horizontalMax * dir;
        Velocity.x = horizontalSpeed;
    }

    private void verticalUpdate()
    {
        groundCheck = Physics2D.OverlapCircle(groundC[0].position, 0.02f, ground) ||
                      Physics2D.OverlapCircle(groundC[1].position, 0.02f, ground);
        headCheck = Physics2D.OverlapCircle(headC.position, 0.02f, ground);

        if (!groundCheck)
        {
            isGround = false;
            verticalSpeed = verticalSpeed + dropSpeed / 12.0f;
            if (verticalSpeed <= dropMax) verticalSpeed = dropMax;
        }
        else
        {
            isGround = true;
            verticalSpeed = 0.0f;
        }

        if (headCheck && verticalSpeed > 0.05f)
        {
            verticalSpeed = -0.05f;
        }

        jump();
        Velocity.y = verticalSpeed;
    }

    private void jump()
    {
        if (isGround == true && upInput == 1)
        {
            verticalSpeed = jumpMax;
            isGround = false;
        }
    }
}