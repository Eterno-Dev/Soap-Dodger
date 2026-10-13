using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuchilla : MonoBehaviour
{
    int colisiones = 0;
    int maxColisones = 5;
    [SerializeField]
    Sprite cuchillaDegradada;

    public void Update()
    {
        if(colisiones == maxColisones)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("arriba/abajo") || collision.gameObject.CompareTag("lado"))
        {
            colisiones++;
            if(colisiones >= 3)
            {
                transform.GetComponent<SpriteRenderer>().sprite = cuchillaDegradada;
            }
        }
    }
}