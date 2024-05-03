using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Movement Settings")]
    [SerializeField] private float moveSpeed;
    private float speed = 7;
    private SpriteRenderer sr;
    private Color basecolor;
    
    // Jump variables
    [SerializeField] private int jumpNbr = 1;
    [SerializeField] private int jumpForce = 5;
    [SerializeField] private int trampoJump = 10;
    private Vector2 drawCenter;
    private int jumpCount = 0;


    [Header("Dash Settings")]
    [SerializeField] private float dashLength = .3f; 
    [SerializeField] private float dashCD = 0.5f;
    [SerializeField] private float dashSpeed = 14;
    private int dashCount = 0;
    private int dashNbr = 1;
    private bool canDash;

    // Variables
    private bool _debug = false;
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        basecolor = sr.color;
        jumpCount = jumpNbr;
        dashCount = dashNbr;
        speed = moveSpeed;
        canDash = true;
    }


    void Update()
    {
        // Deplacement 
        if (_debug == false)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            sr.color = basecolor;
            rb.gravityScale = 1;
            horizontalInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }

        if (_debug == true) 
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            rb.gravityScale = 0;
            sr.color = new Color(0.5f, 0.5f, 0.5f, 1);
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);

        }


        // Saut
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded()) jumpCount = jumpNbr;
            if (jumpCount > 0)
            {
                jumpCount--;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                
            }
            
        }

        // Dash
        if (IsGrounded())
            {
                dashCount = dashNbr;   
            }
        if (Input.GetButtonDown("Dash"))
        {
            if (canDash && dashCount > 0)
            {
                canDash = false;
                speed = dashSpeed;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                sr.color = new Color32(77,255,150,255);
                StartCoroutine(dash());
                dashCount--;
            }

        }

        // DEBUG
        if(Input.GetButtonDown("Debug"))
        {
            _debug =  !_debug;
            //Debug.Log("debug mode");
        }

    }

    public bool IsGrounded()
    { 
        float rayLength = 0.4f;
        int layerMask = 1 << 6;
        if (Physics2D.Raycast(transform.position, Vector2.down, rayLength, layerMask))
        {
            return true;
        }
        return false;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==9)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * trampoJump, ForceMode2D.Impulse);
        }
    }

    IEnumerator dash()
    { 
        // Attendre la fin du dash
        yield return new WaitForSeconds(dashLength);
        speed = moveSpeed;  
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        sr.color = basecolor;

        // Cooldown de dash
        yield return new WaitForSeconds(dashCD);
        canDash = true;

    }
}


