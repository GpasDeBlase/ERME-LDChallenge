using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private Vector2 checkpos;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Color basecolor;

    void Start()
    {
        checkpos = transform.localPosition;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        basecolor = sr.color;
        
    }


    void Update()
    {
        if (Input.GetButtonDown("Respawn"))
        {
            transform.position = checkpos + new Vector2(0f,1f);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Entre dans un checkpoint
        if (collider.gameObject.layer == 7)
        {
            checkpos = collider.transform.position;
        }

        //Entre dans un piège
        if (collider.gameObject.layer == 8)
        {
            StartCoroutine(dieAndRespawn());  
        }
    }

    IEnumerator dieAndRespawn()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        transform.position = checkpos + new Vector2(0f,1f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        sr.color = basecolor;
    }
}
