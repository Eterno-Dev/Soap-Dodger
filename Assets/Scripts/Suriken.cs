using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suriken : MonoBehaviour
{
    Rigidbody2D rb;
    CircleCollider2D circleCollider;
    List<Cuchilla> cuchillas = new List<Cuchilla>();
    int cuchillasCount = 0;
    bool cuchillasActivas = false;
    float speed = 5f;
    float rotationSpeed = 200f;
    Vector2 direction;
    public AudioClip sonidoGiro;
    public AudioClip sonidoRomper;
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    public void Start()
    {
       
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        cuchillasCount = transform.childCount;
        for (int i = 0; i < cuchillasCount; i++)
        {
            cuchillas.Add(transform.GetChild(i).GetComponent<Cuchilla>());
        }
        StartCoroutine(ActivarCuchillas());
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource1.clip = sonidoGiro;
        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource2.clip = sonidoRomper;
        audioSource1.volume = 0.5f;
    }

    public void Update()
    {
        cuchillasCount = transform.childCount;
        if(cuchillasCount == 0)
        {
            Destroy(gameObject);
            audioSource2.Play();
        }
        rb.linearVelocity = direction * speed;
        rb.angularVelocity = rotationSpeed;
        if(!audioSource1.isPlaying)
        {
            if(cuchillasActivas == true)
            {
                audioSource1.Play();
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("arriba/abajo"))
        {
            Bounce(1);
        } else if(collision.gameObject.CompareTag("lado"))
        {
            Bounce(0);
        }
    }

    public void Bounce(int bounceDir) //BounceDir es 0(lateral) o 1(arriba/abajo).
    {
        float dirX = direction.x;
        float dirY = direction.y;
        if(bounceDir == 0)
        {
            dirX = -dirX;
        } else if(bounceDir == 1)
        {
            dirY = -dirY;
        }
        direction = new Vector2(dirX, dirY).normalized;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    IEnumerator ActivarCuchillas()
    {
        yield return new WaitForSeconds(3f);
        cuchillasActivas = true;
        gameObject.tag = "Enemy";
        foreach(Cuchilla cuchilla in cuchillas)
        {
            cuchilla.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
    }
}