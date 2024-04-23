using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Movement Settings")]
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;
    private float horizontalInput;
    private float speed;
    private SpriteRenderer sr;
    private Color basecolor;
    // Jump variables
    [SerializeField] private int jumpNbr = 1;
    [SerializeField] private int jumpForce;
    private Vector2 drawCenter;
    private int jumpCount = 0;
    
    

    [Header("Dash Settings")]
    [SerializeField] private float dashLength = .5f; 
    [SerializeField] private float dashCD = 1f;
    [SerializeField] private float dashSpeed;
    private int dashCount = 0;
    private int dashNbr = 1;
    private bool canDash;

    


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
        // Deplacement horizontal
        horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

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

    }

    public bool IsGrounded()
    { 
        float rayLength = 1.1f;
        int layerMask = 1 << 6;
        if (Physics2D.Raycast(transform.position, Vector2.down, rayLength, layerMask))
        {
            return true;
        }
        return false;
        
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


