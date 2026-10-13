using UnityEngine;
using System.Collections;

public class ProyectilDeSeguimiento : MonoBehaviour
{
    public float velocidadProyectil = 10f; // Velocidad de movimiento del proyectil
    public float tiempoSeguirJugador = 5f; // Tiempo durante el cual sigue al jugador
    Transform jugador; // Referencia al jugador
    private Vector3 posicionGuardadaJugador; // Posición guardada del jugador después de 5 segundos
    private Rigidbody2D rb;
    private BoxCollider2D boxCol;

    public bool disparado = false;

    RoundManager RM;
    PlayerScoreManager SM;
    float timer;

    void Awake()
    {
        disparado = false;
        boxCol = GetComponent<BoxCollider2D>();
        if (boxCol != null)
        {
            boxCol.enabled = false; // Desactivar collider desde el primer frame para evitar matar al spawnear
        }
    }

    void Start()
    {
        SM = FindAnyObjectByType<PlayerScoreManager>();
        RM = FindAnyObjectByType<RoundManager>();
        rb = GetComponent<Rigidbody2D>();
        
        GameObject pjObj = GameObject.FindGameObjectWithTag("Player");
        if (pjObj != null)
        {
            jugador = pjObj.transform;
        }

        StartCoroutine(SeguirJugador());
    }

    private void Update()
    {
        timer = RM != null ? RM.timer : 0f;
        if (Mathf.Ceil(timer) <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator SeguirJugador()
    {
        float tiempoPasado = 0f;

        // Durante los primeros 5 segundos, el proyectil apunta al jugador sin collider activo
        while (tiempoPasado < tiempoSeguirJugador)
        {
            if (jugador != null)
            {
                Vector3 direccion = (jugador.position - transform.position).normalized;
                transform.up = direccion;
            }

            tiempoPasado += Time.deltaTime;
            yield return null;
        }

        // Guarda la posición del jugador cuando se cumplen los 5 segundos
        if (jugador != null)
        {
            posicionGuardadaJugador = jugador.position;
        }

        // Deja de seguir al jugador
        rb.linearVelocity = Vector2.zero;

        // Disparar hacia la posición guardada
        StartCoroutine(DispararHaciaPosicion());
    }

    IEnumerator DispararHaciaPosicion()
    {
        yield return new WaitForSeconds(0.5f);

        // Se activa como letal y se habilita el collider al momento del lanzamiento
        disparado = true;
        if (boxCol != null)
        {
            boxCol.enabled = true;
        }

        Vector3 direccion = (posicionGuardadaJugador - transform.position).normalized;
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
