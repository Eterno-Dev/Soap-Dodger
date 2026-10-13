using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class ProyectilDeSeguimiento : MonoBehaviour
{
    public float velocidadProyectil = 10f; // Velocidad de movimiento del proyectil
    public float tiempoSeguirJugador = 5f; // Tiempo durante el cual sigue al jugador
    Transform jugador; // Referencia al jugador
    private Vector3 posicionGuardadaJugador; // Posición guardada del jugador después de 5 segundos
    //private bool siguiendoJugador = true; // Bandera para controlar si debe seguir al jugador o no
    private Rigidbody2D rb;
    int i;

    bool disparado;

    RoundManager RM;
    PlayerScoreManager SM; // Referencia al script del jugador
    float timer;
    void Start()
    {
        disparado = false;
        SM = FindAnyObjectByType<PlayerScoreManager>();
        RM = FindAnyObjectByType<RoundManager>();
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D del proyectil
        jugador = GameObject.FindGameObjectWithTag("Player").transform; // Asigna al jugador si no está asignado
        
        StartCoroutine(SeguirJugador());
    }

    private void Update()
    {
        if(disparado == false)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        timer = RM.timer;
        if (Mathf.Ceil(timer) <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator SeguirJugador()
    {
        float tiempoPasado = 0f;

        // Durante los primeros 5 segundos, el proyectil sigue al jugador
        while (tiempoPasado < tiempoSeguirJugador)
        {
            // Calcula la dirección hacia el jugador
            Vector3 direccion = (jugador.position - transform.position).normalized;

            // Hace que el proyectil mire hacia el jugador
            transform.up = direccion;

            // Aumenta el tiempo transcurrido
            tiempoPasado += Time.deltaTime;

            yield return null;
        }
        disparado = true;
        // Guarda la posición del jugador cuando se cumplen los 5 segundos
        posicionGuardadaJugador = jugador.position;

        // Deja de seguir al jugador
        rb.linearVelocity = Vector2.zero; // Detiene el movimiento

        // Ahora el proyectil disparará hacia la posición guardada
        StartCoroutine(DispararHaciaPosicion());
    }

    IEnumerator DispararHaciaPosicion()
    {
        // Espera un momento (si es necesario, por ejemplo, medio segundo)
        yield return new WaitForSeconds(0.5f);

        // Calcula la dirección hacia la posición guardada
        Vector3 direccion = (posicionGuardadaJugador - transform.position).normalized;

        // Establece la velocidad del proyectil en esa dirección
        rb.linearVelocity = direccion * velocidadProyectil;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("destruirProy"))
        {
            Destroy(gameObject);
        }
    }
}
