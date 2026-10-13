using UnityEngine;

public class Proyectil : MonoBehaviour
{
    RoundManager RM;
    float timer;

    private void Start()
    {
        RM = FindAnyObjectByType<RoundManager>();
        
    }

    private void Update()
    {
        timer = RM.timer;
        if (Mathf.Ceil(timer) <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("destruirProy"))
        {
            Destroy(gameObject);
        }
    }
}
