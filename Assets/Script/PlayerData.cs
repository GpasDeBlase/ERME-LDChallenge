using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{

    [SerializeField] private Text tCompteur;
    [SerializeField] private GameObject uiCommande;
    private bool affUI = false;
    [SerializeField] private GameObject[] checkpoints;
    [SerializeField] private ParticleSystem particule;
    private int compteur;
    private Vector2 checkpos;
    private Vector2 startPos;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Color basecolor;


    void Start()
    {
        checkpos = transform.localPosition;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        basecolor = sr.color;
        startPos = transform.localPosition;
        compteur = 0;
    }


    void Update()
    {
        if (Input.GetButtonDown("Respawn"))
        {
            checkpos = startPos;
            transform.position = startPos + new Vector2(0f,1f);
            compteur = 0;
        }
        tCompteur.text = compteur.ToString();

        if (Input.GetKeyDown(KeyCode.T)) affUI = !affUI;
            
        if (affUI == true) uiCommande.SetActive(true);
        if (affUI == false) uiCommande.SetActive(false);

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();


        if (Input.GetKeyDown(KeyCode.Alpha1)) transform.position = new Vector3(0f, 1f, 0f) + checkpoints[0].transform.position;
        if (Input.GetKeyDown(KeyCode.Alpha2)) transform.position = new Vector3(0f, 1f, 0f) + checkpoints[1].transform.position;
        if (Input.GetKeyDown(KeyCode.Alpha3)) transform.position = new Vector3(0f, 1f, 0f) + checkpoints[2].transform.position;
        if (Input.GetKeyDown(KeyCode.Alpha4)) transform.position = new Vector3(0f, 1f, 0f) + checkpoints[3].transform.position;
        if (Input.GetKeyDown(KeyCode.Alpha5)) transform.position = new Vector3(0f, 1f, 0f) + checkpoints[4].transform.position;


    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Entre dans un checkpoint
        if (collider.gameObject.layer == 7)
        {
            checkpos = collider.transform.position;
        }

        // Entre dans un piège
        if (collider.gameObject.layer == 8)
        {
            StartCoroutine(dieAndRespawn());  
        }

        // Fini le niveau
        if (collider.gameObject.layer == 10)
        {
            particule.Play();
        }
    }

    IEnumerator dieAndRespawn()
    {
        transform.position = checkpos + new Vector2(0f, 1f);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        sr.color = Color.red;        
        yield return new WaitForSeconds(0.5f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        sr.color = basecolor;
        compteur++;
    }
}
